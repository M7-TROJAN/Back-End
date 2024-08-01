using _01_InternalConfiguration.Data;

namespace _01_InternalConfiguration
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                foreach (var wallet in context.Wallets)
                {
                    Console.WriteLine(wallet);
                }
            }

        }
    }
}
