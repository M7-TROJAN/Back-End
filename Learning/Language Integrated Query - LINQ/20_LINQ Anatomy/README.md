# LINQ Anatomy

LINQ (Language Integrated Query) is a powerful feature in C# that provides a consistent way to query and manipulate data. Understanding the anatomy of LINQ involves breaking down its components and how they interact. This guide will cover the essential elements of LINQ, using the provided card game examples to illustrate the concepts.

## Table of Contents
1. Introduction to LINQ
2. LINQ Query Syntax vs Method Syntax
3. Deferred vs Immediate Execution
4. LINQ Operators
5. Execution and Optimization
6. Examples with Card Game

---

### 1. Introduction to LINQ

LINQ (Language Integrated Query) is a feature of C# that provides query capabilities directly in the language syntax. It can query various data sources like collections, databases, XML documents, and more. LINQ brings a unified syntax to different types of data querying.

### 2. LINQ Query Syntax vs Method Syntax

LINQ can be written in two syntaxes: Query Syntax and Method Syntax.

- **Query Syntax**: Similar to SQL, it provides a readable, declarative way to express queries.
- **Method Syntax**: Uses extension methods to chain query operations. It's more powerful and flexible but can be less readable.

**Example:**

Query Syntax:
```csharp
var query = from card in deck
            where card.Value > 5
            orderby card.Value
            select card;
```

Method Syntax:
```csharp
var query = deck.Where(card => card.Value > 5)
                .OrderBy(card => card.Value);
```

### 3. Deferred vs Immediate Execution

- **Deferred Execution**: The query is not executed until the data is actually iterated over. This allows for query composition and optimization.
- **Immediate Execution**: The query is executed immediately, and the results are stored in memory.

**Example:**

Deferred Execution:
```csharp
var query = deck.Where(card => card.Value > 5);
```

Immediate Execution:
```csharp
var list = deck.Where(card => card.Value > 5).ToList();
```

### 4. LINQ Operators

LINQ provides a variety of operators that can be used to filter, project, join, and manipulate data. Here are some common ones:

- **Where**: Filters elements based on a predicate.
- **Select**: Projects each element into a new form.
- **OrderBy/ThenBy**: Sorts elements in ascending order.
- **GroupBy**: Groups elements based on a key.
- **Join**: Joins two sequences based on a key.
- **Take/Skip**: Limits the number of elements in a sequence.

### 5. Execution and Optimization

Understanding the execution order of LINQ queries can lead to better performance and more efficient code.

- **Order of Operations**: Filters should be placed before sorting to reduce the number of elements to sort.
- **Streaming vs Non-Streaming**: Streaming operators (e.g., `Where`, `Select`) process elements one at a time. Non-streaming operators (e.g., `OrderBy`, `GroupBy`) require processing the entire collection.

### 6. Examples with Card Game

Let's use the card game examples provided to illustrate these concepts.

**Sample Data:**
```csharp
public class Card
{
    public enum Suites { HEARTS, DIAMONDS, CLUBS, SPADES }

    public int Value { get; set; }
    public Suites Suite { get; set; }

    public string NamedValue
    {
        get
        {
            return Value switch
            {
                14 => "Ace",
                13 => "King",
                12 => "Queen",
                11 => "Jack",
                _ => Value.ToString()
            };
        }
    }

    public string Name => $"{NamedValue} of {Suite}";
    public bool IsRed => Suite == Suites.HEARTS || Suite == Suites.DIAMONDS;

    public Card(int value, Suites suite)
    {
        Value = value;
        Suite = suite;
    }
}

public class Deck
{
    private static Random rnd = new Random();
    public IEnumerable<Card> FillDeck()
    {
        for (int i = 0; i < 52; i++)
        {
            Card.Suites suite = (Card.Suites)(i / 13);
            int val = i % 13 + 2;
            yield return new Card(val, suite);
        }
    }

    public IEnumerable<Card> Shuffle()
    {
        return FillDeck().OrderBy(x => rnd.Next());
    }
}

public static class Extensions
{
    public static void PrintDeck(this IEnumerable<Card> cards, string title)
    {
        Console.WriteLine($"\n\n\n###### {title} ######");
        foreach (Card card in cards)
        {
            Console.WriteLine(card.Name);
        }
    }
}
```

**Demo Execution Order:**
```csharp
private static void DemoExecutionOrder()
{
    var numbers = new int[] { 8, 2, 3, 4, 1, 6, 5, 12, 9 };

    var query = numbers
        .Where(x =>
        {
            Console.WriteLine($"Where({x} > 5) => {x > 5}");
            return x > 5;
        })
        .Select(x =>
        {
            Console.WriteLine($"\tSelect({x} * {x}) => {x * x}");
            return x * x;
        })
        .Where(x =>
        {
            var result = x % 6 == 0;
            Console.WriteLine($"\t\tWhere({x} % 6 == 0) => {result}");
            if (result)
                Console.WriteLine($"\t\t\t\tTake: {x}");
            return result;
        })
        .Take(2);

    var list = query.ToList();

    foreach (var item in list)
        Console.Write($" {item}");
}
```

**Output:**
```
Where(8 > 5) => True
    Select(8 * 8) => 64
        Where(64 % 6 == 0) => False
Where(2 > 5) => False
Where(3 > 5) => False
Where(4 > 5) => False
Where(1 > 5) => False
Where(6 > 5) => True
    Select(6 * 6) => 36
        Where(36 % 6 == 0) => True
                Take: 36
Where(5 > 5) => False
Where(12 > 5) => True
    Select(12 * 12) => 144
        Where(144 % 6 == 0) => True
                Take: 144
 36 144
```

**Example: Filtering and Ordering Cards**

```csharp
private static void DemoFilterOrder()
{
    var deck = new Deck();

    var query1 = deck.Shuffle()
        .Where(x => x.IsRed)
        .OrderBy(x => x.Value)
        .Take(10);

    query1.PrintDeck("Top 10 red cards");

    var query2 = deck.Shuffle()
        .OrderBy(x => x.Value)
        .Take(10)
        .Where(x => x.IsRed);

    query2.PrintDeck("Red cards in the top 10");
}
```

### Conclusion

Understanding the anatomy of LINQ involves mastering its syntax, execution strategies, and various operators. By learning these concepts, you can write more efficient and readable code. The examples provided with the card game context should help solidify these concepts in a practical way.