using InternalAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternalAPI.DataAccess.Configurations
{
    public class CachedExchangeRatesConfiguration
        : IEntityTypeConfiguration<CachedExchangeRates>
    {
        public void Configure(EntityTypeBuilder<CachedExchangeRates> builder)
        {
            builder.HasKey(c => c.RelevantOnDate);
            builder.Property(c => c.ExchangeRatesJson).HasColumnType("jsonb").IsRequired();
            builder.Property(c => c.BaseCurrency).IsRequired();

            builder.Ignore(c => c.ExchangeRates);
        }
    }
}
