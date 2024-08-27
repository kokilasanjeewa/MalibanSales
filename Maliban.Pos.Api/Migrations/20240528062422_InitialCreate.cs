using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<long>(
            //    name: "SaleDetailId",
            //    table: "MealOrder_Headers",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<long>(
            //    name: "mohid",
            //    table: "MealOrder_Headers",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .Annotation("SqlServer:Identity", "1, 1")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateTable(
                name: "CardtMethods",
                columns: table => new
                {
                    CardMethodId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CardName = table.Column<string>(maxLength: 50, nullable: false),
                    CardDescription = table.Column<string>(maxLength: 100, nullable: true),
                    CardMethodUid = table.Column<string>(nullable: false),
                    ServiceCharge = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PaymentMethodId = table.Column<int>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardtMethods", x => x.CardMethodId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardtMethods");

            //migrationBuilder.AlterColumn<int>(
            //    name: "SaleDetailId",
            //    table: "MealOrder_Headers",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(long),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "mohid",
            //    table: "MealOrder_Headers",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(long))
            //    .Annotation("SqlServer:Identity", "1, 1")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
