using CurrenciesDataAccessLibrary.Models;

namespace CurrenciesDataAccessLibrary.Contracts
{
    public interface IExchangeRatesRepository
    {
        Task<CachedExchangeRates?> GetLastCachedExchangeRatesAsync(CancellationToken cancellationToken);

        Task<CachedExchangeRates?> GetHistoricalCachedExchangeRatesAsync(
            DateOnly date,
            CancellationToken cancellationToken);

        Task SaveCacheDataAsync(string baseCurrency,
                                ExchangeRateDTOModel[] exchangeRates,
                                DateTime relevantOnDate,
                                CancellationToken cancellationToken);

        Task<CachedExchangeRates[]> GetAllCachedExchangeRatesAsync(CancellationToken cancellationToken);

        Task UpdateCachedExchangeRatesAsync(DateTime relevantOnDate, string newBaseCurrency, ExchangeRateDTOModel[] newExchangeRates, CancellationToken cancellationToken);
    }
}