using System.Globalization;
using System.Text;

namespace ComputerUpTime;

internal class WorkDay(DateTime day)
{
    private readonly List<WorkDayActivity> activities = [];

    private DateTime Start { get; set; } = day;

    private DateTime End { get; set; } = day;

    private DateTime RoundedStart => Start.RoundToFiveMinutes();

    private DateTime RoundedEnd => End.RoundToFiveMinutes();

    public void ExpandToInclude(WorkDayActivity activity)
    {
        this.activities.Add(activity);

        if (Start > activity.TimeStamp)
        {
            Start = activity.TimeStamp;
            return;
        }

        if (End < activity.TimeStamp)
        {
            End = activity.TimeStamp;
        }
    }

    public override string ToString()
    {
        StringBuilder builder = new();

        builder.Append($"{Start.ToString("d", new CultureInfo("de-DE"))}: ");
        builder.Append($"{RoundedStart.TimeOfDay:hh\\:mm} - {RoundedEnd.TimeOfDay:hh\\:mm}");
        builder.Append($"               ({Start.TimeOfDay} - {End.TimeOfDay})");

        foreach (var activity in activities)
        {
            builder.Append($"{Environment.NewLine}");
            builder.Append("                                         ");
            builder.Append($"{activity.TimeStamp.TimeOfDay:hh\\:mm} - {activity.Kind}");
        }

        builder.Append($"{Environment.NewLine}");

        return builder.ToString();
    }
}