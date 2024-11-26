using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolBytes.Migrations
{
    public partial class QRRegistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Attendance",
                table: "Registration",
                nullable: false,
                defaultValue: false);
        }

        
    }
}
