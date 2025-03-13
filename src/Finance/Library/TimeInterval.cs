namespace Finance.Library;

public enum TimeInterval
{
    Day,
    Month,
    Year
}

public static partial class DateTimeExtensions
{
    public static DateTime Truncate(this DateTime date, TimeInterval interval)
    {
        return interval switch
        {
            TimeInterval.Day => new DateTime(date.Year, date.Month, date.Day),
            TimeInterval.Month => new DateTime(date.Year, date.Month, 1),
            TimeInterval.Year => new DateTime(date.Year, 1, 1),
            _ => throw new ArgumentException("Unsupported interval", nameof(interval))
        };
    }
}
