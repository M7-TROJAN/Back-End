# Paginate

Pagination is a crucial concept in handling large datasets efficiently by dividing them into manageable chunks or pages. This technique is widely used in applications to provide a better user experience and improve performance. In this document, we will explore the concept of pagination, best practices for implementing it, and provide detailed examples using the `Paginate` method in C#.

## What is Pagination?

Pagination refers to the process of dividing a dataset into discrete pages, each containing a subset of the data. This allows users to navigate through large datasets more easily and improves application performance by only loading the data needed for the current page.

### Benefits of Pagination

1. **Performance Improvement**: Reduces the amount of data loaded into memory, speeding up data retrieval and rendering times.
2. **User Experience**: Enhances usability by preventing overwhelming users with too much information at once.
3. **Bandwidth Efficiency**: Limits the amount of data transferred between the server and client, conserving bandwidth.

## Implementing Pagination in C#

### Basic Pagination Method

Here is a basic implementation of a `Paginate` method that takes an `IEnumerable<T>` and returns a specific page of data:

```csharp
public static IEnumerable<TSource> Paginate<TSource>(this IEnumerable<TSource> source, int page = 1, int pageSize = 10)
{
    if (source == null)
        throw new ArgumentNullException($"{nameof(source)}");

    if (page <= 0)
    {
        // page = 1; // reset the value
        throw new ArgumentException($"{nameof(page)}");
    }

    if (pageSize <= 0)
    {
        // pageSize = 10; // reset the value
        throw new ArgumentException($"{nameof(pageSize)}");
    }

    if (!source.Any())
        return Enumerable.Empty<TSource>();

    return source.Skip((page - 1) * pageSize).Take(pageSize);
}
```

### Improved Pagination Method with Nullable Parameters

This version allows for nullable `page` and `pageSize` parameters, providing more flexibility:

```csharp
public static IEnumerable<TSource> Paginate<TSource>(this IEnumerable<TSource> source, int? page, int? pageSize)
{
    if (source == null)
        throw new ArgumentNullException($"{nameof(source)}");

    if (!page.HasValue)
        page = 1;

    if (!pageSize.HasValue)
        pageSize = 10;

    if (!source.Any())
        return Enumerable.Empty<TSource>();

    return source.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
}
```

### Combining Filtering with Pagination

The following method demonstrates how to combine filtering and pagination for more complex data handling scenarios:

```csharp
public static IEnumerable<TSource> WhereWithPaginate<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, int page = 1, int pageSize = 10)
{
    if (source == null)
        throw new ArgumentNullException($"{nameof(source)}");

    if (predicate == null)
        throw new ArgumentNullException($"{nameof(predicate)}");

    var result = Enumerable.Where(source, predicate);

    return Paginate(result, page, pageSize);
}
```

## Example Usage

### Sample Data

Let's define a sample dataset to demonstrate pagination:

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

