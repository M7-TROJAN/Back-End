namespace _03_RetrieveSingleItem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    var idToFind = 1;

                    // retrive by primary key
                    var wallet = context.Wallets.Find(idToFind);

                    if (wallet is null)
                    {
                        Console.WriteLine("Wallet not found");
                    }
                    else 
                    {
                        Console.WriteLine(wallet);
                    }


                    // retrive by LINQ

                    var Holder = "Mansour";

                    var wallet2 = context.Wallets
                        .FirstOrDefault(w => w.Holder == Holder);

                    if (wallet2 is null)
                    {
                        Console.WriteLine("Wallet not found");
                        return;
                    }

                    // else Display the wallet
                    Console.WriteLine(wallet2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
