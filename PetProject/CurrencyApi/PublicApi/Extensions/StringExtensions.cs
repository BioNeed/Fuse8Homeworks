namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNotNullAndNotEquals(this string checking, string value)
        {
            return checking != null &&
                checking.Equals(value, StringComparison.OrdinalIgnoreCase) == false;
        }
    }
}
