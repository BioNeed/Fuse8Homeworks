namespace Fuse8_ByteMinds.SummerSchool.Domain
{
    public readonly struct MonthInfo
    {
        public MonthInfo(Month month, string monthName)
        {
            Month = month;
            MonthName = monthName;
        }

        public Month Month { get; }

        public string MonthName { get; }
    }
}
