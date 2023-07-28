namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services.FindingExceptions
{
    public interface IResponseHandler
    {
        Task RaiseExceptionAsync(HttpResponseMessage response);
    }
}
