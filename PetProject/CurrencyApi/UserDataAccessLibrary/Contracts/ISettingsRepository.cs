using UserDataAccessLibrary.Models;

namespace UserDataAccessLibrary.Contracts
{
    public interface ISettingsRepository
    {
        Task<Settings> GetApplicationSettingsAsync(CancellationToken cancellationToken);

        Task SetDefaultCurrencyAsync(string newDefaultCurrency, CancellationToken cancellationToken);

        Task SetCurrencyRoundCountAsync(int newRoundCount, CancellationToken cancellationToken);
    }
}