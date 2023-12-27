using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class initialdb54 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
/*             migrationBuilder.DropColumn(
                name: "MI_Employer_EmployeeUid",
                table: "MI_Employer_Employees");

            migrationBuilder.AddColumn<decimal>(
                name: "CreditBalance",
                table: "SaleHeaders",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CreditLimit",
                table: "SaleHeaders",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousAmount",
                table: "SaleHeaders",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<DateTime>(
                name: "hrExpiryDate",
                table: "MI_Employer_Employees",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "hrDOBofSpouse",
                table: "MI_Employer_Employees",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "hrDOBofMother",
                table: "MI_Employer_Employees",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "hrDOBofFather",
                table: "MI_Employer_Employees",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Effecteddate",
                table: "MI_Employer_Employees",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateResigned",
                table: "MI_Employer_Employees",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "MI_Employer_Employees",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfAppointment",
                table: "MI_Employer_Employees",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateDeleted",
                table: "MI_Employer_Employees",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConfirmDate",
                table: "MI_Employer_Employees",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<decimal>(
                name: "CreditBalance",
                table: "CustomerReturnHeaders",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "CustomerReturnHeaders",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);
 */
          /*   migrationBuilder.CreateTable(
                name: "UserBranchLocations",
                columns: table => new
                {
                    UserBranchLocationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserBranchLocationUid = table.Column<string>(nullable: true),
                    BranchLocationId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBranchLocations", x => x.UserBranchLocationId);
                    table.ForeignKey(
                        name: "FK_UserBranchLocations_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBranchLocations_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBranchLocations_BranchLocationId",
                table: "UserBranchLocations",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBranchLocations_Id",
                table: "UserBranchLocations",
                column: "Id"); */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        /*     migrationBuilder.DropTable(
                name: "UserBranchLocations"); */

          /*   migrationBuilder.DropColumn(
                name: "CreditBalance",
                table: "SaleHeaders");

            migrationBuilder.DropColumn(
                name: "CreditLimit",
                table: "SaleHeaders");

            migrationBuilder.DropColumn(
                name: "PreviousAmount",
                table: "SaleHeaders");

            migrationBuilder.DropColumn(
                name: "CreditBalance",
                table: "CustomerReturnHeaders");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "CustomerReturnHeaders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "hrExpiryDate",
                table: "MI_Employer_Employees",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "hrDOBofSpouse",
                table: "MI_Employer_Employees",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "hrDOBofMother",
                table: "MI_Employer_Employees",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "hrDOBofFather",
                table: "MI_Employer_Employees",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Effecteddate",
                table: "MI_Employer_Employees",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateResigned",
                table: "MI_Employer_Employees",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "MI_Employer_Employees",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfAppointment",
                table: "MI_Employer_Employees",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateDeleted",
                table: "MI_Employer_Employees",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConfirmDate",
                table: "MI_Employer_Employees",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MI_Employer_EmployeeUid",
                table: "MI_Employer_Employees",
                nullable: true);
 */        }
    }
}
