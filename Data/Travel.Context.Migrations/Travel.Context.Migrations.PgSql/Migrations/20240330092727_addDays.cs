using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Travel.Context.Migrations.PgSql.Migrations
{
    /// <inheritdoc />
    public partial class addDays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activities_trips_TripId",
                table: "activities");

            migrationBuilder.RenameColumn(
                name: "TripId",
                table: "activities",
                newName: "DayId");

            migrationBuilder.RenameIndex(
                name: "IX_activities_TripId",
                table: "activities",
                newName: "IX_activities_DayId");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublicated",
                table: "trips",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "days",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TripId = table.Column<int>(type: "integer", nullable: true),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Uid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_days", x => x.Id);
                    table.ForeignKey(
                        name: "FK_days_trips_TripId",
                        column: x => x.TripId,
                        principalTable: "trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_days_TripId",
                table: "days",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_days_Uid",
                table: "days",
                column: "Uid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_activities_days_DayId",
                table: "activities",
                column: "DayId",
                principalTable: "days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activities_days_DayId",
                table: "activities");

            migrationBuilder.DropTable(
                name: "days");

            migrationBuilder.DropColumn(
                name: "IsPublicated",
                table: "trips");

            migrationBuilder.RenameColumn(
                name: "DayId",
                table: "activities",
                newName: "TripId");

            migrationBuilder.RenameIndex(
                name: "IX_activities_DayId",
                table: "activities",
                newName: "IX_activities_TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_activities_trips_TripId",
                table: "activities",
                column: "TripId",
                principalTable: "trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
