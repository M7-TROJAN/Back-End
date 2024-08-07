using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace _01_CodeFirstMigration.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FName = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    LName = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "CHAR(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.CheckConstraint("chk_Gender", "Gender IN ('f', 'm')");
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "FName", "Gender", "LName" },
                values: new object[,]
                {
                    { 1, "Fatima", "f", "Ali" },
                    { 2, "Noor", "f", "Saleh" },
                    { 3, "Omar", "m", "Youssef" },
                    { 4, "Huda", "m", "Ahmed" },
                    { 5, "Amira", "f", "Tariq" },
                    { 6, "Zainab", "f", "Ismail" },
                    { 7, "Yousef", "m", "Farid" },
                    { 8, "Layla", "f", "Mustafa" },
                    { 9, "Mohammed", "m", "Adel" },
                    { 10, "Samira", "f", "Nabil" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
