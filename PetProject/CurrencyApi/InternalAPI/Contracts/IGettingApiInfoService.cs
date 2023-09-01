using InternalAPI.Models;

namespace InternalAPI.Contracts;

/// <summary>
/// Сервис, получающий информацию об InternalApi
/// </summary>
public interface IGettingApiInfoService
{
    /// <summary>
    /// Получить информацию об InternalApi
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Инфо об InternalApi</returns>
    Task<ApiInfoModel> GetApiInfoAsync(CancellationToken cancellationToken);
}
