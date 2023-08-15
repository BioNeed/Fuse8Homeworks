using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using InternalAPI.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts
{
    public interface ICurrencyService
    {
        Task<ExchangeRateModel> GetExchangeRateAsync(string currencyCode, CancellationToken cancellationToken);

        Task<ExchangeRateModel> GetExchangeRateOnDateTimeAsync(string currencyCode, DateTime dateTime, CancellationToken cancellationToken);

        Task<CurrencyConfigurationModel> GetSettingsAsync(CancellationToken cancellationToken);
    }
}