namespace Fuse8_ByteMinds.SummerSchool.Domain;

/// <summary>
/// Значения ресурсов для календаря
/// </summary>
public class CalendarResource
{
    public static readonly CalendarResource Instance = new ();

    public static readonly string January;
    public static readonly string February;

    private static readonly MonthInfo[] MonthInfos;

    static CalendarResource()
    {
        MonthInfos = new[]
        {
            new MonthInfo(Month.January, "Январь"),
            new MonthInfo(Month.February, "Февраль"),
            new MonthInfo(Month.March, "Март"),
            new MonthInfo(Month.April, "Апрель"),
            new MonthInfo(Month.May, "Май"),
            new MonthInfo(Month.June, "Июнь"),
            new MonthInfo(Month.July, "Июль"),
            new MonthInfo(Month.August, "Август"),
            new MonthInfo(Month.September, "Сентябрь"),
            new MonthInfo(Month.October, "Октябрь"),
            new MonthInfo(Month.November, "Ноябрь"),
            new MonthInfo(Month.December, "Декабрь"),
        };

        January = GetMonthByNumber(0);
        February = GetMonthByNumber(1);
    }

    public string this[Month month]
    {
        get
        {
            foreach (MonthInfo monthInfo in MonthInfos)
            {
                if (monthInfo.Month == month)
                {
                    return monthInfo.MonthName;
                }
            }

            throw new ArgumentOutOfRangeException(
                nameof(month),
                $"Month {month} is invalid");
        }
    }

    private static string GetMonthByNumber(int number)
        => MonthInfos[number].MonthName;
}

public enum Month
{
    January,
    February,
    March,
    April,
    May,
    June,
    July,
    August,
    September,
    October,
    November,
    December,
}