using C04.EfficientUpdating.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C04.EfficientUpdating.Data.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedNever(); // no auto increment

            builder.Property(a => a.Title)
                .HasColumnName("Title")
                .HasColumnType("varchar")
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(a => a.Price)
                .HasColumnName("Price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.ToTable("Books");
        }
    }
}
