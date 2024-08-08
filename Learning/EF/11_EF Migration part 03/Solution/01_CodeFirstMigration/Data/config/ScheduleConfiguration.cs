
using _01_CodeFirstMigration.Entities;
using _01_CodeFirstMigration.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _01_CodeFirstMigration.Data.config
{
    internal class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.HasKey(s => s.Id); // Set the Id property as the primary key.
            builder.Property(s => s.Id).ValueGeneratedNever(); // Prevent the Id property from being auto-incremented.

            builder.Property(s => s.Title)
                .HasConversion(
                x => x.ToString(), // when saving to the database, convert the enum to string
                x => (ScheduleEnum)Enum.Parse(typeof(ScheduleEnum), x) // when reading from the database, convert the string to enum
                );

            builder.Property(s => s.SUN)
                .IsRequired(); 

            builder.Property(s => s.MON)
                .IsRequired(); 

            builder.Property(s => s.TUE)
                .IsRequired(); 

            builder.Property(s => s.WED)
                .IsRequired(); 

            builder.Property(s => s.THU)
                .IsRequired(); 

            builder.Property(s => s.FRI)
                .IsRequired(); 

            builder.Property(s => s.SAT)
                .IsRequired(); 


            builder.ToTable("Schedules"); // Set the table name to "Schedule" in the database.



        }

    }
}
