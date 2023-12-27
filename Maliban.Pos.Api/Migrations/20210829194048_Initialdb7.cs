using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustmentDetails_Prices_PriceId",
                table: "StockAdjustmentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustmentDetails_StockAdjustmentHeaders_StockAdjustmentHeaderId",
                table: "StockAdjustmentDetails");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "StockAdjustmentDetails",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustmentDetails_Prices_PriceId",
                table: "StockAdjustmentDetails",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "PriceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustmentDetails_StockAdjustmentHeaders_StockAdjustmentHeaderId",
                table: "StockAdjustmentDetails",
                column: "StockAdjustmentHeaderId",
                principalTable: "StockAdjustmentHeaders",
                principalColumn: "StockAdjustmentHeaderId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustmentDetails_Prices_PriceId",
                table: "StockAdjustmentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustmentDetails_StockAdjustmentHeaders_StockAdjustmentHeaderId",
                table: "StockAdjustmentDetails");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "StockAdjustmentDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustmentDetails_Prices_PriceId",
                table: "StockAdjustmentDetails",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "PriceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustmentDetails_StockAdjustmentHeaders_StockAdjustmentHeaderId",
                table: "StockAdjustmentDetails",
                column: "StockAdjustmentHeaderId",
                principalTable: "StockAdjustmentHeaders",
                principalColumn: "StockAdjustmentHeaderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
