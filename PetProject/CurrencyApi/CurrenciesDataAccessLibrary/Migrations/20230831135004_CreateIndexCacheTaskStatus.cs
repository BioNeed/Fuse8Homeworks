using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrenciesDataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class CreateIndexCacheTaskStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_cache_tasks_status",
                schema: "cur",
                table: "cache_tasks",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_cache_tasks_status",
                schema: "cur",
                table: "cache_tasks");
        }
    }
}
