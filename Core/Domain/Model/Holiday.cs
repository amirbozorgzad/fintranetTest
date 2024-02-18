namespace Core.Domain.Model;

public class Holiday : Entity
{
    public long CalendarId { get; set; }

    public DateTime HolidayDate { get; set; }

    public Calendar Calendar { get; set; }
}