using System;
using System.Collections.Generic;
using System.Linq;

namespace ComputerUpTime
{
    internal class ActivityMapper
    {
        private readonly IEnumerable<IWorkDayActivity> _activities;
        private readonly IWorkDayLogger _logger;
        private readonly Dictionary<DateTime, WorkDay> workDays = new Dictionary<DateTime, WorkDay>();

        public ActivityMapper(IEnumerable<IWorkDayActivity> activities, IWorkDayLogger logger)
        {
            _activities = activities;
            _logger = logger;
        }

        public void Run()
        {
            if (!_activities.Any())
            {
                Console.WriteLine();
                Console.WriteLine("The system event log does not contain any data.");
                Console.ReadKey();
                return;
            }

            var oldestEntryToConsider = DateTime.Now - new TimeSpan(60, 0, 0, 0);

            Console.WriteLine("Searching newest entries...");
            foreach (var entry in _activities)
            {
                if (entry.TimeStamp < oldestEntryToConsider)
                {
                    continue;
                }

                WorkDay currentDay = GetCurrentDay(entry.TimeStamp);

                currentDay.ExpandToInclude(entry.TimeStamp);
            }

            foreach (var workDay in workDays)
            {
                Console.WriteLine(
                    "{0}: {1} - {2}               ({3} - {4})",
                    workDay.Key.Date.ToShortDateString(),
                    workDay.Value.RoundedStart.TimeOfDay,
                    workDay.Value.RoundedEnd.TimeOfDay,
                    workDay.Value.Start.TimeOfDay,
                    workDay.Value.End.TimeOfDay);
            }
        }

        private WorkDay GetCurrentDay(DateTime entryTimeGenerated)
        {
            if (workDays.ContainsKey(entryTimeGenerated.Date) == false)
            {
                workDays[entryTimeGenerated.Date] = new WorkDay(entryTimeGenerated.Date);
            }

            return workDays[entryTimeGenerated.Date];
        }
    }
}