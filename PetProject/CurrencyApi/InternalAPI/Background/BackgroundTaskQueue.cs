using System.Threading.Channels;
using InternalAPI.Contracts;
using InternalAPI.Models;

namespace InternalAPI.Background
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<WorkItem> _queue;

        public BackgroundTaskQueue()
        {
            var options = new BoundedChannelOptions(100) { FullMode = BoundedChannelFullMode.Wait };
            _queue = Channel.CreateBounded<WorkItem>(options);
        }

        public ValueTask QueueAsync(WorkItem command, CancellationToken cancellationToken)
            => _queue.Writer.WriteAsync(command, cancellationToken);

        public ValueTask<WorkItem> DequeueAsync(CancellationToken cancellationToken)
            => _queue.Reader.ReadAsync(cancellationToken);
    }
}
