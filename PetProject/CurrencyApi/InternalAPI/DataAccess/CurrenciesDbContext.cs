using InternalAPI.Constants;
using InternalAPI.DataAccess.Configurations;
using InternalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InternalAPI.DataAccess
{
    public class CurrenciesDbContext : DbContext
    {
        public CurrenciesDbContext(DbContextOptions<CurrenciesDbContext> options)
            : base(options)
        {
        }

        public DbSet<CachedExchangeRates> CachedExchangeRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(ApiConstants.SchemaNames.Currencies);
            modelBuilder.ApplyConfigurationsFromAssembly(
                assembly: GetType().Assembly,
                predicate: p => p.Namespace == typeof(CachedExchangeRatesConfiguration).Namespace);
        }
    }
}