﻿using Microsoft.EntityFrameworkCore;
using UserDataAccessLibrary.Contracts;
using UserDataAccessLibrary.Database;
using UserDataAccessLibrary.Enums;
using UserDataAccessLibrary.Models;

namespace UserDataAccessLibrary.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly UserDbContext _userDbContext;

        public SettingsRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<Settings> GetApplicationSettingsAsync(CancellationToken cancellationToken)
        {
            return await _userDbContext.Settings.AsNoTracking().FirstAsync(cancellationToken);
        }

        public async Task SetDefaultCurrencyAsync(string newDefaultCurrency,
                                                  CancellationToken cancellationToken)
        {
            CurrencyType defaultCurrency = Enum.Parse<CurrencyType>(
                newDefaultCurrency,
                true);

            Settings settings = await _userDbContext.Settings.FirstAsync(cancellationToken);
            settings.DefaultCurrency = defaultCurrency;

            _userDbContext.SaveChanges();
        }

        public async Task SetCurrencyRoundCountAsync(int newRoundCount,
                                          CancellationToken cancellationToken)
        {
            Settings settings = await _userDbContext.Settings.FirstAsync(cancellationToken);
            settings.CurrencyRoundCount = newRoundCount;

            _userDbContext.SaveChanges();
        }
    }
}