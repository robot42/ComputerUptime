namespace ComputerUpTime;

internal class WorkDayActivity
{
    public WorkDayActivity(DateTime timeStamp)
    {
        this.TimeStamp = timeStamp;
    }

    public DateTime TimeStamp { get; }
}