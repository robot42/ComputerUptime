using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace ComputerUpTime;

// excluded because this is the entry point to the real program called
// by Windows and the system boundary to the event logging.
[ExcludeFromCodeCoverage]
internal class Program
{
    [SupportedOSPlatform("windows")]
    private static void Main(string[] _)
    {
        var log = new EventLog("System");
        var desiredInstanceIds =  new List<long>
        {
            (long) WorkDayActivityKind.Sleep,
            (long) WorkDayActivityKind.WakeUp,
            (long) WorkDayActivityKind.Shutdown,
            (long) WorkDayActivityKind.Started
        };

        var mapper = new ActivityMapper(
            log.Entries
                .Cast<EventLogEntry>()
                .Where(entry => desiredInstanceIds.Contains(entry.InstanceId))
                .Select(entry => new WorkDayActivity(entry.TimeGenerated, (WorkDayActivityKind)entry.InstanceId))
                .OrderBy(entry => entry.TimeStamp),
            new SystemTime(),
            new WorkDayLogger());

        mapper.Run();

        Console.ReadKey();
    }
}
