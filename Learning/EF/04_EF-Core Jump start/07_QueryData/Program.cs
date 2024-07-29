namespace _07_QueryData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                // get wallets where balance > 4000

                context.Database.EnsureCreated();

                var wallets = context.Wallets.Where(w => w.Balance > 4000);

                // you can also use the following query
                // var wallets = from w in context.Wallets where w.Balance > 4000 select w;

                // you can order the result by Holder
                // var wallets = context.Wallets.Where(w => w.Balance > 4000).OrderBy(w => w.Holder);

                Console.WriteLine("Wallets with balance > 4000:");
                foreach (var wallet in wallets)
                {
                    Console.WriteLine(wallet);
                }
            }
        }
    }
}
