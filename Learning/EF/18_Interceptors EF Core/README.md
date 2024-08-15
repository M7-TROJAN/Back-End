# Interceptors in EF Core (ادوات الأعتراض)

## Overview

Interceptors in Entity Framework Core (EF Core) are a powerful feature that allows developers to intercept and modify the execution of operations on the database. This can be particularly useful for scenarios such as logging, modifying queries, or implementing specific business logic like soft deletion. Before diving into interceptors, it's essential to understand the concepts of hard delete and soft delete.

## Hard Delete vs. Soft Delete

### Hard Delete

A **hard delete** refers to the permanent removal of a record from the database. Once a record is hard deleted, it is no longer accessible and cannot be recovered unless there is a backup.

#### Example: Hard Delete

##### Sample table

```csharp
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
}
```

Let's demonstrate a hard delete using the provided `Book` table.

```csharp
Console.WriteLine("Before Delete");
DatabaseHelper.ShowBooks(); // method to get Books from database and then print them 

using (var context = new AppDbContext())
{
    var book = context.Books.First();
    context.Books.Remove(book);
    context.SaveChanges();
}

Console.WriteLine();
Console.WriteLine("After Delete Book Id = '1'");
DatabaseHelper.ShowBooks(); // method to get Books from database and then print them 
```

#### Output:

```
Before Delete

Books
-----
Id: 1, Title: Domain-Driv..., Author:           Eric Evans
Id: 2, Title: Domain-Driv..., Author:           Eric Evans
Id: 3, Title: C# In Depth..., Author:           John Skeet
Id: 4, Title: Real world ..., Author:           John Skeet
Id: 5, Title: Grokking Al..., Author:      Aditya Bhargava

After Delete Book Id = '1'

Books
-----
Id: 2, Title: Domain-Driv..., Author:           Eric Evans
Id: 3, Title: C# In Depth..., Author:           John Skeet
Id: 4, Title: Real world ..., Author:           John Skeet
Id: 5, Title: Grokking Al..., Author:      Aditya Bhargava
```

#### Explanation:

In a hard delete, the record with `Id = 1` has been permanently removed from the database. This means the record no longer exists, and any references to it will result in errors if not handled properly.

### Soft Delete

A **soft delete** is a technique where a record is marked as deleted instead of being physically removed from the database. This is typically done by adding a boolean flag or other marker to indicate that the record is deleted. The record remains in the database, allowing it to be restored or excluded from queries when necessary.

#### Implementing Soft Delete

To implement soft delete, we need to modify our `Book` entity to include a column that indicates whether a record is deleted.

#### Modified `Book` Entity

```csharp
public interface ISoftDeleteable
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }

    void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.Now;
    }

    void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
    }
}

public class Book : ISoftDeleteable
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public bool IsDeleted { get ; set; }
    public DateTime? DeletedAt { get; set; }
}
```

#### Configuration for Soft Delete

To support the soft delete mechanism, the entity configuration needs to be updated.

```csharp
public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).ValueGeneratedOnAdd(); // Auto-increment

        builder.Property(b => b.Title)
            .HasColumnName("BookTitle") // Column name in the database
            .HasColumnType("VARCHAR") // Data type in the database
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Author)
            .HasColumnType("VARCHAR") // Data type in the database
            .HasColumnName("BookAuthor") // Column name in the database
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.IsDeleted)
            .HasColumnType("BIT")
            .HasDefaultValue(false);

        builder.Property(b => b.DeletedAt)
            .HasColumnType("DATETIME")
            .HasDefaultValue(null);

        builder.ToTable("Books"); // Table name in the database
    }
}
```

#### Explanation:

In this setup:
- `IsDeleted` is a boolean flag that indicates whether the record is considered deleted.
- `DeletedAt` stores the timestamp of when the record was marked as deleted.

### Implementing Interceptors for Soft Delete

Now that we have the infrastructure for soft delete, we can use an EF Core interceptor to automatically change the behavior of the `SaveChanges` method. When a delete operation is detected, the interceptor will mark the record as deleted instead of physically removing it.

#### Soft Delete Interceptor

```csharp
public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null)
            return result;

        foreach (var entry in eventData.Context.ChangeTracker.Entries())
        {
            // if (entry is null || entry.State != EntityState.Deleted || !(entry.Entity is ISoftDeleteable entity))
            if (entry is not { State: EntityState.Deleted, Entity: ISoftDeleteable entity })
                continue;

            entry.State = EntityState.Modified;
            entity.Delete();
        }

        return result;
    }
}
```

