namespace ComputerUpTime;

internal sealed class SystemTime : ISystemTime
{
    public DateTime Now()
    {
        return DateTime.Now;
    }
}