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

        public async Task<CachedExchangeRates?> GetLastCachedExchangeRatesAsync(CancellationToken cancellationToken)
        {
            CachedExchangeRates cachedExchangeRates =
                await _currenciesDbContext.CachedExchangeRates.AsNoTracking()
                    .OrderByDescending(c => c.RelevantOnDate)
                    .FirstOrDefaultAsync(cancellationToken);

            return cachedExchangeRates;
        }

        public async Task<CachedExchangeRates?> GetHistoricalCacheDataAsync(
            DateOnly date,
            CancellationToken cancellationToken)
        {
            DateTime dateTime = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);

            CachedExchangeRates cachedExchangeRates =
                await _currenciesDbContext.CachedExchangeRates.AsNoTracking()
                    .OrderByDescending(c => c.RelevantOnDate)
                    .FirstOrDefaultAsync(
                        c => c.RelevantOnDate.Date == dateTime.Date,
                        cancellationToken);

            return cachedExchangeRates;
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
    
        public async Task<CachedExchangeRates[]> GetAllExchangeRatesWhere()
        {
            throw new NotImplementedException();
        }
    }
}
