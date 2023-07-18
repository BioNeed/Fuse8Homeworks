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

        if (collection.Any() == false)
        {
            return string.Empty;
        }

        StringBuilder outputBuilder = new StringBuilder(collection.Count());

        outputBuilder.Append(collection.First().ToString());

        foreach (T item in collection.Skip(1))
        {
            outputBuilder.Append(separator);
            outputBuilder.Append(item.ToString());
        }

        return outputBuilder.ToString();
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