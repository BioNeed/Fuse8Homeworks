using CurrenciesDataAccessLibrary.Contracts;
using CurrenciesDataAccessLibrary.Enums;
using CurrenciesDataAccessLibrary.Models;
using InternalAPI.Contracts;
using InternalAPI.Models;

namespace InternalAPI.Services
{
    public class CacheRecalculator : IWorker
    {
        private readonly ICacheTasksRepository _cacheTasksRepository;
        private readonly IExchangeRatesRepository _exchangeRatesRepository;
        private readonly ICacheSettingsRepository _cacheSettingsRepository;

        public CacheRecalculator(ICacheTasksRepository cacheTasksRepository,
                                 ICacheSettingsRepository cacheSettingsRepository,
                                 IExchangeRatesRepository exchangeRatesRepository)
        {
            _cacheTasksRepository = cacheTasksRepository;
            _exchangeRatesRepository = exchangeRatesRepository;
            _cacheSettingsRepository = cacheSettingsRepository;
        }

        public async Task ProcessWorkItemAsync(WorkItem item, CancellationToken cancellationToken)
        {
            CacheSettings cacheSettings = await _cacheSettingsRepository.GetCacheSettingsAsync(cancellationToken);
            string cacheBaseCurrency = cacheSettings.BaseCurrency;

            CacheTask cacheTask = await _cacheTasksRepository
                .GetCacheTaskWithInfoAsync(item.TaskId, cancellationToken);

            await _cacheTasksRepository
                .SetCacheTaskStatusAsync(item.TaskId, CacheTaskStatus.Processing, cancellationToken);

            if (cacheBaseCurrency == cacheTask.TaskInfo.NewBaseCurrency)
            {
                await _cacheTasksRepository.SetCacheTaskStatusAsync(
                    item.TaskId,
                    CacheTaskStatus.CompletedSuccessfully,
                    cancellationToken);

                return;
            }

            CachedExchangeRates[] allCachedExchangeRates =
                await _exchangeRatesRepository.GetAllCachedExchangeRatesAsync(cancellationToken);

            foreach (CachedExchangeRates cachedExchangeRates in allCachedExchangeRates)
            {
                RecalculateExchangeRatesWithNewBase(cacheBaseCurrency,
                                               cacheTask.TaskInfo.NewBaseCurrency,
                                               cachedExchangeRates.ExchangeRates);

                await _exchangeRatesRepository.UpdateCachedExchangeRatesAsync(
                    cachedExchangeRates.RelevantOnDate,
                    cacheTask.TaskInfo.NewBaseCurrency,
                    cachedExchangeRates.ExchangeRates,
                    cancellationToken);
            }

            await _cacheSettingsRepository.SetBaseCurrencyAsync(
                cacheTask.TaskInfo.NewBaseCurrency, cancellationToken);

            await _cacheTasksRepository.SetCacheTaskStatusAsync(
                cacheTask.Id,
                CacheTaskStatus.CompletedSuccessfully,
                cancellationToken);
        }

        private void RecalculateExchangeRatesWithNewBase(string cacheBaseCurrency,
                                                    string newBaseCurrency,
                                                    ExchangeRateDTOModel[] exchangeRates)
        {
            decimal newBaseExchangeRate = FindExchangeRateValue(
                                            newBaseCurrency,
                                            exchangeRates);

            foreach (ExchangeRateDTOModel exchangeRateDTO in exchangeRates)
            {
                if (exchangeRateDTO.Code.Equals(cacheBaseCurrency,
                                                StringComparison.OrdinalIgnoreCase))
                {
                    exchangeRateDTO.Value = 1 / newBaseExchangeRate;
                }
                else
                {
                    exchangeRateDTO.Value /= newBaseExchangeRate;
                }
            }
        }

        private decimal FindExchangeRateValue(string currencyCode, ExchangeRateDTOModel[] exchangeRates)
        {
            foreach (ExchangeRateDTOModel exchangeRate in exchangeRates)
            {
                if (currencyCode.Equals(exchangeRate.Code, StringComparison.OrdinalIgnoreCase))
                {
                    return exchangeRate.Value;
                }
            }

            throw new InvalidOperationException();
        }
    }
}
