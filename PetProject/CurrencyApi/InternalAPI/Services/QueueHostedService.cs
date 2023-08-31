using InternalAPI.Contracts;
using InternalAPI.Models;

namespace InternalAPI.Services
{
    public class QueueHostedService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<QueueHostedService> _logger;
        private readonly IServiceProvider _services;

        public QueueHostedService(IBackgroundTaskQueue taskQueue,
                                  ILogger<QueueHostedService> logger,
                                  IServiceProvider services)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _services = services;


        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested == false)
            {
                WorkItem workItem = await _taskQueue.DequeueAsync(stoppingToken);

                try
                {
                    using IServiceScope scope = _services.CreateScope();


                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка во время обработки {TaskId}", workItem.TaskId);
                }
            }
        }
    }
}
