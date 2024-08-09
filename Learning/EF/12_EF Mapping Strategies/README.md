# Inheritance Mapping in Entity Framework Core

Inheritance mapping in Entity Framework Core allows you to map .NET inheritance hierarchies to relational databases. There are three main strategies for inheritance mapping:

1. **Table Per Hierarchy (TPH)**
2. **Table Per Type (TPT)**
3. **Table Per Concrete Type (TPC)**

## 1. Table Per Hierarchy (TPH)

### Overview
- **TPH** maps all entities in an inheritance hierarchy to a single database table.
- A discriminator column is used to distinguish between different entity types.
- This approach is simple and efficient in terms of database performance, as it involves fewer joins.

### Example

Consider the following class hierarchy:

```csharp
public class Participant
{
    public int Id { get; set; }
    public string? FName { get; set; }
    public string? LName { get; set; }
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

### Configuration

In the `DbContext`, you can configure TPH using the `OnModelCreating` method:

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
            .IsRequired();
    }
}
```

### Database Structure

In the database, there will be a single table called `Participants`:

| Id  | FName   | LName  | Gender | ParticipantType | University | YearOfGraduation | IsIntern | Company   | JobTitle         |
| --- | ------- | ------ | ------ | --------------- | ---------- | ---------------- | -------- | --------- | ---------------- |
| 1   | Mahmoud | Mattar | m      | INDV            | MIT        | 2020             | true     | NULL      | NULL             |
| 2   | Asmaa   | Adel   | f      | COPR            | NULL       | NULL             | NULL     | Microsoft | Software Engineer|

### Querying

When querying the `Participants` table, Entity Framework Core will use the discriminator column to retrieve the correct entity type.

```csharp
using (var context = new AppDbContext())
{
    var participants = context.Participants.ToList(); // this will retrive just the base type properties (id, FName, LName, Gender)
    var individuals = context.Participants.OfType<Individual>().ToList(); // this will retrive the base type properties and the derived type properties (University, YearOfGraduation, IsIntern)
    var corporates = context.Participants.OfType<Corporate>().ToList(); // this will retrive the base type properties and the derived type properties (Company, JobTitle)
}
```

## 2. Table Per Type (TPT)

### Overview
- **TPT** maps each entity in the inheritance hierarchy to its own table.
- The base type's properties are stored in one table, and the derived type's properties are stored in separate tables.
- This approach requires more joins, which might impact performance, but it ensures a normalized database structure.

### Example

Given the same class hierarchy as above:

```csharp
public class Participant
{
    public int Id { get; set; }
    public string? FName { get; set; }
    public string? LName { get; set; }
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

### Configuration

In the `DbContext`, TPT can be configured as follows:

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Participant> Participants { get; set; }
    public DbSet<Individual> Individuals { get; set; }
    public DbSet<Corporate> Corporates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Individual>().ToTable("Individuals");
        modelBuilder.Entity<Corporate>().ToTable("Corporates");
    }
}
```

### Database Structure

In the database, there will be three tables: `Participants`, `Individuals`, and `Corporates`.

- **Participants** table:

| Id  | FName   | LName  | Gender |
| --- | ------- | ------ | ------ |
| 1   | Mahmoud | Mattar | m      |
| 2   | Asmaa   | Adel   | f      |

- **Individuals** table:

| Id  | University | YearOfGraduation | IsIntern |
| --- | ---------- | ---------------- | -------- |
| 1   | MIT        | 2020             | true     |

- **Corporates** table:

| Id  | Company   | JobTitle         |
| --- | --------- | ---------------- |
| 2   | Microsoft | Software Engineer|

### Querying

Entity Framework Core will automatically join the necessary tables when querying for derived types:

```csharp
using (var context = new AppDbContext())
{
    var individuals = context.Individuals.ToList(); // Joins Individuals and Participants tables
    var corporates = context.Corporates.ToList(); // Joins Corporates and Participants tables
}
```

## 3. Table Per Concrete Type (TPC)

### Overview
- **TPC** maps each entity type, including base and derived types, to its own table.
- Each table contains all the properties of the entity, including those inherited from the base class.
- This approach avoids joins entirely but introduces redundancy, as the base type's properties are repeated in each derived type's table.

### Example

Using the same class hierarchy:

```csharp
public class Participant
{
    public int Id { get; set; }
    public string? FName { get; set; }
    public string? LName { get; set; }
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

### Configuration

To configure TPC in Entity Framework Core:

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Individual> Individuals { get; set; }
    public DbSet<Corporate> Corporates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Individual>().ToTable("Individuals")
            .HasNoDiscriminator()
            .HasBaseType((Type)null); // Indicating no base type

        modelBuilder.Entity<Corporate>().ToTable("Corporates")
            .HasNoDiscriminator()
            .HasBaseType((Type)null); // Indicating no base type
    }
}
```

### Database Structure

In the database, there will be two separate tables:

- **Individuals** table:

| Id  | FName   | LName  | Gender | University | YearOfGraduation | IsIntern |
| --- | ------- | ------ | ------ | ---------- | ---------------- | -------- |
| 1   | Mahmoud | Mattar | m      | MIT        | 2020             | true     |

- **Corporates** table:

| Id  | FName  | LName | Gender | Company   | JobTitle         |
| --- | ------ | ----- | ------ | --------- | ---------------- |
| 2   | Asmaa  | Adel  | f      | Microsoft | Software Engineer|

### Querying

Each entity type is mapped to its own table, so querying is straightforward and doesn't involve joins:

```csharp
using (var context = new AppDbContext())
{
    var individuals = context.Individuals.ToList(); // Retrieves data from Individuals table
    var corporates = context.Corporates.ToList(); // Retrieves data from Corporates table
}
```

---

## Summary

- **Table Per Hierarchy (TPH):** One table for the entire hierarchy, uses a discriminator column.
- **Table Per Type (TPT):** One table for each entity type, base properties in a separate table.
- **Table Per Concrete Type (TPC):** Each entity type has its own table with all properties, including base properties.

Each approach has its own advantages and trade-offs, depending on your application's performance and data structure requirements.
