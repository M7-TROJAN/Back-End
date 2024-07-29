namespace _08_ImplementTransaction
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // make a transfer from wallet 7 to wallet 10 with amount 1000

                        var ammountToTransfer = 1000;

                        var walletFrom = context.Wallets.Find(7);
                        var walletTo = context.Wallets.Find(10);

                        if(walletFrom is null || walletTo is null)
                        {
                            Console.WriteLine("Wallet not found");
                            return;
                        }

                        // operation #1 withdraw `ammountToTransfer` from wallet 7
                        walletFrom.Balance -= ammountToTransfer;
                        context.SaveChanges();

                        // operation #2 deposit `ammountToTransfer` to wallet 10
                        walletTo.Balance += ammountToTransfer;
                        context.SaveChanges();


                        transaction.Commit();

                        Console.WriteLine("Transfer done successfully");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
