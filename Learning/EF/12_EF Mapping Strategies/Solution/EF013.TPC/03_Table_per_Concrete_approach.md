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

### Conclusion

The Table per Concrete Type (TPC) strategy in Entity Framework Core is a good fit when you want to have a clear separation between different concrete types and prefer to avoid complex joins. However, this comes at the cost of data redundancy and potentially more complex schema management. Understanding when and how to use TPC, TPH, or TPT depends on the specific needs of your application.