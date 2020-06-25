using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class newhoidong2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "IdNguoiTao",
                table: "HoiDong",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdNguoiTao",
                table: "HoiDong");
        }
    }
}
