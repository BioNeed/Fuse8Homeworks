﻿using CurrenciesDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrenciesDataAccessLibrary.Database.Configurations
{
    public class CacheTaskConfiguration
        : IEntityTypeConfiguration<CacheTask>
    {
        public void Configure(EntityTypeBuilder<CacheTask> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CreatedAt).IsRequired();
            builder.Property(c => c.Status).IsRequired();
            builder.HasOne(c => c.TaskInfo)
                .WithOne()
                .HasForeignKey<CacheTaskInfo>();

            builder.ToTable(tableBuilder => tableBuilder.HasCheckConstraint(
                name: "date_time_before_or_equal_now",
                sql: "relevant_on_date at time zone 'UTC' <= timezone('UTC', now())"));
        }
    }
}
