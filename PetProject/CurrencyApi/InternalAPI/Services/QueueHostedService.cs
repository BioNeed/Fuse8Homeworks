using CurrenciesDataAccessLibrary.Contracts;
using CurrenciesDataAccessLibrary.Enums;
using CurrenciesDataAccessLibrary.Models;
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

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _services.CreateScope();
            ICacheTasksRepository cacheTasksRepository = scope.ServiceProvider
                .GetRequiredService<ICacheTasksRepository>();
            CacheTask[] cacheTasks =
                await cacheTasksRepository.GetAllUncompletedTasksAsync(cancellationToken);

            int tasksCount = cacheTasks.Length;

            if (tasksCount == 0)
            {
                return;
            }

            CacheTask cacheTaskToProcess;
            if (tasksCount > 1)
            {
                CacheTask[] sortedCacheTasks = cacheTasks.OrderByDescending(c => c.CreatedAt).ToArray();
                cacheTaskToProcess = sortedCacheTasks[0];

                for (int i = 1; i < tasksCount; i++)
                {
                    await cacheTasksRepository.SetCacheTaskStatusAsync(
                        sortedCacheTasks[i].Id,
                        CacheTaskStatus.Cancelled,
                        cancellationToken);
                }
            }
            else
            {
                cacheTaskToProcess = cacheTasks[0];
            }

            await _taskQueue.QueueAsync(new WorkItem(cacheTaskToProcess.Id), cancellationToken);

            await base.StartAsync(cancellationToken);
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

                    await SetTaskAsError(workItem, stoppingToken);
                }
            }
        }

        private async Task SetTaskAsError(WorkItem workItem, CancellationToken cancellationToken)
        {
            using IServiceScope scope = _services.CreateScope();
            ICacheTasksRepository cacheTasksRepository = scope.ServiceProvider
                                .GetRequiredService<ICacheTasksRepository>();

            await cacheTasksRepository.SetCacheTaskStatusAsync(
                workItem.TaskId,
                CacheTaskStatus.CompletedWithError,
                cancellationToken);
        }
    }
}
