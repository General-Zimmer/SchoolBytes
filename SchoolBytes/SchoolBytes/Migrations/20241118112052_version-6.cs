using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolBytes.Migrations
{
    public partial class version6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registration_courseModules_CourseModuleId1",
                table: "Registration");

            migrationBuilder.DropIndex(
                name: "IX_Registration_CourseModuleId1",
                table: "Registration");

            migrationBuilder.DropColumn(
                name: "CourseModuleId1",
                table: "Registration");

            migrationBuilder.AddColumn<string>(
                name: "Waitlist",
                table: "courseModules",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Waitlist",
                table: "courseModules");

            migrationBuilder.AddColumn<int>(
                name: "CourseModuleId1",
                table: "Registration",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registration_CourseModuleId1",
                table: "Registration",
                column: "CourseModuleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Registration_courseModules_CourseModuleId1",
                table: "Registration",
                column: "CourseModuleId1",
                principalTable: "courseModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