#### Explanation:

- **SoftDeleteInterceptor**: This interceptor overrides the `SavingChanges` method to intercept delete operations. If a delete operation is detected and the entity implements the `ISoftDeleteable` interface, the operation is converted into an update operation that marks the entity as deleted.

### Registering the Interceptor in DbContext

The interceptor must be registered in the `DbContext` to take effect.

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("connectionString")
            .AddInterceptors(new SoftDeleteInterceptor());
    }
}
```

### Example: Soft Delete in Action

Let's see how the soft delete works in practice.

```csharp
public static void Main(string[] args)
{
    DatabaseHelper.RecreateCleanDatabase();
    DatabaseHelper.PopulateDatabase();

    Console.WriteLine();
    Console.WriteLine("Before Delete");

    DatabaseHelper.ShowBooks();

    using (var context = new AppDbContext())
    {
        var book = context.Books.First();
        context.Books.Remove(book);
        context.SaveChanges();
    }
    Console.WriteLine();
    Console.WriteLine("After Delete Book Id = '1'");

    DatabaseHelper.ShowBooks();

    Console.ReadKey();
}
```

#### Output:

```
Before Delete

Books
-----
Id: 1, Title: Domain-Driv..., Author:           Eric Evans, IsDeleted: False
Id: 2, Title: Domain-Driv..., Author:           Eric Evans, IsDeleted: False
Id: 3, Title: C# In Depth..., Author:           John Skeet, IsDeleted: False
Id: 4, Title: Real world ..., Author:           John Skeet, IsDeleted: False
Id: 5, Title: Grokking Al..., Author:      Aditya Bhargava, IsDeleted: False

After Delete Book Id = '1'

Books
-----
Id: 1, Title: Domain-Driv..., Author:           Eric Evans, IsDeleted: True
Id: 2, Title: Domain-Driv..., Author:           Eric Evans, IsDeleted: False
Id: 3, Title: C# In Depth..., Author:           John Skeet, IsDeleted: False
Id: 4, Title: Real world ..., Author:           John Skeet, IsDeleted: False
Id: 5, Title: Grokking Al..., Author:      Aditya Bhargava, IsDeleted: False
```

#### Explanation:

As seen in the output, the first record is not physically deleted from the database but is marked as `IsDeleted = True`. This allows the record to be filtered out in queries or restored if needed.

### Querying Soft Deleted Records

To ensure that soft deleted records are not included in regular queries, a query filter can be applied.

```csharp
public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasQueryFilter(b => !b.IsDeleted);
        
        // Other configurations...
    }
}
```

#### Explanation:

The `HasQueryFilter` method automatically applies a filter to all queries, ensuring that only records where `IsDeleted = false` are returned.

## Summary

- **Hard Delete**: Permanently removes records from the database.
- **Soft Delete**: Marks records as deleted without physically removing them.
- **Interceptors**: EF Core feature that allows you to intercept and modify database operations. In this example, we used an interceptor to implement soft delete functionality.

By using interceptors, you can easily implement advanced behaviors like soft delete, logging, or custom validations, making your application more flexible and maintainable.


Now Let's break down the `SoftDeleteInterceptor` code step by step to understand how it works in EF Core.

### Overview of Interceptors

Interceptors in EF Core allow you to intercept operations being performed by EF Core and modify them. In this case, we're intercepting the `SaveChanges` method to implement a soft delete mechanism. This interceptor listens for delete operations, and instead of allowing the entity to be deleted from the database, it modifies the entity's state to indicate it has been soft deleted.

### `SoftDeleteInterceptor` Class

```csharp
public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null)
            return result;

        foreach (var entry in eventData.Context.ChangeTracker.Entries())
        {
            // if (entry is null || entry.State != EntityState.Deleted || !(entry.Entity is ISoftDeleteable entity))
            if (entry is not { State: EntityState.Deleted, Entity: ISoftDeleteable entity })
                continue;

            entry.State = EntityState.Modified;
            entity.Delete();
        }

        return result;
    }
}
```

### Breakdown of the Code

#### 1. Class Definition: `SoftDeleteInterceptor`

```csharp
public class SoftDeleteInterceptor : SaveChangesInterceptor
```

- **Inheritance**: The `SoftDeleteInterceptor` class inherits from the `SaveChangesInterceptor` class provided by EF Core.
- **Purpose**: By inheriting from `SaveChangesInterceptor`, this class can override methods related to the saving process, specifically targeting changes in entities before they are persisted to the database.

#### 2. Method Override: `SavingChanges`

```csharp
public override InterceptionResult<int> SavingChanges(
    DbContextEventData eventData, InterceptionResult<int> result)
