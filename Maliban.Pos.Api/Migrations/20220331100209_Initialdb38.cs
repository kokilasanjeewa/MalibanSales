using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb38 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          /*  migrationBuilder.DropColumn(
                name: "Add",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "Delete",
                table: "RolePermissions");

            migrationBuilder.RenameColumn(
                name: "Update",
                table: "RolePermissions",
                newName: "Deny");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "RolePermissions",
                newName: "Submenu");

            migrationBuilder.RenameColumn(
                name: "Read",
                table: "RolePermissions",
                newName: "Allow");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "PermissionTypes",
                newName: "AccessControllerName");

            migrationBuilder.AddColumn<string>(
                name: "AccessControllerName",
                table: "RolePermissions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainMenu",
                table: "RolePermissions",
                nullable: true);*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.DropColumn(
                name: "AccessControllerName",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "MainMenu",
                table: "RolePermissions");

            migrationBuilder.RenameColumn(
                name: "Submenu",
                table: "RolePermissions",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Deny",
                table: "RolePermissions",
                newName: "Update");

            migrationBuilder.RenameColumn(
                name: "Allow",
                table: "RolePermissions",
                newName: "Read");

            migrationBuilder.RenameColumn(
                name: "AccessControllerName",
                table: "PermissionTypes",
                newName: "Type");

            migrationBuilder.AddColumn<bool>(
                name: "Add",
                table: "RolePermissions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "RolePermissions",
                nullable: false,
                defaultValue: false);*/
        }
    }
}
