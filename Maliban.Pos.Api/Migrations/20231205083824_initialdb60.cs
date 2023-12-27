using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class initialdb60 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
/*            migrationBuilder.AddColumn<decimal>(
                name: "CashBalance",
                table: "SaleHeaders",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CashLimit",
                table: "SaleHeaders",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CustomerCashReturnAmount",
                table: "SaleHeaders",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PaymentReferenceId",
                table: "SaleHeaders",
                maxLength: 12,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousCashAmount",
                table: "SaleHeaders",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PurchaseOrderHeaders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsApprove",
                table: "PurchaseOrderHeaders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "cApprovalType",
                table: "PurchaseOrderHeaders",
                maxLength: 10,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ApproveQuantity",
                table: "PurchaseOrderDetails",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "cApprovalType",
                table: "PurchaseOrderDetails",
                maxLength: 10,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "CashBalance",
                table: "MI_Employer_Employees",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CashLimit",
                table: "MI_Employer_Employees",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousCashBalance",
                table: "MI_Employer_Employees",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "BranchName",
                table: "BranchLocations",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);*/

            migrationBuilder.CreateTable(
                name: "MealOrderHeaders",
                columns: table => new
                {
                    MealOrderHeaderId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MealOrderHeaderUid = table.Column<string>(nullable: false),
                    MealOrderCreateDate = table.Column<DateTime>(nullable: false),
                    MealOrderTimeStamp = table.Column<DateTime>(nullable: false),
                    PaymentId = table.Column<int>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    MI_Employer_EmployeeId = table.Column<int>(nullable: false),
                    EmployeeCode = table.Column<string>(maxLength: 5, nullable: false),
                    MealType = table.Column<int>(nullable: false),
                    BookDateFrom = table.Column<DateTime>(nullable: false),
                    BookDateTo = table.Column<DateTime>(nullable: false)
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
                    MealOrderDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MealOrderDetailUid = table.Column<string>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    PriceId = table.Column<long>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    MealOrderHeaderId = table.Column<long>(nullable: false)
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

/*            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderHeaders_Id",
                table: "PurchaseOrderHeaders",
                column: "Id");*/

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

/*            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderHeaders_AspNetUsers_Id",
                table: "PurchaseOrderHeaders",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
 /*           migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderHeaders_AspNetUsers_Id",
                table: "PurchaseOrderHeaders");

            migrationBuilder.DropTable(
                name: "MealOrderDetails");

            migrationBuilder.DropTable(
                name: "MealOrderHeaders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderHeaders_Id",
                table: "PurchaseOrderHeaders");

            migrationBuilder.DropColumn(
                name: "CashBalance",
                table: "SaleHeaders");

            migrationBuilder.DropColumn(
                name: "CashLimit",
                table: "SaleHeaders");

            migrationBuilder.DropColumn(
                name: "CustomerCashReturnAmount",
                table: "SaleHeaders");

            migrationBuilder.DropColumn(
                name: "PaymentReferenceId",
                table: "SaleHeaders");

            migrationBuilder.DropColumn(
                name: "PreviousCashAmount",
                table: "SaleHeaders");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PurchaseOrderHeaders");

            migrationBuilder.DropColumn(
                name: "IsApprove",
                table: "PurchaseOrderHeaders");

            migrationBuilder.DropColumn(
                name: "cApprovalType",
                table: "PurchaseOrderHeaders");

            migrationBuilder.DropColumn(
                name: "ApproveQuantity",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropColumn(
                name: "cApprovalType",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropColumn(
                name: "CashBalance",
                table: "MI_Employer_Employees");

            migrationBuilder.DropColumn(
                name: "CashLimit",
                table: "MI_Employer_Employees");

            migrationBuilder.DropColumn(
                name: "PreviousCashBalance",
                table: "MI_Employer_Employees");

            migrationBuilder.AlterColumn<string>(
                name: "BranchName",
                table: "BranchLocations",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200);*/
        }
    }
}
