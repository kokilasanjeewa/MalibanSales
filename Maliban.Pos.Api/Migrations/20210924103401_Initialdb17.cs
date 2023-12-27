using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReturnDetails_PurchaseReturnHeaders_PurchaseReturnHeaderId",
                table: "PurchaseReturnDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReturnDetails_PurchaseReturnHeaders_PurchaseReturnHeaderId",
                table: "PurchaseReturnDetails",
                column: "PurchaseReturnHeaderId",
                principalTable: "PurchaseReturnHeaders",
                principalColumn: "PurchaseReturnHeaderId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReturnDetails_PurchaseReturnHeaders_PurchaseReturnHeaderId",
                table: "PurchaseReturnDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReturnDetails_PurchaseReturnHeaders_PurchaseReturnHeaderId",
                table: "PurchaseReturnDetails",
                column: "PurchaseReturnHeaderId",
                principalTable: "PurchaseReturnHeaders",
                principalColumn: "PurchaseReturnHeaderId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
