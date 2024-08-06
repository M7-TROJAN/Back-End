
using _02_CodeFirstMigration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Reflection;

namespace _02_CodeFirstMigration.Data.Configuration
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id); // this line is added to set the Id as the primary key
            builder.Property(s => s.Id).ValueGeneratedNever(); // this line is added to prevent the auto increment of the Id

            builder.HasAlternateKey(s => s.Name); // Unique constraint means that the value of the property must be unique across all rows in the table

            builder.Property(s => s.Name)
                .HasColumnType("VARCHAR") // this line is added to set the data type of the Name to VARCHAR
                .HasMaxLength(60) // this line is added to set the maximum length of the Name to 255
                .IsRequired(); // this line is added to make the Name required

            builder.Property(s => s.Gender)
                .HasColumnType("CHAR") // this line is added to set the data type
                .HasMaxLength(1) // this line is added to set the maximum length
                .IsRequired(); // this line is added to make

            builder.ToTable("Students"); // this line is added to set the table name to Students

            builder.ToTable(t => t.HasCheckConstraint("chk_Gender", "Gender IN ('f', 'm')")); // this line is added to set the check constraint

            // Define the relationship between Student and Enrollment entities
            builder.HasMany(s => s.Enrollments) // Specify that a Student can be associated with many Enrollments
                .WithOne(e => e.Student) // Specify that each Enrollment is associated with one Student
                .HasForeignKey(e => e.StudentId); // Specify the foreign key property in the Enrollment entity that references the Student entity

            // this line is added to insert data into the Students table
            builder.HasData(LoadStudents());
        }

        private Student[] LoadStudents()
        {
            return new Student[]
            {
                new Student { Id = 1, Name = "Fatima Ali", Gender = 'f' },
                new Student { Id = 2, Name = "Noor Saleh", Gender = 'f' },
                new Student { Id = 3, Name = "Omar Youssef", Gender = 'm' },
                new Student { Id = 4, Name = "Huda Ahmed", Gender = 'm' },
                new Student { Id = 5, Name = "Amira Tariq", Gender = 'f' },
                new Student { Id = 6, Name = "Zainab Ismail", Gender = 'f' },
                new Student { Id = 7, Name = "Yousef Farid", Gender = 'm' },
                new Student { Id = 8, Name = "Layla Mustafa", Gender = 'f' },
                new Student { Id = 9, Name = "Mohammed Adel", Gender = 'm' },
                new Student { Id = 10, Name = "Samira Nabil", Gender = 'f' }
            };
        }

    }
}
