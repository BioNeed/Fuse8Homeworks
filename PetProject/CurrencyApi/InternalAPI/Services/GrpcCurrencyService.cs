using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InternalAPI.Contracts;
using InternalAPI.Contracts.GrpcContracts;
using InternalAPI.Enums;
using InternalAPI.Models;
using System.Threading;

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

        public override async Task<ExchangeRate> GetCurrentExchangeRate(
            CurrencyInfo currencyInfo, ServerCallContext context)
        {
            CurrencyType currencyType = System.Enum.Parse<CurrencyType>(currencyInfo.CurrencyCode, true);
            ExchangeRateDTOModel exchangeRateDto = await _cachedCurrencyAPI
                .GetCurrentExchangeRateAsync(currencyType, context.CancellationToken);

            return MapDtoToExchangeRate(exchangeRateDto);
        }

        public override async Task<ExchangeRate> GetExchangeRateOnDate(
            CurrencyOnDateRequest currencyOnDate, ServerCallContext context)
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

        public override async Task<ExchangeRateWithBase> GetCurrentFavouriteExchangeRate(
            FavouriteInfo favouriteInfo, ServerCallContext context)
        {
            decimal favouriteExchangeRate = await
                GetCurrentExchangeRateValueFromCacheAsync(favouriteInfo.Currency,
                                                          context.CancellationToken);

            if (_cachedCurrencyAPI.BaseCurrency.Equals(
                    favouriteInfo.BaseCurrency, StringComparison.OrdinalIgnoreCase))
            {
                return new ExchangeRateWithBase
                {
                    BaseCurrency = favouriteInfo.BaseCurrency,
                    Currency = favouriteInfo.Currency,
                    Value = favouriteExchangeRate,
                };
            }

            decimal favouriteBaseExchangeRate = await
               GetCurrentExchangeRateValueFromCacheAsync(favouriteInfo.BaseCurrency,
                                                         context.CancellationToken);

            decimal resultExchangeRate = favouriteExchangeRate
                                         / favouriteBaseExchangeRate;

            return new ExchangeRateWithBase
            {
                BaseCurrency = favouriteInfo.BaseCurrency,
                Currency = favouriteInfo.Currency,
                Value = resultExchangeRate,
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

        private async Task<decimal> GetCurrentExchangeRateValueFromCacheAsync(
            string currency,
            CancellationToken cancellationToken)
        {
            CurrencyType currencyType = System.Enum.Parse<CurrencyType>(
                currency, ignoreCase: true);

            ExchangeRateDTOModel exchangeRate = await _cachedCurrencyAPI
                .GetCurrentExchangeRateAsync(currencyType,
                                             cancellationToken);

            return exchangeRate.Value;
        }
    }
}
