# Documenting Code with XML 

XML comments provide a way to document your code in C# using special tags that can be processed by tools to generate documentation in various formats, such as HTML or Markdown. These comments are written directly above the code elements they describe and follow a specific format.

## XML Comment Format

XML comments in C# are enclosed within `///`

**general format:**
```csharp
/// <summary>
/// Description of the method, class, property, or field.
/// </summary>
/// <param name="paramName">Description of the parameter.</param>
/// <returns>Description of the return value.</returns>
```

## Example: Documenting a Method

Consider a method for adding two numbers:

```csharp
/// <summary>
/// Adds two numbers and returns the result.
/// </summary>
/// <param name="num1">The first number to add.</param>
/// <param name="num2">The second number to add.</param>
/// <returns>The sum of <paramref name="num1"/> and <paramref name="num2"/>.</returns>
public int Add(int num1, int num2)
{
    return num1 + num2;
}
```

In this example:
- `<summary>` provides a brief description of what the method does.
- `<param>` tags describe each parameter, including their names and descriptions.
- `<returns>` describes the return value of the method.

## Example: Documenting a Class

Consider a class representing a person:

```csharp
/// <summary>
/// Represents a person with a name and age.
/// </summary>
public class Person
{
    /// <summary>
    /// Gets or sets the name of the person.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the age of the person.
    /// </summary>
    public int Age { get; set; }
}
```

In this example:
- The `<summary>` tag provides a brief description of the class.
- `<summary>` tags for properties describe their purpose.

## Generating Documentation

After adding XML comments to your code, you can generate documentation using tools like Visual Studio or external tools like Sandcastle or DocFX. This documentation can be in HTML, Markdown, or other formats.

## Conclusion

XML comments provide a structured and standardized way to document your code, making it easier for developers to understand its purpose and usage. By documenting your code effectively, you contribute to better maintainability and collaboration within your development team.
