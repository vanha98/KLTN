using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class newupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TinhTrangDot1",
                table: "DeTaiNghienCuu");

            migrationBuilder.DropColumn(
                name: "TinhTrangDot2",
                table: "DeTaiNghienCuu");

            migrationBuilder.DropColumn(
                name: "IDNguoiTao",
                table: "CTXetDuyetVaDanhGia");

            migrationBuilder.AddColumn<long>(
                name: "IDGiangVien",
                table: "CTXetDuyetVaDanhGia",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "VaiTro",
                table: "CTXetDuyetVaDanhGia",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CTXetDuyetVaDanhGia_IDGiangVien",
                table: "CTXetDuyetVaDanhGia",
                column: "IDGiangVien");

            migrationBuilder.AddForeignKey(
                name: "FK__CTXetDuye__IDGia__7AEE31B7",
                table: "CTXetDuyetVaDanhGia",
                column: "IDGiangVien",
                principalTable: "GiangVien",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__CTXetDuye__IDGia__7AEE31B7",
                table: "CTXetDuyetVaDanhGia");

            migrationBuilder.DropIndex(
                name: "IX_CTXetDuyetVaDanhGia_IDGiangVien",
                table: "CTXetDuyetVaDanhGia");

            migrationBuilder.DropColumn(
                name: "IDGiangVien",
                table: "CTXetDuyetVaDanhGia");

            migrationBuilder.DropColumn(
                name: "VaiTro",
                table: "CTXetDuyetVaDanhGia");

            migrationBuilder.AddColumn<int>(
                name: "TinhTrangDot1",
                table: "DeTaiNghienCuu",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TinhTrangDot2",
                table: "DeTaiNghienCuu",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IDNguoiTao",
                table: "CTXetDuyetVaDanhGia",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
