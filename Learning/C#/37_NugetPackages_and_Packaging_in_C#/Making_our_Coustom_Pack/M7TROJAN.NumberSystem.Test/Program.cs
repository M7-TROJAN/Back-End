using System;
using M7TROJAN.NumberSystem;
using M7TROJAN.NumberSystem.Modles;
namespace M7TROJAN.NumberSystem.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---- From Decimal ----");

            DecimalSystem d = new DecimalSystem("10");
            var octal = d.To(EnNumberBase.OCTAL);
            var binary = d.To(EnNumberBase.BINARY);
            var hex = d.To(EnNumberBase.HEXADECIMAL);

            Console.WriteLine($"({d.Value}){(int)EnNumberBase.DECIMAL} = ({binary}){(int)EnNumberBase.BINARY}");
            Console.WriteLine($"({d.Value}){(int)EnNumberBase.DECIMAL} = ({octal}){(int)EnNumberBase.OCTAL}");
            Console.WriteLine($"({d.Value}){(int)EnNumberBase.DECIMAL} = ({hex}){(int)EnNumberBase.HEXADECIMAL}");
            Console.WriteLine();

            Console.WriteLine("---- From Binary ----");

            BinarySystem b = new BinarySystem("1010");
            octal = b.To(EnNumberBase.OCTAL);
            var dec = b.To(EnNumberBase.DECIMAL);
            hex = b.To(EnNumberBase.HEXADECIMAL);
            
            Console.WriteLine($"({b.Value}){(int)EnNumberBase.BINARY} = ({dec}){(int)EnNumberBase.DECIMAL}");
            Console.WriteLine($"({b.Value}){(int)EnNumberBase.BINARY} = ({octal}){(int)EnNumberBase.OCTAL}");
            Console.WriteLine($"({b.Value}){(int)EnNumberBase.BINARY} = ({hex}){(int)EnNumberBase.HEXADECIMAL}");
            Console.WriteLine();

            Console.WriteLine("---- From Octal ----");

            OctalSystem o = new OctalSystem("12");
            dec = o.To(EnNumberBase.DECIMAL);
            binary = o.To(EnNumberBase.BINARY);
            hex = o.To(EnNumberBase.HEXADECIMAL);

            Console.WriteLine($"({o.Value}){(int)EnNumberBase.OCTAL} = ({dec}){(int)EnNumberBase.DECIMAL}");
            Console.WriteLine($"({o.Value}){(int)EnNumberBase.OCTAL} = ({binary}){(int)EnNumberBase.BINARY}");
            Console.WriteLine($"({o.Value}){(int)EnNumberBase.OCTAL} = ({hex}){(int)EnNumberBase.HEXADECIMAL}");
            Console.WriteLine();

            Console.WriteLine("---- From Hexadecimal ----");
            
            HexadecimalSystem h = new HexadecimalSystem("A");
            dec = h.To(EnNumberBase.DECIMAL);
            binary = h.To(EnNumberBase.BINARY);
            octal = h.To(EnNumberBase.OCTAL);

            Console.WriteLine($"({h.Value}){(int)EnNumberBase.HEXADECIMAL} = ({dec}){(int)EnNumberBase.DECIMAL}");
            Console.WriteLine($"({h.Value}){(int)EnNumberBase.HEXADECIMAL} = ({binary}){(int)EnNumberBase.BINARY}");
            Console.WriteLine($"({h.Value}){(int)EnNumberBase.HEXADECIMAL} = ({octal}){(int)EnNumberBase.OCTAL}");
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}