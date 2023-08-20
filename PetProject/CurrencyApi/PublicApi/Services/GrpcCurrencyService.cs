using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts.GrpcContracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    public class GrpcCurrencyService : IGrpcCurrencyService
    {
        private readonly IOptionsSnapshot<CurrencyConfigurationModel> _configuration;
        private readonly GrpcCurrency.GrpcCurrencyClient _grpcCurrencyClient;

        public GrpcCurrencyService(GrpcCurrency.GrpcCurrencyClient grpcCurrencyClient, IOptionsSnapshot<CurrencyConfigurationModel> configuration)
        {
            _grpcCurrencyClient = grpcCurrencyClient;
            _configuration = configuration;
        }

        public async Task<ExchangeRateModel> GetExchangeRateAsync(string currencyCode, CancellationToken cancellationToken)
        {
            CurrencyInfo currencyInfo = new CurrencyInfo
            {
                CurrencyCode = currencyCode,
            };

            ExchangeRate exchangeRate = await _grpcCurrencyClient.GetCurrentExchangeRateAsync(
                currencyInfo,
                cancellationToken: cancellationToken);

            decimal roundedValue = decimal.Round(exchangeRate.Value, _configuration.Value.CurrencyRoundCount);
            return new ExchangeRateModel
            {
                Code = exchangeRate.Code,
                Value = roundedValue,
            };
        }

        public async Task<ExchangeRateModel> GetExchangeRateOnDateTimeAsync(
            string currencyCode,
            DateTime dateTime,
            CancellationToken cancellationToken)
        {
            CurrencyOnDateRequest request = new CurrencyOnDateRequest
            {
                CurrencyCode = currencyCode,
                Date = Timestamp.FromDateTime(dateTime),
            };

            ExchangeRate exchangeRate = await _grpcCurrencyClient
                .GetExchangeRateOnDateAsync(request, cancellationToken: cancellationToken);

            decimal roundedValue = decimal.Round(exchangeRate.Value, _configuration.Value.CurrencyRoundCount);
            return new ExchangeRateModel
            {
                Code = exchangeRate.Code,
                Value = roundedValue,
            };
        }

        public async Task<CurrencyConfigurationModel> GetSettingsAsync(CancellationToken cancellationToken)
        {
            ApiInfo apiInfo = await _grpcCurrencyClient.GetApiInfoAsync(new Empty(), cancellationToken: cancellationToken);

            return new CurrencyConfigurationModel
            {
                DefaultCurrency = _configuration.Value.DefaultCurrency,
                BaseCurrency = apiInfo.BaseCurrency,
                NewRequestsAvailable = apiInfo.IsRequestAvailable,
                CurrencyRoundCount = _configuration.Value.CurrencyRoundCount,
            };
        }
    }
}
