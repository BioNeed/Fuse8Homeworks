namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions.FindingExceptions
{
    public interface IResponseHandler
    {
        Task TryRaiseSpecificExceptionsAsync(HttpResponseMessage response);
    }
}
