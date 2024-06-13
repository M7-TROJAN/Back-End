## Stream and I/O

### Stream Architecture

Streams are used to read and write data in .NET applications. They provide a way to handle input and output (I/O) operations. The .NET framework includes several types of streams, each with its own use case.

#### Key Concepts:

- **Stream**: An abstract base class for reading and writing bytes.
- **Input Streams**: Used for reading data (e.g., FileStream, MemoryStream).
- **Output Streams**: Used for writing data (e.g., FileStream, MemoryStream).
- **Buffering**: Improves performance by reducing the number of read/write operations.
- **Synchronous vs Asynchronous**: Synchronous operations block the thread until the operation completes, while asynchronous operations do not.

### Managed / Unmanaged Code

#### Managed Code:

- **Managed Code**: Runs under the control of the .NET runtime (CLR - Common Language Runtime). The CLR provides services like garbage collection, type safety, exception handling, etc.
- **Example**:
  ```csharp
  // Managed code example
  public class ManagedExample
  {
      public void DoWork()
      {
          Console.WriteLine("This is managed code.");
      }
  }
  ```

#### Unmanaged Code:

- **Unmanaged Code**: Executes directly by the OS. It is written in languages like C or C++ and requires manual memory management.
- **Example**:
  ```csharp
  // Unmanaged code example using PInvoke
  using System;
  using System.Runtime.InteropServices;

  public class UnmanagedExample
  {
      [DllImport("user32.dll")]
      public static extern int MessageBox(IntPtr hWnd, string text, string caption, int options);

      public void ShowMessage()
      {
          MessageBox(IntPtr.Zero, "Hello, World!", "Unmanaged Code", 0);
      }
  }
  ```

### FileStream (Back Store)

**FileStream** is used for reading from and writing to files. It provides a stream for file operations.

#### Example:

```csharp
using System;
using System.IO;

public class FileStreamExample
{
    public void WriteToFile(string filePath, string content)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            byte[] info = new System.Text.UTF8Encoding(true).GetBytes(content);
            fs.Write(info, 0, info.Length);
        }
    }

    public void ReadFromFile(string filePath)
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            byte[] b = new byte[1024];
            UTF8Encoding temp = new UTF8Encoding(true);

            while (fs.Read(b, 0, b.Length) > 0)
            {
                Console.WriteLine(temp.GetString(b));
            }
        }
    }
}
```

### Stream Reader/Writer (Adapter)

**StreamReader** and **StreamWriter** are used for reading and writing characters to and from a stream, respectively.

#### Example:

```csharp
using System;
using System.IO;

public class StreamReaderWriterExample
{
    public void WriteToFile(string filePath, string content)
    {
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine(content);
        }
    }

    public void ReadFromFile(string filePath)
    {
        using (StreamReader sr = new StreamReader(filePath))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
    }
}
```

### DeflateStream (Decorator)

**DeflateStream** is used for compressing and decompressing streams. It uses the Deflate algorithm to compress data.

#### Example:

```csharp
using System;
using System.IO;
using System.IO.Compression;

public class DeflateStreamExample
{
    public void CompressFile(string inputFile, string outputFile)
    {
        using (FileStream originalFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
        {
            using (FileStream compressedFileStream = new FileStream(outputFile, FileMode.Create))
            {
                using (DeflateStream compressionStream = new DeflateStream(compressedFileStream, CompressionMode.Compress))
                {
                    originalFileStream.CopyTo(compressionStream);
                }
            }
        }
    }

    public void DecompressFile(string inputFile, string outputFile)
    {
        using (FileStream compressedFileStream = new FileStream(inputFile, FileMode.Open))
        {
            using (FileStream decompressedFileStream = new FileStream(outputFile, FileMode.Create))
            {
                using (DeflateStream decompressionStream = new DeflateStream(compressedFileStream, CompressionMode.Decompress))
                {
                    decompressionStream.CopyTo(decompressedFileStream);
                }
            }
        }
    }
}
```

### File Class

The **File** class provides static methods for creating, copying, deleting, moving, and opening files. It also helps in reading from and writing to a file.

#### Example:

```csharp
using System;
using System.IO;

public class FileClassExample
{
    public void CreateAndWriteFile(string filePath, string content)
    {
        File.WriteAllText(filePath, content);
    }

    public void ReadFile(string filePath)
    {
        string content = File.ReadAllText(filePath);
        Console.WriteLine(content);
    }

    public void CopyFile(string sourceFilePath, string destFilePath)
    {
        File.Copy(sourceFilePath, destFilePath, true);
    }

    public void MoveFile(string sourceFilePath, string destFilePath)
    {
        File.Move(sourceFilePath, destFilePath);
    }

    public void DeleteFile(string filePath)
    {
        File.Delete(filePath);
    }
}
```

### Summary

- **Stream Architecture**: The foundation for reading and writing data in .NET.
- **Managed/Unmanaged Code**: Managed code runs under the CLR, whereas unmanaged code runs directly by the OS.
- **FileStream**: Provides file-based stream operations.
- **StreamReader/Writer**: Facilitates reading and writing characters.
- **DeflateStream**: Enables compression and decompression.
- **File Class**: Offers static methods for common file operations.