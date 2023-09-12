namespace InternalAPI.Models
{
    /// <summary>
    /// Настройки приложения и доступные запросы
    /// </summary>
    public record ApiInfoModel
    {
        /// <summary>
        /// Базовая валюта
        /// </summary>
        public string BaseCurrency { get; init; }

        /// <summary>
        /// Есть ли ещё доступные запросы
        /// </summary>
        public bool NewRequestsAvailable { get; init; }
    }
}