var products = new List<Product>
{
    new Product { Id = 1, Name = "Product 1", Price = 10.0m },
    new Product { Id = 2, Name = "Product 2", Price = 20.0m },
    new Product { Id = 3, Name = "Product 3", Price = 30.0m },
    new Product { Id = 4, Name = "Product 4", Price = 40.0m },
    new Product { Id = 5, Name = "Product 5", Price = 50.0m },
    new Product { Id = 6, Name = "Product 6", Price = 60.0m },
    new Product { Id = 7, Name = "Product 7", Price = 70.0m },
    new Product { Id = 8, Name = "Product 8", Price = 80.0m },
    new Product { Id = 9, Name = "Product 9", Price = 90.0m },
    new Product { Id = 10, Name = "Product 10", Price = 100.0m },
    new Product { Id = 11, Name = "Product 11", Price = 110.0m },
    new Product { Id = 12, Name = "Product 12", Price = 120.0m }
};
```

### Paginate Example

Using the `Paginate` method to retrieve the second page of products with a page size of 5:

```csharp
var paginatedProducts = products.Paginate(2, 5);
foreach (var product in paginatedProducts)
{
    Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Price: {product.Price}");
}
```

**Output:**

```
Id: 6, Name: Product 6, Price: 60.0
Id: 7, Name: Product 7, Price: 70.0
Id: 8, Name: Product 8, Price: 80.0
Id: 9, Name: Product 9, Price: 90.0
Id: 10, Name: Product 10, Price: 100.0
```

### WhereWithPaginate Example

Filtering products with a price greater than $50 and then paginating the results:

```csharp
var filteredAndPaginatedProducts = products.WhereWithPaginate(p => p.Price > 50, 1, 5);
foreach (var product in filteredAndPaginatedProducts)
{
    Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Price: {product.Price}");
}
```

**Output:**

```
Id: 6, Name: Product 6, Price: 60.0
Id: 7, Name: Product 7, Price: 70.0
Id: 8, Name: Product 8, Price: 80.0
Id: 9, Name: Product 9, Price: 90.0
Id: 10, Name: Product 10, Price: 100.0
```

## Best Practices for Pagination

1. **Validation**: Ensure page and pageSize parameters are validated to prevent negative or zero values.
2. **Defaults**: Provide default values for page and pageSize to handle cases where they are not specified.
3. **Deferred Execution**: Use deferred execution (e.g., `IQueryable` or LINQ methods) to improve performance by executing queries only when needed.
4. **Error Handling**: Implement robust error handling to manage cases where the dataset is null or empty.
5. **Consistency**: Maintain consistent paging behavior across the application to ensure a uniform user experience.

## Use Cases in Real-World Scenarios

1. **Web Applications**: Displaying search results, product listings, or user comments in pages.
2. **APIs**: Implementing pagination in API endpoints to return manageable chunks of data to clients.
3. **Reporting**: Generating paginated reports to handle large datasets efficiently.
4. **Data Analysis**: Processing large datasets in chunks to optimize memory usage and performance.

By following these best practices and using the provided methods, you can efficiently implement and manage pagination in your applications, improving performance and user experience.

## improved version of the code

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

public static class PaginationExtensions
{
    /// <summary>
    /// Paginates the source collection based on the specified page number and page size.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source collection.</typeparam>
    /// <param name="source">The source collection to paginate.</param>
    /// <param name="page">The page number to retrieve. Defaults to 1.</param>
    /// <param name="pageSize">The size of each page. Defaults to 10.</param>
    /// <returns>A collection containing the elements of the specified page.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the source collection is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the page number or page size is less than or equal to zero.</exception>
    public static IEnumerable<TSource> Paginate<TSource>(this IEnumerable<TSource> source, int page = 1, int pageSize = 10)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source), "Source collection cannot be null.");

        if (page <= 0)
            throw new ArgumentException("Page number must be greater than zero.", nameof(page));

        if (pageSize <= 0)
            throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));

        return source.Skip((page - 1) * pageSize).Take(pageSize);
    }

    /// <summary>
    /// Paginates the source collection based on the specified page number and page size, with nullable parameters.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source collection.</typeparam>
    /// <param name="source">The source collection to paginate.</param>
    /// <param name="page">The page number to retrieve. Defaults to 1 if null.</param>
    /// <param name="pageSize">The size of each page. Defaults to 10 if null.</param>
    /// <returns>A collection containing the elements of the specified page.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the source collection is null.</exception>
    public static IEnumerable<TSource> Paginate<TSource>(this IEnumerable<TSource> source, int? page, int? pageSize)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source), "Source collection cannot be null.");

        page ??= 1;
        pageSize ??= 10;

        return source.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
    }

    /// <summary>
    /// Filters the source collection based on the specified predicate and then paginates the result.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source collection.</typeparam>
    /// <param name="source">The source collection to filter and paginate.</param>
    /// <param name="predicate">The predicate to filter the source collection.</param>
    /// <param name="page">The page number to retrieve. Defaults to 1.</param>
    /// <param name="pageSize">The size of each page. Defaults to 10.</param>
    /// <returns>A collection containing the filtered elements of the specified page.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the source collection or predicate is null.</exception>
    public static IEnumerable<TSource> WhereWithPaginate<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, int page = 1, int pageSize = 10)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source), "Source collection cannot be null.");

        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate), "Predicate cannot be null.");

        var filteredResult = source.Where(predicate);

        return Paginate(filteredResult, page, pageSize);
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

internal class Program
{
    private static void Main(string[] args)
    {
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Price = 10.0m },
            new Product { Id = 2, Name = "Product 2", Price = 20.0m },
            new Product { Id = 3, Name = "Product 3", Price = 30.0m },
            new Product { Id = 4, Name = "Product 4", Price = 40.0m },
            new Product { Id = 5, Name = "Product 5", Price = 50.0m },
            new Product { Id = 6, Name = "Product 6", Price = 60.0m },
            new Product { Id = 7, Name = "Product 7", Price = 70.0m },
            new Product { Id = 8, Name = "Product 8", Price = 80.0m },
            new Product { Id = 9, Name = "Product 9", Price = 90.0m },
            new Product { Id = 10, Name = "Product 10", Price = 100.0m },
            new Product { Id = 11, Name = "Product 11", Price = 110.0m },
            new Product { Id = 12, Name = "Product 12", Price = 120.0m }
        };

        // Paginate Example
        var paginatedProducts = products.Paginate(2, 5);
        Console.WriteLine("Paginated Products (Page 2, Page Size 5):");
        foreach (var product in paginatedProducts)
        {
            Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Price: {product.Price}");
        }

        Console.WriteLine();

        // WhereWithPaginate Example
        var filteredAndPaginatedProducts = products.WhereWithPaginate(p => p.Price > 50, 1, 5);
        Console.WriteLine("Filtered and Paginated Products (Price > 50, Page 1, Page Size 5):");
        foreach (var product in filteredAndPaginatedProducts)
        {
            Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Price: {product.Price}");
        }

        Console.ReadKey();
    }
}
```

### Explanation

1. **`Paginate` Method**: This method accepts an `IEnumerable<T>` and paginates it based on the provided page number and page size. It throws appropriate exceptions if the source is null or if the page/pageSize parameters are invalid.

2. **`Paginate` Method with Nullable Parameters**: This version allows nullable `page` and `pageSize` parameters, providing default values if they are null.

3. **`WhereWithPaginate` Method**: This method first filters the source collection using the provided predicate and then paginates the filtered result.

4. **Sample Data and Usage**: The `Program` class demonstrates how to use these methods with a sample `Product` list. The output is printed to the console to show the results of pagination and filtering.

By following these best practices and using the provided methods, you can efficiently implement and manage pagination in your applications, improving performance and user experience.
