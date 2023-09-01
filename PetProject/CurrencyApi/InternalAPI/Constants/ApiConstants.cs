namespace InternalAPI.Constants
{
    /// <summary>
    /// Класс строковых констант InternalApi
    /// </summary>
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

            public const string CacheBaseCurrencyNotFoundExceptionMessage =
                "Не найдена указанная базовая валюта. Проверьте BaseCurrency в appsettings.json";

            public const string CacheTaskProcessingTimedOut =
                "Фоновая задача кэша выполнялась слишком долго";
        }

        public static class Formats
        {
            public const string DateFormat = "yyyy-MM-dd";
        }

        public static class HttpClientNames
        {
            public const string CurrencyApi = "CurrencyApiClient";
        }

        public static class PortNames
        {
            public const string GrpcPort = "GrpcPort";
            public const string RestApiPort = "RestApiPort";
        }

        public static class ConnectionStringNames
        {
            public const string SummerSchool = "SummerSchool";
            public const string DockerSummerSchool = "DockerSummerSchool";
        }
    }
}
