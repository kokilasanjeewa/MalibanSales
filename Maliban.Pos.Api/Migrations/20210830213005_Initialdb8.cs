using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdjustmentType",
                table: "StockAdjustmentHeaders");

            migrationBuilder.AddColumn<int>(
                name: "AdjustmentTypeId",
                table: "StockAdjustmentHeaders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousQuantity",
                table: "StockAdjustmentDetails",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StockAdjustmentTotal",
                table: "StockAdjustmentDetails",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "AdjustmentTypes",
                columns: table => new
                {
                    AdjustmentTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AdjustmentName = table.Column<string>(maxLength: 50, nullable: false),
                    AdjustmentDescription = table.Column<string>(maxLength: 100, nullable: true),
                    AdjustmentTypeUid = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdjustmentTypes", x => x.AdjustmentTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustmentHeaders_AdjustmentTypeId",
                table: "StockAdjustmentHeaders",
                column: "AdjustmentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustmentHeaders_AdjustmentTypes_AdjustmentTypeId",
                table: "StockAdjustmentHeaders",
                column: "AdjustmentTypeId",
                principalTable: "AdjustmentTypes",
                principalColumn: "AdjustmentTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustmentHeaders_AdjustmentTypes_AdjustmentTypeId",
                table: "StockAdjustmentHeaders");

            migrationBuilder.DropTable(
                name: "AdjustmentTypes");

            migrationBuilder.DropIndex(
                name: "IX_StockAdjustmentHeaders_AdjustmentTypeId",
                table: "StockAdjustmentHeaders");

            migrationBuilder.DropColumn(
                name: "AdjustmentTypeId",
                table: "StockAdjustmentHeaders");

            migrationBuilder.DropColumn(
                name: "PreviousQuantity",
                table: "StockAdjustmentDetails");

            migrationBuilder.DropColumn(
                name: "StockAdjustmentTotal",
                table: "StockAdjustmentDetails");

            migrationBuilder.AddColumn<int>(
                name: "AdjustmentType",
                table: "StockAdjustmentHeaders",
                nullable: false,
                defaultValue: 0);
        }
    }
}
