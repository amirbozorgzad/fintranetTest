namespace Core.Domain.Model;

public class Holiday
{
    public long CalendarId { get; set; }
    public DateOnly HolidayDate { get; set; }
    public Calendar Calendar { get; set; }
}