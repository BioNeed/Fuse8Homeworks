using CurrenciesDataAccessLibrary.Enums;

namespace CurrenciesDataAccessLibrary.Repositories
{
    public interface ICacheTasksRepository
    {
        Task AddCacheTaskAsync(Guid taskId, string newBaseCurrency, CancellationToken cancellationToken);

        Task SetCacheTaskStatusAsync(Guid taskId, CacheTaskStatus status, CancellationToken cancellationToken);
    }
}