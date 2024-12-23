using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolBytes.Migrations
{
    public partial class Version9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "foodModules",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "courseModules",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "foodModules");

            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "courseModules");
        }
    }
}
