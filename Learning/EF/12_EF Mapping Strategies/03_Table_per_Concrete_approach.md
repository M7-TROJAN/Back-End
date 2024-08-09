### Table per Concrete Type (TPC) in Entity Framework Core

The **Table per Concrete Type (TPC)** approach is another inheritance mapping strategy in Entity Framework Core. Unlike **Table per Hierarchy (TPH)** and **Table per Type (TPT)**, TPC creates a separate table for each concrete class in the inheritance hierarchy. This strategy does not use any form of discrimination column, as each table is independent and contains all the properties of the concrete class.

#### Key Concepts of TPC:

1. **Separate Tables for Each Concrete Type**:
   - Each concrete class in the hierarchy gets its own table.
   - No table is created for the abstract base class.
   - Each table contains columns for all properties in the concrete class, including those inherited from the base class.

2. **No Discriminator Column**:
   - Unlike TPH, TPC doesn't need a discriminator column since each table represents a single concrete type.

3. **Duplication of Base Class Properties**:
   - The columns for the base class properties are duplicated across the tables for the concrete types.
   - This can lead to redundancy but simplifies querying by avoiding joins.

#### Example Scenario

Let's start with the same class hierarchy as before:

```csharp
public abstract class Participant
{
    public int Id { get; set; }
    public string? FName { get; set; }
    public string? LName { get; set; }
    [RegularExpression("^[fm]$", ErrorMessage = "Gender must be 'f' or 'm'")]
    public char Gender { get; set; }
}

public class Individual : Participant
{
    public string University { get; set; }
    public int YearOfGraduation { get; set; }
    public bool IsIntern { get; set; }
}

public class Corporate : Participant
{
    public string Company { get; set; }
    public string JobTitle { get; set; }
}
```

#### Database Structure with TPC

With TPC, Entity Framework Core will create two separate tables: one for `Individual` and one for `Corporate`.

- **Individuals Table**:
  - Columns: `Id`, `FName`, `LName`, `Gender`, `University`, `YearOfGraduation`, `IsIntern`
  
- **Corporates Table**:
  - Columns: `Id`, `FName`, `LName`, `Gender`, `Company`, `JobTitle`

Here, the `Id`, `FName`, `LName`, and `Gender` columns are duplicated across both tables.

#### Configuring TPC in EF Core

To configure TPC in EF Core, you'll need to use Fluent API in the `OnModelCreating` method.

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Participant> Participants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Individual>().ToTable("Individuals");
        modelBuilder.Entity<Corporate>().ToTable("Corporates");

        // Additional configurations
        modelBuilder.Entity<Individual>().Property(i => i.FName).HasMaxLength(50);
        modelBuilder.Entity<Corporate>().Property(c => c.Company).HasMaxLength(100);
    }
}
```

#### Migration Script

When you run `Add-Migration`, EF Core generates migration scripts that create two separate tables:

```csharp
public partial class Initial : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Individuals",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FName = table.Column<string>(maxLength: 50, nullable: true),
                LName = table.Column<string>(maxLength: 50, nullable: true),
                Gender = table.Column<string>(maxLength: 1, nullable: false),
                University = table.Column<string>(nullable: true),
                YearOfGraduation = table.Column<int>(nullable: false),
                IsIntern = table.Column<bool>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Individuals", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Corporates",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FName = table.Column<string>(maxLength: 50, nullable: true),
                LName = table.Column<string>(maxLength: 50, nullable: true),
                Gender = table.Column<string>(maxLength: 1, nullable: false),
                Company = table.Column<string>(maxLength: 100, nullable: true),
                JobTitle = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Corporates", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Individuals");
        migrationBuilder.DropTable(name: "Corporates");
    }
}
```

#### Querying Data with TPC

Querying in TPC is straightforward since each table is independent:

```csharp
using (var context = new AppDbContext())
{
    // Query Individuals
    var individuals = context.Set<Individual>().ToList();
    foreach (var individual in individuals)
    {
        Console.WriteLine($"{individual.FName} {individual.LName} - {individual.University}");
    }

    // Query Corporates
    var corporates = context.Set<Corporate>().ToList();
    foreach (var corporate in corporates)
    {
        Console.WriteLine($"{corporate.FName} {corporate.LName} - {corporate.Company} - {corporate.JobTitle}");
    }
}
```

#### Advantages and Disadvantages of TPC

**Advantages**:
- Simple querying since each table represents a single concrete type.
- No need for joins or discriminator columns.
- Schema can evolve independently for each type.

**Disadvantages**:
- Data redundancy: Base class properties are duplicated in each table.
- Schema updates require changes in multiple tables.
- Can lead to increased storage usage due to duplication.

another example of implementing the Table per Concrete Class (TPC) approach using the  classes: `Quiz`, `MultipleChoiceQuiz`, and `TrueAndFalseQuiz`. I'll cover everything from setting up the classes, configuring the database context, applying migrations, and querying the data.

### 1. Defining the Model Classes

You have an abstract base class `Quiz`, with two derived classes `MultipleChoiceQuiz` and `TrueAndFalseQuiz`. Here are the classes:

```csharp
public abstract class Quiz
{
    public int Id { get; set; }
    public string Title { get; set; }
    public Course Course { get; set; }
}

