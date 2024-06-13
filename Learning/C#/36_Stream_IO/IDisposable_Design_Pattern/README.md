# Using the IDisposable Design Pattern in C#

## Overview
The `IDisposable` design pattern is essential for managing resources, such as file handles and database connections, ensuring that they are correctly released when no longer needed. This prevents resource leaks and enhances application performance.

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


## Case 1: Not Recommended Approach

This approach is not recommended because it manually calls `Dispose()`, which can lead to resource leaks if an exception occurs before `Dispose()` is called.

```csharp
var currencyService = new currencyService();
var currencies = currencyService.GetCurrencies();
currencyService.Dispose();
Console.WriteLine(currencies);
```

## Case 2: Recommended Approach with try-finally

This approach uses a try-finally block to ensure `Dispose()` is called even if an exception occurs.

```csharp
currencyService currencyService = null;
try
{
    currencyService = new currencyService();
    var currencies = currencyService.GetCurrencies();
    Console.WriteLine(currencies);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
finally
{
    currencyService?.Dispose();
}
```

## Case 3: More Recommended (using Statement) - Best Practice

Introduced in .NET Framework 2.0+, the `using` statement ensures that `Dispose()` is called automatically at the end of the block.

```csharp
using (var currencyService = new currencyService())
{
    var currencies = currencyService.GetCurrencies();
    Console.WriteLine(currencies);
} // Dispose() will be called automatically
```

## Case 4: Using Statement Without Block Body (C# 8.0)

Introduced in C# 8.0, this approach uses the `using` statement without a block body, simplifying the code.

```csharp
using var currencyService = new currencyService();
var currencies = currencyService.GetCurrencies();
Console.WriteLine(currencies);
```

## Case 5: Using Statement with Multiple Objects

This approach shows how to use the `using` statement with multiple objects, ensuring both are disposed of correctly.

```csharp
using (var currencyService = new currencyService())
using (var currencyService2 = new currencyService())
{
    var currencies = currencyService.GetCurrencies();
    var currencies2 = currencyService2.GetCurrencies();
    Console.WriteLine(currencies);
    Console.WriteLine(currencies2);
} // Dispose() will be called automatically
```

## Implementing the `IDisposable` Interface

Below is the implementation of the `currencyService` class, which includes the `IDisposable` interface and proper disposal pattern.

```csharp
class currencyService : IDisposable
{
    private readonly HttpClient _httpClient;
    private bool _disposed = false;

    public currencyService()
    {
        _httpClient = new HttpClient();
    }

    ~currencyService()
    {
        Dispose(false);
    }

    public string GetCurrencies()
    {
        const string url = "https://www.coinbase.com/api/v2/currencies";
        var response = _httpClient.GetStringAsync(url).Result;
        return response;
    }

    // if disposing is true  => (clean up managed resources and unmanaged resources)
    // if disposing is false => (clean up unmanaged resources and large fields)
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        // Dispose Logic
        if (disposing)
        {
            // dispose managed resources
            _httpClient.Dispose();
        }

        // dispose unmanaged resources
        // sets large fields to null
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);

        // tell the GC not to finalize the object (because we already cleaned up the resources)
        GC.SuppressFinalize(this);
    }
}
```

### Key Points

- **Manual Disposal (Not Recommended)**: Risks resource leaks if exceptions occur.
- **try-finally (Recommended)**: Ensures resources are always released, even if an exception occurs.
- **using Statement (Best Practice)**: Simplifies code and ensures resources are disposed of automatically.
- **Multiple using Statements**: Manages multiple resources effectively.

By following these guidelines, you can manage resources efficiently and prevent potential memory leaks in your applications.
