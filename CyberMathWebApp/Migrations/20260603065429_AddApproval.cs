    using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MathBattle.Migrations
{
    /// <inheritdoc />
    public partial class AddApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TimeLimitSeconds = table.Column<int>(type: "integer", nullable: false),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserEquations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Expression = table.Column<string>(type: "text", nullable: false),
                    Answer = table.Column<int>(type: "integer", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    CompetitionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEquations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEquations_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Competitions",
                columns: new[] { "Id", "IsApproved", "Name", "TimeLimitSeconds" },
                values: new object[] { 1, false, "Boss Fight: Множення", 15 });

            migrationBuilder.UpdateData(
                table: "Equations",
                keyColumn: "Id",
                keyValue: 2,
                column: "Answer",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Equations",
                keyColumn: "Id",
                keyValue: 3,
                column: "Answer",
                value: 12);

            migrationBuilder.InsertData(
                table: "UserEquations",
                columns: new[] { "Id", "Answer", "CompetitionId", "Expression", "OrderIndex" },
                values: new object[,]
                {
                    { 1, 144, 1, "12 * 12", 1 },
                    { 2, 75, 1, "5 * 15", 2 },
                    { 3, 25, 1, "100 / 4", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserEquations_CompetitionId",
                table: "UserEquations",
                column: "CompetitionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserEquations");

            migrationBuilder.DropTable(
                name: "Competitions");

            migrationBuilder.UpdateData(
                table: "Equations",
                keyColumn: "Id",
                keyValue: 2,
                column: "Answer",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Equations",
                keyColumn: "Id",
                keyValue: 3,
                column: "Answer",
                value: 8);
        }
    }
}
