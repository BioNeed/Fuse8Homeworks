﻿namespace InternalAPI.Exceptions
{
    public class CurrencyNotFoundException : Exception
    {
        public CurrencyNotFoundException(string message = "")
            : base(message)
        {
        }
    }
}
