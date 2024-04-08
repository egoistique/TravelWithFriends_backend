using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travel.Context.Migrations.PgSql.Migrations
{
    /// <inheritdoc />
    public partial class addFromSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FromSearch",
                table: "activities",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromSearch",
                table: "activities");
        }
    }
}
