
# Managing Unmanaged Resources with `IDisposable` in C#

This document provides an example of managing unmanaged resources, such as file handles, using the `IDisposable` interface. We'll demonstrate the use of the `Dispose` method to ensure proper cleanup of these resources.

## Example: File Handling with `IDisposable`

We'll create a `FileManager` class that implements `IDisposable` and uses a `FileStream` to write data to a file. The `Dispose` method will ensure the file is properly closed when we're done with it.

```csharp
using System;
using System.Collections.Generic;
using System.IO;

namespace Learning
{
    class Program
    {
        static void Main()
        {
            using (var fileManager = new FileManager("example.txt"))
            {
                fileManager.WriteData("Hello, world!");
                fileManager.WriteData("Writing some data to the file.");
            } // Dispose is automatically called here
            
            var fileManager2 = new FileManager("example2.txt");
            fileManager2.WriteData("Hello, world!");
            fileManager2.WriteData("Writing some data to the file.");
            fileManager2.Dispose(); // Dispose is called manually
        }
    }

    public class FileManager : IDisposable
    {
        private FileStream fileStream;
        private StreamWriter writer;
        private bool disposed = false;

        public FileManager(string filePath)
        {
            fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            writer = new StreamWriter(fileStream);
        }

        public void WriteData(string data)
        {
            if (disposed)
                throw new ObjectDisposedException("FileManager");

            writer.WriteLine(data);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    writer?.Dispose();
                    fileStream?.Dispose();
                }

                disposed = true;
            }
        }

        ~FileManager()
        {
            Dispose(disposing: false);
        }
    }
}
```

### Explanation

**`FileManager` Class**: Manages a `FileStream` and `StreamWriter` to write data to a file.
- Implements `IDisposable` to ensure the file is properly closed.
- `Dispose` method closes the `StreamWriter` and `FileStream`.
- `Dispose(bool disposing)` handles both managed and unmanaged resources.
- Destructor (`~FileManager`) ensures resources are freed if `Dispose` is not called.

### Usage of the `Dispose` Method

In the example above:
- The `using` statement ensures that `Dispose` is called automatically when the block is exited, even if an exception occurs.
- If not using the `using` statement, you should call `Dispose` manually to ensure resources are freed.

By implementing `IDisposable` and using the `Dispose` method, we ensure that unmanaged resources are properly cleaned up, preventing resource leaks and other potential issues in our applications.

