using CurrenciesDataAccessLibrary.Models;

namespace CurrenciesDataAccessLibrary.Contracts
{
    public interface ICacheSettingsRepository
    {
        CacheSettings GetCacheSettings();

        Task<CacheSettings> GetCacheSettingsAsync(CancellationToken cancellationToken = default);

        Task SetBaseCurrencyAsync(string newBaseCurrency, CancellationToken cancellationToken);
    }
}