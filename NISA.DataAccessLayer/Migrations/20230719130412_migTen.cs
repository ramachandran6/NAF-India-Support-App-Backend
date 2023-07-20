using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NISA.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class migTen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userDetails_EmployeeRole_roleId",
                table: "userDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeRole",
                table: "EmployeeRole");

            migrationBuilder.RenameTable(
                name: "EmployeeRole",
                newName: "employeeRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_employeeRoles",
                table: "employeeRoles",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_userDetails_employeeRoles_roleId",
                table: "userDetails",
                column: "roleId",
                principalTable: "employeeRoles",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userDetails_employeeRoles_roleId",
                table: "userDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_employeeRoles",
                table: "employeeRoles");

            migrationBuilder.RenameTable(
                name: "employeeRoles",
                newName: "EmployeeRole");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeRole",
                table: "EmployeeRole",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_userDetails_EmployeeRole_roleId",
                table: "userDetails",
                column: "roleId",
                principalTable: "EmployeeRole",
                principalColumn: "id");
        }
    }
}
