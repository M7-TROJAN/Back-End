namespace _02_RetrieveData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    var wallets = context.Wallets;

                    foreach (var wallet in wallets)
                    {
                        Console.WriteLine(wallet);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
