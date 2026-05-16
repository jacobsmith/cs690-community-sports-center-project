using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsTracker.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseReset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValueInCents",
                table: "Equipment");

            migrationBuilder.AddColumn<decimal>(
                name: "ValueInDecimal",
                table: "Equipment",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValueInDecimal",
                table: "Equipment");

            migrationBuilder.AddColumn<int>(
                name: "ValueInCents",
                table: "Equipment",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
