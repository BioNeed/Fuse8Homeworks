namespace Fuse8_ByteMinds.SummerSchool.Domain;

/// <summary>
/// Значения ресурсов для календаря
/// </summary>
public class CalendarResource
{
    public static readonly CalendarResource Instance = new ();

    public static readonly string January = GetMonthByNumber(0);
    public static readonly string February = GetMonthByNumber(1);

    private static readonly string[] MonthNames;

    static CalendarResource()
    {
        MonthNames = new[]
        {
            "Январь",
            "Февраль",
            "Март",
            "Апрель",
            "Май",
            "Июнь",
            "Июль",
            "Август",
            "Сентябрь",
            "Октябрь",
            "Ноябрь",
            "Декабрь",
        };
    }

    public string this[Month month]
    {
        get
        {
            return MonthNames[(int)month];
        }
    }

    private static string GetMonthByNumber(int number)
        => MonthNames[number];
}

public enum Month
{
    January = 0,
    February = 1,
    March = 2,
    April = 3,
    May = 4,
    June = 5,
    July = 6,
    August = 7,
    September = 8,
    October = 9,
    November = 10,
    December = 11,
}