using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NISA.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class migEight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "userId",
                table: "ticketHandlingDetails",
                newName: "genUserId");

            migrationBuilder.AddColumn<int>(
                name: "deptUserId",
                table: "ticketHandlingDetails",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deptUserId",
                table: "ticketHandlingDetails");

            migrationBuilder.RenameColumn(
                name: "genUserId",
                table: "ticketHandlingDetails",
                newName: "userId");
        }
    }
}
