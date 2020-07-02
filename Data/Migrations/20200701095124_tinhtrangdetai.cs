using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class tinhtrangdetai : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TinhTrangPheDuyet",
                table: "DeTaiNghienCuu");

            migrationBuilder.AddColumn<int>(
                name: "TinhTrangDeTai",
                table: "DeTaiNghienCuu",
                nullable: true,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TinhTrangDeTai",
                table: "DeTaiNghienCuu");

            migrationBuilder.AddColumn<int>(
                name: "TinhTrangPheDuyet",
                table: "DeTaiNghienCuu",
                type: "int",
                nullable: true,
                defaultValue: 1);
        }
    }
}
