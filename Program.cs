using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ComputerUpTime
{
    class Program
    {
        private static readonly Dictionary<DateTime, WorkDay> workDays = new Dictionary<DateTime, WorkDay>();

        static void Main(string[] args)
        {
            var log = new EventLog("System");

            if (log.Entries.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("The system event log does not contain any data.");
                Console.ReadKey();
                return;
            }

            var oldestEntryToConsider = DateTime.Now - new TimeSpan(60, 0, 0, 0);

            Console.WriteLine("Searching newest entries...");
            foreach (EventLogEntry entry in log.Entries)
            {
                if (entry.TimeGenerated < oldestEntryToConsider)
                {
                    continue;
                }

                WorkDay currentDay = GetCurrentDay(entry.TimeGenerated);

                currentDay.ExpandToInclude(entry.TimeGenerated);
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

            Console.ReadKey();
        }

        private static WorkDay GetCurrentDay(DateTime entryTimeGenerated)
        {
            if (workDays.ContainsKey(entryTimeGenerated.Date) == false)
            {
                workDays[entryTimeGenerated.Date] = new WorkDay(entryTimeGenerated.Date);
            }

            return workDays[entryTimeGenerated.Date];
        }
    }
}
