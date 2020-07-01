using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class updateMoDot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiemToiDa",
                table: "MoDot",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiemToiThieu",
                table: "MoDot",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiemToiDa",
                table: "MoDot");

            migrationBuilder.DropColumn(
                name: "DiemToiThieu",
                table: "MoDot");
        }
    }
}
