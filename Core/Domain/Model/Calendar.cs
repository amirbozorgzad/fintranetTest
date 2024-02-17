using Core.Contract.Enum;

namespace Core.Domain.Model;

public class Calendar : Entity
{
    public Calendar()
    {
        HoliDays = new HashSet<Holiday>();
    }

    public long cityId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public WeekDays WorkingDaysInWeek { get; set; }
    public Months HolidaysMonth { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public City City { get; set; }
    public ICollection<Holiday> HoliDays { get; set; }

    public static bool IsMonthInHolidaysMonth(DateOnly date, Months holidaysMonth)
    {
        var result = false;
        switch (date.Month)
        {
            case 1:
            {
                result = (holidaysMonth & Months.January) == Months.January;
                break;
            }
            case 2:
            {
                result = (holidaysMonth & Months.February) == Months.February;
                break;
            }
            case 3:
            {
                result = (holidaysMonth & Months.March) == Months.March;
                break;
            }
            case 4:
            {
                result = (holidaysMonth & Months.April) == Months.April;
                break;
            }
            case 5:
            {
                result = (holidaysMonth & Months.May) == Months.May;
                break;
            }
                ;
            case 6:
            {
                result = (holidaysMonth & Months.June) == Months.June;
                break;
            }
            case 7:
            {
                result = (holidaysMonth & Months.July) == Months.July;
                break;
            }
            case 8:
            {
                result = (holidaysMonth & Months.August) == Months.August;
                break;
            }
            case 9:
            {
                result = (holidaysMonth & Months.September) == Months.September;
                break;
            }
            case 10:
            {
                result = (holidaysMonth & Months.October) == Months.October;
                break;
            }
            case 11:
            {
                result = (holidaysMonth & Months.November) == Months.November;
                break;
            }
            case 12:
            {
                result = (holidaysMonth & Months.December) == Months.December;
                break;
            }
        }

        return result;
    }

    public static bool IsDateInWorkingDays(DateOnly date, WeekDays workingWeekDays)
    {
        var result = false;
        switch (date.DayOfWeek)
        {
            case DayOfWeek.Monday:
            {
                result = (workingWeekDays & WeekDays.Monday) == WeekDays.Monday;
                break;
            }
                ;
            case DayOfWeek.Tuesday:
            {
                result = (workingWeekDays & WeekDays.Tuesday) == WeekDays.Tuesday;
                break;
            }
                ;
            case DayOfWeek.Wednesday:
            {
                result = (workingWeekDays & WeekDays.Wednesday) == WeekDays.Wednesday;
                break;
            }
                ;
            case DayOfWeek.Thursday:
            {
                result = (workingWeekDays & WeekDays.Thursday) == WeekDays.Thursday;
                break;
            }
                ;
            case DayOfWeek.Friday:
            {
                result = (workingWeekDays & WeekDays.Friday) == WeekDays.Friday;
                break;
            }
                ;
            case DayOfWeek.Saturday:
            {
                result = (workingWeekDays & WeekDays.Saturday) == WeekDays.Saturday;
                break;
            }
                ;
            case DayOfWeek.Sunday:
            {
                result = (workingWeekDays & WeekDays.Sunday) == WeekDays.Sunday;
                break;
            }
                ;
        }

        return result;
    }
}