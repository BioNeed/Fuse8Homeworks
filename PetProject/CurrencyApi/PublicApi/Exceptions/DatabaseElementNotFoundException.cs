﻿namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions
{
    public class DatabaseElementNotFoundException : Exception
    {
        public DatabaseElementNotFoundException(string message = "")
            : base(message)
        {
        }
    }
}
