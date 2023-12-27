using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class initialdb49 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           /*  migrationBuilder.AddColumn<string>(
                name: "PurchaseOrderReferenceNo",
                table: "PurchaseOrderHeaders",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GoodReceptNoteReferenceNo",
                table: "GoodReceptNoteHeaders",
                nullable: false,
                defaultValue: ""); */

          /*   migrationBuilder.CreateTable(
                name: "CustomerMasterSettings",
                columns: table => new
                {
                    CustomerMasterSettingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CustomerMasterSettingUid = table.Column<string>(nullable: false),
                    CustomerMasterSettingDescription = table.Column<string>(maxLength: 100, nullable: true),
                    BranchLocationId = table.Column<int>(nullable: false),
                    CustomerCategory = table.Column<string>(nullable: true),
                    CustomerCategoryCode = table.Column<int>(nullable: false),
                    BillingCycleStartDate = table.Column<string>(nullable: true),
                    BillingCycleEndDate = table.Column<string>(nullable: true),
                    CreditHoldingDays = table.Column<int>(nullable: false),
                    CreaditBalancePerDays = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerMasterSettings", x => x.CustomerMasterSettingId);
                    table.ForeignKey(
                        name: "FK_CustomerMasterSettings_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerMasterSettings_BranchLocationId",
                table: "CustomerMasterSettings",
                column: "BranchLocationId"); */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           /*  migrationBuilder.DropTable(
                name: "CustomerMasterSettings");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderReferenceNo",
                table: "PurchaseOrderHeaders");

            migrationBuilder.DropColumn(
                name: "GoodReceptNoteReferenceNo",
                table: "GoodReceptNoteHeaders"); */
        }
    }
}
