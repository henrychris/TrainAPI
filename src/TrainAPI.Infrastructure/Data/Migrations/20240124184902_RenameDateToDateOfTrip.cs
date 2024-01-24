using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainAPI.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameDateToDateOfTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                schema: "TrainDb",
                table: "Trips",
                newName: "DateOfTrip");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfTrip",
                schema: "TrainDb",
                table: "Trips",
                newName: "Date");
        }
    }
}