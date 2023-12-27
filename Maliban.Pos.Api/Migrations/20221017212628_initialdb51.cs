using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class initialdb51 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          /*   migrationBuilder.DropForeignKey(
                name: "FK_MonthEndProcessHeaders_AspNetUsers_Id",
                table: "MonthEndProcessHeaders");

            migrationBuilder.DropIndex(
                name: "IX_MonthEndProcessHeaders_Id",
                table: "MonthEndProcessHeaders");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "MonthEndProcessHeaders");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MonthEndProcessHeaders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MonthEndProcessHeaders");

            migrationBuilder.DropColumn(
                name: "MEPHeaderUid",
                table: "MonthEndProcessHeaders");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "MonthEndProcessHeaders");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "MonthEndProcessHeaders");

            migrationBuilder.DropColumn(
                name: "MonthEndTransctionDate",
                table: "MonthEndProcessHeaders");

            migrationBuilder.DropColumn(
                name: "Paid",
                table: "MonthEndProcessHeaders");

            migrationBuilder.DropColumn(
                name: "Scheme",
                table: "MonthEndProcessHeaders");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "MonthEndProcessDetails");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "MonthEndProcessDetails");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MonthEndProcessDetails");

            migrationBuilder.DropColumn(
                name: "EPFNo",
                table: "MonthEndProcessDetails");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MonthEndProcessDetails");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "MonthEndProcessDetails");

            migrationBuilder.DropColumn(
                name: "MEPDetailUid",
                table: "MonthEndProcessDetails");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "MonthEndProcessDetails");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "MonthEndProcessDetails");

            migrationBuilder.DropColumn(
                name: "MonthEndTransctionDate",
                table: "MonthEndProcessDetails");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "MonthEndProcessDetails");

            migrationBuilder.DropColumn(
                name: "Paid",
                table: "MonthEndProcessDetails");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "MonthEndProcessDetails");

            migrationBuilder.DropColumn(
                name: "Scheme",
                table: "MonthEndProcessDetails");

            migrationBuilder.DropColumn(
                name: "SchemeCode",
                table: "MonthEndProcessDetails");

            migrationBuilder.AddColumn<long>(
                name: "SaleHeaderId",
                table: "CustomerReturnHeaders",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SaleDetailId",
                table: "CustomerReturnDetails",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReturnHeaders_SaleHeaderId",
                table: "CustomerReturnHeaders",
                column: "SaleHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReturnDetails_SaleDetailId",
                table: "CustomerReturnDetails",
                column: "SaleDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerReturnDetails_SaleDetails_SaleDetailId",
                table: "CustomerReturnDetails",
                column: "SaleDetailId",
                principalTable: "SaleDetails",
                principalColumn: "SaleDetailId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerReturnHeaders_SaleHeaders_SaleHeaderId",
                table: "CustomerReturnHeaders",
                column: "SaleHeaderId",
                principalTable: "SaleHeaders",
                principalColumn: "SaleHeaderId",
                onDelete: ReferentialAction.Cascade); */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /* migrationBuilder.DropForeignKey(
                name: "FK_CustomerReturnDetails_SaleDetails_SaleDetailId",
                table: "CustomerReturnDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerReturnHeaders_SaleHeaders_SaleHeaderId",
                table: "CustomerReturnHeaders");

            migrationBuilder.DropIndex(
                name: "IX_CustomerReturnHeaders_SaleHeaderId",
                table: "CustomerReturnHeaders");

            migrationBuilder.DropIndex(
                name: "IX_CustomerReturnDetails_SaleDetailId",
                table: "CustomerReturnDetails");

            migrationBuilder.DropColumn(
                name: "SaleHeaderId",
                table: "CustomerReturnHeaders");

            migrationBuilder.DropColumn(
                name: "SaleDetailId",
                table: "CustomerReturnDetails");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "MonthEndProcessHeaders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MonthEndProcessHeaders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MonthEndProcessHeaders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MEPHeaderUid",
                table: "MonthEndProcessHeaders",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "MonthEndProcessHeaders",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "MonthEndProcessHeaders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "MonthEndTransctionDate",
                table: "MonthEndProcessHeaders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "MonthEndProcessHeaders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Scheme",
                table: "MonthEndProcessHeaders",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "MonthEndProcessDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "MonthEndProcessDetails",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "MonthEndProcessDetails",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPFNo",
                table: "MonthEndProcessDetails",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MonthEndProcessDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "MonthEndProcessDetails",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MEPDetailUid",
                table: "MonthEndProcessDetails",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "MonthEndProcessDetails",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "MonthEndProcessDetails",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "MonthEndTransctionDate",
                table: "MonthEndProcessDetails",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "MonthEndProcessDetails",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "MonthEndProcessDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "MonthEndProcessDetails",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Scheme",
                table: "MonthEndProcessDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchemeCode",
                table: "MonthEndProcessDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MonthEndProcessHeaders_Id",
                table: "MonthEndProcessHeaders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthEndProcessHeaders_AspNetUsers_Id",
                table: "MonthEndProcessHeaders",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade); */
        }
    }
}
