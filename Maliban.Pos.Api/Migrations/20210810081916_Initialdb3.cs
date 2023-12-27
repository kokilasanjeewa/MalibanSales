using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetails_Prices_PriceId",
                table: "TransactionDetails");

            migrationBuilder.AddColumn<DateTime>(
                name: "ProductExpireDate",
                table: "TransactionDetails",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetails_Prices_PriceId",
                table: "TransactionDetails",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "PriceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetails_Prices_PriceId",
                table: "TransactionDetails");

            migrationBuilder.DropColumn(
                name: "ProductExpireDate",
                table: "TransactionDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetails_Prices_PriceId",
                table: "TransactionDetails",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "PriceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
