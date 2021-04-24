using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ComputerUpTime
{
    // excluded because this is the entry point to the real program called
    // by Windows and the system boundary to the event logging.
    [ExcludeFromCodeCoverage]
    class Program
    {
        static void Main(string[] _)
        {
            var log = new EventLog("System");
            var mapper = new ActivityMapper(
                log.Entries.Cast<EventLogEntry>().Select(entry => new WorkDayActivity(entry.TimeGenerated)),
                new WorkDayLogger());

            mapper.Run();

            Console.ReadKey();
        }
    }
}