
using _02_CodeFirstMigration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace _02_CodeFirstMigration.Data.Configuration
{
    internal class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.HasKey(s => s.Id); // this line is added to set the Id as the primary key
            builder.Property(s => s.Id).ValueGeneratedNever(); // this line is added to prevent the auto increment of the Id

            builder.Property(s => s.SectionName)
                .HasColumnType("VARCHAR") // this line is added to set the data type of the SectionName to VARCHAR
                .HasMaxLength(50) // this line is added to set the maximum length of the SectionName to 50
                .IsRequired(); // this line is added to make the SectionName required

            
            // Define the relationship with Course
            builder.HasOne(s => s.Course) // This line specifies that the Section entity has a one-to-one relationship with the Course entity
                .WithMany(c => c.Sections) // This line specifies that the Course entity has a one-to-many relationship with the Section entity
                .HasForeignKey(s => s.CourseId); // This line specifies the foreign key property in the Section entity

            // Define the relationship with Instructor
            builder.HasOne(s => s.Instructor) // This line specifies that the Section entity has a one-to-one relationship with the Instructor entity
                .WithMany(i => i.Sections) // This line specifies that the Instructor entity has a one-to-many relationship with the Section entity
                .HasForeignKey(s => s.InstructorId); // This line specifies the foreign key property in the Section entity

            builder.HasMany(s => s.SectionSchedules) // This line specifies that the Section entity has a one-to-many relationship with the SectionSchedule entity
                .WithOne(ss => ss.Section) // This line specifies that the SectionSchedule entity has a one-to-one relationship with the Section entity
                .HasForeignKey(ss => ss.SectionId); // This line specifies the foreign key property in the SectionSchedule entity

            builder.HasMany(s => s.Enrollments) // This line specifies that the Section entity has a one-to-many relationship with the Enrollment entity
                .WithOne(e => e.Section) // This line specifies that the Enrollment entity has a one-to-one relationship with the Section entity
                .HasForeignKey(e => e.SectionId); // This line specifies the foreign key property in the Enrollment entity

            builder.HasIndex(s => s.CourseId); // This line is added to create an index on the CourseId column

            builder.HasIndex(s => s.InstructorId); // This line is added to create an index on the InstructorId column

            builder.ToTable("Sections"); // this line is added to set the table name to Sections

            // this line is added to insert data into the Sections table
            builder.HasData(LoadSections());
        }

        private Section[] LoadSections()
        {
            return new Section[]
            {
                new Section { Id = 1,  SectionName = "S_MA1", CourseId = 1, InstructorId = 1 },
                new Section { Id = 2,  SectionName = "S_MA2", CourseId = 1, InstructorId = 2 },
                new Section { Id = 3,  SectionName = "S_PH1", CourseId = 2, InstructorId = 1 },
                new Section { Id = 4,  SectionName = "S_PH2", CourseId = 2, InstructorId = 3 },
                new Section { Id = 5,  SectionName = "S_CH1", CourseId = 3, InstructorId = 2 },
                new Section { Id = 6,  SectionName = "S_CH2", CourseId = 3, InstructorId = 3 },
                new Section { Id = 7,  SectionName = "S_BI1", CourseId = 4, InstructorId = 4 },
                new Section { Id = 8,  SectionName = "S_BI2", CourseId = 4, InstructorId = 5 },
                new Section { Id = 9,  SectionName = "S_CS1", CourseId = 5, InstructorId = 4 },
                new Section { Id = 10, SectionName = "S_CS2", CourseId = 5, InstructorId = 5 },
                new Section { Id = 11, SectionName = "S_CS3", CourseId = 5, InstructorId = 4 }
            };
        }
    }
}
