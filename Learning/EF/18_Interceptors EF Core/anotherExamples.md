Interceptors in EF Core are powerful tools that allow you to customize the behavior of your database operations. Besides soft deletes, they can be used in various scenarios to enforce rules, log operations, modify data, or handle other custom behaviors.

Here are a few examples of different scenarios where you might implement an interceptor:

### 1. **Audit Logging Interceptor**
   - **Scenario**: Automatically log the creation and modification of entities, including the date and user who performed the operation.
   - **Purpose**: Track changes to data for auditing purposes.

#### Implementation

```csharp
public interface IAuditable
{
    DateTime CreatedAt { get; set; }
    string CreatedBy { get; set; }
    DateTime? UpdatedAt { get; set; }
    string UpdatedBy { get; set; }
}

public class Book : IAuditable
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
}

public class AuditLoggingInterceptor : SaveChangesInterceptor
{
    private readonly string _currentUser;

    public AuditLoggingInterceptor(string currentUser)
    {
        _currentUser = currentUser;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context == null) return result;

        var now = DateTime.UtcNow;

        foreach (var entry in eventData.Context.ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedBy = _currentUser;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy = _currentUser;
            }
        }

        return base.SavingChanges(eventData, result);
    }
}
```

#### Usage

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("connectionString")
            .AddInterceptors(new AuditLoggingInterceptor("current_user"));
    }
}
```

### 2. **Query Performance Logging Interceptor**
   - **Scenario**: Log the execution time of each query to monitor and optimize performance.
   - **Purpose**: Identify slow queries and track database performance over time.

#### Implementation

```csharp
public class QueryPerformanceInterceptor : DbCommandInterceptor
{
    public override async Task<DbCommand> ReaderExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        DbCommandInterceptorResult result)
    {
        var stopwatch = Stopwatch.StartNew();
        var resultCommand = await base.ReaderExecutedAsync(command, eventData, result);
        stopwatch.Stop();

        Console.WriteLine($"Query took {stopwatch.ElapsedMilliseconds} ms: {command.CommandText}");

        return resultCommand;
    }
}
```

#### Usage

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("connectionString")
            .AddInterceptors(new QueryPerformanceInterceptor());
    }
}
```

### 3. **Global String Trimming Interceptor**
   - **Scenario**: Ensure all string properties are trimmed of leading and trailing spaces before saving to the database.
   - **Purpose**: Prevent accidental whitespace from being saved in string fields, which can cause issues in data integrity.

#### Implementation

```csharp
public class StringTrimmingInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context == null) return result;

        foreach (var entry in eventData.Context.ChangeTracker.Entries())
        {
            foreach (var property in entry.Properties)
            {
                if (property.CurrentValue is string strValue)
                {
                    property.CurrentValue = strValue.Trim();
                }
            }
        }

        return base.SavingChanges(eventData, result);
    }
}
```

#### Usage

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("connectionString")
            .AddInterceptors(new StringTrimmingInterceptor());
    }
}
```

### 4. **Prevent Unauthorized Deletions Interceptor**
   - **Scenario**: Prevent deletions of specific records by unauthorized users.
   - **Purpose**: Enforce security rules at the database level.

#### Implementation

```csharp
public class PreventDeletionInterceptor : SaveChangesInterceptor
{
    private readonly string _currentUser;

    public PreventDeletionInterceptor(string currentUser)
    {
        _currentUser = currentUser;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context == null) return result;

        foreach (var entry in eventData.Context.ChangeTracker.Entries<Book>())
        {
            if (entry.State == EntityState.Deleted)
            {
                if (_currentUser != "admin")
                {
                    throw new UnauthorizedAccessException("You are not authorized to delete this record.");
                }
            }
        }

        return base.SavingChanges(eventData, result);
    }
}
```

#### Usage

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("connectionString")
            .AddInterceptors(new PreventDeletionInterceptor("current_user"));
    }
}
```

### 5. **Dynamic Tenant Filtering Interceptor**
   - **Scenario**: Implement multi-tenancy by automatically filtering data based on the tenant ID.
   - **Purpose**: Ensure that users only see data relevant to their tenant.

#### Implementation

```csharp
public interface ITenantEntity
{
    int TenantId { get; set; }
}

public class Book : ITenantEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int TenantId { get; set; }
}

public class TenantFilterInterceptor : SaveChangesInterceptor
{
    private readonly int _tenantId;

    public TenantFilterInterceptor(int tenantId)
    {
        _tenantId = tenantId;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context == null) return result;

        foreach (var entry in eventData.Context.ChangeTracker.Entries<ITenantEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.TenantId = _tenantId;
            }
        }

        return base.SavingChanges(eventData, result);
    }
}
```

#### Usage

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("connectionString")
            .AddInterceptors(new TenantFilterInterceptor(tenantId: 1));
    }
}
```

### Summary

Interceptors in EF Core provide a way to implement custom logic during database operations, whether it's auditing changes, enforcing security, optimizing queries, or applying business rules like multi-tenancy. The flexibility of interceptors allows you to tailor the behavior of your application to meet specific needs, ensuring better data integrity, security, and performance.
