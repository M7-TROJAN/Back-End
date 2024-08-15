using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Interceptors.Migrations
{
    /// <inheritdoc />
    public partial class modifyColumnsName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Books",
                newName: "BookTitle");

            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Books",
                newName: "BookAuthor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookTitle",
                table: "Books",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "BookAuthor",
                table: "Books",
                newName: "Author");
        }
    }
}
