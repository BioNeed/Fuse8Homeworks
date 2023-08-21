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

            builder.HasCheckConstraint("not_equal_currencies", "currency != base_currency");
            builder.HasIndex(f => f.Name).IsUnique();
        }
    }
}
