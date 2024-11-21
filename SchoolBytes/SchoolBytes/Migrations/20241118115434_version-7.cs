using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolBytes.Migrations
{
    public partial class version7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Registration");

            migrationBuilder.DropColumn(
                name: "Waitlist",
                table: "courseModules");

            migrationBuilder.CreateTable(
                name: "WaitRegistration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    participantId = table.Column<int>(nullable: true),
                    CourseModuleId = table.Column<int>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitRegistration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaitRegistration_courseModules_CourseModuleId",
                        column: x => x.CourseModuleId,
                        principalTable: "courseModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WaitRegistration_participants_participantId",
                        column: x => x.participantId,
                        principalTable: "participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaitRegistration_CourseModuleId",
                table: "WaitRegistration",
                column: "CourseModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_WaitRegistration_participantId",
                table: "WaitRegistration",
                column: "participantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaitRegistration");

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "Registration",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Waitlist",
                table: "courseModules",
                nullable: true);
        }
    }
}
