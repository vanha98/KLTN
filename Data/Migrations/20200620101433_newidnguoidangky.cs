using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class newidnguoidangky : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "IDNguoiDangKy",
                table: "DeTaiNghienCuu",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IDNguoiDangKy",
                table: "DeTaiNghienCuu");
        }
    }
}
