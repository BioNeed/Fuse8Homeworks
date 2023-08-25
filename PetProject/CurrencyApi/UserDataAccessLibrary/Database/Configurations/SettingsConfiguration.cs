using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserDataAccessLibrary.Models;

namespace UserDataAccessLibrary.Database.Configurations
{
    public class SettingsConfiguration
        : IEntityTypeConfiguration<Settings>
    {
        public void Configure(EntityTypeBuilder<Settings> builder)
        {
            builder.Property<int>("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd();
            builder.HasKey("Id");

            builder.Property(s => s.DefaultCurrency).IsRequired();
            builder.Property(s => s.CurrencyRoundCount).IsRequired();
        }
    }
}
