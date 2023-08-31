using CurrenciesDataAccessLibrary.Contracts;
using CurrenciesDataAccessLibrary.Database;
using CurrenciesDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrenciesDataAccessLibrary.Repositories
{
    public class ExchangeRatesRepository : IExchangeRatesRepository
    {
        private readonly CurrenciesDbContext _currenciesDbContext;

        public ExchangeRatesRepository(CurrenciesDbContext currenciesDbContext)
        {
            _currenciesDbContext = currenciesDbContext;
        }

        public Task<CachedExchangeRates?> GetLastCachedExchangeRatesAsync(CancellationToken cancellationToken)
        {
            return _currenciesDbContext.CachedExchangeRates.AsNoTracking()
                    .OrderByDescending(c => c.RelevantOnDate)
                    .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<CachedExchangeRates?> GetHistoricalCachedExchangeRatesAsync(
            DateOnly date,
            CancellationToken cancellationToken)
        {
            DateTime dateTime = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);

            return _currenciesDbContext.CachedExchangeRates.AsNoTracking()
                    .OrderByDescending(c => c.RelevantOnDate)
                    .FirstOrDefaultAsync(
                        c => c.RelevantOnDate.Date == dateTime.Date,
                        cancellationToken);
        }

        public async Task SaveCacheDataAsync(string baseCurrency,
                                             ExchangeRateDTOModel[] exchangeRates,
                                             DateTime relevantOnDate,
                                             CancellationToken cancellationToken)
        {
            CachedExchangeRates cachedExchangeRates = new CachedExchangeRates
            {
                BaseCurrency = baseCurrency,
                ExchangeRates = exchangeRates,
                RelevantOnDate = relevantOnDate,
            };

            await _currenciesDbContext.CachedExchangeRates
                .AddAsync(cachedExchangeRates, cancellationToken);

            await _currenciesDbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<CachedExchangeRates[]> GetAllCachedExchangeRatesAsync(
            CancellationToken cancellationToken)
        {
            return _currenciesDbContext.CachedExchangeRates.AsNoTracking()
                .ToArrayAsync(cancellationToken);
        }

        public async Task UpdateCachedExchangeRatesAsync(DateTime relevantOnDate,
                                                         string newBaseCurrency,
                                                         ExchangeRateDTOModel[] newExchangeRates,
                                                         CancellationToken cancellationToken)
        {
            CachedExchangeRates cachedExchangeRates =
                await _currenciesDbContext.CachedExchangeRates
                                .FirstAsync(c => c.RelevantOnDate == relevantOnDate,
                                            cancellationToken);

            cachedExchangeRates.BaseCurrency = newBaseCurrency;
            cachedExchangeRates.ExchangeRates = newExchangeRates;

            await _currenciesDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
