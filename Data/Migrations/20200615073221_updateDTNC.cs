using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class updateDTNC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__DeTaiNghi__IDNho__4F7CD00D",
                table: "DeTaiNghienCuu");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Nhom_Sin__CD149E43F71CE773",
                table: "Nhom_SinhVien");

            migrationBuilder.DropIndex(
                name: "IX_DeTaiNghienCuu_IDNhom",
                table: "DeTaiNghienCuu");

            migrationBuilder.DropColumn(
                name: "IDNhom",
                table: "DeTaiNghienCuu");

            migrationBuilder.DropColumn(
                name: "NgaySVDangKy",
                table: "DeTaiNghienCuu");

            migrationBuilder.AddColumn<long>(
                name: "IDDeTaiNghienCuu",
                table: "Nhom_SinhVien",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK__Nhom_Sin__CD149E43F71CE773",
                table: "Nhom_SinhVien",
                columns: new[] { "IDNhom", "IDSinhVien", "IDDeTaiNghienCuu" });

            migrationBuilder.CreateIndex(
                name: "IX_Nhom_SinhVien_IDDeTaiNghienCuu",
                table: "Nhom_SinhVien",
                column: "IDDeTaiNghienCuu");

            migrationBuilder.AddForeignKey(
                name: "FK__Nhom_Sinh__IDDet__3F416244",
                table: "Nhom_SinhVien",
                column: "IDDeTaiNghienCuu",
                principalTable: "DeTaiNghienCuu",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Nhom_Sinh__IDDet__3F416244",
                table: "Nhom_SinhVien");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Nhom_Sin__CD149E43F71CE773",
                table: "Nhom_SinhVien");

            migrationBuilder.DropIndex(
                name: "IX_Nhom_SinhVien_IDDeTaiNghienCuu",
                table: "Nhom_SinhVien");

            migrationBuilder.DropColumn(
                name: "IDDeTaiNghienCuu",
                table: "Nhom_SinhVien");

            migrationBuilder.AddColumn<int>(
                name: "IDNhom",
                table: "DeTaiNghienCuu",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgaySVDangKy",
                table: "DeTaiNghienCuu",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__Nhom_Sin__CD149E43F71CE773",
                table: "Nhom_SinhVien",
                columns: new[] { "IDNhom", "IDSinhVien" });

            migrationBuilder.CreateIndex(
                name: "IX_DeTaiNghienCuu_IDNhom",
                table: "DeTaiNghienCuu",
                column: "IDNhom");

            migrationBuilder.AddForeignKey(
                name: "FK__DeTaiNghi__IDNho__4F7CD00D",
                table: "DeTaiNghienCuu",
                column: "IDNhom",
                principalTable: "Nhom",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
