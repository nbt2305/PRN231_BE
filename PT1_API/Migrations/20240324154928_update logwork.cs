using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PT1_API.Migrations
{
    public partial class updatelogwork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectCode",
                table: "LogWorks");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "LogWorks",
                type: "int",
                maxLength: 250,
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "LogWorks");

            migrationBuilder.AddColumn<string>(
                name: "ProjectCode",
                table: "LogWorks",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }
    }
}
