using System;
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
                name: "employeeRoles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employeeRoles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "imageEntities",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: true),
                    ticketId = table.Column<int>(type: "int", nullable: true),
                    uploadedDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_imageEntities", x => x.ImageId);
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
                name: "ticketComments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ticketRefnum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    commentedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    commentedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticketComments", x => x.id);
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
                    isLoggedIn = table.Column<bool>(type: "bit", nullable: true),
                    department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    roleId = table.Column<int>(type: "int", nullable: true),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    departmentLookupRefId = table.Column<int>(type: "int", nullable: true),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_userDetails_employeeRoles_roleId",
                        column: x => x.roleId,
                        principalTable: "employeeRoles",
                        principalColumn: "id");
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
                    department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    departmentLookUpId = table.Column<int>(type: "int", nullable: true),
                    startDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    endDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    owner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    priority = table.Column<int>(type: "int", nullable: true),
                    severity = table.Column<int>(type: "int", nullable: true),
                    assignedTo = table.Column<int>(type: "int", nullable: false),
                    age = table.Column<int>(type: "int", nullable: true),
                    attachments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true),
                    isReopened = table.Column<bool>(type: "bit", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "ticketHistoryTables",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ticketRefNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    priority = table.Column<int>(type: "int", nullable: true),
                    severity = table.Column<int>(type: "int", nullable: true),
                    department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    departmentLookUpRefId = table.Column<int>(type: "int", nullable: true),
                    attachments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    endDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updatedBy = table.Column<int>(type: "int", nullable: true),
                    updatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticketHistoryTables", x => x.id);
                    table.ForeignKey(
                        name: "FK_ticketHistoryTables_lookUpTables_departmentLookUpRefId",
                        column: x => x.departmentLookUpRefId,
                        principalTable: "lookUpTables",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ticketHistoryTables_userDetails_updatedBy",
                        column: x => x.updatedBy,
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
                name: "IX_ticketHistoryTables_departmentLookUpRefId",
                table: "ticketHistoryTables",
                column: "departmentLookUpRefId");

            migrationBuilder.CreateIndex(
                name: "IX_ticketHistoryTables_updatedBy",
                table: "ticketHistoryTables",
                column: "updatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_userDetails_departmentLookupRefId",
                table: "userDetails",
                column: "departmentLookupRefId");

            migrationBuilder.CreateIndex(
                name: "IX_userDetails_roleId",
                table: "userDetails",
                column: "roleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attachmentDetails");

            migrationBuilder.DropTable(
                name: "imageEntities");

            migrationBuilder.DropTable(
                name: "ticketComments");

            migrationBuilder.DropTable(
                name: "ticketDetails");

            migrationBuilder.DropTable(
                name: "ticketHistoryTables");

            migrationBuilder.DropTable(
                name: "userDetails");

            migrationBuilder.DropTable(
                name: "employeeRoles");

            migrationBuilder.DropTable(
                name: "lookUpTables");
        }
    }
}
