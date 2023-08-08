namespace InternalAPI.Exceptions
{
    public class InvalidDateFormatException : Exception
    {
        public InvalidDateFormatException(string message = "")
            : base(message)
        {
        }
    }
}
