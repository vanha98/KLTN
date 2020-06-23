using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class YeuCauPHeDuyet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "YeuCauPheDuyet",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDDeTai = table.Column<long>(nullable: false),
                    IdNguoiDuyet = table.Column<long>(nullable: false),
                    LoaiYeuCau = table.Column<int>(nullable: false),
                    NgayTao = table.Column<DateTime>(type: "date", nullable: false),
                    NgayDuyet = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YeuCauPheDuyet", x => x.ID);
                    table.ForeignKey(
                        name: "FK__YeuCauPhe__IDDeT__6DCBFF34",
                        column: x => x.IDDeTai,
                        principalTable: "DeTaiNghienCuu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauPheDuyet_IDDeTai",
                table: "YeuCauPheDuyet",
                column: "IDDeTai");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YeuCauPheDuyet");
        }
    }
}
