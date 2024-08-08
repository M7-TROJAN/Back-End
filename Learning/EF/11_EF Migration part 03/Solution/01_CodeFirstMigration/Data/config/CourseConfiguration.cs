
using _01_CodeFirstMigration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _01_CodeFirstMigration.Data.config
{
    internal class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id); // Set the Id property as the primary key.
            builder.Property(c => c.Id).ValueGeneratedNever(); // Prevent the Id property from being auto-incremented.

            builder.Property(c => c.CourseName)
                .HasColumnName("CourseName") // Set the column name of the CourseName property to CourseName.
                .HasColumnType("VARCHAR") // Set the data type of the CourseName property to VARCHAR.
                .HasMaxLength(50) // Set the maximum length of the CourseName property to 50 characters.
                .IsRequired(); // Make the CourseName property required (non-nullable).

            builder.Property(c => c.Price)
                .HasColumnName("Price") // Set the column name of the Price property to Price.
                .HasPrecision(15, 2) // Set the precision of the Price property to 15 digits, with 2 decimal places.
                .IsRequired(); // Make the Price property required (non-nullable).

            builder.ToTable("Courses"); // Set the table name to "Courses" in the database.

        }
    }
}
