using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternalAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreatedTableCachedExchangeRates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cur");

            migrationBuilder.CreateTable(
                name: "cached_exchange_rates",
                schema: "cur",
                columns: table => new
                {
                    relevant_on_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    base_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    exchange_rates_json = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cached_exchange_rates", x => x.relevant_on_date);
                    table.CheckConstraint("CK_cached_exchange_rates_base_currency_MinLength", "LENGTH(base_currency) >= 3");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cached_exchange_rates",
                schema: "cur");
        }
    }
}
