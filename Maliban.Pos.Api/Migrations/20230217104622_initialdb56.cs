using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class initialdb56 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        /*     migrationBuilder.AddColumn<long>(
                name: "TransactionDetailId",
                table: "CustomerReturnDetails",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReturnDetails_TransactionDetailId",
                table: "CustomerReturnDetails",
                column: "TransactionDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerReturnDetails_TransactionDetails_TransactionDetailId",
                table: "CustomerReturnDetails",
                column: "TransactionDetailId",
                principalTable: "TransactionDetails",
                principalColumn: "TransactionDetailId",
                onDelete: ReferentialAction.Cascade); */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
         /*    migrationBuilder.DropForeignKey(
                name: "FK_CustomerReturnDetails_TransactionDetails_TransactionDetailId",
                table: "CustomerReturnDetails");

            migrationBuilder.DropIndex(
                name: "IX_CustomerReturnDetails_TransactionDetailId",
                table: "CustomerReturnDetails");

            migrationBuilder.DropColumn(
                name: "TransactionDetailId",
                table: "CustomerReturnDetails"); */
        }
    }
}
