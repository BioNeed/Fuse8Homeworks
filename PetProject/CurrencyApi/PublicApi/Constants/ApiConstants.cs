namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Constants
{
    public static class ApiConstants
    {
        public static class ErrorMessages
        {
            public const string UnknownExceptionMessage =
                "Произошла непредвиденная ошибка";

            public const string FavouriteNotFoundByNameExceptionMessage =
                "Избранное с указанным именем не найдено";
        }

        public static class Formats
        {
            public const string DateFormat = "yyyy-MM-dd";
        }

        public static class ConnectionStringNames
        {
            public const string SummerSchool = "SummerSchool";
        }

        public static class SchemaNames
        {
            public const string User = "user";
        }
    }
}
