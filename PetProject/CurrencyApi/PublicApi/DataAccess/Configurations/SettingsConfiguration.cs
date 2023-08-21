﻿using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.DataAccess.Configurations
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