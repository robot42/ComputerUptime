namespace ComputerUpTime;

internal class ActivityMapper
{
    private readonly IEnumerable<WorkDayActivity> _activities;
    private readonly ISystemTime _dateTime;
    private readonly IWorkDayLogger _logger;
    private readonly Dictionary<DateTime, WorkDay> _workDays = new();

    public ActivityMapper(IEnumerable<WorkDayActivity> activities, ISystemTime dateTime, IWorkDayLogger logger)
    {
        _activities = activities;
        _dateTime = dateTime;
        _logger = logger;
    }

    internal static TimeSpan ActivityTimeLimit { get; } = TimeSpan.FromDays(60);

    public void Run()
    {
        if (!_activities.Any())
        {
            _logger.Log("The system event log does not contain any data.");
            return;
        }

        var oldestEntryToConsider = _dateTime.Now() - ActivityTimeLimit;

        _logger.Log("Searching newest entries...");
        _activities.ToList().ForEach(entry =>
        {
            if (entry.TimeStamp < oldestEntryToConsider) return;

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

    private WorkDay GetCurrentDay(DateTime entryTimeGenerated)
    {
        if (_workDays.TryGetValue(entryTimeGenerated.Date, out var result)) return result;

        result = new WorkDay(entryTimeGenerated);
        _workDays[entryTimeGenerated.Date] = result;

        return result;
    }
}