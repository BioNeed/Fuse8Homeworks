using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UserDataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class CreateFavouriteExchangeRatesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "favourite_exchange_rate",
                schema: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    base_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_favourite_exchange_rate", x => x.id);
                    table.CheckConstraint("CK_favourite_exchange_rate_base_currency_MinLength", "LENGTH(base_currency) >= 3");
                    table.CheckConstraint("CK_favourite_exchange_rate_currency_MinLength", "LENGTH(currency) >= 3");
                    table.CheckConstraint("not_equal_currencies", "currency != base_currency");
                });

            migrationBuilder.CreateIndex(
                name: "ix_favourite_exchange_rate_currency_base_currency",
                schema: "user",
                table: "favourite_exchange_rate",
                columns: new[] { "currency", "base_currency" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_favourite_exchange_rate_name",
                schema: "user",
                table: "favourite_exchange_rate",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "favourite_exchange_rate",
                schema: "user");
        }
    }
}
