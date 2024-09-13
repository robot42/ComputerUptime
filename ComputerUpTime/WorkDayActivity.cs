namespace ComputerUpTime;

internal enum WorkDayActivityKind
{
    Sleep = 42,
    WakeUp = 507,
    Shutdown = 13,
    Started = 12
}

internal record WorkDayActivity(DateTime TimeStamp, WorkDayActivityKind Kind);