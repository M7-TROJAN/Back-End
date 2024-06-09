using System;
using System.Reflection;

namespace ReflectionDemo
{
    class Program
    {
        static void Main()
        {
            // How To Invoke a Member using Reflection
            var account = new BankAccount("123-456", "Mahmoud Mattar", 1000m);

            // Get the Withdraw method using reflection
            MethodInfo withdrawMethod = typeof(BankAccount).GetMethod("Withdraw", BindingFlags.Instance | BindingFlags.Public);
            
            // Invoke the Withdraw method on the account object
            withdrawMethod.Invoke(account, new object[] { 2000m });

            // Print the account details
            Console.WriteLine(account);

            // How To Create an Instance using Reflection
            Type t = typeof(BankAccount);

            // Create an instance of BankAccount using reflection
            var account2 = Activator.CreateInstance(t, new object[] { "123-456", "Mahmoud MohaMed", 1000m }) as BankAccount;

            // Print the second account details
            Console.WriteLine(account2);

            Console.ReadKey();
        }

        private static void Account_OnNegativeBalance(object? sender, EventArgs e)
        {
            // Safely cast sender to BankAccount and check for null
            var bankAccount = sender as BankAccount;
            if (bankAccount != null)
            {
                Console.WriteLine($"Account {bankAccount.AccountNo} has a negative balance of {bankAccount.Balance}");
            }
            else
            {
                Console.WriteLine("Error: sender is null or not a BankAccount.");
            }
        }
    }

    public class BankAccount
    {
        private readonly string _accountNo;
        private string _holder;
        private decimal _balance;

        public string AccountNo => _accountNo;
        public string Holder => _holder;
        public decimal Balance => _balance;

        // Initialize the event handler to prevent null reference issues
        public event EventHandler OnNegativeBalance = delegate { };

        public BankAccount(string accountNo, string holder, decimal balance)
        {
            _accountNo = accountNo;
            _holder = holder;
            _balance = balance;
        }

        public void Deposit(decimal amount)
        {
            _balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            Deposit(-amount);
            if (Balance < 0)
            {
                // Use EventArgs.Empty instead of null
                OnNegativeBalance?.Invoke(this, EventArgs.Empty);
            }
        }

        public override string ToString()
        {
            return $"Account No.: {AccountNo},  Holder: {Holder},  Balance: {Balance}";
        }
    }
}
