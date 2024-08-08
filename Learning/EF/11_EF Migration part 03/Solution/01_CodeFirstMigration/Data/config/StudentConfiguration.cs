using _01_CodeFirstMigration.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _01_CodeFirstMigration.Data.config
{
    internal class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).ValueGeneratedNever();

            builder.Property(s => s.FName)
                .HasColumnName("FName")
                .HasColumnType("VARCHAR")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(s => s.LName)
                .HasColumnName("LName")
                .HasColumnType("VARCHAR")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(s => s.Gender)
                .HasColumnName("Gender")
                .HasColumnType("CHAR")
                .HasMaxLength(1)
                .IsRequired();


            builder.ToTable(t => t.HasCheckConstraint("chk_Gender", "Gender IN ('f', 'm')"));
            
            builder.ToTable("Students");

        }

    }
}
