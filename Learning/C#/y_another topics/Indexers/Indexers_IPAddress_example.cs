
// In this example, we will model an IP Address.
// "When we say 'modeling something,' it means we will represent it as a class."
// "Modeling" refers to representing a concept or entity using a class.

using System;

namespace Indexers
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creating an IP instance with initial values
            IP ip1 = new IP(1, 255, 126, 41);
            Console.WriteLine($"IP Address: {ip1.Address}"); // Output: IP Address: 1.255.126.41

            // Displaying the IP address in decimal format
            Console.WriteLine($"IP Address (Decimal): {ip1.Address}");
            
            // Displaying the IP address in binary format
            Console.WriteLine($"IP Address (Binary): {ip1.AddressBainary}");

            // Displaying the IP address in Hexadecimal format
            Console.WriteLine($"IP Address (Hexadecimal): {ip1.AddressHexadecimal}");

            // Modifying individual segments using the indexer
            ip1[0] = 100;
            ip1[1] = 50;
            ip1[2] = 20;
            ip1[3] = 32;
            Console.WriteLine($"IP Address: {ip1.Address}"); // Output: IP Address: 100.50.20.32

            // Creating an IP instance from a string
            var ip2 = new IP("123.123.123.123");
            Console.WriteLine($"IP Address: {ip2.Address}"); // Output: IP Address: 123.123.123.123

            // Creating an IP instance with an invalid IP string
            var ip3 = new IP("123.123.h23.123");
            Console.WriteLine($"IP Address: {ip3.Address}"); // Output: IP Address: 0.0.0.0

            Console.ReadKey();
        }
    }

    class IP
    {
        private byte[] segments;

        // Constructor for creating an IP instance with individual segments
        public IP(byte segment1, byte segment2, byte segment3, byte segment4)
        {
            segments = new byte[] { segment1, segment2, segment3, segment4 };
        }


        // Constructor for creating an IP instance from a string
        public IP(string ipAddress)
        {
            var segs = ipAddress.Split('.');
        
            if (segs.Length != 4)
            {
                // If the IP address does not have exactly four segments, set all segments to 0
                segments = new byte[] { 0, 0, 0, 0 };
                return;
            }
        
            for (int i = 0; i < segs.Length; i++)
            {
                // Parsing each segment from the string
                if (!byte.TryParse(segs[i], out byte segment) || segment < 1 || segment > 255)
                {
                    // Handling the case of an invalid IP by setting all segments to 0
                    segments = new byte[] { 0, 0, 0, 0 };
                    return;
        
                    // Alternatively, you can throw an exception for an invalid IP
                    // throw new ArgumentException("Invalid IP address format.");
                }
        
                segments[i] = segment;
            }
        }


        // Indexer for accessing and modifying individual segments
        public byte this[byte index]
        {
            get { return segments[index]; }
            set { segments[index] = value; }
        }

        // Property for getting the formatted IP address
        public string Address => string.Join(".", segments);

        public string AddressBainary => string.Join(".", segments.Select(segment => Convert.ToString(segment, 2).PadLeft(8, '0')));

        public string AddressHexadecimal => string.Join(".", segments.Select(s => s.ToString("X").PadLeft(2, '0')));
    }
}




// Indexer Syntax:

/*

 public returnType this[parameterType parameter]
{
    // get accessor
    get
    {
        // return the value based on the provided parameter
    }

    // set accessor
    set
    {
        // set the value based on the provided parameter
    }
}

*/
