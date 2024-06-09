
## Attributes in C#

Attributes in C# are a powerful feature that allows developers to add metadata to code elements. This metadata can be used for various purposes, including documentation, enforcing coding standards, modifying runtime behavior, and more. 

### What, Why, and How

**What:**  
Attributes are special classes in .NET that add metadata to code elements like classes, methods, properties, and more.

**Why:**  
They are used to provide additional information about the code which can be accessed at runtime using reflection. This helps in various tasks like serialization, enforcing rules, code generation, etc.

**How:**  
Attributes are applied using square brackets `[]` before the code element they are describing.

Example:
```csharp
[Obsolete("This class is obsolete. Use NewClass instead.")]
public class OldClass
{
    // class implementation
}
```

### Where to Apply

Attributes can be applied to almost any code element in C#, including:

- Assemblies
- Classes
- Methods
- Properties
- Fields
- Events
- Parameters

Example:
```csharp
[Serializable]
public class Person
{
    [Obsolete("Use NewMethod instead.")]
    public void OldMethod() { }

    [NonSerialized]
    private int age;

    public string Name { get; set; }
}
```

### Common Attributes

Some commonly used attributes in .NET:

- `[Obsolete]`: Marks a code element as obsolete.
- `[Serializable]`: Indicates that a class can be serialized.
- `[NonSerialized]`: Prevents a field from being serialized.
- `[DllImport]`: Used to import a method from a DLL.

Example:
```csharp
public class CommonAttributesExample
{
    [Obsolete("Use NewMethod instead.")]
    public void OldMethod() { }

    [Serializable]
    public class SerializableClass
    {
        [NonSerialized]
        private int nonSerializedField;

        public string Name { get; set; }
    }
}
```

### Custom Attribute

You can create your own attributes by inheriting from the `System.Attribute` class.

Example:
```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
public class DeveloperAttribute : Attribute
{
    public string Name { get; }
    public string Level { get; }

    public DeveloperAttribute(string name, string level)
    {
        Name = name;
        Level = level;
    }
}

[Developer("John Doe", "Senior")]
public class CustomAttributeExample
{
    [Developer("Jane Smith", "Junior")]
    public void CustomMethod() { }
}
```

### AttributeUsage

The `AttributeUsage` attribute specifies how a custom attribute can be applied. It allows you to define:

- The targets the attribute can be applied to.
- Whether the attribute can be inherited.
- Whether multiple instances of the attribute can be applied to a single element.

Example:
```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class SampleAttribute : Attribute
{
    public string Description { get; }

    public SampleAttribute(string description)
    {
        Description = description;
    }
}

[Sample("This is a sample class.")]
public class AttributeUsageExample
{
    [Sample("This is a sample method.")]
    public void SampleMethod() { }
}
```

### Reflection and Attributes

Reflection is used to inspect and interact with attributes at runtime.

Example:
```csharp
using System;
using System.Reflection;

[Developer("John Doe", "Senior")]
public class ReflectionExample
{
    [Developer("Jane Smith", "Junior")]
    public void ExampleMethod() { }

    public static void Main()
    {
        Type type = typeof(ReflectionExample);

        // Get custom attributes of the class
        var classAttributes = type.GetCustomAttributes(typeof(DeveloperAttribute), false);
        Console.WriteLine("Class Attributes:");
        foreach (DeveloperAttribute attr in classAttributes)
        {
            Console.WriteLine($"{attr.Name} - {attr.Level}");
        }

        // Get custom attributes of the method
        MethodInfo method = type.GetMethod("ExampleMethod");
        var methodAttributes = method.GetCustomAttributes(typeof(DeveloperAttribute), false);
        Console.WriteLine("\nMethod Attributes:");
        foreach (DeveloperAttribute attr in methodAttributes)
        {
            Console.WriteLine($"{attr.Name} - {attr.Level}");
        }
    }
}
```

### Complete Example

Here is a complete example that demonstrates various aspects of attributes in C#:

```csharp
using System;
using System.Reflection;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
public class DeveloperAttribute : Attribute
{
    public string Name { get; }
    public string Level { get; }

    public DeveloperAttribute(string name, string level)
    {
        Name = name;
        Level = level;
    }
}

[Developer("John Doe", "Senior")]
public class ReflectionExample
{
    [Developer("Jane Smith", "Junior")]
    public void ExampleMethod() { }

    public static void Main()
    {
        Type type = typeof(ReflectionExample);

        // Get custom attributes of the class
        var classAttributes = type.GetCustomAttributes(typeof(DeveloperAttribute), false);
        Console.WriteLine("Class Attributes:");
        foreach (DeveloperAttribute attr in classAttributes)
        {
            Console.WriteLine($"{attr.Name} - {attr.Level}");
        }

        // Get custom attributes of the method
        MethodInfo method = type.GetMethod("ExampleMethod");
        var methodAttributes = method.GetCustomAttributes(typeof(DeveloperAttribute), false);
        Console.WriteLine("\nMethod Attributes:");
        foreach (DeveloperAttribute attr in methodAttributes)
        {
            Console.WriteLine($"{attr.Name} - {attr.Level}");
        }
    }
}
```

### Summary

- **What, Why, and How**: Attributes are metadata for code elements, used for various purposes, and applied using square brackets.
- **Where to Apply**: Attributes can be applied to classes, methods, properties, fields, etc.
- **Common Attributes**: Includes `[Obsolete]`, `[Serializable]`, `[NonSerialized]`, and `[DllImport]`.
- **Custom Attribute**: You can create custom attributes by inheriting from `System.Attribute`.
- **AttributeUsage**: Specifies how custom attributes can be applied.
- **Reflection and Attributes**: Use reflection to inspect attributes at runtime.

## Author

- Mahmoud Mohamed
- Email: mahmoud.abdalaziz@outlook.com
- LinkedIn: [Mahmoud Mohamed Abdalaziz](https://www.linkedin.com/in/mahmoud-mohamed-abd/)

Happy learning and coding! 🚀
