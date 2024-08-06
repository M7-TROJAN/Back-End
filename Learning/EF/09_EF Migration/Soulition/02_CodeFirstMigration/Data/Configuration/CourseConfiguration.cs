

using _02_CodeFirstMigration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _02_CodeFirstMigration.Data.Configuration
{
    internal class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id); // this line is added to set the Id as the primary key
            builder.Property(c => c.Id).ValueGeneratedNever(); // this line is added to prevent the auto increment of the Id

            builder.Property(c => c.CourseName)
                .HasColumnType("VARCHAR") // this line is added to set the data type of the CourseName to VARCHAR
                .HasMaxLength(50) // this line is added to set the maximum length of the CourseName to 50
                .IsRequired(); // this line is added to make the CourseName required

            builder.Property(c => c.Price)
                .HasPrecision(15, 2) // this line is added to set the precision of the Price to 15,2 (DESIMAL(15,2))
                .IsRequired(); // this line is added to make the Price required

            // Define the relationship with Section
            builder.HasMany(c => c.Sections) // This line specifies that the Course entity has a one-to-many relationship with the Section entity
                .WithOne(s => s.Course) // This line specifies that the Section entity has a one-to-one relationship with the Course entity
                .HasForeignKey(s => s.CourseId); // This line specifies the foreign key property in the Section entity

            builder.ToTable("Courses"); // this line is added to set the table name to Courses

            // this line is added to insert data into the Courses table
            builder.HasData(LoadCourses());

        }

        private static List<Course> LoadCourses()
        {
            return new List<Course>
            {
                new Course { Id = 1, CourseName = "Mathematics", Price = 1000.00M },
                new Course { Id = 2, CourseName = "Physics", Price = 2000.00M },
                new Course { Id = 3, CourseName = "Chemistry", Price = 1500.00M },
                new Course { Id = 4, CourseName = "Biology", Price = 1200.00M },
                new Course { Id = 5, CourseName = "Computer Science", Price = 3000.00M }
            };
        }
    }
}
