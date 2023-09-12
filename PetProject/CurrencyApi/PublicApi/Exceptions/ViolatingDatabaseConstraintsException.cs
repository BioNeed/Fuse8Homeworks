namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при нарушении ограничений БД
    /// </summary>
    public class ViolatingDatabaseConstraintsException : Exception
    {
        /// <inheritdoc cref="ViolatingDatabaseConstraintsException"/>
        public ViolatingDatabaseConstraintsException(string message = "")
            : base(message)
        {
        }
    }
}
