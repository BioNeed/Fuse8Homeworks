using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InternalAPI.Constants;
using InternalAPI.Contracts;
using InternalAPI.Contracts.GrpcContracts;
using InternalAPI.Enums;
using InternalAPI.Exceptions;
using InternalAPI.Models;
using Microsoft.Extensions.Options;

namespace InternalAPI.Services
{
    public class GrpcCurrencyService : GrpcCurrency.GrpcCurrencyBase
    {
        private readonly IGettingApiConfigService _gettingApiConfigService;
        private readonly ICachedCurrencyAPI _cachedCurrencyAPI;
        private readonly IOptionsSnapshot<CurrencyConfigurationModel> _apiConfig;

        public GrpcCurrencyService(IGettingApiConfigService gettingApiConfigService,
                                   ICachedCurrencyAPI cachedCurrencyAPI,
                                   IOptionsSnapshot<CurrencyConfigurationModel> apiConfig)
        {
            _gettingApiConfigService = gettingApiConfigService;
            _cachedCurrencyAPI = cachedCurrencyAPI;
            _apiConfig = apiConfig;
        }

        public override async Task<ExchangeRate> GetCurrentExchangeRate(CurrencyInfo currencyInfo, ServerCallContext context)
        {
            ApiInfo apiInfo = await GetApiInfo(new Empty(), context);

            if (apiInfo.IsRequestAvailable == false)
            {
                throw new ApiRequestLimitException(ApiConstants.ErrorMessages.RequestLimitExceptionMessage);
            }

            CurrencyType currencyType = System.Enum.Parse<CurrencyType>(currencyInfo.CurrencyCode);
            ExchangeRateDTOModel exchangeRateDto = await _cachedCurrencyAPI
                .GetCurrentExchangeRateAsync(currencyType, context.CancellationToken);

            return MapDtoToExchangeRate(exchangeRateDto);
        }

        public override async Task<ExchangeRate> GetExchangeRateOnDate(CurrencyOnDateRequest currencyOnDate, ServerCallContext context)
        {
            ApiInfo apiInfo = await GetApiInfo(new Empty(), context);

            if (apiInfo.IsRequestAvailable == false)
            {
                throw new ApiRequestLimitException(ApiConstants.ErrorMessages.RequestLimitExceptionMessage);
            }

            CurrencyType currencyType = System.Enum.Parse<CurrencyType>(currencyOnDate.CurrencyCode);
            DateOnly date = DateOnly.FromDateTime(currencyOnDate.Date.ToDateTime());

            ExchangeRateDTOModel exchangeRateDto = await _cachedCurrencyAPI
                .GetExchangeRateOnDateAsync(currencyType, date, context.CancellationToken);

            return MapDtoToExchangeRate(exchangeRateDto);
        }

        public override async Task<ApiInfo> GetApiInfo(Empty emptyRequest, ServerCallContext context)
        {
            CurrencyConfigurationModel fullApiSettings = await _gettingApiConfigService
                .GetApiConfigAsync(_apiConfig.Value, context.CancellationToken);
            return new ApiInfo
            {
                BaseCurrency = fullApiSettings.BaseCurrency,
                IsRequestAvailable = fullApiSettings.RequestCount < fullApiSettings.RequestLimit,
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
