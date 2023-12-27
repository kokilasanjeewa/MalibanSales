using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class initialdb46 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "Products",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 25);

   /*          migrationBuilder.AddColumn<int>(
                name: "GoodReceptNoteHeaderFactoryId",
                table: "GoodReceptNoteHeaders",
                nullable: false,
                defaultValue: 0); */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
/*             migrationBuilder.DropColumn(
                name: "GoodReceptNoteHeaderFactoryId",
                table: "GoodReceptNoteHeaders"); */

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "Products",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);
        }
    }
}
