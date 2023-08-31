using InternalAPI.Models;

namespace InternalAPI.Contracts
{
    public interface IWorker
    {
        Task ProcessWorkItemAsync(WorkItem item, CancellationToken cancellationToken);
    }
}