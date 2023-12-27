using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
	public partial class initialdb57 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
/*             migrationBuilder.DropForeignKey(
				name: "FK_UserBranchLocations_AspNetUsers_Id",
				table: "UserBranchLocations");

			migrationBuilder.DropIndex(
				name: "IX_UserBranchLocations_Id",
				table: "UserBranchLocations");

			migrationBuilder.DropUniqueConstraint(
				name: "AK_ProductCategories_ProductCategoryId",
				table: "ProductCategories");

			migrationBuilder.AddColumn<int>(
				name: "ApplicationUsersId",
				table: "UserBranchLocations",
				nullable: true);

			migrationBuilder.AddColumn<long>(
				name: "TransactionDetailId",
				table: "StockAdjustmentDetails",
				nullable: false,
				defaultValue: 0L);

			migrationBuilder.AddColumn<long>(
				name: "TransactionDetailId",
				table: "PurchaseReturnDetails",
				nullable: false,
				defaultValue: 0L);

			migrationBuilder.CreateIndex(
				name: "IX_UserBranchLocations_ApplicationUsersId",
				table: "UserBranchLocations",
				column: "ApplicationUsersId");

			migrationBuilder.CreateIndex(
				name: "IX_StockAdjustmentDetails_TransactionDetailId",
				table: "StockAdjustmentDetails",
				column: "TransactionDetailId");

			migrationBuilder.CreateIndex(
				name: "IX_PurchaseReturnDetails_TransactionDetailId",
				table: "PurchaseReturnDetails",
				column: "TransactionDetailId");

			migrationBuilder.AddForeignKey(
				name: "FK_PurchaseReturnDetails_TransactionDetails_TransactionDetailId",
				table: "PurchaseReturnDetails",
				column: "TransactionDetailId",
				principalTable: "TransactionDetails",
				principalColumn: "TransactionDetailId",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_StockAdjustmentDetails_TransactionDetails_TransactionDetailId",
				table: "StockAdjustmentDetails",
				column: "TransactionDetailId",
				principalTable: "TransactionDetails",
				principalColumn: "TransactionDetailId",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_UserBranchLocations_AspNetUsers_ApplicationUsersId",
				table: "UserBranchLocations",
				column: "ApplicationUsersId",
				principalTable: "AspNetUsers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict); */
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			/* migrationBuilder.DropForeignKey(
				name: "FK_PurchaseReturnDetails_TransactionDetails_TransactionDetailId",
				table: "PurchaseReturnDetails");

			migrationBuilder.DropForeignKey(
				name: "FK_StockAdjustmentDetails_TransactionDetails_TransactionDetailId",
				table: "StockAdjustmentDetails");

			migrationBuilder.DropForeignKey(
				name: "FK_UserBranchLocations_AspNetUsers_ApplicationUsersId",
				table: "UserBranchLocations");

			migrationBuilder.DropIndex(
				name: "IX_UserBranchLocations_ApplicationUsersId",
				table: "UserBranchLocations");

			migrationBuilder.DropIndex(
				name: "IX_StockAdjustmentDetails_TransactionDetailId",
				table: "StockAdjustmentDetails");

			migrationBuilder.DropIndex(
				name: "IX_PurchaseReturnDetails_TransactionDetailId",
				table: "PurchaseReturnDetails");

			migrationBuilder.DropColumn(
				name: "ApplicationUsersId",
				table: "UserBranchLocations");

			migrationBuilder.DropColumn(
				name: "TransactionDetailId",
				table: "StockAdjustmentDetails");

			migrationBuilder.DropColumn(
				name: "TransactionDetailId",
				table: "PurchaseReturnDetails");

			migrationBuilder.AddUniqueConstraint(
				name: "AK_ProductCategories_ProductCategoryId",
				table: "ProductCategories",
				column: "ProductCategoryId");

			migrationBuilder.CreateIndex(
				name: "IX_UserBranchLocations_Id",
				table: "UserBranchLocations",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_UserBranchLocations_AspNetUsers_Id",
				table: "UserBranchLocations",
				column: "Id",
				principalTable: "AspNetUsers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
 */        
 		}
	}
}
