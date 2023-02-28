using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ComputerUpTime;

// excluded because this is the entry point to the real program called
// by Windows and the system boundary to the event logging.
[ExcludeFromCodeCoverage]
internal class Program
{
    private static void Main(string[] _)
    {
        var log = new EventLog("System");
        var mapper = new ActivityMapper(
            log.Entries.Cast<EventLogEntry>().Select(entry => new WorkDayActivity(entry.TimeGenerated)),
            new SystemTime(),
            new WorkDayLogger());

        mapper.Run();

        Console.ReadKey();
    }
}