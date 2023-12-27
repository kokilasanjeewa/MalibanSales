using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb42 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          /*  migrationBuilder.DropColumn(
                name: "MainMenu",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "Submenu",
                table: "RolePermissions");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Submenus",
                newName: "SubmenuId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MainMenus",
                newName: "MainMenuId");

            migrationBuilder.AddColumn<int>(
                name: "MainMenuId",
                table: "RolePermissions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubmenuId",
                table: "RolePermissions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SubmenuItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    SubMenuId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmenuItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubmenuItems_Submenus_SubMenuId",
                        column: x => x.SubMenuId,
                        principalTable: "Submenus",
                        principalColumn: "SubmenuId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_MainMenuId",
                table: "RolePermissions",
                column: "MainMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_SubmenuId",
                table: "RolePermissions",
                column: "SubmenuId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmenuItems_SubMenuId",
                table: "SubmenuItems",
                column: "SubMenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_MainMenus_MainMenuId",
                table: "RolePermissions",
                column: "MainMenuId",
                principalTable: "MainMenus",
                principalColumn: "MainMenuId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Submenus_SubmenuId",
                table: "RolePermissions",
                column: "SubmenuId",
                principalTable: "Submenus",
                principalColumn: "SubmenuId",
                onDelete: ReferentialAction.Cascade); */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_MainMenus_MainMenuId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Submenus_SubmenuId",
                table: "RolePermissions");

            migrationBuilder.DropTable(
                name: "SubmenuItems");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_MainMenuId",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_SubmenuId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "MainMenuId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "SubmenuId",
                table: "RolePermissions");

            migrationBuilder.RenameColumn(
                name: "SubmenuId",
                table: "Submenus",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MainMenuId",
                table: "MainMenus",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "MainMenu",
                table: "RolePermissions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Submenu",
                table: "RolePermissions",
                nullable: true);*/
        }
    }
}
