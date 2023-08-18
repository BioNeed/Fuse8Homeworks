using Grpc.Net.Client.Balancer;

namespace InternalAPI.Constants
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
        }

        public static class Formats
        {
            public const string DateFormat = "yyyy-MM-dd";
            public const string FileNameDateTimeFormat = "yyyy-MM-dd HH-mm-ss";
        }

        public static class RelativePaths
        {
            public const string CachedCurrenciesOnDates = @"/Cache/CachedCurrenciesOnDates";
        }

        public static class HttpClientNames
        {
            public const string Default = "CurrencyApiClient";
        }

        public static class PortNames
        {
            public const string GrpcPort = "GrpcPort";
            public const string RestApiPort = "RestApiPort";
        }

        public static class ConnectionStringNames
        {
            public const string SummerSchool = "SummerSchool";
        }

        public static class SchemaNames
        {
            public const string Currencies = "cur";
        }
    }
}
