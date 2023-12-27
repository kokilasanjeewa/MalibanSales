using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb41 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
      /*       migrationBuilder.DropColumn(
                name: "Deny",
                table: "RolePermissions"); */

     /*        migrationBuilder.DropColumn(
                name: "Handle",
                table: "AspNetRoles"); */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
/*             migrationBuilder.AddColumn<bool>(
                name: "Deny",
                table: "RolePermissions",
                nullable: false,
                defaultValue: false); */

/*             migrationBuilder.AddColumn<string>(
                name: "Handle",
                table: "AspNetRoles",
                nullable: true); */
        }
    }
}
