### Understanding `BasicSaveWithTracking` in EF Core

The concept of **tracking** in Entity Framework Core (EF Core) is fundamental to how the framework manages entity states and interacts with the database. When you perform operations like adding, updating, or deleting entities, EF Core tracks the changes in the context, which allows it to generate the appropriate SQL commands when `SaveChanges()` is called.

Let's break down the examples you provided, explaining the purpose and what's happening behind the scenes.

### Example 1: **Basic Save**

#### Code:
```csharp
private static void RunBasicSave()
{
    DatabaseHelper.RecreateCleanDatabase();

    using (var context = new AppDbContext())
    {
        var author = new Author { Id = 1, FName = "eric", LName = "Evans" };

        context.Authors.Add(author);

        context.SaveChanges();
    }
}
```

#### Explanation:
- **Purpose:** This example demonstrates how to add a new entity (`Author`) to the database.
- **Behind the Scenes:**
  - **`context.Authors.Add(author);`**: EF Core marks the `author` entity as `Added`. This means EF Core is tracking this entity and knows that it should be inserted into the database.
  - **`context.SaveChanges();`**: EF Core generates an `INSERT` SQL statement to add the new `Author` record to the `Authors` table in the database.

### Example 2: **Basic Update**

#### Code:
```csharp
private static void RunBasicUpdate()
{
    using (var context = new AppDbContext())
    {
        var author = context.Authors.FirstOrDefault(x => x.Id == 1);

        author.FName = "Eric";

        context.SaveChanges();
    }
}
```

#### Explanation:
- **Purpose:** This example shows how to update an existing entity.
- **Behind the Scenes:**
  - **`context.Authors.FirstOrDefault(x => x.Id == 1);`**: EF Core retrieves the `Author` entity from the database. Since EF Core tracks this entity, any changes made to it will be automatically detected.
  - **`author.FName = "Eric";`**: The `FName` property is updated.
  - **`context.SaveChanges();`**: EF Core detects the change to the `FName` property and generates an `UPDATE` SQL statement to apply the change in the database.

### Example 3: **Basic Delete**

#### Code:
```csharp
private static void RunBasicDelete()
{
    using (var context = new AppDbContext())
    {
        var author = context.Authors.FirstOrDefault(x => x.Id == 1);

        context.Authors.Remove(author);

        context.SaveChanges();
    }
}
```

#### Explanation:
- **Purpose:** This example shows how to delete an entity from the database.
- **Behind the Scenes:**
  - **`context.Authors.FirstOrDefault(x => x.Id == 1);`**: EF Core retrieves the `Author` entity.
  - **`context.Authors.Remove(author);`**: The `author` entity is marked as `Deleted`. EF Core knows that this entity should be removed from the database.
  - **`context.SaveChanges();`**: EF Core generates a `DELETE` SQL statement to remove the corresponding record from the `Authors` table in the database.

### Example 4: **Multiple Operations with a Single Save**

#### Code:
```csharp
private static void RunMultipleOperationsWithSingleSave()
{
    using (var context = new AppDbContext())
    {
        var author1 = new Author { Id = 1, FName = "Eric", LName = "Evans" };
        context.Authors.Add(author1);

        var author2 = new Author { Id = 2, FName = "John", LName = "Skeet" };
        context.Authors.Add(author2);

        var author3 = new Author { Id = 3, FName = "ditya", LName = "Bhargava" };
        context.Authors.Add(author3);

        author3.FName = "Aditya";

        context.SaveChanges();
    }
}
```

#### Explanation:
- **Purpose:** This example demonstrates how multiple operations can be performed before a single `SaveChanges()` call.
- **Behind the Scenes:**
  - **Adding Entities:** The `Add` method is called three times, adding three `Author` entities. These entities are marked as `Added`.
  - **Updating an Entity:** The `FName` of `author3` is updated before `SaveChanges()` is called. Since EF Core is tracking the entity, it detects the change.
  - **`context.SaveChanges();`**: EF Core generates the necessary SQL commands (`INSERT` for the first two authors and `INSERT` + `UPDATE` for the third author) in a single transaction.

### Example 5: **Adding Related Entities**

#### Code:
```csharp
private static void RunAddRelatedEntities()
{
    using (var context = new AppDbContext())
    {
        var author = context.Authors.FirstOrDefault(x => x.Id == 1);

        author.Books.Add(new Book
        {
            Id = 1,
            Title = "Domain-Driven Design: Tackling Complexity in the Heart of Software"
        });

        context.SaveChanges();
    }
}
```

#### Explanation:
- **Purpose:** This example shows how to add a related entity (a `Book`) to an existing entity (`Author`).
- **Behind the Scenes:**
  - **Retrieving the Author:** The `Author` entity is retrieved and tracked.
  - **Adding a Book:** The new `Book` entity is added to the `Books` collection of the `Author`. EF Core marks the `Book` as `Added` and understands the relationship between the `Book` and `Author`.
  - **`context.SaveChanges();`**: EF Core generates an `INSERT` SQL statement to add the `Book` to the `Books` table and automatically sets the `AuthorId` foreign key.

### **What Happens Behind the Scenes in EF Core**

- **Change Tracking:** EF Core uses an internal mechanism called the **Change Tracker** to keep track of all entities that have been loaded into the context. It monitors these entities for any changes so that the appropriate SQL commands can be generated when `SaveChanges()` is called.
- **State Management:** Each entity tracked by EF Core has a state (e.g., `Added`, `Modified`, `Deleted`, `Unchanged`). These states help EF Core decide what actions to take when `SaveChanges()` is called.

### **Best Practices**

1. **Minimize the Scope of the Context:** Keep the context's lifetime as short as possible. Long-lived contexts can lead to memory leaks and performance issues.
2. **Batch Operations:** When possible, batch multiple operations into a single `SaveChanges()` call. This reduces the number of database round-trips and can improve performance.
3. **Explicit State Management:** In some cases, you may need to explicitly set the state of an entity (e.g., `context.Entry(entity).State = EntityState.Modified`) to control EF Core's behavior.

### **Conclusion**

Understanding how EF Core tracks entities and manages changes is crucial for building efficient and effective applications. The examples provided show the basics of adding, updating, and deleting entities, as well as working with related entities.