The null-coalescing operator (`??`) in C# is used to provide a default value when a nullable type or reference type is null. This operator is especially useful for simplifying the handling of null values and providing fallback values.

### Basic Syntax

The null-coalescing operator takes the form:
```csharp
x = a ?? b;
```
- If `a` is not null, `x` will be assigned the value of `a`.
- If `a` is null, `x` will be assigned the value of `b`.

### Examples and Scenarios

#### Example 1: Basic Usage
```csharp
string name = null;
string displayName = name ?? "Default Name";
Console.WriteLine(displayName); // Output: Default Name
```
In this example, `name` is null, so `displayName` gets assigned "Default Name".

#### Example 2: Non-null Value
```csharp
string name = "Alice";
string displayName = name ?? "Default Name";
Console.WriteLine(displayName); // Output: Alice
```
Here, `name` is not null, so `displayName` gets assigned the value of `name`.

#### Example 3: Nullable Value Types
```csharp
int? number = null;
int value = number ?? -1;
Console.WriteLine(value); // Output: -1
```
In this example, `number` is a nullable integer. Since `number` is null, `value` is assigned -1.

#### Example 4: Chaining Null-Coalescing Operators
```csharp
string s1 = null;
string s2 = null;
string s3 = "Hello, World!";
string result = s1 ?? s2 ?? s3 ?? "Default";
Console.WriteLine(result); // Output: Hello, World!
```
This example shows chaining the null-coalescing operator. It will take the first non-null value from the sequence.

#### Example 5: Arrays and Collections
```csharp
int[] numbers = null;
int[] defaultNumbers = new int[] { 1, 2, 3 };
int[] resultNumbers = numbers ?? defaultNumbers;
Console.WriteLine(string.Join(", ", resultNumbers)); // Output: 1, 2, 3
```
In this example, `numbers` is null, so `resultNumbers` is assigned the value of `defaultNumbers`.

#### Example 6: Using with Methods
```csharp
public static string GetName()
{
    return null; // Simulating a method that might return null
}

string name = GetName() ?? "No Name";
Console.WriteLine(name); // Output: No Name
```
Here, `GetName()` returns null, so `name` is assigned the fallback value "No Name".

#### Example 7: Complex Object Properties
```csharp
class Person
{
    public string Name { get; set; }
}

Person person = new Person();
string personName = person.Name ?? "Unknown";
Console.WriteLine(personName); // Output: Unknown
```
In this case, `person.Name` is null (since it's not initialized), so `personName` is assigned "Unknown".

### Summary

The null-coalescing operator (`??`) is a concise and readable way to handle null values by providing a default value when the original value is null. This operator simplifies code and makes it easier to ensure that variables have valid values, even in the presence of nulls. By using the examples above, you can see how the null-coalescing operator can be applied in various scenarios to handle potential null values gracefully.
