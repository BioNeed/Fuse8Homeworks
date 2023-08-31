namespace InternalAPI.Models
{
    /// <summary>
    /// Настройки API
    /// </summary>
    public record ApiSettingsModel
    {
        /// <summary>
        /// Базовый адрес для работы с внешним API
        /// </summary>
        public string BaseAddress { get; init; }

        /// <summary>
        /// Токен, по которому есть доступ к внешнему API
        /// </summary>
        public string ApiKey { get; init; }

        /// <summary>
        /// Время устаревания кэша (в часах)
        /// </summary>
        public int CacheExpirationTimeInHours { get; init; }

        /// <summary>
        /// Время ожидания обработки задачи (в секундах)
        /// </summary>
        public int WaitingTimeForTaskProcessingInSeconds { get; init; }
    }
}
