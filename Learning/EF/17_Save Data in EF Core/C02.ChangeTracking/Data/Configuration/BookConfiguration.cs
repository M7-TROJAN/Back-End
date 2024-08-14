using C02.ChangeTracking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C02.ChangeTracking.Data.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd(); // auto increment

            builder.Property(a => a.Title)
                .HasColumnName("Title")
                .HasColumnType("varchar")
                .HasMaxLength(250)
                .IsRequired();

            builder.ToTable("Books");
        }
    }
}
