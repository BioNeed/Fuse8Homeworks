namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    public interface ICheckingBeforeRequests
    {
        Task<bool> IsRequestAvailableAsync();
    }
}