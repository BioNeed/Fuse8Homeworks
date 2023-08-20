using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.DataAccess;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Enums;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly UserDbContext _userDbContext;

        public SettingsRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<Settings> GetApplicationSettingsAsync(CancellationToken cancellationToken)
        {
            return await _userDbContext.Settings.AsNoTracking().FirstAsync(cancellationToken);
        }

        public async Task SetDefaultCurrencyAsync(CurrencyType newDefaultCurrency,
                                                  CancellationToken cancellationToken)
        {
            Settings settings = await _userDbContext.Settings.FirstAsync(cancellationToken);
            settings.DefaultCurrency = newDefaultCurrency;

            _userDbContext.SaveChanges();
        }

        public async Task SetCurrencyRoundCountAsync(int newRoundCount,
                                          CancellationToken cancellationToken)
        {
            Settings settings = await _userDbContext.Settings.FirstAsync(cancellationToken);
            settings.CurrencyRoundCount = newRoundCount;

            _userDbContext.SaveChanges();
        }
    }
}
