using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReturnDetails_SupplierPrices_SupplierPriceId",
                table: "PurchaseReturnDetails");

            migrationBuilder.RenameColumn(
                name: "SupplierPriceId",
                table: "PurchaseReturnDetails",
                newName: "PriceId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseReturnDetails_SupplierPriceId",
                table: "PurchaseReturnDetails",
                newName: "IX_PurchaseReturnDetails_PriceId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "Products",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<decimal>(
                name: "ModifiedUnitPrice",
                table: "Prices",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReturnDetails_Prices_PriceId",
                table: "PurchaseReturnDetails",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "PriceId",
                onDelete: ReferentialAction.Cascade); 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReturnDetails_Prices_PriceId",
                table: "PurchaseReturnDetails");

            migrationBuilder.DropColumn(
                name: "ModifiedUnitPrice",
                table: "Prices");

            migrationBuilder.RenameColumn(
                name: "PriceId",
                table: "PurchaseReturnDetails",
                newName: "SupplierPriceId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseReturnDetails_PriceId",
                table: "PurchaseReturnDetails",
                newName: "IX_PurchaseReturnDetails_SupplierPriceId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "Products",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReturnDetails_SupplierPrices_SupplierPriceId",
                table: "PurchaseReturnDetails",
                column: "SupplierPriceId",
                principalTable: "SupplierPrices",
                principalColumn: "SupplierPriceId",
                onDelete: ReferentialAction.Cascade); 
        }
    }
}
