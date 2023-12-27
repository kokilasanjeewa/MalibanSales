using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustmentHeaders_AdjustmentTypes_AdjustmentTypeId",
                table: "StockAdjustmentHeaders");

            migrationBuilder.AlterColumn<int>(
                name: "AdjustmentTypeId",
                table: "StockAdjustmentHeaders",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustmentHeaders_AdjustmentTypes_AdjustmentTypeId",
                table: "StockAdjustmentHeaders",
                column: "AdjustmentTypeId",
                principalTable: "AdjustmentTypes",
                principalColumn: "AdjustmentTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustmentHeaders_AdjustmentTypes_AdjustmentTypeId",
                table: "StockAdjustmentHeaders");

            migrationBuilder.AlterColumn<int>(
                name: "AdjustmentTypeId",
                table: "StockAdjustmentHeaders",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustmentHeaders_AdjustmentTypes_AdjustmentTypeId",
                table: "StockAdjustmentHeaders",
                column: "AdjustmentTypeId",
                principalTable: "AdjustmentTypes",
                principalColumn: "AdjustmentTypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
