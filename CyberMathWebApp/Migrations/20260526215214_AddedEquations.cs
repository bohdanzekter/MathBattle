using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MathBattle.Migrations
{
    /// <inheritdoc />
    public partial class AddedEquations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Expression = table.Column<string>(type: "text", nullable: false),
                    Answer = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Equations",
                columns: new[] { "Id", "Answer", "Expression" },
                values: new object[,]
                {
                    { 1, 9, "∫ 2x dx (межі: 0..3)" },
                    { 2, 12, "d/dx (x³) при x=2" },
                    { 3, 8, "∫ 3x² dx (межі: 0..2)" },
                    { 4, 4, "log₂(16)" },
                    { 5, 21, "√144 + 3²" },
                    { 6, 30, "d/dx (5x²) при x=3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equations");
        }
    }
}
