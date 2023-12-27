using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class initialdb47 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
/*             migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "SupplierPrices",
                type: "decimal(18, 5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)"); */

          /*   migrationBuilder.AddColumn<long>(
                name: "GoodReceptNoteHeaderId",
                table: "PurchaseReturnHeaders",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "MI_Employer_Employees",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnHeaders_GoodReceptNoteHeaderId",
                table: "PurchaseReturnHeaders",
                column: "GoodReceptNoteHeaderId"); */
/* 
            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReturnHeaders_GoodReceptNoteHeaders_GoodReceptNoteHeaderId",
                table: "PurchaseReturnHeaders",
                column: "GoodReceptNoteHeaderId",
                principalTable: "GoodReceptNoteHeaders",
                principalColumn: "GoodReceptNoteHeaderId",
                onDelete: ReferentialAction.Restrict); */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           /*  migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReturnHeaders_GoodReceptNoteHeaders_GoodReceptNoteHeaderId",
                table: "PurchaseReturnHeaders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReturnHeaders_GoodReceptNoteHeaderId",
                table: "PurchaseReturnHeaders");

            migrationBuilder.DropColumn(
                name: "GoodReceptNoteHeaderId",
                table: "PurchaseReturnHeaders");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "SupplierPrices",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 5)");

            migrationBuilder.AlterColumn<int>(
                name: "Location",
                table: "MI_Employer_Employees",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2); */
        }
    }
}
