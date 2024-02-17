namespace Core.Contract.Enum;

[Flags]
public enum WeekDays
{
    NoDay = 0,
    Sunday = 1,
    Monday = 2,
    Tuesday = 4,
    Wednesday = 8,
    Thursday = 16,
    Friday = 32,
    Saturday = 64,
    EveryDay = Sunday | Monday | Tuesday | Wednesday | Thursday | Friday | Saturday
}