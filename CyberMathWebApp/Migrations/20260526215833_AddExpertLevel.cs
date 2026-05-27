using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathBattle.Migrations
{
    /// <inheritdoc />
    public partial class AddExpertLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Multiplier", "Name" },
                values: new object[] { 4, 5, "Expert (Integrals)" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
