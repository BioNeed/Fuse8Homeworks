﻿using CurrenciesDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrenciesDataAccessLibrary.Database.Configurations
{
    public class CachedExchangeRatesConfiguration
        : IEntityTypeConfiguration<CachedExchangeRates>
    {
        public void Configure(EntityTypeBuilder<CachedExchangeRates> builder)
        {
            builder.HasKey(c => c.RelevantOnDate);
            builder.Property(c => c.ExchangeRatesJson).HasColumnType("jsonb").IsRequired();
            builder.Property(c => c.BaseCurrency).IsRequired();

            builder.ToTable(tableBuilder => tableBuilder.HasCheckConstraint(
                name: "date_time_before_or_equal_now",
                sql: "relevant_on_date at time zone 'UTC' <= timezone('UTC', now())"));
            builder.Ignore(c => c.ExchangeRates);
        }
    }
}
