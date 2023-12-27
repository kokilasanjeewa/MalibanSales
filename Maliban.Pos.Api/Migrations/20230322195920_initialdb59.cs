using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class initialdb59 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
/*             migrationBuilder.AddColumn<decimal>(
                name: "Profit",
                table: "SupplierPrices",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsApprove",
                table: "StockAdjustmentHeaders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "ApproveQuantity",
                table: "StockAdjustmentDetails",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProductExpiryDate",
                table: "PurchaseOrderDetails",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
 */        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {/* 
            migrationBuilder.DropColumn(
                name: "Profit",
                table: "SupplierPrices");

            migrationBuilder.DropColumn(
                name: "IsApprove",
                table: "StockAdjustmentHeaders");

            migrationBuilder.DropColumn(
                name: "ApproveQuantity",
                table: "StockAdjustmentDetails");

            migrationBuilder.DropColumn(
                name: "ProductExpiryDate",
                table: "PurchaseOrderDetails"); */
        }
    }
}
