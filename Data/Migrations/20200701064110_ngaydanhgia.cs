using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ngaydanhgia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NgayTao",
                table: "CTXetDuyetVaDanhGia",
                newName: "NgayTaoCauHoi");

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayDanhGia",
                table: "CTXetDuyetVaDanhGia",
                type: "datetime",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayDanhGia",
                table: "CTXetDuyetVaDanhGia");

            migrationBuilder.RenameColumn(
                name: "NgayTaoCauHoi",
                table: "CTXetDuyetVaDanhGia",
                newName: "NgayTao");
        }
    }
}
