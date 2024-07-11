# Generation Operations in LINQ

In LINQ, generation operations are used to create sequences of data. These operations can be particularly useful when you need to generate a collection of data items on the fly. This lesson covers three main generation operations: `Enumerable.Empty`, `Enumerable.DefaultIfEmpty`, `Enumerable.Range`, and `Enumerable.Repeat`.

## Sample Data

We'll use the following classes and data for our examples:

```csharp
public class Choice
{
    public int Order { get; set; }
    public string Description { get; set; }
}

public class Question
{
    public string Title { get; set; }
    public List<Choice> Choices { get; set; } = new();
    public int CorrectAnswer { get; set; }

    // Question.Default
    public readonly static Question Default = new Question
    {
        Title = "<<<<< QUESTION TITLE GOES HERE >>>>>",
        Choices = new List<Choice>
        {
            new Choice { Order = 1, Description = "<<<<< CHOICE #1 GOES HERE >>>>>" },
            new Choice { Order = 2, Description = "<<<<< CHOICE #2 GOES HERE >>>>>" },
            new Choice { Order = 3, Description = "<<<<< CHOICE #3 GOES HERE >>>>>" },
            new Choice { Order = 4, Description = "<<<<< CHOICE #4 GOES HERE >>>>>" }
        },
        CorrectAnswer = 0
    };

    public override string ToString()
    {
        var choices = "";

        foreach (var item in Choices)
        {
            choices += $"\n\t{item.Order}) {item.Description}";
        }

        return $"{Title}" +
               $"{choices}";
    }
}

public static class QuestionBank
{
    private static Random random = new Random();

    public static IEnumerable<Question> Randomize(int count)
    {
        if (All.Count < count)
            return AllShuffled;
        return AllShuffled.Take(count);
    }

    public static Question PickOne()
    {
        return All[random.Next(0, All.Count)];
    }

    public static void ToQuiz(this IEnumerable<Question> questions)
    {
        foreach (var question in questions)
        {
            Console.WriteLine(question);
            Console.WriteLine();
        }
    }

    public static List<Question> GetQuestionRange(IEnumerable<int> range)
    {
        return All.Where((x, i) => range.Contains(i)).ToList();
    }

    public static List<Question> All => new List<Question>
    {
        new Question
        {
            Title = "What is the capital of France?",
            Choices = new List<Choice>
            {
                new Choice { Order = 1, Description = "Paris" },
                new Choice { Order = 2, Description = "London" },
                new Choice { Order = 3, Description = "Berlin" },
                new Choice { Order = 4, Description = "Madrid" }
            },
            CorrectAnswer = 1
        },
        new Question
        {
            Title = "What is the capital of Spain?",
            Choices = new List<Choice>
            {
                new Choice { Order = 1, Description = "Paris" },
                new Choice { Order = 2, Description = "London" },
                new Choice { Order = 3, Description = "Berlin" },
                new Choice { Order = 4, Description = "Madrid" }
            },
            CorrectAnswer = 4
        },
        // Additional questions omitted for brevity
    };

    private static List<Question> AllShuffled =>
        All.OrderBy(x => random.Next()).ToList();
}
```

## Deferred vs. Immediate Execution

Deferred execution means that the evaluation of a query is delayed until its results are actually iterated over. Immediate execution means that the query is evaluated and executed immediately.

### Example

```csharp
var questions = new List<Question>(); // Empty list, in this line the list is already in memory

// Immediate execution
foreach (var q in questions)
{
    Console.WriteLine(q);
}

// Deferred execution
var questions2 = Enumerable.Empty<Question>(); // In this line, the list is not in memory yet, but it will be when we iterate over it

foreach (var q in questions2)
{
    Console.WriteLine(q);
}

Console.ReadKey();
```

## `Enumerable.Empty`

This method returns an empty sequence of a specified type. It is useful when you need an empty sequence to perform operations like concatenation or default values.

### Example

```csharp
var questions = Enumerable.Empty<Question>();

var question2 = questions.DefaultIfEmpty();

var question3 = questions.DefaultIfEmpty(Question.Default);

question3.ToQuiz();
```

## `Enumerable.DefaultIfEmpty`

This method returns the elements of the specified sequence or a default value if the sequence is empty.

### Example

```csharp
var questions = Enumerable.Empty<Question>();

var question2 = questions.DefaultIfEmpty();

var question3 = questions.DefaultIfEmpty(Question.Default);

question3.ToQuiz();
```

## `Enumerable.Range`

This method generates a sequence of integral numbers within a specified range. It is useful for generating sequences of numbers.

### Example

```csharp
var range = Enumerable.Range(1, 10);

var questions = QuestionBank.GetQuestionRange(range);

questions.ToQuiz();
```

## `Enumerable.Repeat`

This method generates a sequence that contains one repeated value for a specified number of times. It is useful for generating sequences of the same value.

### Example

```csharp
var q = QuestionBank.PickOne();

var questions2 = new List<Question>();
for (int i = 0; i < 10; i++)
{
    questions2.Add(new Question());
}

Console.WriteLine(ReferenceEquals(questions2[0], questions2[1]));

var questions = Enumerable.Repeat(q, 10).ToList();

Console.WriteLine(ReferenceEquals(questions[0], questions[1]));
questions.ToQuiz();
```

## Conclusion

Understanding and using generation operations in LINQ can simplify your code and make it more efficient. These operations allow you to generate sequences of data on demand, which can be particularly useful in various programming scenarios. By leveraging methods like `Enumerable.Empty`, `Enumerable.DefaultIfEmpty`, `Enumerable.Range`, and `Enumerable.Repeat`, you can create flexible and powerful LINQ queries.