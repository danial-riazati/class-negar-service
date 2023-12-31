using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassNegarService.Migrations
{
    public partial class dbclassadded10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EnrollmentId",
                table: "StudentAttendances",
                newName: "SessionId");

            migrationBuilder.AddColumn<int>(
                name: "SessionId",
                table: "ProfessorAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "ProfessorAttendances");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "StudentAttendances",
                newName: "EnrollmentId");
        }
    }
}
