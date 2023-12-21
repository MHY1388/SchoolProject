using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class removemanytomany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Section_Days_DayId",
                table: "Section");

            migrationBuilder.DropTable(
                name: "PresenceSection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Section",
                table: "Section");

            migrationBuilder.RenameTable(
                name: "Section",
                newName: "Sections");

            migrationBuilder.RenameIndex(
                name: "IX_Section_DayId",
                table: "Sections",
                newName: "IX_Sections_DayId");

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "Presences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sections",
                table: "Sections",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Presences_SectionId",
                table: "Presences",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Presences_Sections_SectionId",
                table: "Presences",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Days_DayId",
                table: "Sections",
                column: "DayId",
                principalTable: "Days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presences_Sections_SectionId",
                table: "Presences");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Days_DayId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Presences_SectionId",
                table: "Presences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sections",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "Presences");

            migrationBuilder.RenameTable(
                name: "Sections",
                newName: "Section");

            migrationBuilder.RenameIndex(
                name: "IX_Sections_DayId",
                table: "Section",
                newName: "IX_Section_DayId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Section",
                table: "Section",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Section_Days_DayId",
                table: "Section",
                column: "DayId",
                principalTable: "Days",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
