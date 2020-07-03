using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class updatefinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenTepCauTraLoi",
                table: "CTXetDuyetVaDanhGia",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TepDinhKemCauTraLoi",
                table: "CTXetDuyetVaDanhGia",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenTepCauTraLoi",
                table: "CTXetDuyetVaDanhGia");

            migrationBuilder.DropColumn(
                name: "TepDinhKemCauTraLoi",
                table: "CTXetDuyetVaDanhGia");
        }
    }
}
