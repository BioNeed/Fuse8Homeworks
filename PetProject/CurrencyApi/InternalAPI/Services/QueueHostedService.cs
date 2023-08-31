using CurrenciesDataAccessLibrary.Contracts;
using CurrenciesDataAccessLibrary.Enums;
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

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _services.CreateScope();
            ICacheTasksRepository worker = scope.ServiceProvider.GetRequiredService<ICacheTasksRepository>();


        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested == false)
            {
                WorkItem workItem = await _taskQueue.DequeueAsync(stoppingToken);

                try
                {
                    using IServiceScope scope = _services.CreateScope();
                    IWorker worker = scope.ServiceProvider.GetRequiredService<IWorker>();
                    await worker.ProcessWorkItemAsync(workItem, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка во время обработки {TaskId}", workItem.TaskId);
                    using IServiceScope scope = _services.CreateScope();
                    ICacheTasksRepository cacheTasksRepository = scope.ServiceProvider
                                        .GetRequiredService<ICacheTasksRepository>();

                    await cacheTasksRepository.SetCacheTaskStatusAsync(
                        workItem.TaskId,
                        CacheTaskStatus.CompletedWithError,
                        stoppingToken);
                }
            }
        }
    }
}
