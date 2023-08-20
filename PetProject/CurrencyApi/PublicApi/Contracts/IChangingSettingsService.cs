using Fuse8_ByteMinds.SummerSchool.PublicApi.Enums;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts
{
    public interface IChangingSettingsService
    {
        Task SetDefaultCurrencyAsync(CurrencyType currencyType, CancellationToken cancellationToken);
    }
}