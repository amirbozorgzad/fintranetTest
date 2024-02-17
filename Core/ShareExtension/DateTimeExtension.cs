namespace Core.ShareExtension;

public static class DateTimeExtension
{
    public static DateTime ToDateTime(this DateOnly date)
    {
        return new DateTime(date.Year, date.Month, date.Day);
    }

    public static DateOnly ToDateOnly(this DateTime dateTime)
    {
        return new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
    }

    public static TimeSpan ToTimeSpan(this DateTime date)
    {
        return new TimeSpan(date.Hour, date.Minute, 0);
    }
}