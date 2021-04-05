using System;

namespace ComputerUpTime
{
    internal class WorkDayLogger : IWorkDayLogger
    {
        public void Log(WorkDay workDay)
        {
            Console.WriteLine(workDay);
        }
    }
}