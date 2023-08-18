using InternalAPI.Constants;
using InternalAPI.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;

namespace InternalAPI.DataAccess
{
    public class CurrenciesDbContext : DbContext
    {
        public CurrenciesDbContext(DbContextOptions<CurrenciesDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(ApiConstants.SchemaNames.Currencies);
            modelBuilder.ApplyConfigurationsFromAssembly(
                assembly: GetType().Assembly,
                predicate: p => p.Namespace == typeof(ExchangeRateConfiguration).Namespace);
        }
    }
}