```

- **Method**: `SavingChanges` is a method that you override to intercept the process before the changes are saved to the database.
- **Parameters**:
  - `DbContextEventData eventData`: Provides data about the context and the operation being performed. It includes the `DbContext` instance, allowing access to the tracked entities.
  - `InterceptionResult<int> result`: Represents the result of the `SaveChanges` method. It allows you to control whether the save operation should proceed, be canceled, or be altered.
- **Return Value**: The method returns an `InterceptionResult<int>`, which indicates whether the operation should continue as usual, be modified, or be stopped.

#### 3. Null Check for `Context`

```csharp
if (eventData.Context is null)
    return result;
```

- **Purpose**: This line checks if the `Context` property in `eventData` is `null`.
- **Reason**: If the context is `null`, it means that there is no active `DbContext` associated with the current operation, so the method simply returns the original `result`, allowing the operation to continue without any changes.

#### 4. Iterating Through Tracked Entities

```csharp
foreach (var entry in eventData.Context.ChangeTracker.Entries())
```

- **Purpose**: This loop iterates through all the entities currently being tracked by the `DbContext`. The `ChangeTracker` keeps track of changes made to entities and their states (e.g., Added, Modified, Deleted, etc.).
- **Why**: The loop allows the interceptor to inspect each entity that is about to be persisted to the database, so it can detect delete operations.

#### 5. Conditional Check for Deletion and Interface Implementation

```csharp
// if (entry is null || entry.State != EntityState.Deleted || !(entry.Entity is ISoftDeleteable entity))
if (entry is not { State: EntityState.Deleted, Entity: ISoftDeleteable entity })
    continue;
```

- **Old Conditional**:
  ```csharp
  if (entry is null || entry.State != EntityState.Deleted || !(entry.Entity is ISoftDeleteable entity))
  ```
- **Updated Conditional**:
  ```csharp
  if (entry is not { State: EntityState.Deleted, Entity: ISoftDeleteable entity })
      continue;
  ```
- **Purpose**:
  - The condition checks if:
    - The `entry` is not `null`.
    - The `entry`'s state is `EntityState.Deleted`, meaning it's marked for deletion.
    - The `entry.Entity` implements the `ISoftDeleteable` interface, which indicates that the entity supports soft deletion.
  - If any of these conditions fail, the `continue` statement skips to the next entity in the loop.
- **Why**: This ensures that the soft delete logic only applies to entities that are being deleted and that implement the `ISoftDeleteable` interface.

#### 6. Modifying the Entity State to `Modified`

```csharp
entry.State = EntityState.Modified;
```

- **Purpose**: Changes the state of the entity from `Deleted` to `Modified`.
- **Why**: By setting the state to `Modified`, EF Core will treat the entity as being updated rather than deleted. This allows the entity to be updated with the soft delete flag instead of being removed from the database.

#### 7. Calling the `Delete` Method on the Entity

```csharp
entity.Delete();
```

- **Purpose**: Calls the `Delete` method on the entity, which is defined by the `ISoftDeleteable` interface.
- **Why**: The `Delete` method marks the entity as deleted by setting the `IsDeleted` flag to `true` and optionally setting the `DeletedAt` timestamp. This effectively "soft deletes" the entity.

#### 8. Returning the Result

```csharp
return result;
```

- **Purpose**: The method returns the `result`, allowing the save operation to continue as usual.
- **Why**: After modifying the necessary entities, the interceptor doesn't need to stop or alter the save operation further, so it returns the original result.

### Summary

- **`SoftDeleteInterceptor`** is designed to intercept `SaveChanges` operations in EF Core.
- **Purpose**: Automatically convert delete operations into soft delete operations by modifying the entity's state and setting a deletion flag.
- **Key Steps**:
  - Check each tracked entity to see if it's marked for deletion and supports soft deletion.
  - If so, change its state to `Modified` and mark it as deleted using the soft delete flag.
- **Outcome**: The entity remains in the database but is marked as deleted, allowing it to be excluded from queries without being physically removed.
