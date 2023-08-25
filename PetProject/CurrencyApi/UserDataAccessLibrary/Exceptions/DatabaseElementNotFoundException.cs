namespace UserDataAccessLibrary.Exceptions
{
    public class DatabaseElementNotFoundException : Exception
    {
        public DatabaseElementNotFoundException(string message = "")
            : base(message)
        {
        }
    }
}
