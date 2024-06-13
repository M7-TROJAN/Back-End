### Stream Decorator Pattern in C#

The Decorator pattern is a structural design pattern that allows behavior to be added to an individual object, either statically or dynamically, without affecting the behavior of other objects from the same class. This pattern is useful for adhering to the Open/Closed Principle (OCP) in object-oriented design.

In the context of streams in C#, the decorator pattern is used to add responsibilities to the stream object. For example, `DeflateStream` and `GZipStream` are decorators that compress data written to a stream and decompress data read from a stream.

### Example: Using DeflateStream

Below is an example demonstrating how to use `DeflateStream` to compress and decompress data in C#.

```csharp
using System;
using System.IO;
using System.IO.Compression;

namespace StreamDecoratorExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string filePath = @"D:\c#\StreamDecorator\sample.txt";
            string compressedFilePath = @"D:\c#\StreamDecorator\sample.txt.deflate";

            // Write some data to a file
            WriteDataToFile(filePath, "Hello, this is a test of the DeflateStream decorator pattern in C#!");

            // Compress the file
            CompressFile(filePath, compressedFilePath);

            // Decompress the file
            string decompressedData = DecompressFile(compressedFilePath);

            Console.WriteLine("Decompressed Data: " + decompressedData);

            Console.ReadKey();
        }

        // Write some data to a file
        static void WriteDataToFile(string filePath, string data)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.Write(data);
            }
        }

        // Compress the file
        static void CompressFile(string inputFile, string outputFile)
        {
            using (var inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            using (var outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            using (var compressionStream = new DeflateStream(outputFileStream, CompressionMode.Compress))
            {
                inputFileStream.CopyTo(compressionStream);
            }

            Console.WriteLine("File compressed to: " + outputFile);
        }

        // Decompress the file
        static string DecompressFile(string compressedFile)
        {
            using (var inputFileStream = new FileStream(compressedFile, FileMode.Open, FileAccess.Read))
            using (var decompressionStream = new DeflateStream(inputFileStream, CompressionMode.Decompress))
            using (var outputStream = new MemoryStream())
            {
                decompressionStream.CopyTo(outputStream);
                outputStream.Position = 0;

                using (var reader = new StreamReader(outputStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
```

### Explanation

#### Writing Data to a File
The `WriteDataToFile` method writes the provided string data to a file using a `StreamWriter`.

```csharp
static void WriteDataToFile(string filePath, string data)
{
    using (var writer = new StreamWriter(filePath))
    {
        writer.Write(data);
    }
}
```

#### Compressing the File
The `CompressFile` method reads data from the input file, compresses it using `DeflateStream`, and writes the compressed data to the output file.

```csharp
static void CompressFile(string inputFile, string outputFile)
{
    using (var inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
    using (var outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
    using (var compressionStream = new DeflateStream(outputFileStream, CompressionMode.Compress))
    {
        inputFileStream.CopyTo(compressionStream);
    }

    Console.WriteLine("File compressed to: " + outputFile);
}
```

#### Decompressing the File
The `DecompressFile` method reads the compressed data from the input file, decompresses it using `DeflateStream`, and returns the decompressed string data.

```csharp
static string DecompressFile(string compressedFile)
{
    using (var inputFileStream = new FileStream(compressedFile, FileMode.Open, FileAccess.Read))
    using (var decompressionStream = new DeflateStream(inputFileStream, CompressionMode.Decompress))
    using (var outputStream = new MemoryStream())
    {
        decompressionStream.CopyTo(outputStream);
        outputStream.Position = 0;

        using (var reader = new StreamReader(outputStream))
        {
            return reader.ReadToEnd();
        }
    }
}
```

### Summary
The decorator pattern in C# is used to extend the functionality of streams in a flexible manner. In this example, `DeflateStream` is used to compress and decompress data, showcasing how additional responsibilities (compression) can be added to a `FileStream` without modifying its structure. This approach adheres to the Open/Closed Principle by allowing new functionalities to be added through composition rather than inheritance.