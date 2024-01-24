using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using TrainAPI.Domain.Entities;

#nullable disable

namespace TrainAPI.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class retryMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TravellerCategories",
                schema: "TrainDb",
                table: "Coaches",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(List<TravellerPairs>),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string>(
                name: "Seats",
                schema: "TrainDb",
                table: "Coaches",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(List<Seat>),
                oldType: "jsonb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<TravellerPairs>>(
                name: "TravellerCategories",
                schema: "TrainDb",
                table: "Coaches",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<List<Seat>>(
                name: "Seats",
                schema: "TrainDb",
                table: "Coaches",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldNullable: true);
        }
    }
}
