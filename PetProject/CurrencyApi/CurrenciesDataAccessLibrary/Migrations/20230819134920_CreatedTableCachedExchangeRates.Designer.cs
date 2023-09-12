﻿// <auto-generated />
using System;
using CurrenciesDataAccessLibrary.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CurrenciesDataAccessLibrary.Migrations
{
    [DbContext(typeof(CurrenciesDbContext))]
    [Migration("20230819134920_CreatedTableCachedExchangeRates")]
    partial class CreatedTableCachedExchangeRates
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("cur")
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("InternalAPI.Models.CachedExchangeRates", b =>
                {
                    b.Property<DateTime>("RelevantOnDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("relevant_on_date");

                    b.Property<string>("BaseCurrency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("base_currency");

                    b.Property<string>("ExchangeRatesJson")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("exchange_rates_json");

                    b.HasKey("RelevantOnDate")
                        .HasName("pk_cached_exchange_rates");

                    b.ToTable("cached_exchange_rates", "cur", t =>
                        {
                            t.HasCheckConstraint("CK_cached_exchange_rates_base_currency_MinLength", "LENGTH(base_currency) >= 3");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
