using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.DataAccess.Configurations
{
    public class FavouriteExchangeRateConfiguration
        : IEntityTypeConfiguration<FavouriteExchangeRate>
    {
        public void Configure(EntityTypeBuilder<FavouriteExchangeRate> builder)
        {
            builder.Property<int>("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd();
            builder.HasKey("Id");

            builder.Property(f => f.Name).IsRequired();
            builder.Property(f => f.BaseCurrency).IsRequired();
            builder.Property(f => f.Currency).IsRequired();

            builder.ToTable(tableBuilder => tableBuilder.HasCheckConstraint(
                name: "not_equal_currencies",
                sql: "currency != base_currency"));

            builder.HasIndex(f => f.Name).IsUnique();
            builder.HasIndex(f => new { f.Currency, f.BaseCurrency }).IsUnique();
        }
    }
}
