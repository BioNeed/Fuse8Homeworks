using CurrenciesDataAccessLibrary.Contracts;
using InternalAPI.Contracts;
using InternalAPI.Enums;
using InternalAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InternalAPI.Controllers
{
    /// <summary>
    /// Методы для работы с кэшем валют
    /// </summary>
    [Route("cache")]
    public class CurrencyCacheController : ControllerBase
    {
        private readonly ICacheTasksRepository _cacheTasksRepository;
        private readonly IBackgroundTaskQueue _taskQueue;

        public CurrencyCacheController(ICacheTasksRepository cacheTasksRepository,
                               IBackgroundTaskQueue taskQueue)
        {
            _cacheTasksRepository = cacheTasksRepository;
            _taskQueue = taskQueue;
        }

        /// <summary>
        /// Пересчитать кэш под новую базовую валюту
        /// </summary>
        /// <param name="newBaseCurrency">Новая базовая валюта</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <response code="202">
        /// Возвращает, если удалось принять запрос на пересчет кэша
        /// </response>
        /// <response code="500">
        /// Возвращает в случае внутренних ошибок
        /// </response>
        [Route("{newBaseCurrency}")]
        [HttpPost]
        public async Task<ActionResult> RecalculateCacheWithNewBaseCurrencyAsync(
            CurrencyType newBaseCurrency,
            CancellationToken cancellationToken)
        {
            Guid taskId = Guid.NewGuid();
            await _cacheTasksRepository.AddCacheTaskAsync(taskId,
                                                          newBaseCurrency.ToString(),
                                                          cancellationToken);

            await _taskQueue.QueueAsync(new WorkItem(taskId), cancellationToken);

            return Accepted(taskId);
        }
    }
}