public class MultipleChoiceQuiz : Quiz
{
    public string OptionA { get; set; }
    public string OptionB { get; set; }
    public string OptionC { get; set; }
    public string OptionD { get; set; }

    public char CorrectAnswer { get; set; }
}

public class TrueAndFalseQuiz : Quiz
{
    public bool CorrectAnswer { get; set; }
}
```

### 2. Configuring the TPC Mapping Strategy

We need to configure Entity Framework Core to use the TPC (Table per Concrete Class) inheritance strategy. This is done in the `QuizConfiguration` class using the `UseTpcMappingStrategy()` method:

```csharp
public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.UseTpcMappingStrategy();
    }
}
```

### 3. Configuring the `DbContext`

Next, we configure the `DbContext` to apply the TPC strategy and the entity configurations:

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<MultipleChoiceQuiz> MultipleChoiceQuizzes { get; set; }
    public DbSet<TrueAndFalseQuiz> TrueAndFalseQuizzes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new QuizConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"YourConnectionStringHere");
    }
}
```

### 4. Creating the Database Migration

With the context and configurations in place, you can now create a migration to generate the database schema:

```bash
Add-Migration InitialTPC
Update-Database
```

### 5. Understanding the Generated Tables

When using the TPC strategy, EF Core will generate separate tables for each concrete class in the hierarchy. In this case:

1. **MultipleChoiceQuizzes**: Contains columns `Id`, `Title`, `OptionA`, `OptionB`, `OptionC`, `OptionD`, `CorrectAnswer`, and a `CourseId` (if `Course` is a related entity).
2. **TrueAndFalseQuizzes**: Contains columns `Id`, `Title`, `CorrectAnswer`, and a `CourseId`.

No table will be created for the abstract base class `Quiz`.

### 6. Querying the Data

Let's create some sample data and query it to see how the TPC strategy works in practice.

```csharp
static void Main(string[] args)
{
    using (var context = new AppDbContext())
    {
        // Seed some data
        var multipleChoiceQuiz = new MultipleChoiceQuiz
        {
            Title = "Math Quiz",
            OptionA = "2",
            OptionB = "3",
            OptionC = "4",
            OptionD = "5",
            CorrectAnswer = 'C',
            Course = new Course { Name = "Math" } // Assuming Course entity exists
        };

        var trueAndFalseQuiz = new TrueAndFalseQuiz
        {
            Title = "History Quiz",
            CorrectAnswer = true,
            Course = new Course { Name = "History" }
        };

        context.Add(multipleChoiceQuiz);
        context.Add(trueAndFalseQuiz);
        context.SaveChanges();

        // Querying MultipleChoiceQuizzes
        var mcQuizzes = context.MultipleChoiceQuizzes.ToList();
        Console.WriteLine("Multiple Choice Quizzes:");
        foreach (var quiz in mcQuizzes)
        {
            Console.WriteLine($"Title: {quiz.Title}, Correct Answer: {quiz.CorrectAnswer}");
        }

        // Querying TrueAndFalseQuizzes
        var tfQuizzes = context.TrueAndFalseQuizzes.ToList();
        Console.WriteLine("\nTrue and False Quizzes:");
        foreach (var quiz in tfQuizzes)
        {
            Console.WriteLine($"Title: {quiz.Title}, Correct Answer: {quiz.CorrectAnswer}");
        }

        // Querying All Quizzes (base type)
        var quizzes = context.Quizzes.ToList();
        Console.WriteLine("\nAll Quizzes:");
        foreach (var quiz in quizzes)
        {
            Console.WriteLine($"Title: {quiz.Title}");
        }
    }

    Console.ReadKey();
}
```

### 7. Result Explanation

When you run this program:

1. **Multiple Choice Quizzes** will be queried from the `MultipleChoiceQuizzes` table.
2. **True and False Quizzes** will be queried from the `TrueAndFalseQuizzes` table.
3. **All Quizzes** (base type) will include quizzes from both tables, as EF Core automatically handles the mapping.

### 8. Key Points about TPC

- **Separation of Data**: Each concrete class has its own table. No shared columns with other classes.
- **No Base Class Table**: The base class (`Quiz`) does not have its own table.
- **Performance Considerations**: TPC can result in faster queries because there are no joins between base and derived tables. However, the schema can become redundant and difficult to manage with many derived types.

### 9. Advantages and Disadvantages

**Advantages:**
- **Simplicity**: Each derived class has its own table, which can make understanding the database schema straightforward.
- **Performance**: Queries are generally faster as there are no joins required.

**Disadvantages:**
- **Redundancy**: Common columns (e.g., `Title`) are repeated across tables, leading to potential redundancy.
- **Schema Management**: The database schema can become cumbersome if there are many derived types.

This example and explanation should give you a comprehensive understanding of how to implement and work with the Table per Concrete Class (TPC) inheritance strategy in Entity Framework Core.


### Conclusion

The Table per Concrete Type (TPC) strategy in Entity Framework Core is a good fit when you want to have a clear separation between different concrete types and prefer to avoid complex joins. However, this comes at the cost of data redundancy and potentially more complex schema management. Understanding when and how to use TPC, TPH, or TPT depends on the specific needs of your application.
