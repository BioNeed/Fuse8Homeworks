namespace InternalAPI.Exceptions
{
    public class CacheBaseCurrencyNotFoundException : Exception
    {
        public CacheBaseCurrencyNotFoundException(string message = "")
            : base(message)
        {
        }
    }
}
