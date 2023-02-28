using System.Diagnostics.CodeAnalysis;

namespace ComputerUpTime;

internal class WorkDayLogger : IWorkDayLogger
{
    // excluded because this implementation of IWorkDayLogger
    // is intended to cross the system boundaries.
    [ExcludeFromCodeCoverage]
    public void Log(string text)
    {
        Console.WriteLine(text);
    }
}