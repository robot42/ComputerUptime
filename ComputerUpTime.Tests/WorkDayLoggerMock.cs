using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ComputerUpTime.Tests
{
    [ExcludeFromCodeCoverage]
    internal class WorkDayLoggerMock : IWorkDayLogger
    {
        public void Log(string text)
        {
            this.Output.Add(text);
        }

        public List<string> Output { get; } = new List<string>();
    }
}