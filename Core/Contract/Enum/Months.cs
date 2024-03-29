namespace Core.Contract.Enum;

[Flags]
public enum Months
{
    NoMonth = 0,
    January = 1,
    February = 2,
    March = 4,
    April = 8,
    May = 16,
    June = 32,
    July = 64,
    August = 128,
    September = 256,
    October = 512,
    November = 1024,
    December = 2048,

    EveryMonth = January | February | March | April | May | June | July | August | September | October | November |
                 December
}