namespace _06_DeleteData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                // delete wallet where id = 9

                var wallet = context.Wallets.Find(9);

                if (wallet != null)
                {
                    context.Wallets.Remove(wallet);
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Wallet not found");
                }
            }
        }
    }
}
