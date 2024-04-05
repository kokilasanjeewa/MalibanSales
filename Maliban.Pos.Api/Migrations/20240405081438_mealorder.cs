using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class mealorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        /*    migrationBuilder.DropForeignKey(
                name: "FK_StockTransferDetails_StockTransferHeaders_StockTransferHeaderId",
                table: "StockTransferDetails");

            migrationBuilder.DropTable(
                name: "MealOrderDetails");

            migrationBuilder.DropTable(
                name: "MealOrderHeaders");

            migrationBuilder.AlterColumn<decimal>(
                name: "dQuantity",
                table: "TransactionDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "dOUT",
                table: "TransactionDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "dNetQuantity",
                table: "TransactionDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "dIssueQuantity",
                table: "TransactionDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "dIN",
                table: "TransactionDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "StockTransferHeaders",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorizedBy",
                table: "StockTransferHeaders",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateReceived",
                table: "StockTransferHeaders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsApprove",
                table: "StockTransferHeaders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsComfirm",
                table: "StockTransferHeaders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsProcess",
                table: "StockTransferHeaders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReceivedBy",
                table: "StockTransferHeaders",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "cApprovalType",
                table: "StockTransferHeaders",
                maxLength: 10,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "cDocType",
                table: "StockTransferHeaders",
                maxLength: 5,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "dTransferQuantity",
                table: "StockTransferDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "dQuantity",
                table: "StockTransferDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "dNetQuantity",
                table: "StockTransferDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "dIssueQuantity",
                table: "StockTransferDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ApproveQuantity",
                table: "StockTransferDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PreviousQuantity",
                table: "StockAdjustmentDetails",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentQuantity",
                table: "StockAdjustmentDetails",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApproveQuantity",
                table: "StockAdjustmentDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AdjustmentQuantity",
                table: "StockAdjustmentDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitsInStock",
                table: "ShopStocks",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SaleQuantity",
                table: "ShopStocks",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ReturnQuantity",
                table: "SaleDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "SaleDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ReturnQuantity",
                table: "PurchaseReturnDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FreeQuantity",
                table: "PurchaseReturnDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ReturnQuantity",
                table: "PurchaseOrderDetails",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "PurchaseOrderDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "GRNQuantity",
                table: "PurchaseOrderDetails",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "FreeQuantity",
                table: "PurchaseOrderDetails",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentQuantity",
                table: "PurchaseOrderDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BonusQuantity",
                table: "PurchaseOrderDetails",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApproveQuantity",
                table: "PurchaseOrderDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ItemQuantity",
                table: "GoodReceptNoteDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FreeItemQuantity",
                table: "GoodReceptNoteDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BalanceItemQuantity",
                table: "GoodReceptNoteDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SaleQuantity",
                table: "CustomerReturnDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ReturnQuantity",
                table: "CustomerReturnDetails",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentQuantity",
                table: "CustomerReturnDetails",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BalanceQuantity",
                table: "CustomerReturnDetails",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

        */ /*   migrationBuilder.CreateTable(
                name: "MealOrder_Headers",
                columns: table => new
                {
                    mohid = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    MealTypeId = table.Column<int>(nullable: false),
                    MI_Employer_EmployeeId = table.Column<int>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: false),
                    CreatedSystem = table.Column<int>(nullable: false),
                    bActive = table.Column<bool>(nullable: false),
                    SaleDetailId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealOrder_Headers", x => x.mohid);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransferDetails_StockTransferHeaders_StockTransferHeaderId",
                table: "StockTransferDetails",
                column: "StockTransferHeaderId",
                principalTable: "StockTransferHeaders",
                principalColumn: "StockTransferHeaderId",
                onDelete: ReferentialAction.Restrict);*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
      /*      migrationBuilder.DropForeignKey(
                name: "FK_StockTransferDetails_StockTransferHeaders_StockTransferHeaderId",
                table: "StockTransferDetails");*/

          /*  migrationBuilder.DropTable(
                name: "MealOrder_Headers");*/

          /*  migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "StockTransferHeaders");

            migrationBuilder.DropColumn(
                name: "AuthorizedBy",
                table: "StockTransferHeaders");

            migrationBuilder.DropColumn(
                name: "DateReceived",
                table: "StockTransferHeaders");

            migrationBuilder.DropColumn(
                name: "IsApprove",
                table: "StockTransferHeaders");

            migrationBuilder.DropColumn(
                name: "IsComfirm",
                table: "StockTransferHeaders");

            migrationBuilder.DropColumn(
                name: "IsProcess",
                table: "StockTransferHeaders");

            migrationBuilder.DropColumn(
                name: "ReceivedBy",
                table: "StockTransferHeaders");

            migrationBuilder.DropColumn(
                name: "cApprovalType",
                table: "StockTransferHeaders");

            migrationBuilder.DropColumn(
                name: "cDocType",
                table: "StockTransferHeaders");

            migrationBuilder.AlterColumn<decimal>(
                name: "dQuantity",
                table: "TransactionDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "dOUT",
                table: "TransactionDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "dNetQuantity",
                table: "TransactionDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "dIssueQuantity",
                table: "TransactionDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "dIN",
                table: "TransactionDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "dTransferQuantity",
                table: "StockTransferDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "dQuantity",
                table: "StockTransferDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "dNetQuantity",
                table: "StockTransferDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "dIssueQuantity",
                table: "StockTransferDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ApproveQuantity",
                table: "StockTransferDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PreviousQuantity",
                table: "StockAdjustmentDetails",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentQuantity",
                table: "StockAdjustmentDetails",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApproveQuantity",
                table: "StockAdjustmentDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AdjustmentQuantity",
                table: "StockAdjustmentDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitsInStock",
                table: "ShopStocks",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SaleQuantity",
                table: "ShopStocks",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ReturnQuantity",
                table: "SaleDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "SaleDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ReturnQuantity",
                table: "PurchaseReturnDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FreeQuantity",
                table: "PurchaseReturnDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ReturnQuantity",
                table: "PurchaseOrderDetails",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "PurchaseOrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "GRNQuantity",
                table: "PurchaseOrderDetails",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "FreeQuantity",
                table: "PurchaseOrderDetails",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentQuantity",
                table: "PurchaseOrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BonusQuantity",
                table: "PurchaseOrderDetails",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApproveQuantity",
                table: "PurchaseOrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ItemQuantity",
                table: "GoodReceptNoteDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FreeItemQuantity",
                table: "GoodReceptNoteDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BalanceItemQuantity",
                table: "GoodReceptNoteDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SaleQuantity",
                table: "CustomerReturnDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ReturnQuantity",
                table: "CustomerReturnDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentQuantity",
                table: "CustomerReturnDetails",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BalanceQuantity",
                table: "CustomerReturnDetails",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MealOrderHeaders",
                columns: table => new
                {
                    MealOrderHeaderId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    BookDateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookDateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BranchLocationId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmployeeCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    MI_Employer_EmployeeId = table.Column<int>(type: "int", nullable: false),
                    MealOrderCreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MealOrderHeaderUid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MealOrderTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MealType = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealOrderHeaders", x => x.MealOrderHeaderId);
                    table.ForeignKey(
                        name: "FK_MealOrderHeaders_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealOrderHeaders_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealOrderHeaders_MI_Employer_Employees_MI_Employer_EmployeeId",
                        column: x => x.MI_Employer_EmployeeId,
                        principalTable: "MI_Employer_Employees",
                        principalColumn: "MI_Employer_EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealOrderHeaders_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealOrderDetails",
                columns: table => new
                {
                    MealOrderDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    MealOrderDetailUid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MealOrderHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PriceId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealOrderDetails", x => x.MealOrderDetailId);
                    table.ForeignKey(
                        name: "FK_MealOrderDetails_MealOrderHeaders_MealOrderHeaderId",
                        column: x => x.MealOrderHeaderId,
                        principalTable: "MealOrderHeaders",
                        principalColumn: "MealOrderHeaderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealOrderDetails_Prices_PriceId",
                        column: x => x.PriceId,
                        principalTable: "Prices",
                        principalColumn: "PriceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealOrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealOrderDetails_MealOrderHeaderId",
                table: "MealOrderDetails",
                column: "MealOrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_MealOrderDetails_PriceId",
                table: "MealOrderDetails",
                column: "PriceId");

            migrationBuilder.CreateIndex(
                name: "IX_MealOrderDetails_ProductId",
                table: "MealOrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MealOrderHeaders_BranchLocationId",
                table: "MealOrderHeaders",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_MealOrderHeaders_Id",
                table: "MealOrderHeaders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MealOrderHeaders_MI_Employer_EmployeeId",
                table: "MealOrderHeaders",
                column: "MI_Employer_EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_MealOrderHeaders_PaymentId",
                table: "MealOrderHeaders",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransferDetails_StockTransferHeaders_StockTransferHeaderId",
                table: "StockTransferDetails",
                column: "StockTransferHeaderId",
                principalTable: "StockTransferHeaders",
                principalColumn: "StockTransferHeaderId",
                onDelete: ReferentialAction.Cascade);*/
        }
    }
}
