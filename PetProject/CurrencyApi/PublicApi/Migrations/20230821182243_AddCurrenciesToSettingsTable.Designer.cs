﻿// <auto-generated />
using Fuse8_ByteMinds.SummerSchool.PublicApi.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Migrations
{
    [DbContext(typeof(UserDbContext))]
    [Migration("20230821182243_AddCurrenciesToSettingsTable")]
    partial class AddCurrenciesToSettingsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("user")
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Fuse8_ByteMinds.SummerSchool.PublicApi.Models.FavouriteExchangeRate", b =>
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

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("currency");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_favourites");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_favourites_name");

                    b.HasIndex("Currency", "BaseCurrency")
                        .IsUnique()
                        .HasDatabaseName("ix_favourites_currency_base_currency");

                    b.ToTable("favourites", "user", t =>
                        {
                            t.HasCheckConstraint("CK_favourites_base_currency_MinLength", "LENGTH(base_currency) >= 3");

                            t.HasCheckConstraint("CK_favourites_currency_MinLength", "LENGTH(currency) >= 3");

                            t.HasCheckConstraint("not_equal_currencies", "currency != base_currency");
                        });
                });

            modelBuilder.Entity("Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Settings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CurrencyRoundCount")
                        .HasColumnType("integer")
                        .HasColumnName("currency_round_count");

                    b.Property<int>("DefaultCurrency")
                        .HasColumnType("integer")
                        .HasColumnName("default_currency");

                    b.HasKey("Id")
                        .HasName("pk_settings");

                    b.ToTable("settings", "user", t =>
                        {
                            t.HasCheckConstraint("CK_settings_currency_round_count_Range", "currency_round_count >= 1 AND currency_round_count <= 8");

                            t.HasCheckConstraint("CK_settings_default_currency_Enum", "default_currency IN (0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178)");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
