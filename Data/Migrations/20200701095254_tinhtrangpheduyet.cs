using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class tinhtrangpheduyet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TinhTrangPheDuyet",
                table: "DeTaiNghienCuu",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TinhTrangPheDuyet",
                table: "DeTaiNghienCuu");
        }
    }
}
