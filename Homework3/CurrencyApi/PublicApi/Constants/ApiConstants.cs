namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Constants
{
    public static class ApiConstants
    {
        public static class Uris
        {
            public const string BaseAddress = "Uris:BaseAddress";
            public const string GetSettings = "/settings";
            public const string GetStatus = "v3/status";
            public const string GetCurrency = "v3/latest?";
            public const string GetCurrencyHistorical = "/v3/historical?";
        }

        public static class HttpClientsNames
        {
            public const string CurrencyApi = "CurrencyApiClient";
        }

        public static class ApiKeys
        {
            public const string Default = "APIKeys:Default";
        }

        public static class ErrorMessages
        {
            public const string RequestLimitExceptionMessage =
                "Превышен лимит доступных запросов к Currencyapi API";

            public const string UnknownExceptionMessage =
                "Произошла непредвиденная ошибка";
        }

        public static class ValidationRules
        {
            public const string DateFormat = "yyyy-MM-dd";
        }
    }
}
