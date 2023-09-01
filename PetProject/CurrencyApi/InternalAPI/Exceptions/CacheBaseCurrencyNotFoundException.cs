namespace InternalAPI.Exceptions
{
    /// <summary>
    /// Исключение, возникающее, когда не найдена базовая валюта кэша из таблицы
    /// </summary>
    public class CacheBaseCurrencyNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheBaseCurrencyNotFoundException"/> class.
        /// <inheritdoc cref="CacheBaseCurrencyNotFoundException"/>
        /// </summary>
        /// <param name="message">Сообщение с деталями об ошибке</param>
        public CacheBaseCurrencyNotFoundException(string message = "")
            : base(message)
        {
        }
    }
}
