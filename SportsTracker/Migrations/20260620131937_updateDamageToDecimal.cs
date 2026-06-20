using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsTracker.Migrations
{
    /// <inheritdoc />
    public partial class updateDamageToDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "damageAmount",
                table: "EquipmentDamage",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "damageAmount",
                table: "EquipmentDamage",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");
        }
    }
}
