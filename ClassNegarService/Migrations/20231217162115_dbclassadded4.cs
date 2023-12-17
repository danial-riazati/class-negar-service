using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassNegarService.Migrations
{
    public partial class dbclassadded4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeOfDay",
                table: "ClassTimes",
                newName: "StartAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndAt",
                table: "ClassTimes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ClassResouces",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndAt",
                table: "ClassTimes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ClassResouces");

            migrationBuilder.RenameColumn(
                name: "StartAt",
                table: "ClassTimes",
                newName: "TimeOfDay");
        }
    }
}
