using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb43 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /* migrationBuilder.RenameColumn(
                name: "Id",
                table: "SubmenuItems",
                newName: "SubmenuItemId");

            migrationBuilder.AddColumn<int>(
                name: "SubmenuItemId",
                table: "RolePermissions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_SubmenuItemId",
                table: "RolePermissions",
                column: "SubmenuItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_SubmenuItems_SubmenuItemId",
                table: "RolePermissions",
                column: "SubmenuItemId",
                principalTable: "SubmenuItems",
                principalColumn: "SubmenuItemId",
                onDelete: ReferentialAction.Restrict); */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           /*  migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_SubmenuItems_SubmenuItemId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_SubmenuItemId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "SubmenuItemId",
                table: "RolePermissions");

            migrationBuilder.RenameColumn(
                name: "SubmenuItemId",
                table: "SubmenuItems",
                newName: "Id"); */
        }
    }
}
