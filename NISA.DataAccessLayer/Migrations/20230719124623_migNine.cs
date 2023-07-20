using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NISA.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class migNine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "userDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "roleId",
                table: "userDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmployeeRole",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRole", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userDetails_roleId",
                table: "userDetails",
                column: "roleId");

            migrationBuilder.AddForeignKey(
                name: "FK_userDetails_EmployeeRole_roleId",
                table: "userDetails",
                column: "roleId",
                principalTable: "EmployeeRole",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userDetails_EmployeeRole_roleId",
                table: "userDetails");

            migrationBuilder.DropTable(
                name: "EmployeeRole");

            migrationBuilder.DropIndex(
                name: "IX_userDetails_roleId",
                table: "userDetails");

            migrationBuilder.DropColumn(
                name: "role",
                table: "userDetails");

            migrationBuilder.DropColumn(
                name: "roleId",
                table: "userDetails");
        }
    }
}
