using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCars.Data.Migrations
{
    public partial class UpdateRentalHistoryModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalHistory_Reservations_ReservationsId",
                table: "RentalHistory");

            migrationBuilder.RenameColumn(
                name: "ReservationsId",
                table: "RentalHistory",
                newName: "ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_RentalHistory_ReservationsId",
                table: "RentalHistory",
                newName: "IX_RentalHistory_ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalHistory_Reservations_ReservationId",
                table: "RentalHistory",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalHistory_Reservations_ReservationId",
                table: "RentalHistory");

            migrationBuilder.RenameColumn(
                name: "ReservationId",
                table: "RentalHistory",
                newName: "ReservationsId");

            migrationBuilder.RenameIndex(
                name: "IX_RentalHistory_ReservationId",
                table: "RentalHistory",
                newName: "IX_RentalHistory_ReservationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalHistory_Reservations_ReservationsId",
                table: "RentalHistory",
                column: "ReservationsId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
