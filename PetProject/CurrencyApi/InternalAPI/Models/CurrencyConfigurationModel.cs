namespace InternalAPI.Models
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
        /// Максимальное количество запросов к внешнему API
        /// </summary>
        public int RequestLimit { get; init; }

        /// <summary>
        /// Количество отправленных запросов к внешнему API
        /// </summary>
        public int RequestCount { get; init; }
    }
}
