using System.Text;

namespace Fuse8_ByteMinds.SummerSchool.Domain;

public static class DomainExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
    {
        if (collection == null || collection.Any() == false)
        {
            return true;
        }

        return false;
    }

    public static string JoinToString<T>(this IEnumerable<T> collection, string separator)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection), "Collection cannot be null");
        }

        return string.Join(separator, collection);
    }

    public static int DaysCountBetween(this DateTimeOffset dateTimeOffset, DateTimeOffset otherDateTimeOffset)
    {
        TimeSpan timeSpan = dateTimeOffset.UtcDateTime - otherDateTimeOffset.UtcDateTime;
        int output = timeSpan.Days;

        DateTimeOffset laterDateTimeOffset =
            dateTimeOffset.UtcDateTime > otherDateTimeOffset.UtcDateTime
            ? dateTimeOffset
            : otherDateTimeOffset;

        if (laterDateTimeOffset.UtcDateTime.TimeOfDay.TotalHours < timeSpan.TotalHours)
        {
            output++;
        }

        return output;
    }
}