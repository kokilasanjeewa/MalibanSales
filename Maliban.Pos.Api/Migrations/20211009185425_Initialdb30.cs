using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ProductExpireDate",
                table: "StockAdjustmentDetails",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductExpireDate",
                table: "StockAdjustmentDetails");
        }
    }
}
