﻿
using _01_CodeFirstMigration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _01_CodeFirstMigration.Data.config
{
    internal class OfficeConfiguration : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> builder)
        {
            builder.HasKey(o => o.Id); // Set the primary key of the Office entity to be the Id property.
            builder.Property(o => o.Id).ValueGeneratedNever(); // Prevent the Id property from being auto-incremented.

            builder.Property(o => o.OfficeName)
                .HasColumnName("OfficeName") // Set the column name of the OfficeName property to OfficeName.
                .HasColumnType("VARCHAR") // Set the data type of the OfficeName property to VARCHAR.
                .HasMaxLength(50); // Set the maximum length of the OfficeName property to 50 characters.

            builder.Property(o => o.OfficeLocation)
                .HasColumnName("OfficeLocation") // Set the column name of the OfficeLocation property to OfficeLocation.
                .HasColumnType("VARCHAR") // Set the data type of the OfficeLocation property to VARCHAR.
                .HasMaxLength(50); // Set the maximum length of the OfficeLocation property to 50 characters.


            builder.ToTable("Offices"); // Set the table name to "Offices" in the database.

            // Seed initial data into the Offices table
            builder.HasData(LoadOffices()); // Insert predefined data into the Offices table using the LoadOffices method.
        }

        private static List<Office> LoadOffices()
        {
            return
            [

                    new Office { Id = 1, OfficeName = "Off_05", OfficeLocation = "building A"},
                    new Office { Id = 2, OfficeName = "Off_12", OfficeLocation = "building B"},
                    new Office { Id = 3, OfficeName = "Off_32", OfficeLocation = "Adminstration"},
                    new Office { Id = 4, OfficeName = "Off_44", OfficeLocation = "IT Department"},
                    new Office { Id = 5, OfficeName = "Off_43", OfficeLocation = "IT Department"}
            ];
        }
    }
}