In Entity Framework Core, value conversion is a way to transform property values when reading from and writing to the database. This is particularly useful when the property types in your domain model don't directly map to database column types. 

### Value Conversion Example

Consider your `Schedule` class and the `ScheduleEnum` enum. You want to store the enum value in the database as a string but use it in your application as an enum type. Here's how you can achieve this using value conversion.

### Initial Setup

You have an `enum` defined as:

```csharp
public enum ScheduleEnum
{
    Daily = 1,
    DayAfterDay,
    TwiceAWeek,
    Weekend,
    Compact,
}
```

And your `Schedule` class uses this enum:

```csharp
public class Schedule
{
    public int Id { get; set; }
    public ScheduleEnum Title { get; set; }
    public bool SUN { get; set; }
    public bool MON { get; set; }
    public bool TUE { get; set; }
    public bool WED { get; set; }
    public bool THU { get; set; }
    public bool FRI { get; set; }
    public bool SAT { get; set; }
    public ICollection<Section> Sections { get; set; } = new List<Section>();
}
```

### Applying Value Conversion in `OnModelCreating`

In the `OnModelCreating` method of your `DbContext`, you can configure the conversion like this:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Schedule>(builder =>
    {
        builder.Property(s => s.Title)
            .HasConversion(
                v => v.ToString(), // Converts enum to string when saving to the database
                v => (ScheduleEnum)Enum.Parse(typeof(ScheduleEnum), v) // Converts string to enum when reading from the database
            );

        // Other configurations...
    });
}
```

### Explanation

- `v => v.ToString()`: When saving the `Schedule` entity to the database, this converts the `ScheduleEnum` value to its string representation.
- `v => (ScheduleEnum)Enum.Parse(typeof(ScheduleEnum), v)`: When reading the `Schedule` entity from the database, this converts the string back to the `ScheduleEnum` value.

### Why Use Value Conversion?

1. **Database Compatibility**: Sometimes, the type you want to use in your code (like `enum`) isn't directly supported by your database provider, or you might prefer a different representation (like `string` or `int`) in the database.
2. **Readability**: Storing enums as strings can make the database entries more readable.
3. **Interoperability**: It makes it easier to work with legacy systems or other parts of your application that expect a different type.

### Full Example

Here's how you can incorporate everything together:

#### `Schedule.cs`

```csharp
public class Schedule
{
    public int Id { get; set; }
    public ScheduleEnum Title { get; set; }
    public bool SUN { get; set; }
    public bool MON { get; set; }
    public bool TUE { get; set; }
    public bool WED { get; set; }
    public bool THU { get; set; }
    public bool FRI { get; set; }
    public bool SAT { get; set; }
    public ICollection<Section> Sections { get; set; } = new List<Section>();
}
```

#### `ScheduleEnum.cs`

```csharp
public enum ScheduleEnum
{
    Daily = 1,
    DayAfterDay,
    TwiceAWeek,
    Weekend,
    Compact,
}
```

#### `ApplicationDbContext.cs`

```csharp
public class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("YourConnectionString");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Schedule>(builder =>
        {
            builder.Property(s => s.Title)
                .HasConversion(
                    v => v.ToString(), // Converts enum to string when saving to the database
                    v => (ScheduleEnum)Enum.Parse(typeof(ScheduleEnum), v) // Converts string to enum when reading from the database
                );

            // Other configurations...
        });
    }
}
```

With this setup, your `Schedule` entity can seamlessly use the `ScheduleEnum` type in your application, while storing the enum values as strings in the database.