using C04.EfficientUpdating.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C04.EfficientUpdating.Data.Configuration
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedNever(); // no auto increment

            builder.Property(a => a.FName)
                .HasColumnName("FirstName")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .IsRequired();


            builder.Property(a => a.LName)
                .HasColumnName("LastName")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            builder.HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Create a unique constraint on FName and LName
            builder.HasIndex(a => new { a.FName, a.LName })
                .IsUnique(); // Ensures the combination of FName and LName is unique

            // not mapp full name
            builder.Ignore(a => a.FullName);


            builder.ToTable("Authors");
        }
    }
}
