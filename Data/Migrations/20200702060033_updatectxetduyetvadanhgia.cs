using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class updatectxetduyetvadanhgia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenTep",
                table: "CTXetDuyetVaDanhGia");

            migrationBuilder.DropColumn(
                name: "TepDinhKem",
                table: "CTXetDuyetVaDanhGia");

            migrationBuilder.AddColumn<string>(
                name: "TenTepCauHoi",
                table: "CTXetDuyetVaDanhGia",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TepDinhKemCauHoi",
                table: "CTXetDuyetVaDanhGia",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenTepCauHoi",
                table: "CTXetDuyetVaDanhGia");

            migrationBuilder.DropColumn(
                name: "TepDinhKemCauHoi",
                table: "CTXetDuyetVaDanhGia");

            migrationBuilder.AddColumn<string>(
                name: "TenTep",
                table: "CTXetDuyetVaDanhGia",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TepDinhKem",
                table: "CTXetDuyetVaDanhGia",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
