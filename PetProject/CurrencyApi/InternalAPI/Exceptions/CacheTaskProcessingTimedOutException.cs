namespace InternalAPI.Exceptions
{
    public class CacheTaskProcessingTimedOutException : Exception
    {
        public CacheTaskProcessingTimedOutException(string message = "")
            : base(message)
        {
        }
    }
}
