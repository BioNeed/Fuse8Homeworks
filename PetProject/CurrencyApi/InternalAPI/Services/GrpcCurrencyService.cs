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

        public override async Task<ExchangeRate> GetCurrentExchangeRate(
            CurrencyInfo currencyInfo, ServerCallContext context)
        {
            ExchangeRateDTOModel exchangeRateDto =
                await GetCurrentExchangeRateDtoAsync(currencyInfo.CurrencyCode,
                                                     context.CancellationToken);

            return MapDtoToExchangeRate(exchangeRateDto);
        }

        public override async Task<ExchangeRate> GetExchangeRateOnDate(
            CurrencyOnDateRequest currencyOnDate, ServerCallContext context)
        {
            ExchangeRateDTOModel exchangeRateDto = await GetExchangeRateDtoOnDateAsync(
                currencyOnDate.CurrencyCode,
                currencyOnDate.Date.ToDateTime(),
                context.CancellationToken);

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
            ExchangeRateDTOModel favouriteExchangeRate = await
                GetCurrentExchangeRateDtoAsync(favouriteInfo.Currency,
                                                          context.CancellationToken);

            if (_cachedCurrencyAPI.BaseCurrency.Equals(
                    favouriteInfo.BaseCurrency, StringComparison.OrdinalIgnoreCase))
            {
                return new ExchangeRateWithBase
                {
                    BaseCurrency = favouriteInfo.BaseCurrency,
                    Currency = favouriteInfo.Currency,
                    Value = favouriteExchangeRate.Value,
                };
            }

            ExchangeRateDTOModel favouriteBaseExchangeRate = await
               GetCurrentExchangeRateDtoAsync(favouriteInfo.BaseCurrency,
                                                         context.CancellationToken);

            decimal resultExchangeRate = favouriteExchangeRate.Value
                                         / favouriteBaseExchangeRate.Value;

            return new ExchangeRateWithBase
            {
                BaseCurrency = favouriteInfo.BaseCurrency,
                Currency = favouriteInfo.Currency,
                Value = resultExchangeRate,
            };
        }

        public async override Task<ExchangeRateWithBase> GetFavouriteExchangeRateOnDate(
            FavouriteOnDateRequest request, ServerCallContext context)
        {
            DateTime requestDateTime = request.Date.ToDateTime();
            ExchangeRateDTOModel favouriteExchangeRate = await
                GetExchangeRateDtoOnDateAsync(request.FavouriteInfo.Currency,
                                              requestDateTime,
                                              context.CancellationToken);

            if (_cachedCurrencyAPI.BaseCurrency.Equals(
                    request.FavouriteInfo.BaseCurrency, StringComparison.OrdinalIgnoreCase))
            {
                return new ExchangeRateWithBase
                {
                    BaseCurrency = request.FavouriteInfo.BaseCurrency,
                    Currency = request.FavouriteInfo.Currency,
                    Value = favouriteExchangeRate.Value,
                };
            }

            ExchangeRateDTOModel favouriteBaseExchangeRate = await
                GetExchangeRateDtoOnDateAsync(request.FavouriteInfo.BaseCurrency,
                                              requestDateTime,
                                              context.CancellationToken);

            decimal resultExchangeRate = favouriteExchangeRate.Value
                                         / favouriteBaseExchangeRate.Value;

            return new ExchangeRateWithBase
            {
                BaseCurrency = request.FavouriteInfo.BaseCurrency,
                Currency = request.FavouriteInfo.Currency,
                Value = resultExchangeRate,
            };
        }

        private ExchangeRate MapDtoToExchangeRate(ExchangeRateDTOModel exchangeRateDTO)
        {
            return new ExchangeRate
            {
                Code = exchangeRateDTO.Code.ToString(),
                Value = exchangeRateDTO.Value,
            };
        }

        private async Task<ExchangeRateDTOModel> GetCurrentExchangeRateDtoAsync(
            string currency,
            CancellationToken cancellationToken)
        {
            CurrencyType currencyType = System.Enum.Parse<CurrencyType>(
                currency, ignoreCase: true);

            return await _cachedCurrencyAPI.GetCurrentExchangeRateAsync(
                currencyType,
                cancellationToken);
        }

        private async Task<ExchangeRateDTOModel> GetExchangeRateDtoOnDateAsync(
            string currency,
            DateTime dateTime,
            CancellationToken cancellationToken)
        {
            CurrencyType currencyType = System.Enum.Parse<CurrencyType>(
                currency, ignoreCase: true);

            DateOnly date = DateOnly.FromDateTime(dateTime);

            return await _cachedCurrencyAPI.GetExchangeRateOnDateAsync(
                currencyType,
                date,
                cancellationToken);
        }
    }
}
