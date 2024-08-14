### Change Tracking in EF Core

**Change Tracking** is a key concept in Entity Framework Core (EF Core) that enables the framework to keep track of changes made to entities while they are loaded into the context. This tracking allows EF Core to determine what changes need to be persisted to the database when `SaveChanges()` is called. Understanding how change tracking works will help you manage entities effectively, especially when dealing with disconnected scenarios or optimizing performance.

### Key Concepts and Properties

1. **Change Tracker:**
   - The `ChangeTracker` is a property of the `DbContext` that tracks the state of all entities loaded into the context.
   - It allows EF Core to know which entities have been added, modified, or deleted so that it can generate the appropriate SQL commands when `SaveChanges()` is called.

2. **Entity States:**
   - **Added**: The entity is new and will be inserted into the database.
   - **Modified**: The entity exists in the database and some of its properties have been changed.
   - **Deleted**: The entity exists in the database but will be removed.
   - **Unchanged**: The entity exists in the database and has not been modified.
   - **Detached**: The entity is not being tracked by the context.

3. **Attach Method:**
   - The `Attach` method is used to begin tracking an entity that is not currently being tracked. When you attach an entity, its state is set to `Unchanged`, meaning EF Core assumes it already exists in the database and has not been modified.
   - It's particularly useful in scenarios where you retrieve an entity outside of the context and later want to update or delete it.

4. **DebugView.LongView:**
   - The `ChangeTracker.DebugView.LongView` property provides a detailed view of the entities tracked by the `ChangeTracker` and their states. This is useful for debugging and understanding how EF Core is handling your entities behind the scenes.

### Improved and Explained Examples

#### 1. **AsTrackedEntity**

```csharp
public static void AsTrackedEntity()
{
    Console.WriteLine($">>>> Sample: {nameof(AsTrackedEntity)}");
    Console.WriteLine();

    DatabaseHelper.RecreateCleanDatabase();
    DatabaseHelper.PopulateDatabase();

    using (var context = new AppDbContext())
    {
        var author = context.Authors.First(); // DB Query using LINQ
        // The entity `author` is now being tracked by the context, and its state is `Unchanged`.

        Console.WriteLine(context.ChangeTracker.DebugView.LongView);
        // Displays the current state of the entity as tracked by EF Core.

        author.FName = "Whatever";
        // Changing the `FName` property marks the entity's state as `Modified`.

        Console.WriteLine("After Changing FName");
        Console.WriteLine(context.ChangeTracker.DebugView.LongView);
        // Now, the state of the entity should be `Modified`.

        context.Entry(author).State = EntityState.Modified;
        // Explicitly setting the state to `Modified`, though in this case, it’s redundant because it’s already `Modified`.

        Console.WriteLine("After Changing State to modified, before save changes");
        Console.WriteLine(context.ChangeTracker.DebugView.LongView);

        context.SaveChanges();
        // The changes are persisted to the database, and the state is reset to `Unchanged`.

        Console.WriteLine("After Save Changes");
        Console.WriteLine(context.ChangeTracker.DebugView.LongView);
    }
}
```

- **Purpose:** Demonstrates how EF Core tracks changes to an entity loaded from the database and automatically marks it as `Modified` when a property is updated. It also shows how the state transitions when `SaveChanges()` is called.

#### 2. **AsUnTrackedEntity**

```csharp
public static void AsUnTrackedEntity()
{
    Console.WriteLine($">>>> Sample: {nameof(AsUnTrackedEntity)}");
    Console.WriteLine();

    DatabaseHelper.RecreateCleanDatabase();
    DatabaseHelper.PopulateDatabase();

    using (var context = new AppDbContext())
    {
        var author = context.Authors.AsNoTracking().First();
        // The entity `author` is not tracked by the context because of `AsNoTracking`.

        Console.WriteLine(context.ChangeTracker.DebugView.LongView);
        // Since the entity is not tracked, the ChangeTracker will not have any information about it.
    }
}
```

- **Purpose:** Demonstrates how to retrieve an entity without tracking it using `AsNoTracking`. This is useful for read-only scenarios where you don’t intend to update the entity, leading to better performance by avoiding the overhead of tracking.

#### 3. **Inserting_New_Principal_Author**

```csharp
public static void Inserting_New_Principal_Author()
{
    Console.WriteLine($">>>> Sample: {nameof(Inserting_New_Principal_Author)}");
    Console.WriteLine();

    DatabaseHelper.RecreateCleanDatabase();

    using (var context = new AppDbContext())
    {
        // Mark the author as Added
        context.Add(new Author { Id = 1, FName = "Eric", LName = "Evans" });

        Console.WriteLine("Before SaveChanges:");
        Console.WriteLine(context.ChangeTracker.DebugView.LongView);
        // The state of the entity is `Added`.

        context.SaveChanges();
        // The entity is inserted into the database, and its state becomes `Unchanged`.

        Console.WriteLine("After SaveChanges:");
        Console.WriteLine(context.ChangeTracker.DebugView.LongView);
    }
}
```

- **Purpose:** Demonstrates how EF Core tracks a newly added entity and its transition from `Added` to `Unchanged` after saving to the database.

#### 4. **Attaching_Existing_Author_Principal**

