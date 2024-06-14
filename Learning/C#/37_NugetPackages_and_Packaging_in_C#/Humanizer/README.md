Humanizer is a popular .NET library that provides a variety of functionalities to make working with strings, numbers, dates, times, and enums more human-readable and user-friendly. It helps in converting data into a format that is easier for humans to understand and use.

Let's dive into the key features of Humanizer and provide examples for each to help you understand how to use it effectively.

---

# Humanizer

## Key Features

### 1. Humanize Date and Time

Humanizer makes it easy to represent dates and times in a human-readable format.

#### Example:
```csharp
using System;
using Humanizer;

class Program
{
    static void Main()
    {
        var pastDate = DateTime.UtcNow.AddHours(-24);
        Console.WriteLine(pastDate.Humanize()); // Output: "yesterday"

        var futureDate = DateTime.UtcNow.AddHours(2);
        Console.WriteLine(futureDate.Humanize()); // Output: "2 hours from now"
    }
}
```

### 2. Humanize Numbers

Humanizer can convert numbers into words.

#### Example:
```csharp
using System;
using Humanizer;

class Program
{
    static void Main()
    {
        int number = 1234;
        Console.WriteLine(number.ToWords()); // Output: "one thousand two hundred and thirty-four"
    }
}
```

### 3. Humanize Enums

Humanizer can convert enum values into more readable strings.

#### Example:
```csharp
using System;
using Humanizer;

class Program
{
    enum DaysOfWeek
    {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday
    }

    static void Main()
    {
        DaysOfWeek today = DaysOfWeek.Wednesday;
        Console.WriteLine(today.Humanize()); // Output: "Wednesday"
    }
}
```

### 4. Humanize Strings

Humanizer can format strings in various ways, such as converting them to sentence case or title case.

#### Example:
```csharp
using System;
using Humanizer;

class Program
{
    static void Main()
    {
        string text = "this is a test sentence.";
        Console.WriteLine(text.Transform(To.SentenceCase)); // Output: "This is a test sentence."
    }
}
```

### 5. Pluralize and Singularize

Humanizer can convert words between their singular and plural forms.

#### Example:
```csharp
using System;
using Humanizer;

class Program
{
    static void Main()
    {
        string singular = "apple";
        Console.WriteLine(singular.Pluralize()); // Output: "apples"

        string plural = "apples";
        Console.WriteLine(plural.Singularize()); // Output: "apple"
    }
}
```

### 6. Humanize TimeSpan

Humanizer can convert a `TimeSpan` into a human-readable format.

#### Example:
```csharp
using System;
using Humanizer;

class Program
{
    static void Main()
    {
        TimeSpan timeSpan = TimeSpan.FromMinutes(90);
        Console.WriteLine(timeSpan.Humanize()); // Output: "an hour"
    }
}
```

### 7. Dehumanize

Humanizer can also convert human-readable strings back into machine-readable formats.

#### Example:
```csharp
using System;
using Humanizer;

class Program
{
    static void Main()
    {
        string humanReadable = "two hours";
        TimeSpan timeSpan = humanReadable.DehumanizeTo<TimeSpan>();
        Console.WriteLine(timeSpan); // Output: "02:00:00"
    }
}
```

---

## Installation

To use Humanizer, you need to install the NuGet package. You can do this via the NuGet Package Manager in Visual Studio or by using the .NET CLI:

```shell
dotnet add package Humanizer
```

---

## Conclusion

Humanizer is a powerful and versatile library that makes it easier to present data in a human-readable format. Whether you're working with dates, times, numbers, enums, or strings, Humanizer has functionalities that can help you improve the user experience of your .NET applications. By leveraging Humanizer, you can make your application more intuitive and accessible to users.