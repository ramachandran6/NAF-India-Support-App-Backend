using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NISA.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class migSix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "department",
                table: "userDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "department",
                table: "ticketHistoryTables",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "department",
                table: "ticketDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "department",
                table: "userDetails");

            migrationBuilder.DropColumn(
                name: "department",
                table: "ticketHistoryTables");

            migrationBuilder.DropColumn(
                name: "department",
                table: "ticketDetails");
        }
    }
}
