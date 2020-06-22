using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Kenhthaoluanupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__KenhThaoL__IDGia__60A75C0F",
                table: "KenhThaoLuan");

            migrationBuilder.DropIndex(
                name: "IX_KenhThaoLuan_IDGiangVien",
                table: "KenhThaoLuan");

            migrationBuilder.DropColumn(
                name: "IDGiangVien",
                table: "KenhThaoLuan");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "IDGiangVien",
                table: "KenhThaoLuan",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KenhThaoLuan_IDGiangVien",
                table: "KenhThaoLuan",
                column: "IDGiangVien");

            migrationBuilder.AddForeignKey(
                name: "FK__KenhThaoL__IDGia__60A75C0F",
                table: "KenhThaoLuan",
                column: "IDGiangVien",
                principalTable: "GiangVien",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
