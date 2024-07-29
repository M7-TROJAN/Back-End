namespace _05_UpdateData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {

                // Update the balance of the wallet with ID 13 increased to $5,000.00

                // [13] Rana ($4,500.00) -> Rana ($5,000.00)

                // 1. Find the entity
                var wallet = context.Wallets.Find(13);

                // 2. Update the entity
                wallet.Balance = 5000;

                // 3. Save the changes
                context.SaveChanges();

                Console.WriteLine("Data updated successfully!");

            }
        }
    }
}
