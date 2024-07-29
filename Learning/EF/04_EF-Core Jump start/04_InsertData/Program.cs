using Microsoft.EntityFrameworkCore;

namespace _04_InsertData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    // Create and add a new wallet
                    var wallet = new Wallet
                    {
                        Holder = "Rana",
                        Balance = 4500m
                    };

                    context.Wallets.Add(wallet);

                    // Save the changes and check if the wallet was added successfully
                    if (context.SaveChanges() > 0)
                    {
                        Console.WriteLine("Wallet added successfully");
                        Console.WriteLine($"New Wallet ID: {wallet.Id}"); // Retrieve and display the new wallet ID
                    }
                    else
                    {
                        Console.WriteLine("Failed to add wallet");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

            }
        }
    }
}
