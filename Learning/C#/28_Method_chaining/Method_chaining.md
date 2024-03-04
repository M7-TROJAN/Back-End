Method chaining, also known as fluent interface, is a design pattern in object-oriented programming where multiple methods are chained together in a single statement. This pattern allows for a more expressive and concise syntax, making the code easier to read and understand.

## How Method Chaining Works

In method chaining, each method typically returns an object of the same type or a new object that allows further methods to be called on it. By returning the current object (or a modified copy), the method can be called immediately after the previous one, without the need for intermediate variables.

### Example

```csharp
public class Calculator
{
    private int result;

    public Calculator Add(int number)
    {
        result += number;
        return this;
    }

    public Calculator Subtract(int number)
    {
        result -= number;
        return this;
    }

    public Calculator Multiply(int number)
    {
        result *= number;
        return this;
    }

    public int GetResult()
    {
        return result;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Method chaining example
        int result = new Calculator()
            .Add(5)
            .Multiply(2)
            .Subtract(3)
            .GetResult();

        Console.WriteLine(result); // Output: 7
    }
}
```

In this example, each method of the `Calculator` class returns a reference to the current `Calculator` object (`this`), allowing subsequent methods to be called on it in a chained manner. This results in a more readable and compact way of expressing complex operations.

## Benefits of Method Chaining

- **Expressiveness**: Method chaining allows for a more natural and fluent syntax, making the code easier to understand.
- **Readability**: Chained methods can be read from left to right, describing a sequence of actions in a clear and concise manner.
- **Conciseness**: Method chaining reduces the need for intermediate variables, resulting in more compact code.

## Use Cases

- **Configuration**: Method chaining is commonly used in configuration APIs to set various properties or options in a single statement.
- **Query Builders**: Method chaining is often used in query builder libraries to construct complex database queries using a fluent syntax.
- **Builder Pattern**: Method chaining can be used in builder patterns to construct objects step by step, configuring their properties in a fluent manner.

Method chaining is a powerful technique that enhances the readability and expressiveness of code, providing a cleaner and more maintainable way to work with objects and APIs.
