using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NISA.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class migSeven : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "ticketHandlingDetails");

            migrationBuilder.DropColumn(
                name: "updatedBy",
                table: "ticketHandlingDetails");

            migrationBuilder.DropColumn(
                name: "updatedOn",
                table: "ticketHandlingDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "ticketHandlingDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updatedBy",
                table: "ticketHandlingDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updatedOn",
                table: "ticketHandlingDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
