using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class updateDTNC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "DeTaiNghienCuu");

            migrationBuilder.AddColumn<int>(
                name: "TinhTrangDangKy",
                table: "DeTaiNghienCuu",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TinhTrangPheDuyet",
                table: "DeTaiNghienCuu",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TinhTrangDangKy",
                table: "DeTaiNghienCuu");

            migrationBuilder.DropColumn(
                name: "TinhTrangPheDuyet",
                table: "DeTaiNghienCuu");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "DeTaiNghienCuu",
                type: "int",
                nullable: true);
        }
    }
}
