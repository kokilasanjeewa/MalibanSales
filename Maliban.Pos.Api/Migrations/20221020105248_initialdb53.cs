using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class initialdb53 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          /*   migrationBuilder.CreateTable(
                name: "MI_Employer_Maps",
                columns: table => new
                {
                    MIEmployerMapId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MI_Employer_EmployeeId = table.Column<int>(nullable: false),
                    EPFNo = table.Column<string>(nullable: true),
                    BranchLocationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MI_Employer_Maps", x => x.MIEmployerMapId);
                    table.ForeignKey(
                        name: "FK_MI_Employer_Maps_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MI_Employer_Maps_MI_Employer_Employees_MI_Employer_EmployeeId",
                        column: x => x.MI_Employer_EmployeeId,
                        principalTable: "MI_Employer_Employees",
                        principalColumn: "MI_Employer_EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MI_Employer_Maps_BranchLocationId",
                table: "MI_Employer_Maps",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_MI_Employer_Maps_MI_Employer_EmployeeId",
                table: "MI_Employer_Maps",
                column: "MI_Employer_EmployeeId"); */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          /*   migrationBuilder.DropTable(
                name: "MI_Employer_Maps"); */
        }
    }
}
