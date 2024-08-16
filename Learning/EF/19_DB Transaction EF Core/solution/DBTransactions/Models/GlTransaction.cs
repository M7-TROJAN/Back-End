namespace DBTransactions.Models
{
    public class GlTransaction
    {
        public int ID { get;}

        public decimal Amount { get;}

        public string Notes { get;}

        public string AccountID { get;}

        public DateTime TransactionDate { get;}

        public GlTransaction(decimal amount, string notes, DateTime transactionDate)
        {
            Amount = amount;
            Notes = notes;
            TransactionDate = transactionDate;
        }
    }
}