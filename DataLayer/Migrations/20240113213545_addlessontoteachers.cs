using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class addlessontoteachers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "Teachers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_LessonId",
                table: "Teachers",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Lessons_LessonId",
                table: "Teachers",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Lessons_LessonId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_LessonId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Teachers");
        }
    }
}
