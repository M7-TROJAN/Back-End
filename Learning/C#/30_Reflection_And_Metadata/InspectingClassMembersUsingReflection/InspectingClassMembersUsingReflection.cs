using System;
using System.Reflection;

namespace ReflectionDemo
{
    class Program
    {
        static void Main()
        {
            // Reflecting Members (fields, properties, methods, events) of a type:

            // Get all members (public and non-public, instance only) of the BankAccount class
            MemberInfo[] memberInfos = typeof(BankAccount).GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            Console.WriteLine($"\nMembers of {typeof(BankAccount)}");
            foreach (MemberInfo memberInfo in memberInfos)
            {
                Console.WriteLine(memberInfo.Name);
            }
            Console.WriteLine("\n");
            Console.ReadKey();

            // Get the type information of the BankAccount class
            Type t1 = typeof(BankAccount);

            // Get all fields (non-public, instance only) of the BankAccount class
            FieldInfo[] fieldInfos = t1.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            Console.WriteLine($"\nFields of {typeof(BankAccount)}");
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                Console.WriteLine(fieldInfo.Name);
            }
            Console.WriteLine("\n");
            Console.ReadKey();

            // Get all constructors of the BankAccount class
            ConstructorInfo[] ctors = t1.GetConstructors();
            Console.WriteLine($"\nConstructors of {typeof(BankAccount)}");
            foreach (ConstructorInfo ctor in ctors)
            {
                Console.WriteLine(ctor);
            }
            Console.WriteLine("\n");
            Console.ReadKey();

            // Get a specific constructor of the BankAccount class
            ConstructorInfo constructorInfo = t1.GetConstructor(new Type[] { typeof(string), typeof(string), typeof(decimal) });

            // Get all properties (public, instance only) of the BankAccount class
            PropertyInfo[] propertyInfos = t1.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Console.WriteLine($"\nProperties of {typeof(BankAccount)}");
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                Console.WriteLine(propertyInfo.Name);
            }
            Console.WriteLine("\n");
            Console.ReadKey();

            // Get all methods (public, instance only) of the BankAccount class
            MethodInfo[] methodInfos = t1.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            Console.WriteLine($"\nMethods of {typeof(BankAccount)}");
            foreach (MethodInfo methodInfo in methodInfos)
            {
                Console.WriteLine(methodInfo.Name);
            }
            Console.WriteLine("\n");
            Console.ReadKey();

            // Get all events (public, instance only) of the BankAccount class
            EventInfo[] eventInfos = t1.GetEvents(BindingFlags.Public | BindingFlags.Instance);
            Console.WriteLine($"\nEvents of {typeof(BankAccount)}");
            foreach (EventInfo eventInfo in eventInfos)
            {
                Console.WriteLine(eventInfo.Name);
            }
            Console.WriteLine("\n");
            Console.ReadKey();

            // Get parameters of a specific method
            ParameterInfo[] parameterInfos = methodInfos[5].GetParameters();
            Console.WriteLine($"\nParameters of {methodInfos[5].Name}");
            foreach (ParameterInfo parameterInfo in parameterInfos)
            {
                Console.WriteLine(parameterInfo.Name);
            }
            Console.WriteLine("\n");
            Console.ReadKey();
        }

        private static void Account_OnNegativeBalance(object? sender, EventArgs e)
        {
            var bankAccount = (BankAccount)sender;
            Console.WriteLine($"Account {bankAccount.AccountNo} has a negative balance of {bankAccount.Balance}");
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

        public event EventHandler OnNegativeBalance;

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
            if (this.Balance < 0)
            {
                this.OnNegativeBalance?.Invoke(this, EventArgs.Empty);
            }
        }

        public override string ToString()
        {
            return $"Account No.: {AccountNo},  Holder: {Holder},  Balance: {Balance}";
        }
    }
}
