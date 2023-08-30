using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrenciesDataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class CreatedTableCacheTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "cache_task_info",
                schema: "cur",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    new_base_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cache_task_info", x => x.id);
                    table.CheckConstraint("CK_cache_task_info_new_base_currency_MinLength", "LENGTH(new_base_currency) >= 3");
                });

            migrationBuilder.CreateTable(
                name: "cache_tasks",
                schema: "cur",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cache_tasks", x => x.id);
                    table.CheckConstraint("CK_cache_tasks_status_Enum", "status IN (0, 1, 2, 3, 4)");
                    table.ForeignKey(
                        name: "fk_cache_tasks_cache_task_info_id",
                        column: x => x.id,
                        principalSchema: "cur",
                        principalTable: "cache_task_info",
                        principalColumn: "id");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cache_tasks",
                schema: "cur");

            migrationBuilder.DropTable(
                name: "cache_task_info",
                schema: "cur");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,");
        }
    }
}
