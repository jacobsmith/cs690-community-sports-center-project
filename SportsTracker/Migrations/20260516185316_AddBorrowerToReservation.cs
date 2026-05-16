using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddBorrowerToReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "borrowerId",
                table: "Reservation",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_borrowerId",
                table: "Reservation",
                column: "borrowerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Borrower_borrowerId",
                table: "Reservation",
                column: "borrowerId",
                principalTable: "Borrower",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Borrower_borrowerId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_borrowerId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "borrowerId",
                table: "Reservation");
        }
    }
}
