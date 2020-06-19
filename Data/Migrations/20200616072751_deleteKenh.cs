using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class deleteKenh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__BaiPost__IDCTKen__6754599E",
                table: "BaiPost");

            migrationBuilder.DropTable(
                name: "KenhThaoLuan");

            migrationBuilder.DropIndex(
                name: "IX_BaiPost_IDKenhThaoLuan",
                table: "BaiPost");

            migrationBuilder.DropColumn(
                name: "IDKenhThaoLuan",
                table: "BaiPost");

            migrationBuilder.AddColumn<long>(
                name: "IDDeTaiNghienCuu",
                table: "BaiPost",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_BaiPost_IDDeTaiNghienCuu",
                table: "BaiPost",
                column: "IDDeTaiNghienCuu");

            migrationBuilder.AddForeignKey(
                name: "FK__BaiPost__IDDeTa__6754599E",
                table: "BaiPost",
                column: "IDDeTaiNghienCuu",
                principalTable: "DeTaiNghienCuu",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__BaiPost__IDDeTa__6754599E",
                table: "BaiPost");

            migrationBuilder.DropIndex(
                name: "IX_BaiPost_IDDeTaiNghienCuu",
                table: "BaiPost");

            migrationBuilder.DropColumn(
                name: "IDDeTaiNghienCuu",
                table: "BaiPost");

            migrationBuilder.AddColumn<int>(
                name: "IDKenhThaoLuan",
                table: "BaiPost",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "KenhThaoLuan",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDDeTai = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KenhThaoLuan", x => x.ID);
                    table.ForeignKey(
                        name: "FK__KenhThaoL__IDDeTai__60A75C0F",
                        column: x => x.IDDeTai,
                        principalTable: "DeTaiNghienCuu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaiPost_IDKenhThaoLuan",
                table: "BaiPost",
                column: "IDKenhThaoLuan");

            migrationBuilder.CreateIndex(
                name: "IX_KenhThaoLuan_IDDeTai",
                table: "KenhThaoLuan",
                column: "IDDeTai");

            migrationBuilder.AddForeignKey(
                name: "FK__BaiPost__IDCTKen__6754599E",
                table: "BaiPost",
                column: "IDKenhThaoLuan",
                principalTable: "KenhThaoLuan",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
