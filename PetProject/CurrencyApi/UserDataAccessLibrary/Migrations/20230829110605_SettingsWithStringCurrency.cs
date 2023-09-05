﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserDataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class SettingsWithStringCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_settings_default_currency_Enum",
                schema: "user",
                table: "settings");

            migrationBuilder.AlterColumn<string>(
                name: "default_currency",
                schema: "user",
                table: "settings",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddCheckConstraint(
                name: "CK_settings_default_currency_MinLength",
                schema: "user",
                table: "settings",
                sql: "LENGTH(default_currency) >= 3");

            migrationBuilder.Sql(@"Insert into ""user"".settings(default_currency, currency_round_count) 
                                        values ('RUB', 2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_settings_default_currency_MinLength",
                schema: "user",
                table: "settings");

            migrationBuilder.AlterColumn<int>(
                name: "default_currency",
                schema: "user",
                table: "settings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3);

            migrationBuilder.AddCheckConstraint(
                name: "CK_settings_default_currency_Enum",
                schema: "user",
                table: "settings",
                sql: "default_currency IN (0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178)");
        }
    }
}