```csharp
public static void Attaching_Existing_Author_Principal()
{
    Console.WriteLine($">>>> Sample: {nameof(Attaching_Existing_Author_Principal)}");
    Console.WriteLine();

    DatabaseHelper.RecreateCleanDatabase();
    DatabaseHelper.PopulateDatabase();

    using (var context = new AppDbContext())
    {
        var author = new Author { Id = 1, FName = "Eric", LName = "Evans" };

        context.Attach(author);
        // The entity `author` is now being tracked as `Unchanged` because it already exists in the database.

        author.LName = "Evanzzz";
        // Changing the `LName` property marks the entity as `Modified`.

        Console.WriteLine("Before SaveChanges:");
        Console.WriteLine(context.ChangeTracker.DebugView.LongView);

        context.SaveChanges();
        // The change is persisted to the database, and the state becomes `Unchanged`.

        Console.WriteLine("After SaveChanges:");
        Console.WriteLine(context.ChangeTracker.DebugView.LongView);
    }
}
```

- **Purpose:** Demonstrates how to attach an existing entity to the context, allowing EF Core to track changes made to it. This is useful when dealing with disconnected entities retrieved from a different context instance.

#### 5. **Updating_Existing_Author_Principal**

```csharp
public static void Updating_Existing_Author_Principal()
{
    Console.WriteLine($">>>> Sample: {nameof(Updating_Existing_Author_Principal)}");
    Console.WriteLine();

    DatabaseHelper.RecreateCleanDatabase();
    DatabaseHelper.PopulateDatabase();

    using (var context = new AppDbContext())
    {
        // Mark the author as Modified
        context.Update(new Author { Id = 1, FName = "EricAAAAA", LName = "Evans" });
        // Even though the entity is new to the context, EF Core marks it as `Modified` because it assumes it already exists in the database.

        Console.WriteLine("Before SaveChanges:");
        Console.WriteLine(context.ChangeTracker.DebugView.LongView);

        context.SaveChanges();
        // The change is persisted to the database, and the state becomes `Unchanged`.

        Console.WriteLine("After SaveChanges:");
        Console.WriteLine(context.ChangeTracker.DebugView.LongView);
    }
}
```

- **Purpose:** Demonstrates how to update an existing entity by marking it as `Modified` and tracking it. This is an alternative to loading the entity first and then modifying it.

#### 6. **Deleting_Existing_Author_Principal**

```csharp
public static void Deleting_Existing_Author_Principal()
{
    Console.WriteLine($">>>> Sample: {nameof(Deleting_Existing_Author_Principal)}");
    Console.WriteLine();

    DatabaseHelper.RecreateCleanDatabase();
    DatabaseHelper.PopulateDatabase();

    using (var context = new AppDbContext())
    {
        // Mark the author as Deleted
        context.Remove(new Author { Id = 1 });
        // EF Core marks the entity as `Deleted`, even though it's not currently being tracked.

        Console.WriteLine("Before SaveChanges:");
        Console.WriteLine(context.ChangeTracker.DebugView.LongView);

        context.SaveChanges();
        // The entity is removed from the database, and the context no longer tracks it.

        Console.WriteLine("After SaveChanges:");
        Console.WriteLine(context.ChangeTracker.DebugView.LongView);
    }
}
```

- **Purpose:** Demonstrates how to delete an entity by marking it as `Deleted` and tracking it. This is useful when you know the entity to be deleted without loading it first.

#### 7. **Deleting_Dependent_Book**

```csharp
public static void Deleting_Dependent_Book()
{
    Console.WriteLine($">>>> Sample: {nameof(Deleting_Dependent_Book)}");
    Console.WriteLine();

    DatabaseHelper.RecreateCleanDatabase();
    DatabaseHelper.PopulateDatabase();

    using (var context = new AppDbContext())
    {
        var book = DatabaseHelper.GetDisconnectedBook();
        // Retrieve a disconnected entity.

        context.Attach(book);
        // Attach the book entity to the context. Its state is now `Unchanged`.

        context.Remove(book);
        // Mark the book entity as `Deleted`. The state is now `Deleted`.

        Console.WriteLine("Before SaveChanges:");
        Console.WriteLine(context.ChangeTracker.DebugView.LongView);

        context.SaveChanges();
        // The book entity is removed from the database, and the context no longer tracks it.

        Console.WriteLine("After SaveChanges:");
        Console.WriteLine(context.ChangeTracker.DebugView.LongView);
    }
}
```

- **Purpose:** Demonstrates how to delete a dependent entity by marking it as `Deleted`. This is particularly useful when you need to remove an entity that was previously detached from the context.

### Summary of Key Points

1. **Tracking vs. No Tracking:**
   - EF Core tracks entities by default, which allows it to automatically detect changes and persist them to the database.
   - Use `AsNoTracking()` when you don't need to modify entities and want to improve performance by avoiding the overhead of tracking.

2. **Entity States:**
   - Understanding the different entity states (`Added`, `Modified`, `Deleted`, `Unchanged`, `Detached`) is crucial for effectively managing the lifecycle of entities within a context.

3. **ChangeTracker DebugView:**
   - The `ChangeTracker.DebugView.LongView` provides a detailed view of what EF Core is tracking, which is useful for debugging and understanding the internal workings of the framework.

4. **Attach and Update Methods:**
   - Use `Attach` to start tracking an existing entity with an `Unchanged` state, and `Update` to mark an entity as `Modified`. These methods are essential when dealing with disconnected entities.

5. **SaveChanges:**
   - When `SaveChanges()` is called, EF Core generates the necessary SQL commands to update the database based on the tracked changes and then resets the state of entities to `Unchanged`.

### Best Practices

- **Use `AsNoTracking()`** for read-only queries to improve performance.
- **Use `Attach` carefully** when dealing with entities that were not retrieved using the same context instance.
- **Inspect `ChangeTracker.DebugView`** during development to ensure EF Core is tracking entities as expected.
- **Minimize the scope of the context** to reduce the risk of memory leaks and ensure that the context is disposed of as soon as possible.

Understanding these concepts will help you manage entities in EF Core more effectively, especially in complex scenarios involving disconnected data and performance optimization.

