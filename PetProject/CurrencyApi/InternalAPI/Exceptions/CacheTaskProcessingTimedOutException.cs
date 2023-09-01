namespace InternalAPI.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при слишком долгой обработке задачи кэша
    /// </summary>
    public class CacheTaskProcessingTimedOutException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheBaseCurrencyNotFoundException"/> class.
        /// <inheritdoc cref="CacheBaseCurrencyNotFoundException"/>
        /// </summary>
        /// <param name="message">Сообщение с деталями об ошибке</param>
        public CacheTaskProcessingTimedOutException(string message = "")
            : base(message)
        {
        }
    }
}
