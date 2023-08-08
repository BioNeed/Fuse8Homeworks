using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;

namespace InternalAPI.Contracts
{
    public interface ICurrencyService
    {
        Task<ExchangeRateModel> GetExchangeRateAsync(
            CurrencyConfigurationModel currencyConfig,
            string? currencyCode);

        Task<ExchangeRateHistoricalModel> GetExchangeRateByDateAsync(
            CurrencyConfigurationModel currencyConfig,
            string currencyCode,
            string dateString);

        Task<CurrencyConfigurationModel> GetSettingsAsync(CurrencyConfigurationModel currencyConfig);
    }
}