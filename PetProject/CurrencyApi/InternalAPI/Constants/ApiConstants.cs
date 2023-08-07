namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Constants
{
    public static class ApiConstants
    {
        public static class Uris
        {
            public const string GetStatus = "v3/status";
            public const string GetCurrency = "v3/latest?";
            public const string GetCurrencyHistorical = "/v3/historical?";
        }

        public static class ErrorMessages
        {
            public const string RequestLimitExceptionMessage =
                "Превышен лимит доступных запросов к Currencyapi API";

            public const string UnknownExceptionMessage =
                "Произошла непредвиденная ошибка";

            public const string InvalidDateFormatExceptionMessage =
                "Неверно указана дата";

            public const string InvalidCurrencyMessage =
                "The selected currencies is invalid.";
        }

        public static class ValidationRules
        {
            public const string DateFormat = "yyyy-MM-dd";
        }
    }
}
