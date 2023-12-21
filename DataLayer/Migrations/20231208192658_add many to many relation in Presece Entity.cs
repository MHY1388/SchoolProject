using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class addmanytomanyrelationinPreseceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presences_Section_SectionId",
                table: "Presences");

            migrationBuilder.DropIndex(
                name: "IX_Presences_SectionId",
                table: "Presences");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "Presences");

            migrationBuilder.CreateTable(
                name: "PresenceSection",
                columns: table => new
                {
                    DataId = table.Column<int>(type: "int", nullable: false),
                    UsagesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresenceSection", x => new { x.DataId, x.UsagesId });
                    table.ForeignKey(
                        name: "FK_PresenceSection_Presences_DataId",
                        column: x => x.DataId,
                        principalTable: "Presences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PresenceSection_Section_UsagesId",
                        column: x => x.UsagesId,
                        principalTable: "Section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PresenceSection_UsagesId",
                table: "PresenceSection",
                column: "UsagesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PresenceSection");

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "Presences",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Presences_SectionId",
                table: "Presences",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Presences_Section_SectionId",
                table: "Presences",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id");
        }
    }
}
