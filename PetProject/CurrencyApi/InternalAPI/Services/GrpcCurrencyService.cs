using CurrenciesDataAccessLibrary.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InternalAPI.Contracts;
using InternalAPI.Contracts.GrpcContracts;
using InternalAPI.Models;

namespace InternalAPI.Services
{
    /// <summary>
    /// Grpc-сервер, отвечающий на запросы Grpc-клиента.
    /// <inheritdoc cref="GrpcCurrency"/>
    /// </summary>
    public class GrpcCurrencyService : GrpcCurrency.GrpcCurrencyBase
    {
        private readonly IGettingApiInfoService _gettingApiInfoService;
        private readonly IGrpcMediumService _grpcMediumService;

        public GrpcCurrencyService(IGettingApiInfoService gettingApiInfoService,
                                   IGrpcMediumService grpcMediumService)
        {
            _gettingApiInfoService = gettingApiInfoService;
            _grpcMediumService = grpcMediumService;
        }

        public override async Task<ExchangeRate> GetCurrentExchangeRate(
            CurrencyInfo currencyInfo, ServerCallContext context)
        {
            ExchangeRateDTOModel exchangeRateDto =
                await _grpcMediumService.GetCurrentExchangeRateDtoAsync(
                    currencyInfo.CurrencyCode, context.CancellationToken);

            return MapDtoToExchangeRate(exchangeRateDto);
        }

        public override async Task<ExchangeRate> GetExchangeRateOnDate(
            CurrencyOnDateRequest currencyOnDate, ServerCallContext context)
        {
            ExchangeRateDTOModel exchangeRateDto = await _grpcMediumService
                .GetExchangeRateDtoOnDateAsync(currencyOnDate.CurrencyCode,
                                               currencyOnDate.Date.ToDateTime(),
                                               context.CancellationToken);

            return MapDtoToExchangeRate(exchangeRateDto);
        }

        public override async Task<ApiInfo> GetApiInfo(Empty emptyRequest, ServerCallContext context)
        {
            ApiInfoModel config = await _gettingApiInfoService
                .GetApiInfoAsync(context.CancellationToken);
            return new ApiInfo
            {
                BaseCurrency = config.BaseCurrency,
                IsRequestAvailable = config.NewRequestsAvailable,
            };
        }

        public override async Task<ExchangeRateWithBase> GetCurrentFavouriteExchangeRate(
            FavouriteInfo favouriteInfo, ServerCallContext context)
        {
            decimal exchangeRateValue = await _grpcMediumService
                .GetCurrentFavouriteExchangeRateAsync(favouriteInfo, context.CancellationToken);

            return new ExchangeRateWithBase
            {
                BaseCurrency = favouriteInfo.BaseCurrency,
                Currency = favouriteInfo.Currency,
                Value = exchangeRateValue,
            };
        }

        public override async Task<ExchangeRateWithBase> GetFavouriteExchangeRateOnDate(
            FavouriteOnDateRequest request, ServerCallContext context)
        {
            decimal exchangeRateValue = await _grpcMediumService
                .GetFavouriteExchangeRateOnDateAsync(request, context.CancellationToken);

            return new ExchangeRateWithBase
            {
                BaseCurrency = request.FavouriteInfo.BaseCurrency,
                Currency = request.FavouriteInfo.Currency,
                Value = exchangeRateValue,
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
    }
}
