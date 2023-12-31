﻿using CurrenciesDataAccessLibrary.Contracts;
using CurrenciesDataAccessLibrary.Enums;
using CurrenciesDataAccessLibrary.Models;
using InternalAPI.Contracts;
using InternalAPI.Models;

namespace InternalAPI.Services
{
    /// <summary>
    /// Сервис, управляющий очередью с задачами
    /// </summary>
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
            CacheTask[] cacheTasks = await cacheTasksRepository
                                        .GetAllUncompletedTasksAsync(cancellationToken);

            int tasksCount = cacheTasks.Length;

            if (tasksCount != 0)
            {
                CacheTask cacheTaskToProcess = await FindCacheTaskToProcessAsync(
                                                                cacheTasksRepository,
                                                                cacheTasks,
                                                                tasksCount,
                                                                cancellationToken);

                await _taskQueue.QueueAsync(new WorkItem(cacheTaskToProcess.Id), cancellationToken);
            }

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                WorkItem workItem = await _taskQueue.DequeueAsync(cancellationToken);

                try
                {
                    using IServiceScope scope = _services.CreateScope();
                    IWorker worker = scope.ServiceProvider.GetRequiredService<IWorker>();
                    await worker.ProcessWorkItemAsync(workItem, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка во время обработки {TaskId}", workItem.TaskId);

                    await SetTaskAsError(workItem, cancellationToken);
                }
            }
        }

        private Task SetTaskAsError(WorkItem workItem, CancellationToken cancellationToken)
        {
            using IServiceScope scope = _services.CreateScope();
            ICacheTasksRepository cacheTasksRepository = scope.ServiceProvider
                                .GetRequiredService<ICacheTasksRepository>();

            return cacheTasksRepository.SetCacheTaskStatusAsync(
                workItem.TaskId,
                CacheTaskStatus.CompletedWithError,
                cancellationToken);
        }

        private async Task<CacheTask> FindCacheTaskToProcessAsync(ICacheTasksRepository cacheTasksRepository,
                                                             CacheTask[] cacheTasks,
                                                             int tasksCount,
                                                             CancellationToken cancellationToken)
        {
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

            return cacheTaskToProcess;
        }
    }
}
