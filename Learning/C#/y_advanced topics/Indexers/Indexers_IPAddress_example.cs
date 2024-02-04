
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
        private byte[] segments = new byte[4];

        // Constructor for creating an IP instance with individual segments
        public IP(byte segment1, byte segment2, byte segment3, byte segment4)
        {
            segments[0] = segment1;
            segments[1] = segment2;
            segments[2] = segment3;
            segments[3] = segment4;
        }

        // Constructor for creating an IP instance from a string
        public IP(string ipAddress)
        {
            var segs = ipAddress.Split('.');

            for (int i = 0; i < segs.Length; i++)
            {
                // Parsing each segment from the string
                if (!byte.TryParse(segs[i], out segments[i]))
                {
                    // Handling the case of an invalid IP by setting all segments to 0
                    segments[0] = 0;
                    segments[1] = 0;
                    segments[2] = 0;
                    segments[3] = 0;
                    break;

                    // Alternatively, you can throw an exception for an invalid IP
                }
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
