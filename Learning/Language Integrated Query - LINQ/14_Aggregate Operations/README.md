# Aggregate Operations in LINQ

LINQ (Language Integrated Query) provides powerful methods for performing aggregate operations on collections. These operations are used to perform calculations on a collection of data and return a single value.

## Count

The `Count` method returns the number of elements in a collection.

### Example

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
int count = numbers.Count();
Console.WriteLine(count); // Output: 5
```

## Max and Min

The `Max` and `Min` methods return the maximum and minimum values in a collection, respectively.

### Example

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
int max = numbers.Max();
int min = numbers.Min();
Console.WriteLine(max); // Output: 5
Console.WriteLine(min); // Output: 1
```

## MaxBy and MinBy

The `MaxBy` and `MinBy` methods return the elements in a collection that have the maximum and minimum values, respectively, according to a specified key selector function.

### Example

```csharp
List<Person> people = new List<Person>
{
    new Person { Name = "Alice", Age = 30 },
    new Person { Name = "Bob", Age = 25 },
    new Person { Name = "Charlie", Age = 35 }
};

Person oldest = people.MaxBy(p => p.Age);
Person youngest = people.MinBy(p => p.Age);
Console.WriteLine(oldest.Name); // Output: Charlie
Console.WriteLine(youngest.Name); // Output: Bob
```

## Sum

The `Sum` method computes the sum of a collection of numeric values.

### Example

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
int sum = numbers.Sum();
Console.WriteLine(sum); // Output: 15
```

## Average

The `Average` method computes the average of a collection of numeric values.

### Example

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
double average = numbers.Average();
Console.WriteLine(average); // Output: 3
```

## Aggregate

The `Aggregate` method applies an accumulator function over a sequence. It can be used to perform complex calculations.

### Example

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
int product = numbers.Aggregate((acc, val) => acc * val);
Console.WriteLine(product); // Output: 120
```

### Example with Seed Value

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
int product = numbers.Aggregate(1, (acc, val) => acc * val);
Console.WriteLine(product); // Output: 120
```

### Example with Seed Value and Result Selector

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
string result = numbers.Aggregate(
    "Product: ",
    (acc, val) => acc + val + ",",
    acc => acc.TrimEnd(',')
);
Console.WriteLine(result); // Output: Product: 1,2,3,4,5
```

## Use Cases in Real-World Scenarios

### 1. Counting Items in a Collection

**Scenario:** Counting the number of products in stock.

```csharp
List<Product> products = GetProducts();
int productCount = products.Count();
```

### 2. Finding the Most Expensive and Cheapest Product

**Scenario:** Finding the highest and lowest priced products.

```csharp
Product mostExpensive = products.MaxBy(p => p.Price);
Product cheapest = products.MinBy(p => p.Price);
```

### 3. Calculating Total Sales

**Scenario:** Summing up the total sales amount.

```csharp
decimal totalSales = sales.Sum(s => s.Amount);
```

### 4. Calculating Average Rating

**Scenario:** Finding the average rating of a product.

```csharp
double averageRating = reviews.Average(r => r.Rating);
```

### 5. Aggregating a Custom Calculation

**Scenario:** Calculating the combined length of all movie titles.

```csharp
int totalTitleLength = movies.Aggregate(0, (acc, movie) => acc + movie.Title.Length);
```

### Data Type Examples

**Example with Integers:**

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
int sum = numbers.Sum();
int max = numbers.Max();
int min = numbers.Min();
double average = numbers.Average();
```

**Example with Complex Types:**

```csharp
List<Person> people = new List<Person>
{
    new Person { Name = "Alice", Age = 30 },
    new Person { Name = "Bob", Age = 25 },
    new Person { Name = "Charlie", Age = 35 }
};

Person oldest = people.MaxBy(p => p.Age);
Person youngest = people.MinBy(p => p.Age);
int totalAge = people.Sum(p => p.Age);
double averageAge = people.Average(p => p.Age);
```


### The `Aggregate` Method in Details

The `Aggregate` method in LINQ performs a custom aggregation operation on a sequence. It allows you to apply a function that processes each element of the sequence in turn, carrying forward an accumulated result. It's a versatile method used for reducing a sequence into a single value based on custom logic.

