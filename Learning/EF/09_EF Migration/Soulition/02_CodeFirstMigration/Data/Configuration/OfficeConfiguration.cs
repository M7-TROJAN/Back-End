
using _02_CodeFirstMigration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _02_CodeFirstMigration.Data.Configuration
{
    internal class OfficeConfiguration : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> builder)
        {
            builder.HasKey(o => o.Id); // this line is added to set the Id as the primary key
            builder.Property(o => o.Id).ValueGeneratedNever(); // this line is added to prevent the auto increment of the Id


            /*builder.Property(o => o.OfficeName).HasMaxLength(50);*/ // nvarchar(50) we need to change it to varchar(50)

            builder.Property(o => o.OfficeName).HasColumnType("VARCHAR") // this line is added to set the data type of the OfficeName to VARCHAR
                .HasMaxLength(50) // this line is added to set the maximum length of the OfficeName to 50
                .IsRequired(); // this line is added to make the OfficeName required

            builder.Property(o => o.OfficeLocation).HasColumnType("VARCHAR") // this line is added to set the data type of the OfficeLocation to VARCHAR
                .HasMaxLength(50) // this line is added to set the maximum length of the OfficeLocation to 50
                .IsRequired(); // this line is added to make the OfficeLocation required

            // Define the relationship with Instructor
            builder.HasMany(o => o.Instructors) // This line specifies that the Office entity has a one-to-many relationship with the Instructor entity
                .WithOne(i => i.Office) // This line specifies that the Instructor entity has a one-to-one relationship with the Office entity
                .HasForeignKey(i => i.OfficeId); // This line specifies the foreign key property in the Instructor entity


            builder.ToTable("Offices"); // this line is added to set the table name to Offices

            // this line is added to insert data into the Offices table
            builder.HasData(LoadOffices());
        }

        private static List<Office> LoadOffices()
        {
            return new List<Office>
            {
                new Office { Id = 1, OfficeName = "Off_05", OfficeLocation = "building A" },
                new Office { Id = 2, OfficeName = "Off_12", OfficeLocation = "building B" },
                new Office { Id = 3, OfficeName = "Off_32", OfficeLocation = "Adminstration" },
                new Office { Id = 4, OfficeName = "Off_44", OfficeLocation = "IT Department" },
                new Office { Id = 5, OfficeName = "Off_43", OfficeLocation = "IT Department" }
            };
        }
    }
}
