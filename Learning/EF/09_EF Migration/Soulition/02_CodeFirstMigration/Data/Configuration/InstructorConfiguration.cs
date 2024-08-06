using _02_CodeFirstMigration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _02_CodeFirstMigration.Data.Configuration
{
    internal class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(i => i.Id); // this line is added to set the Id as the primary key
            builder.Property(i => i.Id).ValueGeneratedNever(); // this line is added to prevent the auto increment of the Id

            builder.Property(i => i.Name)
                .HasColumnType("VARCHAR") // this line is added to set the data type of the FirstName to VARCHAR
                .HasMaxLength(50) // this line is added to set the maximum length of the FirstName to 50
                .IsRequired(); // this line is added to make the FirstName required

            builder.Property(i => i.OfficeId)
                .IsRequired(); // this line is added to make the OfficeId required


            // Define the relationship with Office
            builder.HasOne(i => i.Office) // This line specifies that the Instructor entity has a one-to-one relationship with the Office entity
                .WithMany(o => o.Instructors) // This line specifies that the Office entity has a one-to-many relationship with the Instructor entity
                .HasForeignKey(i => i.OfficeId); // This line specifies the foreign key property in the Instructor entity

            // Define the relationship with Section
            builder.HasMany(i => i.Sections) // This line specifies that the Instructor entity has a one-to-many relationship with the Section entity
                .WithOne(s => s.Instructor) // This line specifies that the Section entity has a one-to-one relationship with the Instructor entity
                .HasForeignKey(s => s.InstructorId); // This line specifies the foreign key property in the Section entity


            builder.ToTable("Instructors"); // this line is added to set the table name to Instructors


            // this line is added to insert data into the Instructors table
            builder.HasData(LoadInstructors());

        }

        private static List<Instructor> LoadInstructors()
        {
            return new List<Instructor>
            {
                new Instructor { Id = 1, Name = "Ahmed Abdullah", OfficeId = 1 },
                new Instructor { Id = 2, Name = "Yasmeen Mohammed", OfficeId = 2 },
                new Instructor { Id = 3, Name = "Khalid Hassan", OfficeId = 3 },
                new Instructor { Id = 4, Name = "Nadia Ali", OfficeId = 4 },
                new Instructor { Id = 5, Name = "Omar Ibrahim", OfficeId = 5 }
            };
        }
    }
}
