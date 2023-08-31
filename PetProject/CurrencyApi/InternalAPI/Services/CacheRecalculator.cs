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
            CacheSettings cacheSettings = _cacheSettingsRepository.GetCacheSettings();
            string cacheBaseCurrency = cacheSettings.BaseCurrency;

            CacheTask cacheTask = await _cacheTasksRepository.GetCacheTaskAsync(item.TaskId, cancellationToken);

            await _cacheTasksRepository.SetCacheTaskStatusAsync(item.TaskId, CacheTaskStatus.Processing, cancellationToken);

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
                decimal newBaseExchangeRate = FindExchangeRateValue(
                    cacheTask.TaskInfo.NewBaseCurrency,
                    cachedExchangeRates.ExchangeRates);

                foreach (ExchangeRateDTOModel exchangeRateDTO in cachedExchangeRates.ExchangeRates)
                {
                    if (exchangeRateDTO.Code.Equals(cacheTask.TaskInfo.NewBaseCurrency,
                                                    StringComparison.OrdinalIgnoreCase))
                    {
                        exchangeRateDTO.Value = 1 / exchangeRateDTO.Value;
                    }
                    else
                    {
                        exchangeRateDTO.Value /= newBaseExchangeRate;
                    }
                }

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
