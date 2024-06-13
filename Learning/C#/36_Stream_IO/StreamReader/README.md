The `StreamReader` class in C# is used for reading characters from a stream in a specific encoding. It is commonly used for reading text from files. Below is a detailed explanation of the `StreamReader` class, including its constructors, methods, properties, and examples.

### `StreamReader` Class Overview

The `StreamReader` class is part of the `System.IO` namespace and provides methods for reading characters from streams, such as files, memory streams, or network streams. It can be used to read text data from a stream in a specific encoding.

### Constructors

The `StreamReader` class has several constructors that allow you to initialize a new instance in different ways:

1. **StreamReader(Stream)**
   - Initializes a new instance of the `StreamReader` class for the specified stream.

   ```csharp
   StreamReader reader = new StreamReader(new FileStream("example.txt", FileMode.Open));
   ```

2. **StreamReader(Stream, Encoding)**
   - Initializes a new instance of the `StreamReader` class for the specified stream, using the specified encoding.

   ```csharp
   StreamReader reader = new StreamReader(new FileStream("example.txt", FileMode.Open), Encoding.UTF8);
   ```

3. **StreamReader(Stream, Encoding, bool)**
   - Initializes a new instance of the `StreamReader` class for the specified stream, using the specified encoding and a Boolean value indicating whether to detect the encoding from the byte order marks.

   ```csharp
   StreamReader reader = new StreamReader(new FileStream("example.txt", FileMode.Open), Encoding.UTF8, true);
   ```

4. **StreamReader(Stream, Encoding, bool, int)**
   - Initializes a new instance of the `StreamReader` class for the specified stream, using the specified encoding, a Boolean value indicating whether to detect the encoding from the byte order marks, and the specified buffer size.

   ```csharp
   StreamReader reader = new StreamReader(new FileStream("example.txt", FileMode.Open), Encoding.UTF8, true, 1024);
   ```

5. **StreamReader(string)**
   - Initializes a new instance of the `StreamReader` class for the specified file name.

   ```csharp
   StreamReader reader = new StreamReader("example.txt");
   ```

6. **StreamReader(string, bool)**
   - Initializes a new instance of the `StreamReader` class for the specified file name, with a Boolean value indicating whether to detect the encoding from the byte order marks.

   ```csharp
   StreamReader reader = new StreamReader("example.txt", true);
   ```

7. **StreamReader(string, Encoding)**
   - Initializes a new instance of the `StreamReader` class for the specified file name, using the specified encoding.

   ```csharp
   StreamReader reader = new StreamReader("example.txt", Encoding.UTF8);
   ```

8. **StreamReader(string, Encoding, bool)**
   - Initializes a new instance of the `StreamReader` class for the specified file name, using the specified encoding and a Boolean value indicating whether to detect the encoding from the byte order marks.

   ```csharp
   StreamReader reader = new StreamReader("example.txt", Encoding.UTF8, true);
   ```

9. **StreamReader(string, Encoding, bool, int)**
   - Initializes a new instance of the `StreamReader` class for the specified file name, using the specified encoding, a Boolean value indicating whether to detect the encoding from the byte order marks, and the specified buffer size.

   ```csharp
   StreamReader reader = new StreamReader("example.txt", Encoding.UTF8, true, 1024);
   ```

### Properties

- **BaseStream**: Gets the underlying stream that interfaces with a backing store.
  ```csharp
  Stream baseStream = reader.BaseStream;
  ```

- **CurrentEncoding**: Gets the current character encoding that the current `StreamReader` object is using.
  ```csharp
  Encoding encoding = reader.CurrentEncoding;
  ```

- **EndOfStream**: Gets a value indicating whether the current stream position is at the end of the stream.
  ```csharp
  bool endOfStream = reader.EndOfStream;
  ```

### Methods

- **Close()**: Closes the `StreamReader` object and the underlying stream.
  ```csharp
  reader.Close();
  ```

- **Dispose()**: Releases all resources used by the `StreamReader`.
  ```csharp
  reader.Dispose();
  ```

- **Peek()**: Returns the next available character but does not consume it.
  ```csharp
  int nextChar = reader.Peek();
  ```

- **Read()**: Reads the next character from the input stream and advances the character position by one character.
  ```csharp
  int nextChar = reader.Read();
  ```

- **Read(char[], int, int)**: Reads a specified maximum number of characters from the current stream into a buffer, beginning at the specified index.
  ```csharp
  char[] buffer = new char[1024];
  int bytesRead = reader.Read(buffer, 0, buffer.Length);
  ```

- **ReadBlock(char[], int, int)**: Reads a specified maximum number of characters from the current stream into a buffer, beginning at the specified index.
  ```csharp
  int bytesRead = reader.ReadBlock(buffer, 0, buffer.Length);
  ```

- **ReadLine()**: Reads a line of characters from the current stream and returns the data as a string.
  ```csharp
  string line = reader.ReadLine();
  ```

- **ReadToEnd()**: Reads all characters from the current position to the end of the stream.
  ```csharp
  string content = reader.ReadToEnd();
  ```

### Examples

#### Example 1: Reading from a File

```csharp
using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        string filePath = "example.txt";

        // Write data to the file
        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            writer.WriteLine("Hello, StreamReader!");
            writer.WriteLine("This is an example of using StreamReader.");
        }

        // Read data from the file
        using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
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
   - `WriteLine` writes strings followed by line terminators to the file.

3. **Reading from the File**:
   - A `StreamReader` instance is created to read from `example.txt`.
   - The `ReadToEnd` method reads all characters from the current position to the end of the stream.
   - The content is displayed in the console.

#### Example 2: Reading from a MemoryStream

```csharp
using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        byte[] byteArray = Encoding.UTF8.GetBytes("Hello, MemoryStream!\nThis is an example of using StreamReader with MemoryStream.");
        using (MemoryStream memoryStream = new MemoryStream(byteArray))
        using (StreamReader reader = new StreamReader(memoryStream, Encoding.UTF8))
        {
            string content = reader.ReadToEnd();
            Console.WriteLine("MemoryStream Content:");
            Console.WriteLine(content);
        }
    }
}
```

### Explanation

1. **Creating the `StreamReader`**:
   - A `StreamReader` instance is created to read from a `MemoryStream`.

2. **Reading from the MemoryStream**:
   - The `ReadToEnd` method reads all characters from the current position to the end of the stream.
   - The content is displayed in the console.