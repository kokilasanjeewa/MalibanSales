using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerReturnDetails_BranchLocations_BranchLocationId",
                table: "CustomerReturnDetails");

            migrationBuilder.DropIndex(
                name: "IX_CustomerReturnDetails_BranchLocationId",
                table: "CustomerReturnDetails");

            migrationBuilder.RenameColumn(
                name: "BranchLocationId",
                table: "CustomerReturnDetails",
                newName: "SaleQuantity");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeCode",
                table: "CustomerReturnHeaders",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CustomerReturnHeaders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "CustomerReturnHeaders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReturnHeaders_Id",
                table: "CustomerReturnHeaders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReturnHeaders_PaymentId",
                table: "CustomerReturnHeaders",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerReturnHeaders_AspNetUsers_Id",
                table: "CustomerReturnHeaders",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerReturnHeaders_Payments_PaymentId",
                table: "CustomerReturnHeaders",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "PaymentId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerReturnHeaders_AspNetUsers_Id",
                table: "CustomerReturnHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerReturnHeaders_Payments_PaymentId",
                table: "CustomerReturnHeaders");

            migrationBuilder.DropIndex(
                name: "IX_CustomerReturnHeaders_Id",
                table: "CustomerReturnHeaders");

            migrationBuilder.DropIndex(
                name: "IX_CustomerReturnHeaders_PaymentId",
                table: "CustomerReturnHeaders");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CustomerReturnHeaders");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "CustomerReturnHeaders");

            migrationBuilder.RenameColumn(
                name: "SaleQuantity",
                table: "CustomerReturnDetails",
                newName: "BranchLocationId");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeCode",
                table: "CustomerReturnHeaders",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReturnDetails_BranchLocationId",
                table: "CustomerReturnDetails",
                column: "BranchLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerReturnDetails_BranchLocations_BranchLocationId",
                table: "CustomerReturnDetails",
                column: "BranchLocationId",
                principalTable: "BranchLocations",
                principalColumn: "BranchLocationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
