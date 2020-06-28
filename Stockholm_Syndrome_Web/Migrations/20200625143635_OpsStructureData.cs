using Microsoft.EntityFrameworkCore.Migrations;

namespace Stockholm_Syndrome_Web.Migrations
{
    public partial class OpsStructureData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AllianceCorpName",
                table: "Ops",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StructureLayer",
                table: "Ops",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StructureName",
                table: "Ops",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StructureType",
                table: "Ops",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllianceCorpName",
                table: "Ops");

            migrationBuilder.DropColumn(
                name: "StructureLayer",
                table: "Ops");

            migrationBuilder.DropColumn(
                name: "StructureName",
                table: "Ops");

            migrationBuilder.DropColumn(
                name: "StructureType",
                table: "Ops");
        }
    }
}
