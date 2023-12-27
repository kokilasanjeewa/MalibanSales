using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         /*    migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReturnDetails_PurchaseOrderHeaders_PurchaseOrderHeaderId",
                table: "PurchaseReturnDetails"); 

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReturnDetails_UnitOfMeasurements_UnitOfMeasurementId",
                table: "PurchaseReturnDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReturnHeaders_PurchaseOrderHeaders_PurchaseOrderHeaderId",
                table: "PurchaseReturnHeaders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReturnHeaders_PurchaseOrderHeaderId",
                table: "PurchaseReturnHeaders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReturnDetails_PurchaseOrderHeaderId",
                table: "PurchaseReturnDetails");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReturnDetails_UnitOfMeasurementId",
                table: "PurchaseReturnDetails");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderHeaderId",
                table: "PurchaseReturnHeaders");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "PurchaseReturnDetails");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderHeaderId",
                table: "PurchaseReturnDetails");

            migrationBuilder.DropColumn(
                name: "UnitOfMeasurementId",
                table: "PurchaseReturnDetails");

            migrationBuilder.AddColumn<string>(
                name: "BRNO",
                table: "Suppliers",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "PurchaseReturnDetails",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BRNO",
                table: "Suppliers");

            migrationBuilder.AddColumn<long>(
                name: "PurchaseOrderHeaderId",
                table: "PurchaseReturnHeaders",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "PurchaseReturnDetails",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "PurchaseReturnDetails",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "PurchaseOrderHeaderId",
                table: "PurchaseReturnDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitOfMeasurementId",
                table: "PurchaseReturnDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnHeaders_PurchaseOrderHeaderId",
                table: "PurchaseReturnHeaders",
                column: "PurchaseOrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnDetails_PurchaseOrderHeaderId",
                table: "PurchaseReturnDetails",
                column: "PurchaseOrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnDetails_UnitOfMeasurementId",
                table: "PurchaseReturnDetails",
                column: "UnitOfMeasurementId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReturnDetails_PurchaseOrderHeaders_PurchaseOrderHeaderId",
                table: "PurchaseReturnDetails",
                column: "PurchaseOrderHeaderId",
                principalTable: "PurchaseOrderHeaders",
                principalColumn: "PurchaseOrderHeaderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReturnDetails_UnitOfMeasurements_UnitOfMeasurementId",
                table: "PurchaseReturnDetails",
                column: "UnitOfMeasurementId",
                principalTable: "UnitOfMeasurements",
                principalColumn: "UnitOfMeasurementId",
                onDelete: ReferentialAction.Cascade);

 /*            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReturnHeaders_PurchaseOrderHeaders_PurchaseOrderHeaderId",
                table: "PurchaseReturnHeaders",
                column: "PurchaseOrderHeaderId",
                principalTable: "PurchaseOrderHeaders",
                principalColumn: "PurchaseOrderHeaderId",
                onDelete: ReferentialAction.Restrict); */
        }
    }
}
