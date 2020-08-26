using Microsoft.EntityFrameworkCore.Migrations;

namespace Stockholm_Syndrome_Web.Migrations
{
    public partial class AutoManagedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AutoManaged",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoManaged",
                table: "AspNetRoles");
        }
    }
}
