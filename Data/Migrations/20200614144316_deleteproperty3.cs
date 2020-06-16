using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class deleteproperty3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TepDinhKem",
                table: "DeTaiNghienCuu",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TepDinhKem",
                table: "DeTaiNghienCuu");
        }
    }
}
