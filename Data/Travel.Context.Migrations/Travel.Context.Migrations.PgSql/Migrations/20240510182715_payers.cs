using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travel.Context.Migrations.PgSql.Migrations
{
    /// <inheritdoc />
    public partial class payers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activities_users_PayerId",
                table: "activities");

            migrationBuilder.DropIndex(
                name: "IX_activities_PayerId",
                table: "activities");

            migrationBuilder.DropColumn(
                name: "PayerId",
                table: "activities");

            migrationBuilder.CreateTable(
                name: "user_wastes",
                columns: table => new
                {
                    PayedActivitiesId = table.Column<int>(type: "integer", nullable: false),
                    PayersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_wastes", x => new { x.PayedActivitiesId, x.PayersId });
                    table.ForeignKey(
                        name: "FK_user_wastes_activities_PayedActivitiesId",
                        column: x => x.PayedActivitiesId,
                        principalTable: "activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_wastes_users_PayersId",
                        column: x => x.PayersId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_wastes_PayersId",
                table: "user_wastes",
                column: "PayersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_wastes");

            migrationBuilder.AddColumn<Guid>(
                name: "PayerId",
                table: "activities",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_activities_PayerId",
                table: "activities",
                column: "PayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_activities_users_PayerId",
                table: "activities",
                column: "PayerId",
                principalTable: "users",
                principalColumn: "Id");
        }
    }
}
