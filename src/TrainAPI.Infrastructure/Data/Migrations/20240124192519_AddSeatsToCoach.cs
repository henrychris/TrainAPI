using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainAPI.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatsToCoach : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableSeats",
                schema: "TrainDb",
                table: "Coaches");

            migrationBuilder.AddColumn<string>(
                name: "Seats",
                schema: "TrainDb",
                table: "Coaches",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seats",
                schema: "TrainDb",
                table: "Coaches");

            migrationBuilder.AddColumn<int>(
                name: "AvailableSeats",
                schema: "TrainDb",
                table: "Coaches",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
