using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Enums;
using UserDataAccessLibrary.Contracts;
using UserDataAccessLibrary.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _settingsRepository;

        public SettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public Task<Settings> GetApplicationSettingsAsync(CancellationToken cancellationToken)
        {
            return _settingsRepository.GetApplicationSettingsAsync(cancellationToken);
        }

        public async Task SetDefaultCurrencyAsync(CurrencyType currencyType,
                                                  CancellationToken cancellationToken)
        {
            await _settingsRepository.SetDefaultCurrencyAsync(currencyType.ToString(),
                                                              cancellationToken);
        }

        public async Task SetCurrencyRoundCountAsync(int newRoundCount,
                                          CancellationToken cancellationToken)
        {
            await _settingsRepository.SetCurrencyRoundCountAsync(newRoundCount, cancellationToken);
        }
    }
}
