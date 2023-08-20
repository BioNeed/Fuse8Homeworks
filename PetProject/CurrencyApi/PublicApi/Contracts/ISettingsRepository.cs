using Fuse8_ByteMinds.SummerSchool.PublicApi.Enums;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts
{
    public interface ISettingsRepository
    {
        Task<Settings> GetApplicationSettingsAsync(CancellationToken cancellationToken);

        Task SetDefaultCurrencyAsync(CurrencyType newDefaultCurrency, CancellationToken cancellationToken);

        Task SetCurrencyRoundCountAsync(int newRoundCount, CancellationToken cancellationToken);
    }
}