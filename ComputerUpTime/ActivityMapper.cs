using System;
using System.Collections.Generic;
using System.Linq;

namespace ComputerUpTime
{
    internal class ActivityMapper
    {
        private readonly IEnumerable<WorkDayActivity> _activities;
        private readonly IWorkDayLogger _logger;
        private readonly Dictionary<DateTime, WorkDay> _workDays = new Dictionary<DateTime, WorkDay>();

        public ActivityMapper(IEnumerable<WorkDayActivity> activities, IWorkDayLogger logger)
        {
            _activities = activities;
            _logger = logger;
        }

        public void Run()
        {
            if (!_activities.Any())
            {
                _logger.Log("The system event log does not contain any data.");
                return;
            }

            var oldestEntryToConsider = DateTime.Now - ActivityTimeLimit;

            _logger.Log("Searching newest entries...");
            _activities.ToList().ForEach(entry =>
            {
                if (entry.TimeStamp < oldestEntryToConsider)
                {
                    return;
                }

                var currentDay = GetCurrentDay(entry.TimeStamp);
                currentDay.ExpandToInclude(entry.TimeStamp);
            });

            if (!_workDays.Any())
            {
                _logger.Log(
                    $"The system event log does not contain any data for the last {ActivityTimeLimit.Days} days.");
                return;
            }

            _workDays.Values.ToList().ForEach(
                workDay => _logger.Log(workDay.ToString()));
        }

        internal static TimeSpan ActivityTimeLimit { get; } = TimeSpan.FromDays(60);

        private WorkDay GetCurrentDay(DateTime entryTimeGenerated)
        {
            if (_workDays.TryGetValue(entryTimeGenerated.Date, out var result))
            {
                return result;
            }

            result = new WorkDay(entryTimeGenerated);
            _workDays[entryTimeGenerated.Date] = result;

            return result;
        }
    }
}