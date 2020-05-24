using Microsoft.EntityFrameworkCore.Migrations;

namespace Stockholm_Syndrome_Web.Migrations
{
    public partial class Structures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExtractionData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Chunk_arrival_time = table.Column<string>(nullable: true),
                    Extraction_start_time = table.Column<string>(nullable: true),
                    Moon_id = table.Column<int>(nullable: false),
                    Natural_decay_time = table.Column<string>(nullable: true),
                    Structure_id = table.Column<long>(nullable: false),
                    LastExtraction = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtractionData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Structures",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StructureId = table.Column<long>(nullable: false),
                    StructureName = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    RoleNeededToManage = table.Column<string>(nullable: true),
                    FuelExpires = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Structures", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtractionData");

            migrationBuilder.DropTable(
                name: "Structures");
        }
    }
}
