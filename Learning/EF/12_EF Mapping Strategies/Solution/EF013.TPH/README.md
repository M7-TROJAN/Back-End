Table per Hierarchy (TPH) inheritance in Entity Framework Core.

### Overview of Inheritance in Entity Framework Core

Inheritance is a fundamental concept in object-oriented programming that allows classes to inherit properties and methods from other classes. Entity Framework Core (EF Core) supports three types of inheritance mapping:

1. **Table per Hierarchy (TPH)**: A single table is used to store data for all the classes in an inheritance hierarchy. A discriminator column is used to identify the type of each record.
2. **Table per Type (TPT)**: Each class in the hierarchy has its own table, and the tables are related by foreign keys.
3. **Table per Concrete Class (TPC)**: Each class in the hierarchy has its own table, but there are no relationships between the tables.

In Our case, WE're dealing with TPH inheritance.

### Table per Hierarchy (TPH)

TPH is the most commonly used inheritance strategy in EF Core. With TPH, EF Core creates a single table to store data for all classes in an inheritance hierarchy. A discriminator column is used to distinguish between different types.

#### Example Code

Here’s the code you provided, which we'll use to understand the TPH concept:

```csharp
public class Participant
{
    public int Id { get; set; }
    public string? FName { get; set; }
    public string? LName { get; set; }
    [RegularExpression("^[fm]$", ErrorMessage = "Gender must be 'f' or 'm'")]
    public char Gender { get; set; }
    public ICollection<Section> Sections { get; set; } = new List<Section>();
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

In this example, `Individual` and `Corporate` inherit from `Participant`.

### Understanding the Migration

When you add a migration using `Add-Migration Initial`, EF Core creates a single table named `Participants` to store data for `Participant`, `Individual`, and `Corporate`. Here’s the generated table:

```csharp
migrationBuilder.CreateTable(
    name: "Participants",
    columns: table => new
    {
        Id = table.Column<int>(type: "int", nullable: false),
        FName = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
        LName = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
        Gender = table.Column<string>(type: "CHAR(1)", maxLength: 1, nullable: false),
        Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
        Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
        JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
        University = table.Column<string>(type: "nvarchar(max)", nullable: true),
        YearOfGraduation = table.Column<int>(type: "int", nullable: true),
        IsIntern = table.Column<bool>(type: "bit", nullable: true)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_Participants", x => x.Id);
        table.CheckConstraint("chk_Gender", "Gender IN ('f', 'm')");
    });
```

### Discriminator Column

The `Discriminator` column is crucial in TPH inheritance. It helps EF Core differentiate between the different types (`Participant`, `Individual`, and `Corporate`). This column stores a string value that indicates the actual type of the record, such as "Individual" or "Corporate".

### Configuration of Discriminator in EF Core

You can explicitly configure the `Discriminator` column and its values using the `HasDiscriminator` method in the `OnModelCreating` method:

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Participant> Participants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Participant>()
            .HasDiscriminator<string>("ParticipantType")
            .HasValue<Individual>("INDV")
            .HasValue<Corporate>("COPR");

        modelBuilder.Entity<Participant>()
            .Property("ParticipantType")
            .HasColumnType("VARCHAR(4)")
            .HasMaxLength(4)
            .IsRequired();
    }
}
```

In this configuration:

- The discriminator column is named `ParticipantType`.
- `Individual` is represented by the value "INDV".
- `Corporate` is represented by the value "COPR".
- The column has a maximum length of 4 and is required.

### Querying Data

When you query the `Participants` DbSet, EF Core will only return the properties defined in the `Participant` class by default:

```csharp
var participants = context.Participants.ToList();
```

However, if you want to retrieve the properties of a derived type (`Individual` or `Corporate`), you can use the `OfType<>` method:

```csharp
var individuals = context.Participants.OfType<Individual>().ToList();
var corporates = context.Participants.OfType<Corporate>().ToList();
```

### Summary

- **Table per Hierarchy (TPH)**: A single table is used for the entire inheritance hierarchy.
- **Discriminator Column**: Used to distinguish between different derived types in the hierarchy.
- **EF Core Configuration**: You can configure the discriminator column using `HasDiscriminator` and set specific values for each type.
- **Querying**: Use `OfType<>` to filter and retrieve specific derived types.

### MD File Summary


```markdown
# Table per Hierarchy (TPH) in EF Core

## Overview
- **TPH** is an inheritance strategy where a single table is used to store data for all classes in an inheritance hierarchy.
- A **discriminator column** is used to identify the type of each record.

## Example

### Entity Classes

```csharp
public class Participant { /* ... */ }
public class Individual : Participant { /* ... */ }
public class Corporate : Participant { /* ... */ }
```

### Generated Table

```sql
CREATE TABLE Participants (
    Id INT NOT NULL,
    FName VARCHAR(50) NOT NULL,
    LName VARCHAR(50) NOT NULL,
    Gender CHAR(1) NOT NULL,
    Discriminator NVARCHAR(13) NOT NULL,
    /* Derived Type Properties */
    Company NVARCHAR(MAX),
    JobTitle NVARCHAR(MAX),
    University NVARCHAR(MAX),
    YearOfGraduation INT,
    IsIntern BIT
);
```

### Discriminator Column
- **Discriminator** column helps to differentiate between different types in the hierarchy.

### Configuration in EF Core

```csharp
modelBuilder.Entity<Participant>()
    .HasDiscriminator<string>("ParticipantType")
    .HasValue<Individual>("INDV")
    .HasValue<Corporate>("COPR");
```

### Querying Data

- **Participants**: `context.Participants.ToList();`
- **Individuals**: `context.Participants.OfType<Individual>().ToList();`
- **Corporates**: `context.Participants.OfType<Corporate>().ToList();`

## Summary
- **TPH** is a powerful way to handle inheritance in EF Core.
- It simplifies the schema but can lead to sparsely populated tables.
- Use the **OfType<>** method to filter for specific derived types.

```