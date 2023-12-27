using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        /*     migrationBuilder.AlterColumn<int>(
                name: "FreeItemQuantity",
                table: "GoodReceptNoteDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
 */
            migrationBuilder.CreateTable(
                name: "TransactionDetails",
                columns: table => new
                {
                    TransactionDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TransactionDetailUid = table.Column<string>(nullable: true),
                    dQuantity = table.Column<int>(nullable: false),
                    ProductAvailable = table.Column<bool>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    PriceId = table.Column<long>(nullable: false),
                    iDocID = table.Column<long>(nullable: false),
                    cDocNo = table.Column<string>(maxLength: 50, nullable: true),
                    dDocDate = table.Column<DateTime>(nullable: false),
                    cDocType = table.Column<int>(maxLength: 5, nullable: false),
                    cExtRefNum = table.Column<string>(maxLength: 20, nullable: true),
                    dIssueQuantity = table.Column<int>(nullable: false),
                    dNetQuantity = table.Column<int>(nullable: false),
                    dCostPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    dSalePrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    dValue = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    dNetValue = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    dIN = table.Column<int>(nullable: false),
                    dOUT = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionDetails", x => x.TransactionDetailId);
                    table.ForeignKey(
                        name: "FK_TransactionDetails_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionDetails_Prices_PriceId",
                        column: x => x.PriceId,
                        principalTable: "Prices",
                        principalColumn: "PriceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_BranchLocationId",
                table: "TransactionDetails",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_PriceId",
                table: "TransactionDetails",
                column: "PriceId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_ProductId",
                table: "TransactionDetails",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionDetails");

     /*        migrationBuilder.AlterColumn<int>(
                name: "FreeItemQuantity",
                table: "GoodReceptNoteDetails",
                nullable: true,
                oldClrType: typeof(int)); */
        }
    }
}
