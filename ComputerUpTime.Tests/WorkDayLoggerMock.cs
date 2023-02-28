using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ComputerUpTime.Tests
{
    [ExcludeFromCodeCoverage]
    internal class WorkDayLoggerMock : IWorkDayLogger
    {
        public List<string> Output { get; } = new List<string>();

        public void Log(string text)
        {
            Output.Add(text);
        }
    }
}