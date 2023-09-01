namespace CurrenciesDataAccessLibrary.Enums
{
    /// <summary>
    /// Статус задачи кэша
    /// </summary>
    public enum CacheTaskStatus
    {
        /// <summary>
        /// Создана
        /// </summary>
        Created,

        /// <summary>
        /// В обработке
        /// </summary>
        Processing,

        /// <summary>
        /// Завершена успешно
        /// </summary>
        CompletedSuccessfully,

        /// <summary>
        /// Завершена с ошибкой
        /// </summary>
        CompletedWithError,

        /// <summary>
        /// Отменена
        /// </summary>
        Cancelled,
    }
}
