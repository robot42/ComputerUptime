using System;
using System.Diagnostics;

namespace ComputerUpTime
{
    internal class WorkDayActivity : IWorkDayActivity
    {
        public WorkDayActivity(EventLogEntry entry)
        {
            this.TimeStamp = entry.TimeGenerated;
        }

        public DateTime TimeStamp { get; }
    }
}