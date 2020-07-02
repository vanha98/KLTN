using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class lastupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenTep",
                table: "XetDuyetVaDanhGia",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenTep",
                table: "CTXetDuyetVaDanhGia",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TepDinhKem",
                table: "CTXetDuyetVaDanhGia",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenTep",
                table: "XetDuyetVaDanhGia");

            migrationBuilder.DropColumn(
                name: "TenTep",
                table: "CTXetDuyetVaDanhGia");

            migrationBuilder.DropColumn(
                name: "TepDinhKem",
                table: "CTXetDuyetVaDanhGia");
        }
    }
}
