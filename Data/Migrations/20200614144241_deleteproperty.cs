using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class deleteproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TepDinhKem",
                table: "DeTaiNghienCuu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "TepDinhKem",
                table: "DeTaiNghienCuu",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
