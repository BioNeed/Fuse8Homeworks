using CurrenciesDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrenciesDataAccessLibrary.Database.Configurations
{
    public class CacheSettingsConfiguration
        : IEntityTypeConfiguration<CacheSettings>
    {
        public void Configure(EntityTypeBuilder<CacheSettings> builder)
        {
            builder.Property<int>("Id")
                    .HasColumnType("int")
                    .ValueGeneratedOnAdd();
            builder.HasKey("Id");

            builder.Property(s => s.BaseCurrency).IsRequired();
        }
    }
}
