// ReSharper disable once CheckNamespace
namespace System
{
    internal static class DateTimeExtension
    {
        internal static DateTime RoundToFiveMinutes(this DateTime start)
        {
            var minutes = Math.Round((1.0 * start.Minute + start.Second / 60.0) / 5);
            return new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0).AddMinutes(minutes * 5);
        }
    }
}