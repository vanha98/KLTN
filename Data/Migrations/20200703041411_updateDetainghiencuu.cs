using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class updateDetainghiencuu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IDDot",
                table: "DeTaiNghienCuu",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayDangKy",
                table: "DeTaiNghienCuu",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IDDot",
                table: "DeTaiNghienCuu");

            migrationBuilder.DropColumn(
                name: "NgayDangKy",
                table: "DeTaiNghienCuu");
        }
    }
}
