using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolBytes.Migrations
{
    public partial class partiRegiFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_participants_courseModules_CourseModuleId1",
                table: "participants");

            migrationBuilder.DropIndex(
                name: "IX_participants_CourseModuleId1",
                table: "participants");

            migrationBuilder.DropColumn(
                name: "CourseModuleId1",
                table: "participants");

            migrationBuilder.CreateTable(
                name: "Registration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    participantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registration_courseModules_Id",
                        column: x => x.Id,
                        principalTable: "courseModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registration_participants_participantId",
                        column: x => x.participantId,
                        principalTable: "participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Registration_participantId",
                table: "Registration",
                column: "participantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registration");

            migrationBuilder.AddColumn<int>(
                name: "CourseModuleId1",
                table: "participants",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_participants_CourseModuleId1",
                table: "participants",
                column: "CourseModuleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_participants_courseModules_CourseModuleId1",
                table: "participants",
                column: "CourseModuleId1",
                principalTable: "courseModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
