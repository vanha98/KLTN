using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class YCChinhSuaUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "YCChinhSuaDeTai",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_YCChinhSuaDeTai",
                table: "YCChinhSuaDeTai",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_YCChinhSuaDeTai_IDDeTai",
                table: "YCChinhSuaDeTai",
                column: "IDDeTai");

            migrationBuilder.AddForeignKey(
                name: "FK__YCChinh__IDDeT__6DZFFF12",
                table: "YCChinhSuaDeTai",
                column: "IDDeTai",
                principalTable: "DeTaiNghienCuu",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__YCChinh__IDDeT__6DZFFF12",
                table: "YCChinhSuaDeTai");

            migrationBuilder.DropPrimaryKey(
                name: "PK_YCChinhSuaDeTai",
                table: "YCChinhSuaDeTai");

            migrationBuilder.DropIndex(
                name: "IX_YCChinhSuaDeTai_IDDeTai",
                table: "YCChinhSuaDeTai");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "YCChinhSuaDeTai");
        }
    }
}
