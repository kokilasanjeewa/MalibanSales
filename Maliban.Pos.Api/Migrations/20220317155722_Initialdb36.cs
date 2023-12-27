using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb36 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         /*    migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "RolePermissions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_ApplicationUserId",
                table: "RolePermissions",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_AspNetUsers_ApplicationUserId",
                table: "RolePermissions",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade); */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_RolePermissions_AspNetUsers_ApplicationUserId",
            //     table: "RolePermissions");

            // migrationBuilder.DropIndex(
            //     name: "IX_RolePermissions_ApplicationUserId",
            //     table: "RolePermissions");

            // migrationBuilder.DropColumn(
            //     name: "ApplicationUserId",
            //     table: "RolePermissions");
        }
    }
}
