namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models
{
    /// <summary>
    /// Настройки приложения и доступные запросы
    /// </summary>
    public record CurrencyConfigurationModel
    {
        /// <summary>
        /// Валюта по умолчанию
        /// </summary>
        public string DefaultCurrency { get; init; }

        /// <summary>
        /// Базовая валюта
        /// </summary>
        public string BaseCurrency { get; init; }

        /// <summary>
        /// Количество знаков после запятой у курса валют
        /// </summary>
        public int CurrencyRoundCount { get; init; }

        /// <summary>
        /// Есть ли ещё доступные запросы
        /// </summary>
        public bool NewRequestsAvailable { get; init; }
    }
}
