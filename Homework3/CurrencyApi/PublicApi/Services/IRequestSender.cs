namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    public interface IRequestSender
    {
        Task<string> SendRequestAsync(string clientName, string requestPath);
    }
}