using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskQueue.DAO.Migrations
{
    public partial class UpdateColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UsersProfiles");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "UsersProfiles",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "UsersProfiles");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UsersProfiles",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);
        }
    }
}
