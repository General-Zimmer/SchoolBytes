using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolBytes.Migrations
{
    public partial class version8attendanceregistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Attendance",
                table: "Registration",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attendance",
                table: "Registration");
        }
    }
}
