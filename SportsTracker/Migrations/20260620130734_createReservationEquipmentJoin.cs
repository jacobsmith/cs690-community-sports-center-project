using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsTracker.Migrations
{
    /// <inheritdoc />
    public partial class createReservationEquipmentJoin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Borrower_borrowerId",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Equipment_equipmentId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_borrowerId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_equipmentId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "borrowerHasItem",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "borrowerId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "equipmentId",
                table: "Reservation");

            migrationBuilder.AddColumn<int>(
                name: "currentlyActiveReservationId",
                table: "Equipment",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EquipmentReservation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    reservationId = table.Column<int>(type: "INTEGER", nullable: false),
                    equipmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    borrowerId = table.Column<int>(type: "INTEGER", nullable: false),
                    returnedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentReservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentReservation_Borrower_borrowerId",
                        column: x => x.borrowerId,
                        principalTable: "Borrower",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentReservation_Equipment_equipmentId",
                        column: x => x.equipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentReservation_Reservation_reservationId",
                        column: x => x.reservationId,
                        principalTable: "Reservation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_currentlyActiveReservationId",
                table: "Equipment",
                column: "currentlyActiveReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentReservation_borrowerId",
                table: "EquipmentReservation",
                column: "borrowerId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentReservation_equipmentId",
                table: "EquipmentReservation",
                column: "equipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentReservation_reservationId",
                table: "EquipmentReservation",
                column: "reservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_Reservation_currentlyActiveReservationId",
                table: "Equipment",
                column: "currentlyActiveReservationId",
                principalTable: "Reservation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_Reservation_currentlyActiveReservationId",
                table: "Equipment");

            migrationBuilder.DropTable(
                name: "EquipmentReservation");

            migrationBuilder.DropIndex(
                name: "IX_Equipment_currentlyActiveReservationId",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "currentlyActiveReservationId",
                table: "Equipment");

            migrationBuilder.AddColumn<bool>(
                name: "borrowerHasItem",
                table: "Reservation",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "borrowerId",
                table: "Reservation",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "equipmentId",
                table: "Reservation",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_borrowerId",
                table: "Reservation",
                column: "borrowerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_equipmentId",
                table: "Reservation",
                column: "equipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Borrower_borrowerId",
                table: "Reservation",
                column: "borrowerId",
                principalTable: "Borrower",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Equipment_equipmentId",
                table: "Reservation",
                column: "equipmentId",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
