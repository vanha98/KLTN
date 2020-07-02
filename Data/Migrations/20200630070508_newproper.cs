using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class newproper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TinhTrangDot1",
                table: "DeTaiNghienCuu",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TinhTrangDot2",
                table: "DeTaiNghienCuu",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TinhTrangDot1",
                table: "DeTaiNghienCuu");

            migrationBuilder.DropColumn(
                name: "TinhTrangDot2",
                table: "DeTaiNghienCuu");
        }
    }
}
