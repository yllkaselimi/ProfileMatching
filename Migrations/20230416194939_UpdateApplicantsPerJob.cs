using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfileMatching.Migrations
{
    public partial class UpdateApplicantsPerJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "JobPosts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicationDate",
                table: "ApplicantsPerJobs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "HiredStatus",
                table: "ApplicantsPerJobs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ActivityDescription",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "RecentLogins",
                columns: table => new
                {
                    RecentLoginID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecentLogins", x => x.RecentLoginID);
                    table.ForeignKey(
                        name: "FK_RecentLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecentLogins_UserId",
                table: "RecentLogins",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecentLogins");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "JobPosts");

            migrationBuilder.DropColumn(
                name: "ApplicationDate",
                table: "ApplicantsPerJobs");

            migrationBuilder.DropColumn(
                name: "HiredStatus",
                table: "ApplicantsPerJobs");

            migrationBuilder.AlterColumn<string>(
                name: "ActivityDescription",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
