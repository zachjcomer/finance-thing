namespace Finance.Library;

public enum TimeInterval
{
    Day,
    Week,
    Month,
    Year
}

public static partial class DateTimeExtensions
{
    public static DateTime Add(this DateTime date, TimeInterval interval)
    {
        return interval switch
        {
            TimeInterval.Day => date.AddDays(1),
            TimeInterval.Week => date.AddDays(7),
            TimeInterval.Month => date.AddMonthsWithClamp(1),
            TimeInterval.Year => date.AddYears(1),
            _ => throw new ArgumentException("Unsupported interval", nameof(interval))
        };
    }
    
    public static DateTime Truncate(this DateTime date, TimeInterval interval)
    {
        return interval switch
        {
            TimeInterval.Day => new DateTime(date.Year, date.Month, date.Day),
            TimeInterval.Week => date.GetFirstDateOfWeek(),
            TimeInterval.Month => new DateTime(date.Year, date.Month, 1),
            TimeInterval.Year => new DateTime(date.Year, 1, 1),
            _ => throw new ArgumentException("Unsupported interval", nameof(interval))
        };
    }

    public static DateTime AddMonthsWithClamp(this DateTime date, int months)
    {
        if (months == 0)
            return date;

        if (date.Day >= DateTime.DaysInMonth(date.Year, date.Month))
        {
            DateTime nextMonth = months > 0 ? date.AddMonths(1) : date.AddMonths(-1);
            DateTime lastDayOfNextMonth = new DateTime(nextMonth.Year, nextMonth.Month, DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month));
            return lastDayOfNextMonth.AddMonthsWithClamp(months > 0 ? months - 1 : months + 1);
        }
        return date.AddMonths(months);
    }

    private static DateTime GetFirstDateOfWeek(this DateTime date)
    {
        return date.AddDays(-(int)date.DayOfWeek);
    }

    public static Transactions.Schedules.Schedule.Builder Daily(this Transactions.Schedules.Schedule.Builder builder)
    => builder.WithInterval(TimeInterval.Day);

    public static Transactions.Schedules.Schedule.Builder Weekly(this Transactions.Schedules.Schedule.Builder builder)
    => builder.WithInterval(TimeInterval.Week);

    public static Transactions.Schedules.Schedule.Builder Monthly(this Transactions.Schedules.Schedule.Builder builder)
    => builder.WithInterval(TimeInterval.Month);

    public static Transactions.Schedules.Schedule.Builder Yearly(this Transactions.Schedules.Schedule.Builder builder)
    => builder.WithInterval(TimeInterval.Year);
}
