namespace ComputerUpTime;

internal class ActivityMapper(IEnumerable<WorkDayActivity> activities, ISystemTime dateTime, IWorkDayLogger logger)
{
    private readonly Dictionary<DateTime, WorkDay> workDays = new();

    internal static TimeSpan ActivityTimeLimit { get; } = TimeSpan.FromDays(60);

    public void Run()
    {
        if (!activities.Any())
        {
            logger.Log("The system event log does not contain any data.");
            return;
        }

        var oldestEntryToConsider = dateTime.Now() - ActivityTimeLimit;

        logger.Log("Searching newest entries...");
        activities.ToList().ForEach(entry =>
        {
            if (entry.TimeStamp < oldestEntryToConsider) return;

            var currentDay = GetCurrentDay(entry.TimeStamp);
            currentDay.ExpandToInclude(entry);
        });

        if (!workDays.Any())
        {
            logger.Log(
                $"The system event log does not contain any data for the last {ActivityTimeLimit.Days} days.");
            return;
        }

        workDays.Values.ToList().ForEach(
            workDay => logger.Log(workDay.ToString()));
    }

    private WorkDay GetCurrentDay(DateTime entryTimeGenerated)
    {
        if (workDays.TryGetValue(entryTimeGenerated.Date, out var result)) return result;

        result = new WorkDay(entryTimeGenerated);
        workDays[entryTimeGenerated.Date] = result;

        return result;
    }
}