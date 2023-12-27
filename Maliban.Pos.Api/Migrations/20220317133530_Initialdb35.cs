using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb35 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
/*            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReturnDetails_PurchaseReturnHeaders_PurchaseReturnHeaderId",
                table: "PurchaseReturnDetails");

            migrationBuilder.AlterColumn<decimal>(
                name: "ReturnQuantity",
                table: "PurchaseReturnDetails",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "FreeQuantity",
                table: "PurchaseReturnDetails",
                nullable: false,
                oldClrType: typeof(int));*/

            // migrationBuilder.AddColumn<string>(
            //     name: "Handle",
            //     table: "AspNetRoles",
            //     nullable: true);

            // migrationBuilder.AddColumn<bool>(
            //     name: "IsActive",
            //     table: "AspNetRoles",
            //     nullable: false,
            //     defaultValue: false);

            // migrationBuilder.AddColumn<string>(
            //     name: "RoleIcon",
            //     table: "AspNetRoles",
            //     nullable: true);

            // migrationBuilder.AddColumn<string>(
            //     name: "RoleName",
            //     table: "AspNetRoles",
            //     nullable: true);

          /*   migrationBuilder.CreateTable(
                name: "PermissionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionTypes", x => x.Id);
                }); */

  /*           migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Read = table.Column<bool>(nullable: false),
                    Delete = table.Column<bool>(nullable: false),
                    Update = table.Column<bool>(nullable: false),
                    Add = table.Column<bool>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    ApplicationRoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_AspNetRoles_ApplicationRoleId",
                        column: x => x.ApplicationRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                }); */

            // migrationBuilder.CreateIndex(
            //     name: "IX_RolePermissions_ApplicationRoleId",
            //     table: "RolePermissions",
            //     column: "ApplicationRoleId");

/*            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReturnDetails_PurchaseReturnHeaders_PurchaseReturnHeaderId",
                table: "PurchaseReturnDetails",
                column: "PurchaseReturnHeaderId",
                principalTable: "PurchaseReturnHeaders",
                principalColumn: "PurchaseReturnHeaderId",
                onDelete: ReferentialAction.Restrict);*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_PurchaseReturnDetails_PurchaseReturnHeaders_PurchaseReturnHeaderId",
            //    table: "PurchaseReturnDetails");

/*             migrationBuilder.DropTable(
                name: "PermissionTypes"); */

/*             migrationBuilder.DropTable(
                name: "RolePermissions"); */

            // migrationBuilder.DropColumn(
            //     name: "Handle",
            //     table: "AspNetRoles");

            // migrationBuilder.DropColumn(
            //     name: "IsActive",
            //     table: "AspNetRoles");

            // migrationBuilder.DropColumn(
            //     name: "RoleIcon",
            //     table: "AspNetRoles");

            // migrationBuilder.DropColumn(
            //     name: "RoleName",
            //     table: "AspNetRoles");

/*            migrationBuilder.AlterColumn<int>(
                name: "ReturnQuantity",
                table: "PurchaseReturnDetails",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "FreeQuantity",
                table: "PurchaseReturnDetails",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReturnDetails_PurchaseReturnHeaders_PurchaseReturnHeaderId",
                table: "PurchaseReturnDetails",
                column: "PurchaseReturnHeaderId",
                principalTable: "PurchaseReturnHeaders",
                principalColumn: "PurchaseReturnHeaderId",
                onDelete: ReferentialAction.Cascade);*/
        }
    }
}
