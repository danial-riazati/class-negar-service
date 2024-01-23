using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassNegarService.Migrations
{
    public partial class dbclassadded12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RfidTag",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RfidTag",
                table: "Users");
        }
    }
}
