using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class updatedata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TepDinhKem",
                table: "DeTaiNghienCuu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TepDinhKem",
                table: "DeTaiNghienCuu",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
