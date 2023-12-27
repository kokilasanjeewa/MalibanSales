using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StockAdjustmentHeaders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustmentHeaders_Id",
                table: "StockAdjustmentHeaders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustmentHeaders_AspNetUsers_Id",
                table: "StockAdjustmentHeaders",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustmentHeaders_AspNetUsers_Id",
                table: "StockAdjustmentHeaders");

            migrationBuilder.DropIndex(
                name: "IX_StockAdjustmentHeaders_Id",
                table: "StockAdjustmentHeaders");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StockAdjustmentHeaders");
        }
    }
}
