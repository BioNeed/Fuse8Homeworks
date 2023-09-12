using CurrenciesDataAccessLibrary.Constants;
using CurrenciesDataAccessLibrary.Database.Configurations;
using CurrenciesDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrenciesDataAccessLibrary.Database
{
    public class CurrenciesDbContext : DbContext
    {
        public CurrenciesDbContext(DbContextOptions<CurrenciesDbContext> options)
            : base(options)
        {
        }

        public DbSet<CachedExchangeRates> CachedExchangeRates { get; set; }

        public DbSet<CacheSettings> CacheSettings { get; set; }

        public DbSet<CacheTask> CacheTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(LibraryConstants.SchemaNames.Currencies);
            modelBuilder.ApplyConfigurationsFromAssembly(
                assembly: GetType().Assembly,
                predicate: p => p.Namespace == typeof(CachedExchangeRatesConfiguration).Namespace);

            modelBuilder.HasPostgresExtension("uuid-ossp");
        }
    }
}