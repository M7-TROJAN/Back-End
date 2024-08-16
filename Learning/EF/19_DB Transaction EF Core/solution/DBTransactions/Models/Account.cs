
namespace DBTransactions.Models
{
    public class BankAccount
    {
        public string AccountID { get; set; }
        public string AccountHolder { get; set; }
        public decimal CurrentBalance { get; set; }

        public List<GlTransaction> Transactions { get; set; } = new();

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be greater than zero.");
            }

            CurrentBalance += amount;
            Transactions.Add(new GlTransaction(amount, "DEPOSIT", DateTime.Now));
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Withdrawal amount must be greater than zero.");
            }

            if (CurrentBalance < amount)
            {
                throw new InvalidOperationException("Insufficient funds.");
            }

            CurrentBalance -= amount;
            Transactions.Add(new GlTransaction(amount * -1, "WITHDRAWAL", DateTime.Now));
        }
    }
}
