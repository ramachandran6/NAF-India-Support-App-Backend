using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NISA.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class migFour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ticketComments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ticketRefnum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    commentedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    commentedOn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticketComments", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ticketComments");
        }
    }
}
