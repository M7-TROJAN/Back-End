
using _01_CodeFirstMigration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _01_CodeFirstMigration.Data.config
{
    internal class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.HasKey(s => s.Id); // Set the Id property as the primary key.
            builder.Property(s => s.Id).ValueGeneratedNever(); // Prevent the Id property from being auto-incremented.

            builder.Property(s => s.SectionName)
                .HasColumnName("SectionName")
                .HasColumnType("VARCHAR")
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(s => s.Course)
                .WithMany(c => c.Sections)
                .HasForeignKey(s => s.CourseId)
                .IsRequired();

            builder.HasOne(s => s.Instructor)
               .WithMany(i => i.Sections)
               .HasForeignKey(s => s.InstructorId)
               .IsRequired(false);

            builder.HasOne(c => c.Schedule)
                .WithMany(x => x.Sections)
                .HasForeignKey(x => x.ScheduleId)
                .IsRequired();

            builder.HasMany(s => s.Students)
                .WithMany(x => x.Sections)
                .UsingEntity<Enrollment>();

            builder.OwnsOne(s => s.TimeSlot, ts =>
            {
                ts.Property(t => t.StartTime)
                    .HasColumnName("StartTime")
                    .HasColumnType("TIME")
                    .IsRequired();

                ts.Property(t => t.EndTime)
                    .HasColumnName("EndTime")
                    .HasColumnType("TIME")
                    .IsRequired();
            });


            builder.ToTable("Sections"); // Set the table name to "Sections" in the database.
        }
    }
}
