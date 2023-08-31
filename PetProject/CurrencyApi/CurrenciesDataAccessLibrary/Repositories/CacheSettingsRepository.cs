using CurrenciesDataAccessLibrary.Contracts;
using CurrenciesDataAccessLibrary.Database;
using CurrenciesDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrenciesDataAccessLibrary.Repositories
{
    public class CacheSettingsRepository : ICacheSettingsRepository
    {
        private readonly CurrenciesDbContext _currenciesDbContext;

        public CacheSettingsRepository(CurrenciesDbContext currenciesDbContext)
        {
            _currenciesDbContext = currenciesDbContext;
        }

        public CacheSettings GetCacheSettings()
        {
            return _currenciesDbContext.CacheSettings.AsNoTracking().First();
        }

        public Task<CacheSettings> GetCacheSettingsAsync(CancellationToken cancellationToken = default)
        {
            return _currenciesDbContext.CacheSettings.AsNoTracking().FirstAsync(cancellationToken);
        }

        public async Task SetBaseCurrencyAsync(string newBaseCurrency,
                                               CancellationToken cancellationToken)
        {
            CacheSettings settings = await _currenciesDbContext.CacheSettings.FirstAsync(cancellationToken);
            settings.BaseCurrency = newBaseCurrency;

            await _currenciesDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
