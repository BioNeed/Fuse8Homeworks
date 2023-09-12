using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserDataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_favourite_exchange_rate",
                schema: "user",
                table: "favourite_exchange_rate");

            migrationBuilder.DropCheckConstraint(
                name: "CK_favourite_exchange_rate_base_currency_MinLength",
                schema: "user",
                table: "favourite_exchange_rate");

            migrationBuilder.DropCheckConstraint(
                name: "CK_favourite_exchange_rate_currency_MinLength",
                schema: "user",
                table: "favourite_exchange_rate");

            migrationBuilder.RenameTable(
                name: "favourite_exchange_rate",
                schema: "user",
                newName: "favourites",
                newSchema: "user");

            migrationBuilder.RenameIndex(
                name: "ix_favourite_exchange_rate_name",
                schema: "user",
                table: "favourites",
                newName: "ix_favourites_name");

            migrationBuilder.RenameIndex(
                name: "ix_favourite_exchange_rate_currency_base_currency",
                schema: "user",
                table: "favourites",
                newName: "ix_favourites_currency_base_currency");

            migrationBuilder.AddPrimaryKey(
                name: "pk_favourites",
                schema: "user",
                table: "favourites",
                column: "id");

            migrationBuilder.AddCheckConstraint(
                name: "CK_favourites_base_currency_MinLength",
                schema: "user",
                table: "favourites",
                sql: "LENGTH(base_currency) >= 3");

            migrationBuilder.AddCheckConstraint(
                name: "CK_favourites_currency_MinLength",
                schema: "user",
                table: "favourites",
                sql: "LENGTH(currency) >= 3");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_favourites",
                schema: "user",
                table: "favourites");

            migrationBuilder.DropCheckConstraint(
                name: "CK_favourites_base_currency_MinLength",
                schema: "user",
                table: "favourites");

            migrationBuilder.DropCheckConstraint(
                name: "CK_favourites_currency_MinLength",
                schema: "user",
                table: "favourites");

            migrationBuilder.RenameTable(
                name: "favourites",
                schema: "user",
                newName: "favourite_exchange_rate",
                newSchema: "user");

            migrationBuilder.RenameIndex(
                name: "ix_favourites_name",
                schema: "user",
                table: "favourite_exchange_rate",
                newName: "ix_favourite_exchange_rate_name");

            migrationBuilder.RenameIndex(
                name: "ix_favourites_currency_base_currency",
                schema: "user",
                table: "favourite_exchange_rate",
                newName: "ix_favourite_exchange_rate_currency_base_currency");

            migrationBuilder.AddPrimaryKey(
                name: "pk_favourite_exchange_rate",
                schema: "user",
                table: "favourite_exchange_rate",
                column: "id");

            migrationBuilder.AddCheckConstraint(
                name: "CK_favourite_exchange_rate_base_currency_MinLength",
                schema: "user",
                table: "favourite_exchange_rate",
                sql: "LENGTH(base_currency) >= 3");

            migrationBuilder.AddCheckConstraint(
                name: "CK_favourite_exchange_rate_currency_MinLength",
                schema: "user",
                table: "favourite_exchange_rate",
                sql: "LENGTH(currency) >= 3");
        }
    }
}
