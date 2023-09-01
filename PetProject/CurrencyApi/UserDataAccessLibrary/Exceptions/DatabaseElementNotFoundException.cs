namespace UserDataAccessLibrary.Exceptions
{
    /// <summary>
    /// Исключение, возникающее, если не найдена ожидаемая запись в таблице
    /// </summary>
    public class DatabaseElementNotFoundException : Exception
    {
        /// <inheritdoc cref="DatabaseElementNotFoundException"/>
        public DatabaseElementNotFoundException(string message = "")
            : base(message)
        {
        }
    }
}
