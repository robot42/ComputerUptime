using System;
using System.Diagnostics;
using System.Linq;

namespace ComputerUpTime
{
    class Program
    {
        static void Main(string[] _)
        {
            var log = new EventLog("System");
            var mapper = new ActivityMapper(
                log.Entries.Cast<EventLogEntry>().Select(entry => new WorkDayActivity(entry)),
                new WorkDayLogger());

            mapper.Run();

            Console.ReadKey();
        }
    }
}