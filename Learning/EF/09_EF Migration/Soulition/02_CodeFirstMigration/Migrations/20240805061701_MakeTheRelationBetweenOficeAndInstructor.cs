using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _02_CodeFirstMigration.Migrations
{
    /// <inheritdoc />
    public partial class MakeTheRelationBetweenOficeAndInstructor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Instructors_OfficeId",
                table: "Instructors",
                column: "OfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructors_Offices_OfficeId",
                table: "Instructors",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructors_Offices_OfficeId",
                table: "Instructors");

            migrationBuilder.DropIndex(
                name: "IX_Instructors_OfficeId",
                table: "Instructors");
        }
    }
}
