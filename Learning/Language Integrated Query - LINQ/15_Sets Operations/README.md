# Set Operations in LINQ

Set operations in LINQ provide methods for performing various set-related operations on sequences. These operations include finding union, intersection, and differences between sequences, as well as eliminating duplicates. This document covers the following methods in detail, with examples and real-world use cases.

## Union and UnionBy

### Union

**Signature:**
```csharp
public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second);
```

**Description:**
Combines two sequences and returns the set union, excluding duplicates.

**Example:**
```csharp
var set1 = new List<int> { 1, 2, 3, 4 };
var set2 = new List<int> { 3, 4, 5, 6 };
var result = set1.Union(set2);
// Output: { 1, 2, 3, 4, 5, 6 }
```

### UnionBy

**Signature:**
```csharp
public static IEnumerable<TSource> UnionBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TKey> keySelector);
```

**Description:**
Combines two sequences and returns the set union based on a specified key selector, excluding duplicates.

**Example:**
```csharp
var set1 = new List<Person> { new Person("Alice", 25), new Person("Bob", 30) };
var set2 = new List<Person> { new Person("Alice", 25), new Person("Charlie", 35) };
var result = set1.UnionBy(set2, p => p.Name);
// Output: { Alice, Bob, Charlie }
```

## Intersect and IntersectBy

### Intersect

**Signature:**
```csharp
public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second);
```

**Description:**
Returns the set intersection of two sequences, excluding duplicates.

**Example:**
```csharp
var set1 = new List<int> { 1, 2, 3, 4 };
var set2 = new List<int> { 3, 4, 5, 6 };
var result = set1.Intersect(set2);
// Output: { 3, 4 }
```

### IntersectBy

**Signature:**
```csharp
public static IEnumerable<TSource> IntersectBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TKey> keySelector);
```

**Description:**
Returns the set intersection of two sequences based on a specified key selector, excluding duplicates.

**Example:**
```csharp
var set1 = new List<Person> { new Person("Alice", 25), new Person("Bob", 30) };
var set2 = new List<Person> { new Person("Alice", 25), new Person("Charlie", 35) };
var result = set1.IntersectBy(set2, p => p.Name);
// Output: { Alice }
```

## Distinct and DistinctBy

### Distinct

**Signature:**
```csharp
public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source);
public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource>? comparer);
```

**Description:**
Returns distinct elements from a sequence by using the default equality comparer or a specified one.

**Example:**
```csharp
var numbers = new List<int> { 1, 2, 2, 3, 4, 4, 5 };
var result = numbers.Distinct();
// Output: { 1, 2, 3, 4, 5 }
```

### DistinctBy

**Signature:**
```csharp
public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);
public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer);
```

**Description:**
Returns distinct elements from a sequence based on a specified key selector.

**Example:**
```csharp
var people = new List<Person> 
{ 
    new Person("Alice", 25), 
    new Person("Bob", 30), 
    new Person("Alice", 30) 
};
var result = people.DistinctBy(p => p.Name);
// Output: { Alice, Bob }
```

## Except and ExceptBy

### Except

**Signature:**
```csharp
public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second);
```

**Description:**
Returns the set difference of two sequences, excluding elements that appear in the second sequence.

**Example:**
```csharp
var set1 = new List<int> { 1, 2, 3, 4 };
var set2 = new List<int> { 3, 4, 5, 6 };
var result = set1.Except(set2);
// Output: { 1, 2 }
```

### ExceptBy

**Signature:**
```csharp
public static IEnumerable<TSource> ExceptBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TKey> keySelector);
```

**Description:**
Returns the set difference of two sequences based on a specified key selector, excluding elements that appear in the second sequence.

**Example:**
```csharp
var set1 = new List<Person> { new Person("Alice", 25), new Person("Bob", 30) };
var set2 = new List<Person> { new Person("Alice", 25), new Person("Charlie", 35) };
var result = set1.ExceptBy(set2, p => p.Name);
// Output: { Bob }
```

## Use Cases in Real-World Scenarios

1. **Combining Customer Lists:**
   ```csharp
   var customers1 = new List<Customer> { new Customer("Alice"), new Customer("Bob") };
   var customers2 = new List<Customer> { new Customer("Charlie"), new Customer("Alice") };
   var allCustomers = customers1.Union(customers2);
   // Output: { Alice, Bob, Charlie }
   ```

2. **Finding Common Elements:**
   ```csharp
   var attendeesDay1 = new List<string> { "Alice", "Bob", "Charlie" };
   var attendeesDay2 = new List<string> { "Bob", "Charlie", "David" };
   var commonAttendees = attendeesDay1.Intersect(attendeesDay2);
   // Output: { Bob, Charlie }
   ```

3. **Removing Duplicates:**
   ```csharp
   var emails = new List<string> { "alice@example.com", "bob@example.com", "alice@example.com" };
   var uniqueEmails = emails.Distinct();
   // Output: { alice@example.com, bob@example.com }
   ```

4. **Excluding Certain Elements:**
   ```csharp
   var allProducts = new List<Product> { new Product("Apple"), new Product("Banana"), new Product("Cherry") };
   var soldOutProducts = new List<Product> { new Product("Banana") };
   var availableProducts = allProducts.Except(soldOutProducts);
   // Output: { Apple, Cherry }
   ```

5. **Complex Data Types:**
   ```csharp
   var orders = new List<Order> 
   { 
       new Order(1, "Alice"), 
       new Order(2, "Bob"), 
       new Order(1, "Alice") 
   };
   var uniqueOrders = orders.DistinctBy(o => o.OrderId);
   // Output: { Order 1, Order 2 }
   ```

These set operations provide powerful capabilities for manipulating collections, enabling you to perform various tasks such as combining, intersecting, and excluding elements, as well as removing duplicates, all within the flexibility of LINQ.