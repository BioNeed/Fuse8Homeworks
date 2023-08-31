using Microsoft.EntityFrameworkCore;
using UserDataAccessLibrary.Contracts;
using UserDataAccessLibrary.Database;
using UserDataAccessLibrary.Models;

namespace UserDataAccessLibrary.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly UserDbContext _userDbContext;

        public SettingsRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public Task<Settings> GetApplicationSettingsAsync(CancellationToken cancellationToken)
        {
            return _userDbContext.Settings.AsNoTracking().FirstAsync(cancellationToken);
        }

        public async Task SetDefaultCurrencyAsync(string newDefaultCurrency,
                                                  CancellationToken cancellationToken)
        {
            Settings settings = await _userDbContext.Settings.FirstAsync(cancellationToken);
            settings.DefaultCurrency = newDefaultCurrency;

            await _userDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task SetCurrencyRoundCountAsync(int newRoundCount,
                                          CancellationToken cancellationToken)
        {
            Settings settings = await _userDbContext.Settings.FirstAsync(cancellationToken);
            settings.CurrencyRoundCount = newRoundCount;

            await _userDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
