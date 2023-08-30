using CurrenciesDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrenciesDataAccessLibrary.Database.Configurations
{
    public class CacheTaskInfoConfiguration
        : IEntityTypeConfiguration<CacheTaskInfo>
    {
        public void Configure(EntityTypeBuilder<CacheTaskInfo> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.NewBaseCurrency).IsRequired();
        }
    }
}
