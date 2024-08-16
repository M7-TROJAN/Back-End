
using DBTransactions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Interceptors.Data.Configurations
{
    public class GlTransactionConfiguration : IEntityTypeConfiguration<GlTransaction>
    {
        public void Configure(EntityTypeBuilder<GlTransaction> builder)
        {
            builder.HasKey(t => t.ID);

            builder.Property(t => t.Amount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(t => t.Notes).IsRequired();
            builder.Property(t => t.TransactionDate).IsRequired();

            builder.ToTable("GlTransactions");

        }
    }
}
