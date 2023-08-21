using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternalAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDateConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "date_time_before_or_equal_now",
                schema: "cur",
                table: "cached_exchange_rates",
                sql: "relevant_on_date at time zone 'UTC' <= timezone('UTC', now())");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "date_time_before_or_equal_now",
                schema: "cur",
                table: "cached_exchange_rates");
        }
    }
}
