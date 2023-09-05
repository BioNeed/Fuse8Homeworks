using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Enums;
using UserDataAccessLibrary.Contracts;
using UserDataAccessLibrary.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    /// <inheritdoc cref="ISettingsService"/>
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

        public Task SetDefaultCurrencyAsync(CurrencyType currencyType,
                                                  CancellationToken cancellationToken)
        {
            return _settingsRepository.SetDefaultCurrencyAsync(currencyType.ToString(),
                                                               cancellationToken);
        }

        public Task SetCurrencyRoundCountAsync(int newRoundCount,
                                          CancellationToken cancellationToken)
        {
            return _settingsRepository.SetCurrencyRoundCountAsync(newRoundCount, cancellationToken);
        }
    }
}
