using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class hoidongupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NgaySua",
                table: "HoiDong",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NguoiSua",
                table: "HoiDong",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgaySua",
                table: "HoiDong");

            migrationBuilder.DropColumn(
                name: "NguoiSua",
                table: "HoiDong");
        }
    }
}
