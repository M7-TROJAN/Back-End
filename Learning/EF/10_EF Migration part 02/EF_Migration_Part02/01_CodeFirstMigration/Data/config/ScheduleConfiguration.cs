
using _01_CodeFirstMigration.Entities;
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
                .HasColumnType("VARCHAR")
                .HasMaxLength(50) 
                .IsRequired(); 

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

            // Seed initial data into the Schedules table
            builder.HasData(LoadSchedules()); // Insert predefined data into the Schedules table using the LoadSchedules method.

        }

        private static List<Schedule> LoadSchedules()
        {
            return
            [
                new Schedule { Id = 1, Title = "Daily", SUN = true, MON = true, TUE = true, WED = true, THU = true, FRI = false, SAT = false },
                new Schedule { Id = 2, Title = "DayAfterDay", SUN = true, MON = false, TUE = true, WED = false, THU = true, FRI = false, SAT = false },
                new Schedule { Id = 3, Title = "Twice-a-Week", SUN = false, MON = true, TUE = false, WED = true, THU = false, FRI = false, SAT = false },
                new Schedule { Id = 4, Title = "Weekend", SUN = false, MON = false, TUE = false, WED = false, THU = false, FRI = true, SAT = true },
                new Schedule { Id = 5, Title = "Compact", SUN = true, MON = true, TUE = true, WED = true, THU = true, FRI = true, SAT = true }

            ];
        }
    }
}
