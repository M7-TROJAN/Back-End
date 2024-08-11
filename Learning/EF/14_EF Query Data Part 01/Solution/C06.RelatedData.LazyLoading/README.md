### Lazy Loading in Entity Framework Core

Lazy Loading is a design pattern used to defer the loading of related data until it is specifically requested. In EF Core, Lazy Loading allows navigation properties to be loaded from the database only when they are accessed for the first time.

#### Key Points:
1. **Lazy Loading**:
   - It delays the loading of related data until you try to access it.
   - It can be particularly useful when dealing with large amounts of related data, as it avoids loading all the related data at once.
   - It can be implemented using proxy classes that EF Core automatically generates and injects at runtime.

2. **Packages for Proxies**:
   - To enable Lazy Loading, you need to install the `Microsoft.EntityFrameworkCore.Proxies` package.

#### Installing the Required Package

You can install the required package using either the Package Manager Console (PMC) or the .NET CLI.

- **Using Package Manager Console**:
   ```powershell
   Install-Package Microsoft.EntityFrameworkCore.Proxies
   ```

- **Using .NET CLI**:
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.Proxies
   ```

#### Configuring Lazy Loading in `OnConfiguring`

Once the package is installed, you need to configure your DbContext to use lazy loading proxies.

Here's how you can do that:

```csharp
public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "YourConnectionStringHere";

        optionsBuilder
            .UseSqlServer(connectionString)
            .UseLazyLoadingProxies(); // Enable Lazy Loading Proxies
    }

    // DbSets and other configurations
}
```

#### Examples

Let's consider the following model:

```csharp
public class Section : Entity
{
    public string? SectionName { get; set; }
    public int CourseId { get; set; }
    public virtual Course? Course { get; set; } // virtual to enable lazy loading
    public int InstructorId { get; set; }
    public virtual Instructor? Instructor { get; set; } // virtual to enable lazy loading
    public int ScheduleId { get; set; }
    public virtual Schedule? Schedule { get; set; } // virtual to enable lazy loading
    public DateRange DateRange { get; set; } = new();
    public TimeSlot TimeSlot { get; set; } = new();
    public virtual List<Participant> Participants { get; set; } = new List<Participant>(); // virtual to enable lazy loading
}
```

1. **Lazy Loading a Single Navigation Property**:

   ```csharp
   using (var context = new AppDbContext())
   {
       var sectionId = 1;

       var section = context.Sections.FirstOrDefault(x => x.Id == sectionId);

       // Course is lazily loaded here
       Console.WriteLine($"Course: {section.Course?.CourseName}");
   }
   ```

   In this example, the `Course` property is not loaded from the database until it is accessed with `section.Course?.CourseName`.

2. **Lazy Loading a Collection Navigation Property**:

   ```csharp
   using (var context = new AppDbContext())
   {
       var sectionId = 1;

       var section = context.Sections.FirstOrDefault(x => x.Id == sectionId);

       // Participants are lazily loaded here
       foreach (var participant in section.Participants)
       {
           Console.WriteLine($"{participant.FName} {participant.LName}");
       }
   }
   ```

   In this case, the `Participants` collection is not loaded until it is accessed in the `foreach` loop.

#### Pros and Cons of Lazy Loading (إيجابيات وسلبيات التحميل البطيء)

- **Pros**:
  - Reduces initial load time by loading only the required data.
  - Simplifies the code by automatically loading related data when needed.

- **Cons**:
  - Can lead to the N+1 query problem, where multiple small queries are made to the database, resulting in performance issues.
  - May cause unexpected behavior if not carefully managed, as related data is loaded behind the scenes.