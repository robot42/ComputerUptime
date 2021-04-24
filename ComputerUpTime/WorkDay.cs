using System;

namespace ComputerUpTime
{
    internal class WorkDay
    {
        public WorkDay(DateTime day)
        {
            Start = day;
            End = day;
        }

        public DateTime Start { get; private set; }

        public DateTime End { get; private set; }

        public DateTime RoundedStart => Start.RoundToFiveMinutes();

        public DateTime RoundedEnd => End.RoundToFiveMinutes();

        public void ExpandToInclude(DateTime entryTimeGenerated)
        {
            if (Start > entryTimeGenerated)
            {
                Start = entryTimeGenerated;
                return;
            }

            if (End < entryTimeGenerated)
            {
                End = entryTimeGenerated;
            }
        }

        public override string ToString()
        {
            return
                $"{Start.ToShortDateString()}:" +
                $" {RoundedStart.TimeOfDay:hh\\:mm} - {RoundedEnd.TimeOfDay:hh\\:mm}" +
                $"               ({Start.TimeOfDay} - {End.TimeOfDay})";
        }
    }
}