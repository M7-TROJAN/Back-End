
using _02_CodeFirstMigration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _02_CodeFirstMigration.Data.Configuration
{
    public class EnrollmentConfigyration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            // Define the composite primary key for the Enrollment entity
            builder.HasKey(e => new { e.StudentId, e.SectionId }); // Set the composite primary key consisting of StudentId and SectionId


            // Define the relationship between Enrollment and Student entities
            builder.HasOne(e => e.Student) // Each Enrollment is associated with one Student
                .WithMany(s => s.Enrollments) // Each Student can be associated with multiple Enrollments
                .HasForeignKey(e => e.StudentId); // Specify the foreign key property in the Enrollment entity that references the Student entity


            // Define the relationship between Enrollment and Section entities
            builder.HasOne(e => e.Section) // Each Enrollment is associated with one Section
                .WithMany(s => s.Enrollments) // Each Section can be associated with multiple Enrollments
                .HasForeignKey(e => e.SectionId); // Specify the foreign key property in the Enrollment entity that references the Section entity

            builder.HasIndex(e => e.SectionId); // Create an index on the SectionId column


            // Set the table name for the Enrollment entity
            builder.ToTable("Enrollments");
            // Specify that the Enrollment entity maps to the "Enrollments" table in the database


            // this line is added to insert data into the Enrollments table
            builder.HasData(LoadEnrollments());

        }

        private Enrollment[] LoadEnrollments()
        {
            return new Enrollment[]
            {
                new Enrollment { StudentId = 1, SectionId = 6 },
                new Enrollment { StudentId = 2, SectionId = 6 },
                new Enrollment { StudentId = 3, SectionId = 7 },
                new Enrollment { StudentId = 4, SectionId = 7 },
                new Enrollment { StudentId = 5, SectionId = 8 },
                new Enrollment { StudentId = 6, SectionId = 8 },
                new Enrollment { StudentId = 7, SectionId = 9 },
                new Enrollment { StudentId = 8, SectionId = 9 },
                new Enrollment { StudentId = 9, SectionId = 10 },
                new Enrollment { StudentId = 10, SectionId = 10 }
            };
        }

    }
}
