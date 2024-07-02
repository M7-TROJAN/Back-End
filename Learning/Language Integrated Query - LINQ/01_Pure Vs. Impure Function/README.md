### Pure Functions

A **pure function** is a function that:

1. **Deterministic**: For the same input, it will always return the same output.
2. **No Side Effects**: It does not cause any observable side effects, such as modifying a global variable, changing the state of an object, or performing I/O operations (like writing to a file or printing to a console).

#### Characteristics of Pure Functions

- **Idempotent**: Repeated calls with the same arguments yield the same result.
- **Referential Transparency**: The function call can be replaced with its output value without changing the program's behavior.
- **Easier to Test**: Since the output depends solely on the input, they are easier to unit test.

#### Example of Pure Function in C#

```csharp
public int Add(int a, int b)
{
    return a + b;
}
```

Here, `Add` is a pure function because it always returns the same output for the same inputs and does not alter any state or perform any I/O operations.

### Impure Functions

An **impure function** is a function that does not adhere to the principles of pure functions. It may:

1. **Have Side Effects**: Modify a global variable, change the state of an object, perform I/O operations, etc.
2. **Non-deterministic**: The output may vary even with the same input due to reliance on external state or variables.

#### Characteristics of Impure Functions

- **Side Effects**: Observable effects beyond returning a value, such as modifying global state or interacting with the outside world.
- **Harder to Test**: Due to dependencies on external state or variables, testing becomes more complex.
- **Potentially Less Predictable**: The behavior may vary based on external factors.

#### Example of Impure Function in C#

```csharp
int counter = 0;

public int IncrementCounter()
{
    counter++;
    return counter;
}
```

Here, `IncrementCounter` is an impure function because it modifies the `counter` variable, which is outside the function's scope.

### Pure vs. Impure Functions in LINQ

LINQ (Language Integrated Query) promotes functional programming paradigms, and understanding pure vs. impure functions is crucial when writing LINQ queries.

#### Pure Functions in LINQ

LINQ queries often leverage pure functions to ensure that they are predictable and side-effect-free.

Example of Pure Function in LINQ:

```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };
var evenNumbers = numbers.Where(n => n % 2 == 0).ToList();
```

In this example, the lambda function `n => n % 2 == 0` is a pure function because it depends only on its input `n` and does not modify any external state.

#### Impure Functions in LINQ

Using impure functions within LINQ queries can lead to unexpected results and make the code harder to understand and maintain.

Example of Impure Function in LINQ:

```csharp
int threshold = 3;
var numbers = new List<int> { 1, 2, 3, 4, 5 };

var results = numbers.Select(n =>
{
    if (n > threshold)
    {
        threshold = n; // Impure action: modifying external state
    }
    return n;
}).ToList();
```

In this example, the lambda function modifies the external variable `threshold`, making it an impure function. This can lead to unpredictable behavior and side effects.

### Advantages of Using Pure Functions

1. **Predictability**: Easier to predict the output based on input.
2. **Testability**: Simplifies unit testing.
3. **Concurrency**: Safer to use in concurrent or parallel execution since they do not modify shared state.

### Conclusion

Understanding the difference between pure and impure functions is essential when working with LINQ and functional programming. Pure functions lead to more predictable, maintainable, and testable code, whereas impure functions can introduce side effects and make the code harder to understand and debug.