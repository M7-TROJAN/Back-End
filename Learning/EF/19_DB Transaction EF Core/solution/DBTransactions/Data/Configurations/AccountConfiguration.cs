
using DBTransactions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Interceptors.Data.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.HasKey(a => a.AccountID);

            builder.Property(a => a.AccountHolder).IsRequired();
            builder.Property(a => a.CurrentBalance).HasColumnType("decimal(18,2)").IsRequired();

            builder.HasMany(b => b.Transactions)
                .WithOne()
                .HasForeignKey(t => t.AccountID);
                

            builder.ToTable("BankAccounts");

        }
    }
}
