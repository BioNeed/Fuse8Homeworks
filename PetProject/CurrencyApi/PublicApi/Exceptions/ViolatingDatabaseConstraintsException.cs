namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions
{
    public class ViolatingDatabaseConstraintsException : Exception
    {
        public ViolatingDatabaseConstraintsException(string message = "")
            : base(message)
        {
        }
    }
}
