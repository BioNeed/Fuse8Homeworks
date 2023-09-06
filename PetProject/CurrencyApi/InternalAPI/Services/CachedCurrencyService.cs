using CurrenciesDataAccessLibrary.Contracts;
using CurrenciesDataAccessLibrary.Models;
using InternalAPI.Constants;
using InternalAPI.Contracts;
using InternalAPI.Enums;
using InternalAPI.Exceptions;
using InternalAPI.Extensions;
using InternalAPI.Models;
using Microsoft.Extensions.Options;

namespace InternalAPI.Services
{
    /// <summary>
    /// <inheritdoc cref="ICachedCurrencyAPI"/>
    /// </summary>
    public class CachedCurrencyService : ICachedCurrencyAPI
    {
        private readonly ICurrencyAPI _currencyAPI;
        private readonly IExchangeRatesRepository _exchangeRatesRepository;
        private readonly ICacheSettingsRepository _cacheSettingsRepository;
        private readonly ICacheTasksRepository _cacheTasksRepository;
        private readonly TimeSpan _cacheExpirationTime;
        private readonly TimeSpan _waitingForTaskProcessingTime;

        public CachedCurrencyService(ICurrencyAPI currencyAPI,
                                     IExchangeRatesRepository exchangeRatesRepository,
                                     IOptionsSnapshot<ApiSettingsModel> apiSettings,
                                     ICacheSettingsRepository cacheSettingsRepository,
                                     ICacheTasksRepository cacheTasksRepository)
        {
            _currencyAPI = currencyAPI;
            _exchangeRatesRepository = exchangeRatesRepository;
            _cacheSettingsRepository = cacheSettingsRepository;
            _cacheTasksRepository = cacheTasksRepository;

            _cacheExpirationTime = TimeSpan
                .FromHours(apiSettings.Value.CacheExpirationTimeInHours);
            _waitingForTaskProcessingTime = TimeSpan
                .FromSeconds(apiSettings.Value.WaitingTimeForTaskProcessingInSeconds);
        }

        public async Task<ExchangeRateDTOModel> GetCurrentExchangeRateAsync(
            CurrencyType currencyType, CancellationToken cancellationToken)
        {
            CachedExchangeRates? lastExchangeRates = await _exchangeRatesRepository
                                    .GetLastCachedExchangeRatesAsync(cancellationToken);

            DateTime currentDateTime = DateTime.UtcNow;

            if (lastExchangeRates == null)
            {
                ExchangeRateModel[] currentExchangeRates = await
                    GetExchangeRatesFromExternalApiAsync(baseCurrency,
                                                         currencyType,
                                                         currentDateTime,
                                                         cancellationToken);

                return FindExchangeRateDTOByType(currencyType, currentExchangeRates);
            }

            if (IsLatestCacheActual(lastExchangeRates, currentDateTime) == true)
            {
                return FindExchangeRateDTOByType(
                    currencyType, lastExchangeRates.ExchangeRates);
            }

            await WaitIfCacheTasksUncompletedAsync(cancellationToken);

            ExchangeRateModel[] exchangeRates = await
                    GetExchangeRatesFromExternalApiAsync(currencyType,
                                                         currentDateTime,
                                                         cancellationToken);

            return FindExchangeRateDTOByType(currencyType, exchangeRates);
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

            string baseCurrency = await GetValidBaseCurrencyAsync(cancellationToken);
            ExchangeRatesHistoricalModel exchangeRatesHistorical = await _currencyAPI
                .GetAllCurrenciesOnDateAsync(baseCurrency, date, cancellationToken);

            await _exchangeRatesRepository.SaveCacheDataAsync(
                baseCurrency,
                exchangeRatesHistorical.ExchangeRates.MapExchangeRatesToDTOs(),
                exchangeRatesHistorical.LastUpdatedAt,
                cancellationToken);

            return FindExchangeRateDTOByType(currencyType, exchangeRatesHistorical.ExchangeRates);
        }

        public async Task<string> GetValidBaseCurrencyAsync(CancellationToken cancellationToken)
        {
            string baseCurrency = (await _cacheSettingsRepository
                .GetCacheSettingsAsync(cancellationToken)).BaseCurrency;

            if (Enum.TryParse<CurrencyType>(
                        baseCurrency,
                        true,
                        out _) == false)
            {
                throw new CacheBaseCurrencyNotFoundException(ApiConstants
                    .ErrorMessages.CacheBaseCurrencyNotFoundExceptionMessage);
            }

            return baseCurrency;
        }

        private ExchangeRateDTOModel FindExchangeRateDTOByType(CurrencyType currencyType, ExchangeRateModel[] exchangeRates)
        {
            string currencyCode = currencyType.ToString();

            ExchangeRateModel foundExchangeRate = exchangeRates.First(e =>
                e.Code.Equals(currencyCode, StringComparison.OrdinalIgnoreCase));

            return new ExchangeRateDTOModel
            {
                Code = currencyCode,
                Value = foundExchangeRate.Value,
            };
        }

        private ExchangeRateDTOModel FindExchangeRateDTOByType(CurrencyType currencyType, ExchangeRateDTOModel[] exchangeRatesDTO)
        {
            string currencyCode = currencyType.ToString();

            return exchangeRatesDTO.First(e => e.Code.Equals(currencyCode, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsLatestCacheActual(CachedExchangeRates latestData, DateTime currentDt)
            => currentDt - latestData.RelevantOnDate < _cacheExpirationTime;

        private async Task WaitIfCacheTasksUncompletedAsync(CancellationToken cancellationToken)
        {
            if (await _cacheTasksRepository
                .HasAnyUncompletedTaskAsync(cancellationToken) == true)
            {
                await Task.Delay(_waitingForTaskProcessingTime, cancellationToken);

                if (await _cacheTasksRepository
                    .HasAnyUncompletedTaskAsync(cancellationToken) == true)
                {
                    throw new CacheTaskProcessingTimedOutException(ApiConstants
                        .ErrorMessages.CacheTaskProcessingTimedOut);
                }
            }
        }

        private async Task<ExchangeRateModel[]> GetExchangeRatesFromExternalApiAsync(CurrencyType currencyType, DateTime currentDateTime, CancellationToken cancellationToken)
        {
            string baseCurrency = await GetValidBaseCurrencyAsync(cancellationToken);

            ExchangeRateModel[] currentExchangeRates = await _currencyAPI.
                        GetAllCurrentCurrenciesAsync(baseCurrency, cancellationToken);

            await _exchangeRatesRepository.SaveCacheDataAsync(
                baseCurrency,
                currentExchangeRates.MapExchangeRatesToDTOs(),
                currentDateTime,
                cancellationToken);

            return currentExchangeRates;
        }
    }
}