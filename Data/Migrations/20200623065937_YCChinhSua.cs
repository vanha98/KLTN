using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class YCChinhSua : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "YCChinhSuaDeTai",
                columns: table => new
                {
                    IDDeTai = table.Column<long>(nullable: false),
                    TenDeTai = table.Column<string>(nullable: true),
                    MoTa = table.Column<string>(nullable: true),
                    TepDinhKem = table.Column<string>(nullable: true),
                    TenTep = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YCChinhSuaDeTai");
        }
    }
}
