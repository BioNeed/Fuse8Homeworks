namespace InternalAPI.Exceptions
{
    /// <summary>
    /// Исключение, возникающие при превышении допустимого количества запросов
    /// </summary>
    public class ApiRequestLimitException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestLimitException"/> class.
        /// <inheritdoc cref="ApiRequestLimitException"/>
        /// </summary>
        /// <param name="message">Сообщение с деталями об ошибке</param>
        public ApiRequestLimitException(string message = "")
            : base(message)
        {
        }
    }
}
