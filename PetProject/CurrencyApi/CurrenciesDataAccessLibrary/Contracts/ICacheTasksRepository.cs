using CurrenciesDataAccessLibrary.Enums;
using CurrenciesDataAccessLibrary.Models;

namespace CurrenciesDataAccessLibrary.Contracts
{
    /// <summary>
    /// Репозиторий для работы с таблицей задач кэша
    /// </summary>
    public interface ICacheTasksRepository
    {
        /// <summary>
        /// Добавить задачу кэша в таблицу
        /// </summary>
        /// <param name="taskId">Id задачи</param>
        /// <param name="newBaseCurrency">Базовая валюта для задачи пересчета кэша</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task AddCacheTaskAsync(Guid taskId, string newBaseCurrency, CancellationToken cancellationToken);

        /// <summary>
        /// Получить все задачи со статусами "Создана" или "В обработке"
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Все задачи со статусами "Создана" или "В обработке"</returns>
        Task<CacheTask[]> GetAllUncompletedTasksAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить задачу кэша с ее данными (например, базовой валютой для пересчета кэша)
        /// </summary>
        /// <param name="taskId">Id задачи</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Задачу кэша с ее данными</returns>
        Task<CacheTask> GetCacheTaskWithInfoAsync(Guid taskId, CancellationToken cancellationToken);

        /// <summary>
        /// Есть ли хотя бы одна задача кэша со статусом "Создана" или "В обработке"?
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>True, если есть хотя бы одна задача с таким статусом</returns>
        Task<bool> HasAnyUncompletedTaskAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Задать статус задачи кэша
        /// </summary>
        /// <param name="taskId">Id задачи для изменения</param>
        /// <param name="status">Новый статус задачи</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task SetCacheTaskStatusAsync(Guid taskId, CacheTaskStatus status, CancellationToken cancellationToken);
    }
}