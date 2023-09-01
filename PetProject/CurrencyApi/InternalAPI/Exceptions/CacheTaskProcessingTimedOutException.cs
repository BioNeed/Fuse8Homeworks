namespace InternalAPI.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при слишком долгой обработке задачи кэша
    /// </summary>
    public class CacheTaskProcessingTimedOutException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheTaskProcessingTimedOutException"/> class.
        /// <inheritdoc cref="CacheTaskProcessingTimedOutException"/>
        /// </summary>
        /// <param name="message">Сообщение с деталями об ошибке</param>
        public CacheTaskProcessingTimedOutException(string message = "")
            : base(message)
        {
        }
    }
}
