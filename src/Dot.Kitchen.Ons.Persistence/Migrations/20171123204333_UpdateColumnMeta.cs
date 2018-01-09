using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Dot.Kitchen.Ons.Persistence.Migrations
{
    public partial class UpdateColumnMeta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Sources",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfRecordsScraped",
                table: "Scrapes",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinishedAt",
                table: "Scrapes",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Sources_Name",
                table: "Sources",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Sources_Name",
                table: "Sources");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Sources",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfRecordsScraped",
                table: "Scrapes",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinishedAt",
                table: "Scrapes",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
