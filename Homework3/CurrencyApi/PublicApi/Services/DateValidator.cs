using System.Globalization;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Constants;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    public class DateValidator
    {
        public bool IsDateValid(string dateString)
        {
            if (DateTime.TryParseExact(dateString,
                ApiConstants.ValidationRules.DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _) == false)
            {
                return false;
            }

            return true;
        }
    }
}