#### Form 1: `Aggregate<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)`

**Signature:**
```csharp
public static TSource Aggregate<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func);
```

**Parameters:**
- `source`: The sequence to aggregate.
- `func`: A function to apply to each element. It takes two parameters: the accumulated value and the current element from the sequence.

**Description:**
This overload uses the first element of the sequence as the initial accumulator value and then applies the aggregation function across the remaining elements. 

**Example:**
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };
int sum = numbers.Aggregate((acc, x) => acc + x);
Console.WriteLine(sum); // Output: 15
```

In this example, the initial accumulator value is `1` (the first element), and the function `acc + x` is applied to each subsequent element.

#### Form 2: `Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)`

**Signature:**
```csharp
public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func);
```

**Parameters:**
- `source`: The sequence to aggregate.
- `seed`: The initial accumulator value.
- `func`: A function to apply to each element. It takes two parameters: the accumulated value and the current element from the sequence.

**Description:**
This overload allows you to specify an initial seed value for the accumulator, which can be of a different type than the elements in the sequence.

**Example:**
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };
int product = numbers.Aggregate(1, (acc, x) => acc * x);
Console.WriteLine(product); // Output: 120
```

Here, the seed value is `1`, and the function `acc * x` is applied to each element, resulting in the product of all elements.

#### Form 3: `Aggregate<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)`

**Signature:**
```csharp
public static TResult Aggregate<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector);
```

**Parameters:**
- `source`: The sequence to aggregate.
- `seed`: The initial accumulator value.
- `func`: A function to apply to each element. It takes two parameters: the accumulated value and the current element from the sequence.
- `resultSelector`: A function to transform the final accumulated value into the result value.

**Description:**
This overload provides the most flexibility, allowing you to transform the accumulated value into a different result type at the end of the aggregation.

**Example:**
```csharp
var words = new List<string> { "Hello", "World", "LINQ" };
int totalLength = words.Aggregate(0, (acc, word) => acc + word.Length, acc => acc);
Console.WriteLine(totalLength); // Output: 13
```

In this example, the seed value is `0`, and the function `acc + word.Length` is applied to each element, accumulating the total length of all words. The result selector simply returns the accumulated value as the result.

### Use Cases in Real-World Scenarios

1. **Summing Numbers:**
   ```csharp
   var numbers = new List<int> { 1, 2, 3, 4, 5 };
   int sum = numbers.Aggregate((acc, x) => acc + x);
   // Output: 15
   ```

2. **Calculating Product:**
   ```csharp
   var numbers = new List<int> { 1, 2, 3, 4, 5 };
   int product = numbers.Aggregate(1, (acc, x) => acc * x);
   // Output: 120
   ```

3. **String Concatenation:**
   ```csharp
   var words = new List<string> { "Hello", "World", "LINQ" };
   string sentence = words.Aggregate((acc, word) => acc + " " + word);
   // Output: "Hello World LINQ"
   ```

4. **Counting Characters:**
   ```csharp
   var words = new List<string> { "Hello", "World", "LINQ" };
   int totalLength = words.Aggregate(0, (acc, word) => acc + word.Length, acc => acc);
   // Output: 13
   ```

5. **Combining Data into Complex Objects:**
   ```csharp
   var numbers = new List<int> { 1, 2, 3, 4, 5 };
   var result = numbers.Aggregate(
       new List<string>(),
       (acc, x) => { acc.Add(x.ToString()); return acc; },
       acc => string.Join(", ", acc)
   );
   // Output: "1, 2, 3, 4, 5"
   ```

## Summary

Aggregate operations in LINQ provide a powerful way to perform calculations on collections, returning a single value. They are essential for tasks like counting, summing, finding minimum and maximum values, and performing custom calculations with the `Aggregate` method. These operations are crucial in many real-world scenarios, such as analyzing data, calculating statistics, and summarizing information.
Understanding the `Aggregate` method allows you to perform complex reductions and transformations on sequences, providing powerful capabilities for custom aggregations and data manipulations in LINQ.