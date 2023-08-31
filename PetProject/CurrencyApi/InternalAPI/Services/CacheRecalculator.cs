using CurrenciesDataAccessLibrary.Contracts;
using CurrenciesDataAccessLibrary.Enums;
using CurrenciesDataAccessLibrary.Models;
using InternalAPI.Models;

namespace InternalAPI.Services
{
    public class CacheRecalculator
    {
        private ICacheTasksRepository _cacheTasksRepository;

        public CacheRecalculator(ICacheTasksRepository cacheTasksRepository)
        {
            _cacheTasksRepository = cacheTasksRepository;
        }

        public async Task ProcessWorkItemAsync(WorkItem item, CancellationToken cancellationToken)
        {
            CacheTask cacheTask = await _cacheTasksRepository.GetCacheTaskAsync(item.TaskId, cancellationToken);

            await _cacheTasksRepository.SetCacheTaskStatusAsync(item.TaskId, CacheTaskStatus.Processing, cancellationToken);

            // Получить все CachedExchangeRates[] без указанной baseCurrency
            // В каждом CachedExchangeRates, в каждом ExchangeRateDTO пересчитать значение курса для новой базовой валюты
            // Изменить базовую валюту у каждого CachedExchangeRates
            // Закинуть в БД обновленные данные о каждом CachedExchangeRates
            // (СОЗДАТЬ ТАБЛИЦУ ДЛЯ БАЗОВОЙ ВАЛЮТЫ)
            // После этого запомнить новую базовую валюту для кэша
        }
    }
}
