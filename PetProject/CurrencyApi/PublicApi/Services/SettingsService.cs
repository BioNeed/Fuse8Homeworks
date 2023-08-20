using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Enums;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _settingsRepository;

        public SettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<Settings> GetApplicationSettings(CancellationToken cancellationToken)
        {
            return await _settingsRepository.GetApplicationSettings(cancellationToken);
        }

        public async Task SetDefaultCurrencyAsync(CurrencyType currencyType,
                                                  CancellationToken cancellationToken)
        {
            await _settingsRepository.SetDefaultCurrencyAsync(currencyType, cancellationToken);
        }
    }
}
