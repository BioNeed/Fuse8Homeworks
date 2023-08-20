using Fuse8_ByteMinds.SummerSchool.PublicApi.Enums;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts
{
    public interface ISettingsRepository
    {
        Task SetDefaultCurrencyAsync(CurrencyType newDefaultCurrency, CancellationToken cancellationToken);
    }
}