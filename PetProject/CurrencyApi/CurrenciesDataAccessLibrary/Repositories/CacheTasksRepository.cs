﻿using CurrenciesDataAccessLibrary.Contracts;
using CurrenciesDataAccessLibrary.Database;
using CurrenciesDataAccessLibrary.Enums;
using CurrenciesDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrenciesDataAccessLibrary.Repositories
{
    public class CacheTasksRepository : ICacheTasksRepository
    {
        private readonly CurrenciesDbContext _currenciesDbContext;

        public CacheTasksRepository(CurrenciesDbContext currenciesDbContext)
        {
            _currenciesDbContext = currenciesDbContext;
        }

        public Task<CacheTask> GetCacheTaskAsync(Guid taskId,
                                            CancellationToken cancellationToken)
        {
            return _currenciesDbContext.CacheTasks.AsNoTracking().FirstAsync(
                                    predicate: c => c.Id == taskId,
                                    cancellationToken: cancellationToken);
        }

        public Task<CacheTask[]> GetAllUncompletedTasksAsync(CancellationToken cancellationToken)
        {
            return _currenciesDbContext.CacheTasks.AsNoTracking()
                .Where(c => c.Status == CacheTaskStatus.Created ||
                            c.Status == CacheTaskStatus.Processing)
                .ToArrayAsync(cancellationToken);
        }

        public async Task SetCacheTaskStatusAsync(Guid taskId,
                                                  CacheTaskStatus status,
                                                  CancellationToken cancellationToken)
        {
            CacheTask cacheTaskToUpdate =
                await _currenciesDbContext.CacheTasks.FirstAsync(
                                    predicate: c => c.Id == taskId,
                                    cancellationToken: cancellationToken);

            cacheTaskToUpdate.Status = status;

            await _currenciesDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddCacheTaskAsync(Guid taskId,
                                             string newBaseCurrency,
                                             CancellationToken cancellationToken)
        {
            CacheTask cacheTask = new CacheTask
            {
                Id = taskId,
                CreatedAt = DateTime.UtcNow,
                Status = CacheTaskStatus.Created,
                TaskInfo = new CacheTaskInfo
                {
                    Id = taskId,
                    NewBaseCurrency = newBaseCurrency,
                },
            };

            await _currenciesDbContext.CacheTasks.AddAsync(cacheTask, cancellationToken);

            await _currenciesDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
