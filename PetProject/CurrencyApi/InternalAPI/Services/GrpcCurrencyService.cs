using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InternalAPI.Contracts;
using InternalAPI.Contracts.GrpcContracts;
using InternalAPI.Enums;
using InternalAPI.Models;

namespace InternalAPI.Services
{
    public class GrpcCurrencyService : GrpcCurrency.GrpcCurrencyBase
    {
        private readonly IGettingApiConfigService _gettingApiConfigService;
        private readonly ICachedCurrencyAPI _cachedCurrencyAPI;

        public GrpcCurrencyService(IGettingApiConfigService gettingApiConfigService,
                                   ICachedCurrencyAPI cachedCurrencyAPI)
        {
            _gettingApiConfigService = gettingApiConfigService;
            _cachedCurrencyAPI = cachedCurrencyAPI;
        }

        public override async Task<ExchangeRate> GetCurrentExchangeRate(CurrencyInfo currencyInfo, ServerCallContext context)
        {
            CurrencyType currencyType = System.Enum.Parse<CurrencyType>(currencyInfo.CurrencyCode, true);
            ExchangeRateDTOModel exchangeRateDto = await _cachedCurrencyAPI
                .GetCurrentExchangeRateAsync(currencyType, context.CancellationToken);

            return MapDtoToExchangeRate(exchangeRateDto);
        }

        public override async Task<ExchangeRate> GetExchangeRateOnDate(CurrencyOnDateRequest currencyOnDate, ServerCallContext context)
        {
            CurrencyType currencyType = System.Enum.Parse<CurrencyType>(currencyOnDate.CurrencyCode, true);
            DateOnly date = DateOnly.FromDateTime(currencyOnDate.Date.ToDateTime());

            ExchangeRateDTOModel exchangeRateDto = await _cachedCurrencyAPI
                .GetExchangeRateOnDateAsync(currencyType, date, context.CancellationToken);

            return MapDtoToExchangeRate(exchangeRateDto);
        }

        public override async Task<ApiInfo> GetApiInfo(Empty emptyRequest, ServerCallContext context)
        {
            ApiInfoModel config = await _gettingApiConfigService
                .GetApiConfigAsync(context.CancellationToken);
            return new ApiInfo
            {
                BaseCurrency = config.BaseCurrency,
                IsRequestAvailable = config.NewRequestsAvailable,
            };
        }

        private ExchangeRate MapDtoToExchangeRate(ExchangeRateDTOModel exchangeRateDTO)
        {
            return new ExchangeRate
            {
                Code = exchangeRateDTO.CurrencyType.ToString(),
                Value = exchangeRateDTO.Value,
            };
        }
    }
}
