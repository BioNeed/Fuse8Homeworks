using CurrenciesDataAccessLibrary.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InternalAPI.Contracts;
using InternalAPI.Contracts.GrpcContracts;
using InternalAPI.Exceptions;
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
        private readonly ILogger<GrpcCurrencyService> _logger;

        public GrpcCurrencyService(IGettingApiInfoService gettingApiInfoService,
                                   IGrpcMediumService grpcMediumService,
                                   ILogger<GrpcCurrencyService> logger)
        {
            _gettingApiInfoService = gettingApiInfoService;
            _grpcMediumService = grpcMediumService;
            _logger = logger;
        }

        public override async Task<ExchangeRate> GetCurrentExchangeRate(
            CurrencyInfo currencyInfo, ServerCallContext context)
        {
            try
            {
                ExchangeRateDTOModel exchangeRateDto =
                    await _grpcMediumService.GetCurrentExchangeRateDtoAsync(
                        currencyInfo.CurrencyCode, context.CancellationToken);

                return MapDtoToExchangeRate(exchangeRateDto);
            }
            catch (ApiRequestLimitException ex)
            {
                WrapToRpcException(ex.Message, StatusCode.ResourceExhausted);
            }
            catch (Exception ex)
            {
                WrapToRpcException(ex.Message);
            }

            return null;
        }

        public override async Task<ExchangeRate> GetExchangeRateOnDate(
            CurrencyOnDateRequest currencyOnDate, ServerCallContext context)
        {
            try
            {
                ExchangeRateDTOModel exchangeRateDto = await _grpcMediumService
                    .GetExchangeRateDtoOnDateAsync(currencyOnDate.CurrencyCode,
                                                   currencyOnDate.Date.ToDateTime(),
                                                   context.CancellationToken);

                return MapDtoToExchangeRate(exchangeRateDto);
            }
            catch (ApiRequestLimitException ex)
            {
                WrapToRpcException(ex.Message, StatusCode.ResourceExhausted);
            }
            catch (Exception ex)
            {
                WrapToRpcException(ex.Message);
            }

            return null;
        }

        public override async Task<ApiInfo> GetApiInfo(Empty emptyRequest, ServerCallContext context)
        {
            try
            {
                ApiInfoModel config = await _gettingApiInfoService
                    .GetApiInfoAsync(context.CancellationToken);
                return new ApiInfo
                {
                    BaseCurrency = config.BaseCurrency,
                    IsRequestAvailable = config.NewRequestsAvailable,
                };
            }
            catch (Exception ex)
            {
                WrapToRpcException(ex.Message);
            }

            return null;
        }

        public override async Task<ExchangeRateWithBase> GetCurrentFavouriteExchangeRate(
            FavouriteInfo favouriteInfo, ServerCallContext context)
        {
            try
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
            catch (ApiRequestLimitException ex)
            {
                WrapToRpcException(ex.Message, StatusCode.ResourceExhausted);
            }
            catch (Exception ex)
            {
                WrapToRpcException(ex.Message);
            }

            return null;
        }

        public override async Task<ExchangeRateWithBase> GetFavouriteExchangeRateOnDate(
            FavouriteOnDateRequest request, ServerCallContext context)
        {
            try
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
            catch (ApiRequestLimitException ex)
            {
                WrapToRpcException(ex.Message, StatusCode.ResourceExhausted);
            }
            catch (Exception ex)
            {
                WrapToRpcException(ex.Message);
            }

            return null;
        }

        private ExchangeRate MapDtoToExchangeRate(ExchangeRateDTOModel exchangeRateDTO)
        {
            return new ExchangeRate
            {
                Code = exchangeRateDTO.Code.ToString(),
                Value = exchangeRateDTO.Value,
            };
        }

        private void WrapToRpcException(string message, StatusCode rpcStatusCode = StatusCode.Internal)
        {
            _logger.LogError("Ошибка! {message}", message);
            throw new RpcException(new Status(rpcStatusCode, message));
        }
    }
}
