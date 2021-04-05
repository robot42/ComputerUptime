using System.Collections.Generic;

namespace ComputerUptime
{
    using System;
    using System.Diagnostics;

    class WorkDay
    {
        public WorkDay(DateTime day)
        {
            this.Start = new DateTime(day.Date.AddDays(1).Ticks);
            this.End = new DateTime(day.Date.Ticks);
        }

        public DateTime Start { get; private set; }

        public DateTime End { get; private set; }

        public DateTime RoundedStart => RoundToFullFiveMinutes(this.Start);

        private static DateTime RoundToFullFiveMinutes(DateTime start)
        {
            var minutes = Math.Round((1.0 * start.Minute + start.Second / 60.0) / 5);
            return new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0).AddMinutes(minutes * 5);
        }

        public DateTime RoundedEnd => RoundToFullFiveMinutes(this.End);

        public void ExpandToInclude(DateTime entryTimeGenerated)
        {
            if (Start > entryTimeGenerated)
            {
                Start = entryTimeGenerated;
            }

            if (End < entryTimeGenerated)
            {
                End = entryTimeGenerated;
            }
        }
    }

    class Program
    {
        private static Dictionary<DateTime, WorkDay> workDays = new Dictionary<DateTime, WorkDay>();

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
