using System;

namespace OperatorOverloading
{
    class Program
    {
        static void Main(string[] args)
        {
            var m1 = new Money(10);
            var m2 = new Money(20);
            var m3 = m1 + m2;
            var m4 = m1 + m2 + m3;
            var m5 = m4 - m2 + m3;

            Console.WriteLine($"M1: {m1.Amount}");
            Console.WriteLine($"M2: {m2.Amount}");
            Console.WriteLine($"M3: {m3.Amount}");
            Console.WriteLine($"M4: {m4.Amount}");
            Console.WriteLine($"M5: {m5.Amount}");

            Console.WriteLine(m1.GetHashCode());

        }
    }

    class Money
    {
        private decimal _amount;

        public decimal Amount => _amount;
        public Money(decimal amount)
        {
            _amount = amount;
        }

        public static Money operator +(Money money1, Money money2) 
        {
            var result = money1.Amount + money2.Amount;

            return new Money(result);
        }

        public static Money operator -(Money money1, Money money2)
        {
            var result = money1.Amount - money2.Amount;

            return new Money(result);
        }


        public static Money operator *(Money money1, Money money2)
        {
            var result = money1.Amount * money2.Amount;

            return new Money(result);
        }

        public static Money operator /(Money money1, Money money2)
        {
            var result = money1.Amount / money2.Amount;

            return new Money(result);
        }

        public static Money operator ++(Money money)
        {
            var result = money.Amount + 1;

            return new Money(result);
        }
        public static Money operator --(Money money)
        {
            var result = money.Amount - 1;

            return new Money(result);
        }

        public static bool operator <(Money money1, Money money2)
        {
            return money1.Amount < money2.Amount;
        }

        public static bool operator >(Money money1, Money money2)
        {
            return money1.Amount > money2.Amount;
        }
        public static bool operator <=(Money money1, Money money2)
        {
            return money1.Amount <= money2.Amount;
        }

        public static bool operator >=(Money money1, Money money2)
        {
            return money1.Amount >= money2.Amount;
        }

        public static bool operator ==(Money money1, Money money2)
        {
            return money1.Amount == money2.Amount;
        }

        public static bool operator !=(Money money1, Money money2)
        {
            // return money1.Amount != money2.Amount;
            // or reuse the == operator This helps to avoid redundancy
            return !(money1 == money2);
        }


        // when We override the == and != operators for a custom type like Money,
        // it's generally recommended to also override the Equals method and GetHashCode method
        // to ensure consistency in equality comparisons.

        public override bool Equals(object? obj)
        {
            if ( (obj == null) || (this.GetType() != obj.GetType()) )
            {
                return false;
            }

            Money other = (Money)obj;
            return this.Amount == other.Amount;
        }

        public override int GetHashCode()
        {
            return Amount.GetHashCode();
        }

    }
}
