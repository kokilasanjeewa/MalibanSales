using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
	public partial class initialdb58 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{/* 
			migrationBuilder.AddColumn<int>(
				name: "cApprovalType",
				table: "StockAdjustmentDetails",
				maxLength: 10,
				nullable: false,
				defaultValue: 0); */
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
/* 			migrationBuilder.DropColumn(
				name: "cApprovalType",
				table: "StockAdjustmentDetails");
 */				
		}
	}
}
