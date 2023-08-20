using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Migrations
{
    /// <inheritdoc />
    public partial class CreateSettingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "user");

            migrationBuilder.CreateTable(
                name: "settings",
                schema: "user",
                columns: table => new
                {
                    default_currency = table.Column<int>(type: "integer", nullable: false),
                    currency_round_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_settings", x => new { x.default_currency, x.currency_round_count });
                    table.CheckConstraint("CK_settings_currency_round_count_Range", "currency_round_count >= 1 AND currency_round_count <= 8");
                    table.CheckConstraint("CK_settings_default_currency_Enum", "default_currency IN (0, 1, 2)");
                });

            migrationBuilder.Sql(@"Insert into ""user"".settings(default_currency, currency_round_count)
                                        values(0, 2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "settings",
                schema: "user");
        }
    }
}
