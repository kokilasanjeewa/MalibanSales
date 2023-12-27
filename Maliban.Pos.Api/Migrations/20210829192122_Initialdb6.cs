using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "StockAdjustmentDetails",
                newName: "AdjustmentQuantity");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "TransactionDetails",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "TransactionDetails",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StockAdjustmentCostAmount",
                table: "StockAdjustmentHeaders",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "StockAdjustmentDate",
                table: "StockAdjustmentHeaders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "StockAdjustmentSaleAmount",
                table: "StockAdjustmentHeaders",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "StockTakenDate",
                table: "StockAdjustmentHeaders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "CostPrice",
                table: "StockAdjustmentDetails",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "PriceId",
                table: "StockAdjustmentDetails",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "RetailPrice",
                table: "StockAdjustmentDetails",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustmentDetails_PriceId",
                table: "StockAdjustmentDetails",
                column: "PriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustmentDetails_Prices_PriceId",
                table: "StockAdjustmentDetails",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "PriceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustmentDetails_Prices_PriceId",
                table: "StockAdjustmentDetails");

            migrationBuilder.DropIndex(
                name: "IX_StockAdjustmentDetails_PriceId",
                table: "StockAdjustmentDetails");

            migrationBuilder.DropColumn(
                name: "StockAdjustmentCostAmount",
                table: "StockAdjustmentHeaders");

            migrationBuilder.DropColumn(
                name: "StockAdjustmentDate",
                table: "StockAdjustmentHeaders");

            migrationBuilder.DropColumn(
                name: "StockAdjustmentSaleAmount",
                table: "StockAdjustmentHeaders");

            migrationBuilder.DropColumn(
                name: "StockTakenDate",
                table: "StockAdjustmentHeaders");

            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "StockAdjustmentDetails");

            migrationBuilder.DropColumn(
                name: "PriceId",
                table: "StockAdjustmentDetails");

            migrationBuilder.DropColumn(
                name: "RetailPrice",
                table: "StockAdjustmentDetails");

            migrationBuilder.RenameColumn(
                name: "AdjustmentQuantity",
                table: "StockAdjustmentDetails",
                newName: "Quantity");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "TransactionDetails",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "TransactionDetails",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);
        }
    }
}
