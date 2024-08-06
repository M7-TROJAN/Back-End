using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace _02_CodeFirstMigration.Migrations
{
    /// <inheritdoc />
    public partial class CreateStudentClassAndEnrollmentClassAndTheirConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(60)", maxLength: 60, nullable: false),
                    Gender = table.Column<string>(type: "CHAR(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.UniqueConstraint("AK_Students_Name", x => x.Name);
                    table.CheckConstraint("chk_Gender", "Gender IN ('f', 'm')");
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => new { x.StudentId, x.SectionId });
                    table.ForeignKey(
                        name: "FK_Enrollments_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Gender", "Name" },
                values: new object[,]
                {
                    { 1, "f", "Fatima Ali" },
                    { 2, "f", "Noor Saleh" },
                    { 3, "m", "Omar Youssef" },
                    { 4, "m", "Huda Ahmed" },
                    { 5, "f", "Amira Tariq" },
                    { 6, "f", "Zainab Ismail" },
                    { 7, "m", "Yousef Farid" },
                    { 8, "f", "Layla Mustafa" },
                    { 9, "m", "Mohammed Adel" },
                    { 10, "f", "Samira Nabil" }
                });

            migrationBuilder.InsertData(
                table: "Enrollments",
                columns: new[] { "SectionId", "StudentId" },
                values: new object[,]
                {
                    { 6, 1 },
                    { 6, 2 },
                    { 7, 3 },
                    { 7, 4 },
                    { 8, 5 },
                    { 8, 6 },
                    { 9, 7 },
                    { 9, 8 },
                    { 10, 9 },
                    { 10, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_SectionId",
                table: "Enrollments",
                column: "SectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
