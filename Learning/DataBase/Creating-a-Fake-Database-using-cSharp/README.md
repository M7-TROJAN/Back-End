
# Bogus - Fake Data Generator for .NET
Bogus is a powerful and flexible fake data generator library for .NET. It allows you to create realistic-looking test data quickly, making it an excellent tool for unit testing, database seeding, and other scenarios where you need realistic but non-sensitive information.

## Installation
- Nuget Package [Bogus](https://www.nuget.org/packages/Bogus/)
  
- To install Bogus, you can use the NuGet Package Manager Console or the .NET CLI.

### Using Package Manager Console:
1. Open Visual Studio.
2. Open the Package Manager Console. You can find it under 'View -> Other Windows -> Package Manager Console'.
3. In the Package Manager Console, type the following command and press Enter:
    ```bash
    Install-Package Bogus
    ```
4. NuGet will download and install the Bogus package along with its dependencies.

### Using .NET CLI:
- Alternatively, you can use the .NET CLI in your terminal or command prompt:

1. Open a terminal or command prompt.

2. Navigate to your project directory.

3. Run the following command:
    ```bash
    dotnet add package Bogus
    ```
This command adds the Bogus package to your project.

After completing these steps, you'll have Bogus installed in your .NET project, and you can start using it for data generation.

## Getting Started
- Here's a simple example of using Bogus to generate fake data:

```csharp
using Bogus;

class Program
{
    static void Main()
    {
        var faker = new Faker();

        // Generate a fake name
        string fakeName = faker.Name.FullName();
        Console.WriteLine($"Fake Name: {fakeName}");

        // Generate a fake email
        string fakeEmail = faker.Internet.Email();
        Console.WriteLine($"Fake Email: {fakeEmail}");
    }
}
```

## Key Features
1. Data Generation:
    Bogus provides methods for generating fake data across various categories, such as names, addresses, phone numbers, email addresses, and more.

    ```csharp
    var faker = new Faker();
    string fakeName = faker.Name.FullName();
    ```

2. Localization:
    You can generate data based on different locales, making it useful for internationalization testing.
    ```csharp
    var faker = new Faker("de"); // German locale
    ```

3. Flexible API:
    Bogus has a rich and fluent API that allows you to customize the generated data to suit your specific needs.
    ```csharp
    var customFaker = new Faker<MyClass>()
    .RuleFor(p => p.Property, f => f.Random.Word());
    ```

4. Seeding:
    You can seed the randomizer to generate the same set of data for testing consistency.
    ```csharp
    var faker = new Faker();
    faker.Random.Seed = 123;
    ```

5. Extensibility:
    Bogus is highly extensible, allowing you to create custom data providers and formatters.
    ```csharp
    faker.Random.UseSeed(123);
    ```

## Examples

- Generating Random Names:
```csharp
var faker = new Faker();
string fakeName = faker.Name.FullName();
Console.WriteLine($"Fake Name: {fakeName}");
```

- Generating Random Email Addresses:
```csharp
var faker = new Faker();
string fakeEmail = faker.Internet.Email();
Console.WriteLine($"Fake Email: {fakeEmail}");
```
- Customizing Data Generation:
```csharp
var customFaker = new Faker<MyClass>()
    .RuleFor(p => p.Property, f => f.Random.Word());
```

## Documentation
For detailed documentation and more advanced features, check the [official documentation](https://github.com/bchavez/Bogus).

## Author

- Mahmoud Mohamed
- Email: mahmoud.abdalaziz@outlook.com
- LinkedIn: [Mahmoud Mohamed Abdalaziz](https://www.linkedin.com/in/mahmoud-mohamed-abd/)

Happy learning and coding! 🚀
