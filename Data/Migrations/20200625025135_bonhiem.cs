using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class bonhiem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__BoNhiem__IDQuanL__4AB81AF0",
                table: "BoNhiem");

            migrationBuilder.DropIndex(
                name: "IX_BoNhiem_IDQuanLy",
                table: "BoNhiem");

            migrationBuilder.DropColumn(
                name: "IDQuanLy",
                table: "BoNhiem");

            migrationBuilder.AddColumn<long>(
                name: "QuanLyId",
                table: "BoNhiem",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<long>(
                name: "IDQuanLy",
                table: "BoNhiem",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoNhiem_IDQuanLy",
                table: "BoNhiem",
                column: "IDQuanLy");

            migrationBuilder.AddForeignKey(
                name: "FK__BoNhiem__IDQuanL__4AB81AF0",
                table: "BoNhiem",
                column: "IDQuanLy",
                principalTable: "QuanLy",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
