using InternalAPI.Models;

namespace InternalAPI.Contracts;

public interface IGettingApiConfigService
{
    Task<ApiInfoModel> GetApiInfoAsync(CancellationToken cancellationToken);
}
