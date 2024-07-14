# Concatenation in LINQ

Concatenation in LINQ involves combining multiple sequences into a single sequence. LINQ provides methods like `Concat` and `Union` to achieve this. This guide will cover the use of these methods, including their differences, use cases, and examples in both method and query syntax.

## Concat Method

The `Concat` method concatenates two sequences. It appends the elements of the second sequence to the first sequence without removing duplicates.

### Syntax

```csharp
public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second);
```

### Example

#### Method Syntax

```csharp
List<int> firstList = new List<int> { 1, 2, 3 };
List<int> secondList = new List<int> { 4, 5, 6 };

IEnumerable<int> concatenatedList = firstList.Concat(secondList);

foreach (var number in concatenatedList)
{
    Console.WriteLine(number); // Output: 1 2 3 4 5 6
}
```

#### Query Syntax

LINQ query syntax doesn't have a direct equivalent for `Concat`, so you need to use method syntax within the query:

```csharp
int[] firstArray = { 1, 2, 3 };
int[] secondArray = { 4, 5, 6 };

var concatenatedArray = firstArray.Concat(secondArray);

foreach (var number in concatenatedArray)
{
    Console.WriteLine(number); // Output: 1 2 3 4 5 6
}
```

## Union Method

The `Union` method combines two sequences and removes duplicates. It uses the default equality comparer to compare values.

### Syntax

```csharp
public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second);
```

### Example

#### Method Syntax

```csharp
List<int> firstList = new List<int> { 1, 2, 3 };
List<int> secondList = new List<int> { 3, 4, 5 };

IEnumerable<int> unionList = firstList.Union(secondList);

foreach (var number in unionList)
{
    Console.WriteLine(number); // Output: 1 2 3 4 5
}
```

#### Query Syntax

Similar to `Concat`, you need to use method syntax within the query for `Union`:

```csharp
int[] firstArray = { 1, 2, 3 };
int[] secondArray = { 3, 4, 5 };

var unionArray = firstArray.Union(secondArray);

foreach (var number in unionArray)
{
    Console.WriteLine(number); // Output: 1 2 3 4 5
}
```

## Differences Between `Concat` and `Union`

- **Duplicates**: `Concat` includes duplicates from both sequences, while `Union` removes duplicates.
- **Performance**: `Concat` is generally faster than `Union` because it does not check for duplicates.
- **Use Cases**:
  - Use `Concat` when you need to simply append one sequence to another without concern for duplicates.
  - Use `Union` when you need to combine two sequences and ensure all elements are unique.

## Use Cases in Real-World Scenarios

### Combining Lists of Objects

#### Example: Merging Customer Lists

Suppose you have two lists of customers from different regions, and you want to create a single list.

```csharp
List<Customer> region1Customers = new List<Customer>
{
    new Customer { Id = 1, Name = "Alice" },
    new Customer { Id = 2, Name = "Bob" }
};

List<Customer> region2Customers = new List<Customer>
{
    new Customer { Id = 3, Name = "Charlie" },
    new Customer { Id = 4, Name = "Dave" }
};

IEnumerable<Customer> allCustomers = region1Customers.Concat(region2Customers);

foreach (var customer in allCustomers)
{
    Console.WriteLine(customer.Name); // Output: Alice Bob Charlie Dave
}
```

### Removing Duplicates While Merging

#### Example: Merging Product Lists

Suppose you have two lists of products, and you want to merge them without duplicates.

```csharp
List<Product> warehouse1Products = new List<Product>
{
    new Product { Id = 1, Name = "Laptop" },
    new Product { Id = 2, Name = "Mouse" }
};

List<Product> warehouse2Products = new List<Product>
{
    new Product { Id = 2, Name = "Mouse" },
    new Product { Id = 3, Name = "Keyboard" }
};

IEnumerable<Product> allProducts = warehouse1Products.Union(warehouse2Products);

foreach (var product in allProducts)
{
    Console.WriteLine(product.Name); // Output: Laptop Mouse Keyboard
}
```

## Conclusion

Concatenation in LINQ is a powerful tool for combining sequences. The `Concat` method appends sequences as they are, while the `Union` method merges sequences and removes duplicates. Understanding the differences and appropriate use cases for these methods will help you manipulate and manage data effectively in your applications.