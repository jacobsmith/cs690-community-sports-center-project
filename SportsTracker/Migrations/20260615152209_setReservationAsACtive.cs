using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsTracker.Migrations
{
    /// <inheritdoc />
    public partial class setReservationAsACtive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "borrowerHasItem",
                table: "Reservation",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "borrowerHasItem",
                table: "Reservation");
        }
    }
}
