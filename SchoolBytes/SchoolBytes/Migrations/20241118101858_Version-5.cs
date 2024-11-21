using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolBytes.Migrations
{
    public partial class Version5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_participants_courseModules_CourseModuleId",
                table: "participants");

            migrationBuilder.DropIndex(
                name: "IX_participants_CourseModuleId",
                table: "participants");

            migrationBuilder.DropColumn(
                name: "CourseModuleId",
                table: "participants");

            migrationBuilder.AddColumn<int>(
                name: "CourseModuleId1",
                table: "Registration",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "Registration",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Registration");

            migrationBuilder.AddColumn<int>(
                name: "CourseModuleId",
                table: "participants",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_participants_CourseModuleId",
                table: "participants",
                column: "CourseModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_participants_courseModules_CourseModuleId",
                table: "participants",
                column: "CourseModuleId",
                principalTable: "courseModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
