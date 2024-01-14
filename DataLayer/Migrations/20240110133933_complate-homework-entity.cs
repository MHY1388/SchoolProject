using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class complatehomeworkentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "HomeWorks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HomeWorks_ClassId",
                table: "HomeWorks",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeWorks_Classes_ClassId",
                table: "HomeWorks",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeWorks_Classes_ClassId",
                table: "HomeWorks");

            migrationBuilder.DropIndex(
                name: "IX_HomeWorks_ClassId",
                table: "HomeWorks");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "HomeWorks");
        }
    }
}
