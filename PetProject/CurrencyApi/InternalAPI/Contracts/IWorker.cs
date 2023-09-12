using InternalAPI.Models;

namespace InternalAPI.Contracts
{
    /// <summary>
    /// Сервис, обрабатывающий задачу
    /// </summary>
    public interface IWorker
    {
        /// <summary>
        /// Выполнить задачу
        /// </summary>
        /// <param name="item">Item с Id задачи, которую нужно выполнить</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task ProcessWorkItemAsync(WorkItem item, CancellationToken cancellationToken);
    }
}