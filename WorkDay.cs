﻿using System;

namespace ComputerUpTime
{
    internal class WorkDay
    {
        public WorkDay(DateTime day)
        {
            this.Start = new DateTime(day.Date.AddDays(1).Ticks);
            this.End = new DateTime(day.Date.Ticks);
        }

        public DateTime Start { get; private set; }

        public DateTime End { get; private set; }

        public DateTime RoundedStart => this.Start.RoundToFiveMinutes();

        public DateTime RoundedEnd => this.End.RoundToFiveMinutes();

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
            return string.Format(
                "{0}: {1} - {2}               ({3} - {4})",
                this.Start.ToShortDateString(),
                this.RoundedStart.TimeOfDay,
                this.RoundedEnd.TimeOfDay,
                this.Start.TimeOfDay,
                this.End.TimeOfDay);
        }
    }
}