using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainAPI.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddJobIdToBookingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobId",
                schema: "TrainDb",
                table: "Bookings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobId",
                schema: "TrainDb",
                table: "Bookings");
        }
    }
}
