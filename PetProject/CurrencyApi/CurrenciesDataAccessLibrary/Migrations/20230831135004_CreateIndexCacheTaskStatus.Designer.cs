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
    [Migration("20230831135004_CreateIndexCacheTaskStatus")]
    partial class CreateIndexCacheTaskStatus
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("cur")
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CurrenciesDataAccessLibrary.Models.CacheSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BaseCurrency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("base_currency");

                    b.HasKey("Id")
                        .HasName("pk_cache_settings");

                    b.ToTable("cache_settings", "cur", t =>
                        {
                            t.HasCheckConstraint("CK_cache_settings_base_currency_MinLength", "LENGTH(base_currency) >= 3");
                        });
                });

            modelBuilder.Entity("CurrenciesDataAccessLibrary.Models.CacheTask", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_cache_tasks");

                    b.HasIndex("Status")
                        .HasDatabaseName("ix_cache_tasks_status");

                    b.ToTable("cache_tasks", "cur", t =>
                        {
                            t.HasCheckConstraint("CK_cache_tasks_status_Enum", "status IN (0, 1, 2, 3, 4)");

                            t.HasCheckConstraint("date_time_before_or_equal_now", "created_at at time zone 'UTC' <= timezone('UTC', now())");
                        });
                });

            modelBuilder.Entity("CurrenciesDataAccessLibrary.Models.CacheTaskInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("NewBaseCurrency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("new_base_currency");

                    b.HasKey("Id")
                        .HasName("pk_cache_task_info");

                    b.ToTable("cache_task_info", "cur", t =>
                        {
                            t.HasCheckConstraint("CK_cache_task_info_new_base_currency_MinLength", "LENGTH(new_base_currency) >= 3");
                        });
                });

            modelBuilder.Entity("CurrenciesDataAccessLibrary.Models.CachedExchangeRates", b =>
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

                            t.HasCheckConstraint("date_time_before_or_equal_now", "relevant_on_date at time zone 'UTC' <= timezone('UTC', now())");
                        });
                });

            modelBuilder.Entity("CurrenciesDataAccessLibrary.Models.CacheTask", b =>
                {
                    b.HasOne("CurrenciesDataAccessLibrary.Models.CacheTaskInfo", "TaskInfo")
                        .WithOne("Task")
                        .HasForeignKey("CurrenciesDataAccessLibrary.Models.CacheTask", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_cache_tasks_cache_task_info_id");

                    b.Navigation("TaskInfo");
                });

            modelBuilder.Entity("CurrenciesDataAccessLibrary.Models.CacheTaskInfo", b =>
                {
                    b.Navigation("Task");
                });
#pragma warning restore 612, 618
        }
    }
}
