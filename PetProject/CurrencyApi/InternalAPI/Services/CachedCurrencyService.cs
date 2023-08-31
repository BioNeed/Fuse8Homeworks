using CurrenciesDataAccessLibrary.Contracts;
using CurrenciesDataAccessLibrary.Models;
using Grpc.Core;
using InternalAPI.Constants;
using InternalAPI.Contracts;
using InternalAPI.Enums;
using InternalAPI.Exceptions;
using InternalAPI.Extensions;
using InternalAPI.Models;
using Microsoft.Extensions.Options;

namespace InternalAPI.Services
{
    public class CachedCurrencyService : ICachedCurrencyAPI
    {
        private readonly ICurrencyAPI _currencyAPI;
        private readonly IExchangeRatesRepository _exchangeRatesRepository;
        private readonly TimeSpan _cacheExpirationTime;

        public CachedCurrencyService(ICurrencyAPI currencyAPI,
                                     IExchangeRatesRepository exchangeRatesRepository,
                                     IOptionsSnapshot<ApiSettingsModel> apiSettings,
                                     ICacheSettingsRepository cacheSettingsRepository,
                                     IHttpContextAccessor httpContextAccessor,
                                     IConfiguration configuration)
        {
            _currencyAPI = currencyAPI;
            _exchangeRatesRepository = exchangeRatesRepository;
            _cacheExpirationTime = TimeSpan.FromHours(apiSettings.Value.CacheExpirationTimeInHours);

            CacheSettings cacheSettings = cacheSettingsRepository.GetCacheSettings();

            if (Enum.TryParse<CurrencyType>(
                cacheSettings.BaseCurrency,
                true,
                out _) == false)
            {
                bool usingByGrpc = httpContextAccessor.HttpContext.Connection.LocalPort ==
                    configuration.GetValue<int>(ApiConstants.PortNames.GrpcPort);

                if (usingByGrpc == true)
                {
                    throw new RpcException(
                        new Status(StatusCode.Internal,
                            ApiConstants.ErrorMessages.CacheBaseCurrencyNotFoundExceptionMessage));
                }

                throw new CacheBaseCurrencyNotFoundException(ApiConstants
                    .ErrorMessages.CacheBaseCurrencyNotFoundExceptionMessage);
            }

            BaseCurrency = cacheSettings.BaseCurrency;
        }

        public string BaseCurrency { get; }

        public async Task<ExchangeRateDTOModel> GetCurrentExchangeRateAsync(
            CurrencyType currencyType, CancellationToken cancellationToken)
        {
            CachedExchangeRates? lastExchangeRates = await _exchangeRatesRepository
                                    .GetLastCachedExchangeRatesAsync(cancellationToken);

            DateTime currentDateTime = DateTime.UtcNow;

            if (lastExchangeRates != null)
            {
                if (currentDateTime - lastExchangeRates.RelevantOnDate < _cacheExpirationTime)
                {
                    return FindExchangeRateDTOByType(
                        currencyType, lastExchangeRates.ExchangeRates);
                }
            }

            ExchangeRateModel[] currentExchangeRates = await _currencyAPI.
                GetAllCurrentCurrenciesAsync(BaseCurrency, cancellationToken);

            await _exchangeRatesRepository.SaveCacheDataAsync(
                BaseCurrency,
                currentExchangeRates.MapExchangeRatesToDTOs(),
                currentDateTime,
                cancellationToken);

            return FindExchangeRateDTOByType(currencyType, currentExchangeRates);
        }

        public async Task<ExchangeRateDTOModel> GetExchangeRateOnDateAsync(CurrencyType currencyType, DateOnly date, CancellationToken cancellationToken)
        {
            CachedExchangeRates? cachedHistoricalExchangeRates =
                await _exchangeRatesRepository.GetHistoricalCachedExchangeRatesAsync(date, cancellationToken);

            if (cachedHistoricalExchangeRates != null)
            {
                return FindExchangeRateDTOByType(
                    currencyType,
                    cachedHistoricalExchangeRates.ExchangeRates);
            }

            ExchangeRatesHistoricalModel exchangeRatesHistorical = await _currencyAPI
                .GetAllCurrenciesOnDateAsync(BaseCurrency, date, cancellationToken);

            await _exchangeRatesRepository.SaveCacheDataAsync(
                BaseCurrency,
                exchangeRatesHistorical.ExchangeRates.MapExchangeRatesToDTOs(),
                exchangeRatesHistorical.LastUpdatedAt,
                cancellationToken);

            return FindExchangeRateDTOByType(currencyType, exchangeRatesHistorical.ExchangeRates);
        }

        private ExchangeRateDTOModel FindExchangeRateDTOByType(CurrencyType currencyType, ExchangeRateModel[] exchangeRates)
        {
            string currencyCode = currencyType.ToString();

            foreach (ExchangeRateModel exchangeRate in exchangeRates)
            {
                if (exchangeRate.Code.Equals(currencyCode, StringComparison.OrdinalIgnoreCase))
                {
                    return new ExchangeRateDTOModel
                    {
                        Code = currencyCode,
                        Value = exchangeRate.Value,
                    };
                }
            }

            throw new InvalidOperationException();
        }

        private ExchangeRateDTOModel FindExchangeRateDTOByType(CurrencyType currencyType, ExchangeRateDTOModel[] exchangeRates)
        {
            string currencyCode = currencyType.ToString();

            foreach (ExchangeRateDTOModel exchangeRateDTO in exchangeRates)
            {
                if (exchangeRateDTO.Code.Equals(currencyCode, StringComparison.OrdinalIgnoreCase))
                {
                    return new ExchangeRateDTOModel
                    {
                        Code = currencyCode,
                        Value = exchangeRateDTO.Value,
                    };
                }
            }

            throw new InvalidOperationException();
        }
    }
}
