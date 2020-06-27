using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class updateQL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoNhiem_QuanLy_QuanLyId",
                table: "BoNhiem");

            migrationBuilder.DropIndex(
                name: "IX_BoNhiem_QuanLyId",
                table: "BoNhiem");

            migrationBuilder.DropColumn(
                name: "QuanLyId",
                table: "BoNhiem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "QuanLyId",
                table: "BoNhiem",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoNhiem_QuanLyId",
                table: "BoNhiem",
                column: "QuanLyId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoNhiem_QuanLy_QuanLyId",
                table: "BoNhiem",
                column: "QuanLyId",
                principalTable: "QuanLy",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
