using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class initialdb50 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          /*   migrationBuilder.AddColumn<decimal>(
                name: "NBTPrecentage",
                table: "GoodReceptNoteDetails",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VATPrecentage",
                table: "GoodReceptNoteDetails",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "BillingCycleStartDate",
                table: "CustomerMasterSettings",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BillingCycleEndDate",
                table: "CustomerMasterSettings",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NBTPrecentage",
                table: "CalculationMasters",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VATPrecentage",
                table: "CalculationMasters",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m); */

          /*   migrationBuilder.CreateTable(
                name: "MonthEndProcessHeaders",
                columns: table => new
                {
                    MEPHeaderId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MEPHeaderUid = table.Column<string>(nullable: false),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    Paid = table.Column<bool>(nullable: false),
                    MonthEndTransctionDate = table.Column<DateTime>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    TotalReturnAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Scheme = table.Column<string>(nullable: true),
                    SchemeCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthEndProcessHeaders", x => x.MEPHeaderId);
                    table.ForeignKey(
                        name: "FK_MonthEndProcessHeaders_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthEndProcessHeaders_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthEndProcessDetails",
                columns: table => new
                {
                    MEPDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MEPDetailUid = table.Column<string>(nullable: false),
                    MI_Employer_EmployeeId = table.Column<int>(nullable: false),
                    Number = table.Column<string>(maxLength: 50, nullable: true),
                    EPFNo = table.Column<string>(maxLength: 20, nullable: false),
                    Location = table.Column<string>(maxLength: 2, nullable: false),
                    CreditLimit = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CreditBalance = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Scheme = table.Column<string>(nullable: true),
                    SchemeCode = table.Column<int>(nullable: false),
                    MonthEndTransctionDate = table.Column<DateTime>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Paid = table.Column<bool>(nullable: false),
                    MEPHeaderId = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthEndProcessDetails", x => x.MEPDetailId);
                    table.ForeignKey(
                        name: "FK_MonthEndProcessDetails_MonthEndProcessHeaders_MEPHeaderId",
                        column: x => x.MEPHeaderId,
                        principalTable: "MonthEndProcessHeaders",
                        principalColumn: "MEPHeaderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonthEndProcessDetails_MEPHeaderId",
                table: "MonthEndProcessDetails",
                column: "MEPHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthEndProcessHeaders_BranchLocationId",
                table: "MonthEndProcessHeaders",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthEndProcessHeaders_Id",
                table: "MonthEndProcessHeaders",
                column: "Id"); */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          /*   migrationBuilder.DropTable(
                name: "MonthEndProcessDetails");

            migrationBuilder.DropTable(
                name: "MonthEndProcessHeaders"); */

         /*    migrationBuilder.DropColumn(
                name: "NBTPrecentage",
                table: "GoodReceptNoteDetails");

            migrationBuilder.DropColumn(
                name: "VATPrecentage",
                table: "GoodReceptNoteDetails");

            migrationBuilder.DropColumn(
                name: "NBTPrecentage",
                table: "CalculationMasters");

            migrationBuilder.DropColumn(
                name: "VATPrecentage",
                table: "CalculationMasters");

            migrationBuilder.AlterColumn<string>(
                name: "BillingCycleStartDate",
                table: "CustomerMasterSettings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "BillingCycleEndDate",
                table: "CustomerMasterSettings",
                nullable: true,
                oldClrType: typeof(int)); */
        }
    }
}
