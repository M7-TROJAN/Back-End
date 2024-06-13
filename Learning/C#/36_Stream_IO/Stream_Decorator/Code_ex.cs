using System;
using System.IO;
using System.IO.Compression;

namespace CAStreamDecorator
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Uncomment to run the specific example
            //Example01();
            Example02();

            Console.ReadKey();
        }

        // Example01: Basic File Operations with BinaryWriter and BinaryReader
        public static void Example01()
        {
            // Create a file and write some data
            using (var stream = File.Create("test.bin"))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(1);
                    writer.Write(2);
                    writer.Write(3);
                }
            }

            // Open the file and read the data
            using (var stream = File.OpenRead("test.bin"))
            {
                using (var reader = new BinaryReader(stream))
                {
                    Console.WriteLine(reader.ReadInt32()); // Output: 1
                    Console.WriteLine(reader.ReadInt32()); // Output: 2
                    Console.WriteLine(reader.ReadInt32()); // Output: 3
                }
            }
        }

        // Example02: Using DeflateStream for Compression and Decompression
        public static void Example02()
        {
            // Create and compress data to a file
            using (var stream = File.Create("test.bin"))
            {
                using (var ds = new DeflateStream(stream, CompressionMode.Compress))
                {
                    ds.WriteByte(65); // A
                    ds.WriteByte(66); // B
                    ds.WriteByte(67); // C
                }
            }

            // Open and decompress data from the file
            using (var stream = File.OpenRead("test.bin"))
            {
                using (var ds = new DeflateStream(stream, CompressionMode.Decompress))
                {
                    Console.WriteLine(ds.ReadByte()); // Output: 65 (A)
                    Console.WriteLine(ds.ReadByte()); // Output: 66 (B)
                    Console.WriteLine(ds.ReadByte()); // Output: 67 (C)
                }
            }
        }
    }
}
