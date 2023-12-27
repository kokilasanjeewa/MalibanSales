using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb45 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
    /*         migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false); */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          /*   migrationBuilder.DropColumn(
                name: "Active",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers"); */
        }
    }
}
