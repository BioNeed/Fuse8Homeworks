using InternalAPI.Models;

namespace InternalAPI.Contracts
{
    public interface IExchangeRatesRepository
    {
        Task<CachedExchangeRates?> GetLastCachedExchangeRatesAsync(CancellationToken cancellationToken);

        Task<CachedExchangeRates?> GetHistoricalCacheDataAsync(
            DateOnly date,
            CancellationToken cancellationToken);

        Task SaveCacheDataAsync(string baseCurrency,
                                ExchangeRateDTOModel[] exchangeRates,
                                DateTime relevantOnDate,
                                CancellationToken cancellationToken);
    }
}