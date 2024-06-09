### What is Conditional Compilation in C#?

Conditional compilation in C# allows you to compile certain parts of your code only when specific conditions are met. This is useful for including or excluding code based on the environment, platform, or configuration. The compiler directives (`#if`, `#else`, `#elif`, `#endif`, etc.) control the compilation process based on whether specific symbols (also known as preprocessor symbols) are defined.

### Key Directives:

1. **`#if`**: Begins a conditional directive.
2. **`#else`**: Provides an alternative if the `#if` condition is not met.
3. **`#elif`**: Specifies a new condition to test if the previous `#if` or `#elif` condition is false.
4. **`#endif`**: Ends a conditional directive.
5. **`#define`**: Defines a symbol.
6. **`#undef`**: Undefines a symbol.

### Basic Example:

```csharp
#define DEBUG

using System;

class Program
{
    static void Main()
    {
        #if DEBUG
            Console.WriteLine("Debug mode");
        #else
            Console.WriteLine("Release mode");
        #endif
    }
}
```

In this example:
- The `#define DEBUG` directive defines the `DEBUG` symbol.
- The `#if DEBUG` directive checks if `DEBUG` is defined.
- If `DEBUG` is defined, the code within the `#if DEBUG` block is compiled.
- If `DEBUG` is not defined, the code within the `#else` block (if present) is compiled instead.

### Use-Cases and Real-World Scenarios:

1. **Debugging and Release Builds**:
    - Conditional compilation is often used to include debugging information or logging code only in debug builds.
    - This keeps the release build clean and efficient.

    ```csharp
    #define DEBUG

    class Program
    {
        static void Main()
        {
            #if DEBUG
                Console.WriteLine("Debug information");
            #endif

            Console.WriteLine("Program running");
        }
    }
    ```

2. **Platform-Specific Code**:
    - Sometimes you need to write code that runs differently on different platforms (Windows, Linux, macOS).

    ```csharp
    using System;

    class Program
    {
        static void Main()
        {
            #if WINDOWS
                Console.WriteLine("Running on Windows");
            #elif LINUX
                Console.WriteLine("Running on Linux");
            #elif MACOS
                Console.WriteLine("Running on macOS");
            #else
                Console.WriteLine("Unknown platform");
            #endif
        }
    }
    ```

    In this case, you can define `WINDOWS`, `LINUX`, or `MACOS` in the project settings or using compiler directives.

3. **Feature Flags**:
    - You might want to enable or disable certain features in your application based on configuration.

    ```csharp
    #define FEATURE_X

    class Program
    {
        static void Main()
        {
            #if FEATURE_X
                Console.WriteLine("Feature X is enabled");
            #else
                Console.WriteLine("Feature X is disabled");
            #endif
        }
    }
    ```

4. **API Versioning**:
    - You might need to support different versions of an API or library.

    ```csharp
    #define V1

    class API
    {
        #if V1
            public void DoSomething()
            {
                Console.WriteLine("API version 1");
            }
        #elif V2
            public void DoSomething()
            {
                Console.WriteLine("API version 2");
            }
        #endif
    }
    ```

### Real-World Scenario: Library Development

In a library, you might want to expose certain methods publicly only when compiled as part of a specific core library, but keep them internal otherwise. This can be achieved using conditional compilation:

```csharp
#if SYSTEM_PRIVATE_CORELIB
    public
#else
    internal
#endif
class MyClass
{
    // Implementation
}
```

Here:
- If `SYSTEM_PRIVATE_CORELIB` is defined, the class `MyClass` is `public`.
- If `SYSTEM_PRIVATE_CORELIB` is not defined, the class `MyClass` is `internal`.

This ensures that certain parts of the code are only accessible in specific contexts, like when building the core library versus a standard application.

### Defining Symbols

Symbols can be defined in several ways:
1. **In Code**: Using `#define` and `#undef`.

    ```csharp
    #define DEBUG
    #undef DEBUG
    ```

2. **Project File**: In the `.csproj` file.

    ```xml
    <PropertyGroup>
      <DefineConstants>DEBUG;TRACE</DefineConstants>
    </PropertyGroup>
    ```

3. **Compiler Command-Line Options**: Using `-define`.

    ```sh
    csc -define:DEBUG Program.cs
    ```

### Conclusion

Conditional compilation is a powerful feature that allows developers to control what code gets included in different builds, based on defined symbols. It’s widely used for debugging, platform-specific code, feature flags, and maintaining different versions of APIs or libraries. Understanding and using conditional compilation can help create flexible, maintainable, and efficient code.

## Author

- Mahmoud Mohamed
- Email: mahmoud.abdalaziz@outlook.com
- LinkedIn: [Mahmoud Mohamed Abdalaziz](https://www.linkedin.com/in/mahmoud-mohamed-abd/)

Happy learning and coding! 🚀
