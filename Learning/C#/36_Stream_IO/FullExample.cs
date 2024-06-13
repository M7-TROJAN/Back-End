using System;
using System.IO;

namespace CAFileStream
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Uncomment the examples to run them one by one
            // Example01();
            // Example02();
            // Example03();
            // Example04();
            // Example05();
            // Example06();
            Example07();
            Console.ReadKey();
        }

        // Example 1: Using FileStream to Write Bytes to a File
        static void Example01()
        {
            string path = @"D:\c#\36_Stream_IO\FileStream\sample.txt";
            using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                Console.WriteLine($"Length: {fs.Length} Byte(s)");
                Console.WriteLine("CanRead: " + fs.CanRead);
                Console.WriteLine("CanWrite: " + fs.CanWrite);
                Console.WriteLine("CanSeek: " + fs.CanSeek);
                Console.WriteLine("CanTimeout: " + fs.CanTimeout);
                Console.WriteLine("Name: " + fs.Name);
                Console.WriteLine("Position: " + fs.Position);

                // Writing bytes to the file
                fs.WriteByte((byte)'A');
                fs.WriteByte((byte)'B');
                Console.WriteLine("Position: " + fs.Position);

                fs.WriteByte(13); // Enter key
                Console.WriteLine("Position: " + fs.Position);

                // Writing a range of bytes to the file
                for (byte i = 65; i < 123; i++)
                {
                    fs.WriteByte(i);
                }
            }
        }

        // Example 2: Using FileStream to Read Bytes from a File
        static void Example02()
        {
            string path = @"D:\c#\36_Stream_IO\FileStream\sample.txt";
            using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                byte[] bytes = new byte[fs.Length];

                // Reading bytes from the file
                fs.Read(bytes, 0, bytes.Length);

                // Displaying the bytes as characters
                foreach (var item in bytes)
                {
                    Console.Write((char)item);
                }
            }
        }

        // Example 3: Using StreamWriter to Write Text to a File
        static void Example03()
        {
            string path = @"D:\c#\36_Stream_IO\FileStream\sample2.txt";
            using (var sw = new StreamWriter(path))
            {
                // Writing lines of text to the file
                sw.WriteLine("Hello World");
                sw.WriteLine("This");
                sw.WriteLine("is");
                sw.WriteLine("a");
                sw.WriteLine("test");
            }
        }

        // Example 4: Using StreamReader to Read Text from a File
        static void Example04()
        {
            string path = @"D:\c#\36_Stream_IO\FileStream\sample2.txt";
            using (var sr = new StreamReader(path))
            {
                string line;
                // Reading the file line by line
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }

            Console.WriteLine("\n\n");

            // Reading the entire file at once
            using (var sr = new StreamReader(path))
            {
                string text = sr.ReadToEnd();
                Console.WriteLine(text);
            }

            Console.WriteLine("\n\n");

            // Reading the file character by character
            using (var sr = new StreamReader(path))
            {
                while (sr.Peek() > 0)
                {
                    Console.Write((char)sr.Read());
                }
            }
        }

        // Example 5: Using the File Class to Write and Read a Single Line of Text
        static void Example05()
        {
            string path = @"D:\c#\36_Stream_IO\FileStream\sample4.txt";
            File.WriteAllText(path, "Hello World");
            Console.WriteLine(File.ReadAllText(path));
        }

        // Example 6: Using the File Class to Write and Read Multiple Lines of Text
        static void Example06()
        {
            string path = @"D:\c#\36_Stream_IO\FileStream\sample4.txt";

            var lines = new string[]
            {
                "\n",
                "Hello World",
                "This",
                "is",
                "a",
                "new",
                "test"
            };

            // Writing multiple lines to the file
            File.WriteAllLines(path, lines);

            // Reading all lines from the file
            var data = File.ReadAllLines(path);
            foreach (var item in data)
            {
                Console.WriteLine(item);
            }
        }

        // Example 7: Using the File Class to Append Text to a File
        static void Example07()
        {
            string path = @"D:\c#\36_Stream_IO\FileStream\sample4.txt";

            var text = "C# is an Amazing Language";

            // Appending text to the file
            File.AppendAllText(path, text);

            // Reading the entire file content
            Console.WriteLine(File.ReadAllText(path));
        }
    }
}
