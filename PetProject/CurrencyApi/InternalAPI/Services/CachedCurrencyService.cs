﻿using Audit.Core;
using CurrenciesDataAccessLibrary.Contracts;
using CurrenciesDataAccessLibrary.Models;
using Grpc.Core;
using InternalAPI.Constants;
using InternalAPI.Contracts;
using InternalAPI.Enums;
using InternalAPI.Exceptions;
using InternalAPI.Extensions;
using InternalAPI.Models;
using Microsoft.AspNetCore.Http;
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
        private readonly ICacheTasksRepository _cacheTasksRepository;
        private readonly TimeSpan _cacheExpirationTime;
        private readonly TimeSpan _waitingForTaskProcessingTime;
        private readonly string _baseCurrency;
        private bool _isBaseCurrencyValidated = false;

        public CachedCurrencyService(ICurrencyAPI currencyAPI,
                                     IExchangeRatesRepository exchangeRatesRepository,
                                     IOptionsSnapshot<ApiSettingsModel> apiSettings,
                                     ICacheSettingsRepository cacheSettingsRepository,
                                     ICacheTasksRepository cacheTasksRepository)
        {
            _currencyAPI = currencyAPI;
            _exchangeRatesRepository = exchangeRatesRepository;
            _cacheTasksRepository = cacheTasksRepository;

            _cacheExpirationTime = TimeSpan
                .FromHours(apiSettings.Value.CacheExpirationTimeInHours);
            _waitingForTaskProcessingTime = TimeSpan
                .FromSeconds(apiSettings.Value.WaitingTimeForTaskProcessingInSeconds);

            CacheSettings cacheSettings = cacheSettingsRepository.GetCacheSettings();

            _baseCurrency = cacheSettings.BaseCurrency;
        }

        public string BaseCurrency
        {
            get
            {
                if (_isBaseCurrencyValidated == false)
                {
                    if (Enum.TryParse<CurrencyType>(
                        _baseCurrency,
                        true,
                        out _) == false)
                    {
                        throw new CacheBaseCurrencyNotFoundException(ApiConstants
                            .ErrorMessages.CacheBaseCurrencyNotFoundExceptionMessage);
                    }

                    _isBaseCurrencyValidated = true;
                }

                return _baseCurrency;
            }
        }

        public async Task<ExchangeRateDTOModel> GetCurrentExchangeRateAsync(
            CurrencyType currencyType, CancellationToken cancellationToken)
        {
            CachedExchangeRates? lastExchangeRates = await _exchangeRatesRepository
                                    .GetLastCachedExchangeRatesAsync(cancellationToken);

            DateTime currentDateTime = DateTime.UtcNow;

            if (lastExchangeRates == null)
            {
                return await GetExchangeRateFromExternalApi(currencyType, currentDateTime, cancellationToken);
            }

            if (IsLatestCacheActual(lastExchangeRates, currentDateTime) == true)
            {
                return FindExchangeRateDTOByType(
                    currencyType, lastExchangeRates.ExchangeRates);
            }

            await WaitIfCacheTasksUncompleted(cancellationToken);

            return await GetExchangeRateFromExternalApi(currencyType, currentDateTime, cancellationToken);
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

        private async Task WaitIfCacheTasksUncompleted(CancellationToken cancellationToken)
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

        private async Task<ExchangeRateDTOModel> GetExchangeRateFromExternalApi(CurrencyType currencyType, DateTime currentDateTime, CancellationToken cancellationToken)
        {
            ExchangeRateModel[] currentExchangeRates = await _currencyAPI.
                            GetAllCurrentCurrenciesAsync(BaseCurrency, cancellationToken);

            await _exchangeRatesRepository.SaveCacheDataAsync(
                BaseCurrency,
                currentExchangeRates.MapExchangeRatesToDTOs(),
                currentDateTime,
                cancellationToken);

            return FindExchangeRateDTOByType(currencyType, currentExchangeRates);
        }
    }
}