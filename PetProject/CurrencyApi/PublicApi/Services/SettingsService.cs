using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Enums;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    public class SettingsService : IChangingSettingsService
    {
        private readonly ISettingsRepository _settingsRepository;

        public SettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task SetDefaultCurrencyAsync(CurrencyType currencyType,
                                                  CancellationToken cancellationToken)
        {
            await _settingsRepository.SetDefaultCurrencyAsync(currencyType, cancellationToken);
        }
    }
}
