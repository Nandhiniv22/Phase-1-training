using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowBookingApp.Migrations
{
    /// <inheritdoc />
    public partial class datetime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "ShowDate",
                table: "Movies",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ShowTime",
                table: "Movies",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ShowDate_ShowTime",
                table: "Movies",
                columns: new[] { "ShowDate", "ShowTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Movies_ShowDate_ShowTime",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ShowDate",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ShowTime",
                table: "Movies");
        }
    }
}
