using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NISA.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class migOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "attachmentDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ticketId = table.Column<int>(type: "int", nullable: false),
                    fileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    uploadedDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attachmentDetails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lookUpTables",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    category = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lookUpTables", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "userDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: true),
                    departmentLookupRefId = table.Column<int>(type: "int", nullable: true),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_userDetails_lookUpTables_departmentLookupRefId",
                        column: x => x.departmentLookupRefId,
                        principalTable: "lookUpTables",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ticketDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: true),
                    ticketRefnum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    departmentLookUpId = table.Column<int>(type: "int", nullable: true),
                    startDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    endDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    owner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    priotity = table.Column<int>(type: "int", nullable: true),
                    severity = table.Column<int>(type: "int", nullable: true),
                    assignedTo = table.Column<int>(type: "int", nullable: false),
                    age = table.Column<int>(type: "int", nullable: true),
                    attachments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticketDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_ticketDetails_lookUpTables_departmentLookUpId",
                        column: x => x.departmentLookUpId,
                        principalTable: "lookUpTables",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ticketDetails_userDetails_userId",
                        column: x => x.userId,
                        principalTable: "userDetails",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ticketDetails_departmentLookUpId",
                table: "ticketDetails",
                column: "departmentLookUpId");

            migrationBuilder.CreateIndex(
                name: "IX_ticketDetails_userId",
                table: "ticketDetails",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_userDetails_departmentLookupRefId",
                table: "userDetails",
                column: "departmentLookupRefId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attachmentDetails");

            migrationBuilder.DropTable(
                name: "ticketDetails");

            migrationBuilder.DropTable(
                name: "userDetails");

            migrationBuilder.DropTable(
                name: "lookUpTables");
        }
    }
}
