using InternalAPI.Models;

namespace InternalAPI.Contracts
{
    public interface IBackgroundTaskQueue
    {
        ValueTask<WorkItem> DequeueAsync(CancellationToken cancellationToken);

        ValueTask QueueAsync(WorkItem command, CancellationToken cancellationToken);
    }
}