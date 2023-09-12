using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CurrenciesDataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class CreatedTableCacheSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cache_settings",
                schema: "cur",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    base_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cache_settings", x => x.id);
                    table.CheckConstraint("CK_cache_settings_base_currency_MinLength", "LENGTH(base_currency) >= 3");
                });

            migrationBuilder.AddCheckConstraint(
                name: "date_time_before_or_equal_now",
                schema: "cur",
                table: "cache_tasks",
                sql: "created_at at time zone 'UTC' <= timezone('UTC', now())");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cache_settings",
                schema: "cur");

            migrationBuilder.DropCheckConstraint(
                name: "date_time_before_or_equal_now",
                schema: "cur",
                table: "cache_tasks");
        }
    }
}
