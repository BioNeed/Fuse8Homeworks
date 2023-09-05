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
        Created = 0,

        /// <summary>
        /// В обработке
        /// </summary>
        Processing = 1,

        /// <summary>
        /// Завершена успешно
        /// </summary>
        CompletedSuccessfully = 2,

        /// <summary>
        /// Завершена с ошибкой
        /// </summary>
        CompletedWithError = 3,

        /// <summary>
        /// Отменена
        /// </summary>
        Cancelled = 4,
    }
}
