

### Improved Code

```csharp
using System;
using System.Collections.Generic;

namespace ImperativeVsDeclarative
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<Person> people = new[]
            {
                new Person { Name = "Ali Saleh", Age = 34, Telephone = "+1(123)456-7890" },
                new Person { Name = "Rim Salem", Age = 19, Telephone = "+1(123)456-7891" },
                new Person { Name = "Ola Salam", Age = 44, Telephone = "+1(123)456-7892" },
                new Person { Name = "Huda Mohd", Age = 32, Telephone = "+1(123)456-7893" },
                new Person { Name = "Omar Kadi", Age = 28, Telephone = "+1(123)456-7894" }
            };

            // Filter people with age greater than or equal to 32 using a predicate
            Func<Person, bool> predicate = p => p.Age >= 32;
            var result = Filter(people, predicate);

            Console.WriteLine("People with Age >= 32");
            Console.WriteLine("---------------------");
            Print(result);

            Console.ReadKey();
        }

        // Generic filter method using a predicate (Declarative)
        static IEnumerable<Person> Filter(IEnumerable<Person> people, Func<Person, bool> predicate)
        {
            foreach (var person in people)
            {
                if (predicate(person))
                {
                    yield return person;
                }
            }
        }

        // Method to print the details of people (Declarative)
        static void Print(IEnumerable<Person> people)
        {
            foreach (var person in people)
            {
                Console.WriteLine($"Name: {person.Name}, Age: {person.Age}, Telephone: {person.Telephone}");
            }
        }
    }

    // Person class definition
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Telephone { get; set; }
    }
}
```

### Imperative vs. Declarative Programming

#### Imperative Programming

**Imperative Programming** is a paradigm that focuses on how to perform tasks. It involves specifying the steps needed to achieve a result, often involving direct manipulation of state and explicit control flow.

- **How**: Describes the steps to achieve the result.
- **State**: Often involves changing state through variables and loops.
- **Control Flow**: Uses statements like loops (`for`, `while`) and conditionals (`if`, `switch`).

Example:
```csharp
static IEnumerable<Person> FilterPeopleWithAgeLessThan(IEnumerable<Person> people, int age)
{
    foreach (var person in people)
    {
        if (person.Age < age)
        {
            yield return person;
        }
    }
}
```

In this example, the method explicitly iterates over the list of people and checks each person's age, detailing the steps to filter the list.

#### Declarative Programming

**Declarative Programming** is a paradigm that focuses on what to achieve rather than how to achieve it. It involves specifying the desired result without explicitly listing the steps to get there.

- **What**: Describes what the result should be.
- **State**: Minimizes explicit state changes, often using immutable data structures.
- **Control Flow**: Abstracted away, often using higher-order functions like `map`, `filter`, and `reduce`.

Example:
```csharp
static IEnumerable<Person> Filter(IEnumerable<Person> people, Func<Person, bool> predicate)
{
    foreach (var person in people)
    {
        if (predicate(person))
        {
            yield return person;
        }
    }
}
```

In this example, the `Filter` method abstracts the filtering logic using a predicate function, focusing on the result rather than the steps.

### Summary

- **Imperative Programming**: Specifies how to perform tasks through explicit steps and control flow.
- **Declarative Programming**: Specifies what the desired result is, often abstracting the control flow and focusing on the result.

In the improved code, we used a more declarative approach by defining a generic `Filter` method that takes a predicate, making the code more flexible and easier to read.
