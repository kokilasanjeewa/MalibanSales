using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "TransactionDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "TransactionDetails",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "TransactionDetails");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "TransactionDetails");
        }
    }
}
