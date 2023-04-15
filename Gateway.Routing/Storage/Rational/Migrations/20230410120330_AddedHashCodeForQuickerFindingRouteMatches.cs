using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Routing.Migrations
{
    /// <inheritdoc />
    public partial class AddedhashCodeforquickerfindingroutematches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MatchHashCode",
                table: "RouteConfigs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchHashCode",
                table: "RouteConfigs");
        }
    }
}
