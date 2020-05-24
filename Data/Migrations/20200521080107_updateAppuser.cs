using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class updateAppuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ho",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Ten",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "SinhVien",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId1",
                table: "SinhVien",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "QuanLy",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId1",
                table: "QuanLy",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "GiangVien",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId1",
                table: "GiangVien",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SinhVien_AppUserId1",
                table: "SinhVien",
                column: "AppUserId1",
                unique: true,
                filter: "[AppUserId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_QuanLy_AppUserId1",
                table: "QuanLy",
                column: "AppUserId1",
                unique: true,
                filter: "[AppUserId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GiangVien_AppUserId1",
                table: "GiangVien",
                column: "AppUserId1",
                unique: true,
                filter: "[AppUserId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_GiangVien_Users_AppUserId1",
                table: "GiangVien",
                column: "AppUserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuanLy_Users_AppUserId1",
                table: "QuanLy",
                column: "AppUserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SinhVien_Users_AppUserId1",
                table: "SinhVien",
                column: "AppUserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiangVien_Users_AppUserId1",
                table: "GiangVien");

            migrationBuilder.DropForeignKey(
                name: "FK_QuanLy_Users_AppUserId1",
                table: "QuanLy");

            migrationBuilder.DropForeignKey(
                name: "FK_SinhVien_Users_AppUserId1",
                table: "SinhVien");

            migrationBuilder.DropIndex(
                name: "IX_SinhVien_AppUserId1",
                table: "SinhVien");

            migrationBuilder.DropIndex(
                name: "IX_QuanLy_AppUserId1",
                table: "QuanLy");

            migrationBuilder.DropIndex(
                name: "IX_GiangVien_AppUserId1",
                table: "GiangVien");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "SinhVien");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "SinhVien");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "QuanLy");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "QuanLy");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "GiangVien");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "GiangVien");

            migrationBuilder.AddColumn<string>(
                name: "Ho",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ten",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
