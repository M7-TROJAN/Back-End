Creating custom attributes in C# can be done in various ways, each serving different purposes. Here are some examples showcasing different use cases:

### 1. **Simple Metadata Attribute**

A basic custom attribute that adds metadata to classes or methods.

```csharp
using System;

class Program
{
    static void Main()
    {
        
        SampleClass sample = new SampleClass();
        
        var type = sample.GetType();
        var classAttributes = type.GetCustomAttributes(false);

        foreach (var attribute in classAttributes)
        {
            if (attribute is AuthorAttribute authorAttribute)
            {
                Console.WriteLine($"Class Author: {authorAttribute.Name}, Date: {authorAttribute.Date}");
            }
        }

    }
}


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorAttribute : Attribute
{
    public string Name { get; }
    public DateTime Date { get; }

    public AuthorAttribute(string name, string date)
    {
        Name = name;
        
        if (DateTime.TryParse(date, out var dateTime))
        {
            Date = dateTime;
        }
        else
        {
            Date = DateTime.Now;
        }
    }
}

// Usage
[Author("Mahmoud Mattar", "2024")]
public class SampleClass
{
    [Author("Mahmoud Mattar", "2023 -02-15")]
    public void SampleMethod()
    {
    }
}
```

### 2. **Validation Attribute**

A custom attribute to validate properties at runtime.

```csharp
using System;

[AttributeUsage(AttributeTargets.Property)]
public class RangeAttribute : Attribute
{
    public int Min { get; }
    public int Max { get; }

    public RangeAttribute(int min, int max)
    {
        Min = min;
        Max = max;
    }

    public bool IsValid(int value)
    {
        return value >= Min && value <= Max;
    }
}

public class Person
{
    [Range(0, 120)]
    public int Age { get; set; }
}

// Validation Logic
public static class Validator
{
    public static bool Validate(object obj)
    {
        var properties = obj.GetType().GetProperties();
        foreach (var property in properties)
        {
            var rangeAttribute = (RangeAttribute)Attribute.GetCustomAttribute(property, typeof(RangeAttribute));
            if (rangeAttribute != null)
            {
                int value = (int)property.GetValue(obj);
                if (!rangeAttribute.IsValid(value))
                {
                    return false;
                }
            }
        }
        return true;
    }
}
```

### 3. **Custom Attribute with Code Analysis (Roslyn Analyzer)**

A custom attribute to mark deprecated elements, with a Roslyn analyzer to produce warnings or errors.

#### Custom Attribute:

```csharp
using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class DeprecatedAttribute : Attribute
{
    public string Message { get; }
    public bool IsError { get; }

    public DeprecatedAttribute(string message, bool isError = false)
    {
        Message = message;
        IsError = isError;
    }
}

// Usage
[Deprecated("This class is deprecated.", false)]
public class OldClass
{
}

[Deprecated("This method is deprecated.", true)]
public void OldMethod()
{
}
```

#### Roslyn Analyzer:

Create a new Roslyn analyzer project:

```sh
dotnet new analyzer -n DeprecatedAnalyzer
```

Implement the analyzer:

```csharp
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DeprecatedAnalyzer : DiagnosticAnalyzer
{
    private const string DiagnosticId = "DEP001";
    private const string Title = "Deprecated usage";
    private const string MessageFormat = "{0}";
    private const string Description = "This element is deprecated.";
    private const string Category = "Usage";

    private static readonly DiagnosticDescriptor WarningRule = new DiagnosticDescriptor(
        DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
    
    private static readonly DiagnosticDescriptor ErrorRule = new DiagnosticDescriptor(
        DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => ImmutableArray.Create(WarningRule, ErrorRule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.ClassDeclaration, SyntaxKind.MethodDeclaration);
    }

    private void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var node = context.Node;
        var symbol = context.SemanticModel.GetDeclaredSymbol(node);
        if (symbol == null) return;

        var deprecatedAttribute = symbol.GetAttributes()
            .FirstOrDefault(ad => ad.AttributeClass?.ToDisplayString() == typeof(DeprecatedAttribute).FullName);
        
        if (deprecatedAttribute == null) return;

        var message = (string)deprecatedAttribute.ConstructorArguments[0].Value;
        var isError = (bool)deprecatedAttribute.ConstructorArguments[1].Value;

        var diagnostic = Diagnostic.Create(isError ? ErrorRule : WarningRule, node.GetLocation(), message);
        context.ReportDiagnostic(diagnostic);
    }
}
```

### 4. **Logging Attribute**

A custom attribute to log method execution times.

```csharp
using System;
using System.Diagnostics;

[AttributeUsage(AttributeTargets.Method)]
public class LogExecutionTimeAttribute : Attribute
{
}

// Aspect-Oriented Programming (AOP) or similar technique to use this attribute
public class Logger
{
    public static void LogExecutionTime(Action method, string methodName)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        method();
        stopwatch.Stop();
        Console.WriteLine($"{methodName} executed in {stopwatch.ElapsedMilliseconds} ms");
    }
}

public class SomeClass
{
    [LogExecutionTime]
    public void SomeMethod()
    {
        // Method implementation
    }
}

// Sample usage
public static void Main()
{
    SomeClass instance = new SomeClass();
    Logger.LogExecutionTime(instance.SomeMethod, nameof(instance.SomeMethod));
}
```

### Summary

Custom attributes can add a lot of flexibility and functionality to your code. They can be used for metadata, validation, logging, or even to influence compilation behavior when combined with tools like Roslyn analyzers. By understanding these use cases, you can create powerful and reusable components in your applications.
