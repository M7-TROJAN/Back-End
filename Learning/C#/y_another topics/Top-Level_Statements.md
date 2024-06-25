### Top-Level Statements

**Top-Level Statements** is a feature introduced in C# 9.0 that allows you to write simpler and more concise code by eliminating the need for explicit `Main` method declarations in console applications. This is useful for small programs, scripts, and quick prototypes.

Example without top-level statements:
```csharp
using System;

namespace MyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
```

Example with top-level statements:
```csharp
using System;

Console.WriteLine("Hello World!");
```

### Native AOT (Ahead-of-Time) Compilation

**Native AOT** is a feature in .NET that allows you to compile your application ahead of time into a native executable. This can significantly improve startup time and reduce the memory footprint of your application. The Native AOT compiler generates platform-specific binaries, removing the need for the .NET runtime to be present on the target machine.

To enable Native AOT publish, you need to set the `PublishAot` property in your project file (`.csproj`).

### Steps to Enable Native AOT Publish

1. **Create a new .NET project or open an existing one**.

2. **Edit the project file (`.csproj`)** to include the Native AOT properties.

Here is an example of a simple console application with Native AOT enabled:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net7.0</TargetFramework>
    <PublishAot>true</PublishAot>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier> <!-- Change as needed -->
    <InvariantGlobalization>true</InvariantGlobalization> <!-- Optional: Improves AOT compatibility -->
  </PropertyGroup>

</Project>
```

### Example Without Top-Level Statements and Native AOT

Let's take the previous "Hello World" example and modify it to disable top-level statements and enable Native AOT publish.

1. **Create a new .NET project**:
    ```bash
    dotnet new console -n MyAotApp
    cd MyAotApp
    ```

2. **Edit the `Program.cs` file** to remove top-level statements:
    ```csharp
    using System;

    namespace MyAotApp
    {
        class Program
        {
            static void Main(string[] args)
            {
                Console.WriteLine("Hello World!");
            }
        }
    }
    ```

3. **Edit the project file (`MyAotApp.csproj`)** to enable Native AOT:
    ```xml
    <Project Sdk="Microsoft.NET.Sdk">

      <PropertyGroup>
        <OutputType>Exe</OutputType>
        <TargetFramework>net7.0</TargetFramework>
        <PublishAot>true</PublishAot>
        <RuntimeIdentifier>win-x64</RuntimeIdentifier> <!-- Change as needed -->
        <InvariantGlobalization>true</InvariantGlobalization> <!-- Optional: Improves AOT compatibility -->
      </PropertyGroup>

    </Project>
    ```

4. **Publish the project with Native AOT**:
    ```bash
    dotnet publish -c Release -r win-x64 --self-contained
    ```

   This command will create a native executable in the `publish` folder of your project. The `--self-contained` flag ensures that all necessary runtime components are included in the output.

### Conclusion

- **Top-Level Statements**: Simplifies the syntax for small programs by removing the need for an explicit `Main` method.
- **Native AOT (Ahead-of-Time) Compilation**: Compiles your application into a native executable, improving startup time and reducing memory usage. 

By following the steps provided, you can convert a simple .NET application to not use top-level statements and enable Native AOT for better performance and distribution.
