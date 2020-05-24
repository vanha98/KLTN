using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class newProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenTep",
                table: "DeTaiNghienCuu",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TepDinhKem",
                table: "DeTaiNghienCuu",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenTep",
                table: "DeTaiNghienCuu");

            migrationBuilder.DropColumn(
                name: "TepDinhKem",
                table: "DeTaiNghienCuu");
        }
    }
}
