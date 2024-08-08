using _01_CodeFirstMigration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace _01_CodeFirstMigration.Data.config
{
    internal class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(i => i.Id); // Set the Id property as the primary key.
            builder.Property(i => i.Id).ValueGeneratedNever(); // Prevent the Id property from being auto-incremented.

            builder.Property(i => i.FName)
                .HasColumnName("FName") // Set the column name of the FName property to FName.
                .HasColumnType("VARCHAR") // Set the data type of the FName property to VARCHAR.
                .HasMaxLength(50) // Set the maximum length of the FName property to 50 characters.
                .IsRequired(); // Make the FName property required (non-nullable).

            builder.Property(i => i.LName)
                .HasColumnName("LName") // Set the column name of the LName property to LName.
                .HasColumnType("VARCHAR") // Set the data type of the LName property to VARCHAR.
                .HasMaxLength(50) // Set the maximum length of the LName property to 50 characters.
                .IsRequired(); // Make the LName property required (non-nullable).

            builder.Property(i => i.OfficeId)
                .HasColumnName("OfficeId") // Set the column name of the OfficeId property to OfficeId.
                .HasColumnType("INT") // Set the data type of the OfficeId property to INT.
                .IsRequired(); // Make the OfficeId property required (non-nullable).

            builder.HasOne(i => i.Office) // Specify that the Instructor entity has a reference to a single Office entity.
                .WithOne(o => o.Instructor) // Specify that the Office entity has a reference to a single Instructor entity.
                .HasForeignKey<Instructor>(x => x.OfficeId) // Specify that the Instructor entity has a foreign key property named OfficeId.
                .IsRequired(false); // Make the OfficeId property optional (nullable).

            builder.ToTable("Instructors"); // Set the table name to "Instructors" in the database.



        }
    }
}
