using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class updateComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Comments__IDBaiP__6A30C649",
                table: "Comments");

            migrationBuilder.AlterColumn<long>(
                name: "IDNguoiTao",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IDBaiPost",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__Comments__IDBaiP__6A30C649",
                table: "Comments",
                column: "IDBaiPost",
                principalTable: "BaiPost",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Comments__IDBaiP__6A30C649",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "IDNguoiTao",
                table: "Comments",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "IDBaiPost",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK__Comments__IDBaiP__6A30C649",
                table: "Comments",
                column: "IDBaiPost",
                principalTable: "BaiPost",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
