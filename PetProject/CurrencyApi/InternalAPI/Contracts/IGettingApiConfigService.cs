using InternalAPI.Models;

namespace InternalAPI.Contracts;

public interface IGettingApiConfigService
{
    Task<ApiInfoModel> GetApiConfigAsync(CancellationToken cancellationToken);
}
