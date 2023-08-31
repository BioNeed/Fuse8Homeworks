using CurrenciesDataAccessLibrary.Enums;
using CurrenciesDataAccessLibrary.Models;

namespace CurrenciesDataAccessLibrary.Contracts
{
    public interface ICacheTasksRepository
    {
        Task AddCacheTaskAsync(Guid taskId, string newBaseCurrency, CancellationToken cancellationToken);
        Task<CacheTask[]> GetAllUncompletedTasksAsync(CancellationToken cancellationToken);
        Task<CacheTask> GetCacheTaskWithInfoAsync(Guid taskId, CancellationToken cancellationToken);

        Task SetCacheTaskStatusAsync(Guid taskId, CacheTaskStatus status, CancellationToken cancellationToken);
    }
}