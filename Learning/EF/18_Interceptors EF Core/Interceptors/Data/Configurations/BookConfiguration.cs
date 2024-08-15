
using Interceptors.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Interceptors.Data.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd(); // Auto-increment

            builder.Property(b => b.Title)
                .HasColumnName("Title") // Column name in the database
                .HasColumnType("VARCHAR") // Data type in the database
                .IsRequired().
                HasMaxLength(100);

            builder.Property(b => b.Author)
                .HasColumnType("VARCHAR") // Data type in the database
                .HasColumnName("Author") // Column name in the database
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.IsDeleted)
                .HasColumnType("BIT")
                .HasDefaultValue(false);

            builder.Property(b => b.DeletedAt)
                .HasColumnType("DATETIME")
                .HasDefaultValue(null);

            builder.HasQueryFilter(b => !b.IsDeleted);


            builder.ToTable("Books"); // Table name in the database
        }
    }
}
