namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    public interface IRequestSender
    {
        Task<HttpResponseMessage> SendRequestAsync(string clientName, string requestPath);
    }
}