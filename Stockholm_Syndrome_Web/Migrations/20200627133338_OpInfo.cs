using Microsoft.EntityFrameworkCore.Migrations;

namespace Stockholm_Syndrome_Web.Migrations
{
    public partial class OpInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OpStatus",
                table: "Ops",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StructureStatus",
                table: "Ops",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpStatus",
                table: "Ops");

            migrationBuilder.DropColumn(
                name: "StructureStatus",
                table: "Ops");
        }
    }
}
