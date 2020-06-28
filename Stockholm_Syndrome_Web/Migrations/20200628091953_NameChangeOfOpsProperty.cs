using Microsoft.EntityFrameworkCore.Migrations;

namespace Stockholm_Syndrome_Web.Migrations
{
    public partial class NameChangeOfOpsProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllianceCorpName",
                table: "Ops");

            migrationBuilder.AddColumn<string>(
                name: "StructureOwner",
                table: "Ops",
                maxLength: 2000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StructureOwner",
                table: "Ops");

            migrationBuilder.AddColumn<string>(
                name: "AllianceCorpName",
                table: "Ops",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);
        }
    }
}
