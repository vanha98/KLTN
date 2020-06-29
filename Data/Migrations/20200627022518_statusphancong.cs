using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class statusphancong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusPhanCong",
                table: "HoiDong",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusPhanCong",
                table: "HoiDong");
        }
    }
}
