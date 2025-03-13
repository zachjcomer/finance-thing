namespace Finance.Library;

public static partial class DateTimeExtensions
{
    public static DateTime Add(this DateTime date, TimeInterval interval)
    {
        return interval switch
        {
            TimeInterval.Day => date.AddDays(1),
            TimeInterval.Month => date.AddMonths(1),
            TimeInterval.Year => date.AddYears(1),
            _ => throw new ArgumentException("Unsupported interval", nameof(interval))
        };
    }
}
