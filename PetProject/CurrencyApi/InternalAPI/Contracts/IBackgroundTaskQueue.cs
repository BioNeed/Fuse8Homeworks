using InternalAPI.Models;

namespace InternalAPI.Contracts
{
    /// <summary>
    /// Фоновая очередь задач
    /// </summary>
    public interface IBackgroundTaskQueue
    {
        ValueTask<WorkItem> DequeueAsync(CancellationToken cancellationToken);

        ValueTask QueueAsync(WorkItem command, CancellationToken cancellationToken);
    }
}