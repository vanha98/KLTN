using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class addmigrationhoidongnew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenHoiDong",
                table: "HoiDong",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenHoiDong",
                table: "HoiDong");
        }
    }
}
