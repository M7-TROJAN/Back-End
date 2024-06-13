### DeflateStream Class in C#

The `DeflateStream` class in C# is part of the `System.IO.Compression` namespace and is used for compressing and decompressing streams using the Deflate algorithm. This class is particularly useful for reducing the size of data for storage or transmission.

#### Key Points:
- The `DeflateStream` class provides methods to compress and decompress streams.
- It can be used with any stream, including `FileStream`, `MemoryStream`, etc.
- Compression can be achieved using `CompressionMode.Compress`.
- Decompression can be achieved using `CompressionMode.Decompress`.

### Constructors
The `DeflateStream` class has the following constructors:

1. **DeflateStream(Stream, CompressionMode)**
   - Initializes a new instance of the `DeflateStream` class using the specified stream and compression mode.
   
2. **DeflateStream(Stream, CompressionMode, Boolean)**
   - Initializes a new instance of the `DeflateStream` class using the specified stream and compression mode, and optionally leaves the stream open.
   
3. **DeflateStream(Stream, CompressionLevel)**
   - Initializes a new instance of the `DeflateStream` class using the specified stream and compression level.
   
4. **DeflateStream(Stream, CompressionLevel, Boolean)**
   - Initializes a new instance of the `DeflateStream` class using the specified stream, compression level, and optionally leaves the stream open.

### Properties
- **BaseStream**: Gets the underlying stream.
- **CanRead**: Gets a value indicating whether the stream supports reading.
- **CanSeek**: Gets a value indicating whether the stream supports seeking (always `false`).
- **CanWrite**: Gets a value indicating whether the stream supports writing.

### Methods
- **Flush()**: Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
- **Read(byte[], int, int)**: Reads a number of decompressed bytes into the specified byte array.
- **Write(byte[], int, int)**: Writes a number of compressed bytes to the stream.

### Example Usage

#### Compressing Data
```csharp
using System;
using System.IO;
using System.IO.Compression;

namespace DeflateStreamExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string originalFile = @"D:\example\original.txt";
            string compressedFile = @"D:\example\compressed.deflate";

            // Create some data to compress
            File.WriteAllText(originalFile, "This is some data to compress using DeflateStream in C#.");

            // Compress the data
            CompressFile(originalFile, compressedFile);
            Console.WriteLine("Data compressed successfully.");
        }

        static void CompressFile(string inputFile, string outputFile)
        {
            using (FileStream originalFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            using (FileStream compressedFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            using (DeflateStream compressionStream = new DeflateStream(compressedFileStream, CompressionMode.Compress))
            {
                originalFileStream.CopyTo(compressionStream);
            }
        }
    }
}
```

#### Decompressing Data
```csharp
using System;
using System.IO;
using System.IO.Compression;

namespace DeflateStreamExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string compressedFile = @"D:\example\compressed.deflate";
            string decompressedFile = @"D:\example\decompressed.txt";

            // Decompress the data
            DecompressFile(compressedFile, decompressedFile);
            Console.WriteLine("Data decompressed successfully.");

            // Read and display the decompressed data
            string decompressedData = File.ReadAllText(decompressedFile);
            Console.WriteLine("Decompressed Data: " + decompressedData);
        }

        static void DecompressFile(string inputFile, string outputFile)
        {
            using (FileStream compressedFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            using (FileStream decompressedFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            using (DeflateStream decompressionStream = new DeflateStream(compressedFileStream, CompressionMode.Decompress))
            {
                decompressionStream.CopyTo(decompressedFileStream);
            }
        }
    }
}
```

### Explanation

1. **Compressing Data**:
   - We create a file `original.txt` and write some data to it.
   - The `CompressFile` method reads the data from `original.txt` and compresses it into `compressed.deflate` using `DeflateStream`.

2. **Decompressing Data**:
   - The `DecompressFile` method reads the compressed data from `compressed.deflate` and decompresses it into `decompressed.txt` using `DeflateStream`.
   - The decompressed data is then read from `decompressed.txt` and displayed.

### Summary
The `DeflateStream` class in C# provides an easy and efficient way to compress and decompress data streams using the Deflate algorithm. It is part of the `System.IO.Compression` namespace and can be used with various types of streams, such as `FileStream` and `MemoryStream`, to reduce the size of data for storage or transmission. The above examples demonstrate how to use `DeflateStream` to compress and decompress files.