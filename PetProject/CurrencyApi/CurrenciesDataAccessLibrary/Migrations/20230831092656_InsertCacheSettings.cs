using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrenciesDataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class InsertCacheSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Insert into ""cur"".cache_settings(base_currency) 
                                        values ('USD')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
