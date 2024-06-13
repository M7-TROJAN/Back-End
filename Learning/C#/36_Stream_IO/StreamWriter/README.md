The `StreamWriter` class in C# is used to write characters to a stream in a specific encoding. It is commonly used for writing text to files. Below is a detailed explanation of the `StreamWriter` class, including its constructors, methods, properties, and examples.

### `StreamWriter` Class Overview

The `StreamWriter` class is part of the `System.IO` namespace and provides methods for writing to streams, such as files, memory streams, or network streams. It can be used to write text data to a stream in a specific encoding.

### Constructors

The `StreamWriter` class has several constructors that allow you to initialize a new instance in different ways:

1. **StreamWriter(Stream)**
   - Initializes a new instance of the `StreamWriter` class for the specified stream.

   ```csharp
   StreamWriter writer = new StreamWriter(new FileStream("example.txt", FileMode.Create));
   ```

2. **StreamWriter(Stream, Encoding)**
   - Initializes a new instance of the `StreamWriter` class for the specified stream, using the specified encoding.

   ```csharp
   StreamWriter writer = new StreamWriter(new FileStream("example.txt", FileMode.Create), Encoding.UTF8);
   ```

3. **StreamWriter(Stream, Encoding, int)**
   - Initializes a new instance of the `StreamWriter` class for the specified stream, using the specified encoding and buffer size.

   ```csharp
   StreamWriter writer = new StreamWriter(new FileStream("example.txt", FileMode.Create), Encoding.UTF8, 1024);
   ```

4. **StreamWriter(Stream, Encoding, int, bool)**
   - Initializes a new instance of the `StreamWriter` class for the specified stream, using the specified encoding, buffer size, and a value that determines whether to leave the stream open after the `StreamWriter` object is disposed.

   ```csharp
   StreamWriter writer = new StreamWriter(new FileStream("example.txt", FileMode.Create), Encoding.UTF8, 1024, true);
   ```

5. **StreamWriter(string)**
   - Initializes a new instance of the `StreamWriter` class for the specified file on the specified path.

   ```csharp
   StreamWriter writer = new StreamWriter("example.txt");
   ```

6. **StreamWriter(string, bool)**
   - Initializes a new instance of the `StreamWriter` class for the specified file on the specified path, with a Boolean to specify whether to append to the file if it exists.

   ```csharp
   StreamWriter writer = new StreamWriter("example.txt", true);
   ```

7. **StreamWriter(string, bool, Encoding)**
   - Initializes a new instance of the `StreamWriter` class for the specified file on the specified path, with a Boolean to specify whether to append to the file if it exists, and with the specified encoding.

   ```csharp
   StreamWriter writer = new StreamWriter("example.txt", true, Encoding.UTF8);
   ```

8. **StreamWriter(string, bool, Encoding, int)**
   - Initializes a new instance of the `StreamWriter` class for the specified file on the specified path, with a Boolean to specify whether to append to the file if it exists, with the specified encoding and buffer size.

   ```csharp
   StreamWriter writer = new StreamWriter("example.txt", true, Encoding.UTF8, 1024);
   ```

### Properties

- **AutoFlush**: Gets or sets a value indicating whether the `StreamWriter` will flush its buffer to the underlying stream after every call to `Write`.
  ```csharp
  writer.AutoFlush = true;
  ```

- **BaseStream**: Gets the underlying stream that interfaces with a backing store.
  ```csharp
  Stream baseStream = writer.BaseStream;
  ```

- **Encoding**: Gets the encoding in which the output is written.
  ```csharp
  Encoding encoding = writer.Encoding;
  ```

### Methods

- **Close()**: Closes the current `StreamWriter` object and the underlying stream.
  ```csharp
  writer.Close();
  ```

- **Flush()**: Clears all buffers for the current writer and causes any buffered data to be written to the underlying device.
  ```csharp
  writer.Flush();
  ```

- **Write()**: Writes a character or string to the stream.
  ```csharp
  writer.Write("Hello, World!");
  ```

- **WriteLine()**: Writes a character or string followed by a line terminator to the stream.
  ```csharp
  writer.WriteLine("Hello, World!");
  ```

- **Dispose()**: Releases all resources used by the `StreamWriter`.
  ```csharp
  writer.Dispose();
  ```

### Examples

#### Example 1: Writing to a File

```csharp
using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        string filePath = "example.txt";

        // Create a StreamWriter instance
        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            // Write data to the file
            writer.WriteLine("Hello, FileStream!");
            writer.Write("This is an example ");
            writer.WriteLine("of StreamWriter.");
        }

        // Read and display the file content
        using (StreamReader reader = new StreamReader(filePath))
        {
            string content = reader.ReadToEnd();
            Console.WriteLine("File Content:");
            Console.WriteLine(content);
        }
    }
}
```

### Explanation

1. **Creating the `StreamWriter`**:
   - A `StreamWriter` instance is created to write to `example.txt`.
   - The `StreamWriter` is set to use UTF-8 encoding.

2. **Writing to the File**:
   - `WriteLine` writes a string followed by a line terminator to the file.
   - `Write` writes a string without a line terminator.
   - The `using` statement ensures the `StreamWriter` is properly disposed of, which also flushes any buffered data to the file.

3. **Reading the File Content**:
   - A `StreamReader` is used to read the content of the file and display it in the console.

#### Example 2: Writing to a MemoryStream

```csharp
using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        using (MemoryStream memoryStream = new MemoryStream())
        using (StreamWriter writer = new StreamWriter(memoryStream))
        {
            writer.WriteLine("Hello, MemoryStream!");
            writer.WriteLine("This is an example of using StreamWriter with MemoryStream.");
            writer.Flush();

            // Read the data back from the MemoryStream
            memoryStream.Position = 0;
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                string content = reader.ReadToEnd();
                Console.WriteLine("MemoryStream Content:");
                Console.WriteLine(content);
            }
        }
    }
}
```

### Explanation

1. **Creating the `StreamWriter`**:
   - A `StreamWriter` instance is created to write to a `MemoryStream`.

2. **Writing to the MemoryStream**:
   - `WriteLine` methods are used to write strings to the `MemoryStream`.
   - `Flush` is called to ensure all buffered data is written to the `MemoryStream`.

3. **Reading from the MemoryStream**:
   - The `Position` of the `MemoryStream` is set to 0 to read from the beginning.
   - A `StreamReader` is used to read the content of the `MemoryStream` and display it in the console.