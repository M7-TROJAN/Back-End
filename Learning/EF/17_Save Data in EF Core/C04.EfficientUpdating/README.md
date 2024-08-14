### Efficient Updating and Deleting in EF Core

When working with a database, especially when dealing with large amounts of data, it's crucial to perform operations as efficiently as possible. This lesson focuses on how to efficiently update and delete records in Entity Framework Core (EF Core), particularly in EF Core 7.0 and later versions. We'll explore methods like `ExecuteUpdate`, `SetProperty`, `ExecuteDelete`, `ExecuteSql`, and strategies to minimize database roundtrips using `MinBatchSize()` and `MaxBatchSize()`.

---

### 1. Understanding Multiple Roundtrips in Database Operations

#### Example 1: Typical Implementation (Pre-EF Core 7.0)

```csharp
public static void Increase_Book_Price_By_10Percent_For_Author1_Typical_Implementation()
{
    using (var context = new AppDbContext())
    {
        var author1Books = context.Books.Where(x => x.AuthorId == 1);

        foreach (var book in author1Books) // Deferred Execution
        {
            book.Price *= 1.1m;
        }

        context.SaveChanges();
    }
}
```

**Explanation:**

- **Deferred Execution:** The `Where` clause filters books for a specific author. However, the actual execution of the query is deferred until the `foreach` loop iterates over the results.
  
- **Multiple Roundtrips:** For each book in the filtered list, the `Price` is updated individually. When `context.SaveChanges()` is called, each update might result in multiple roundtrips to the database—one for each updated record. This can significantly slow down the operation, especially if there are many records.

---

### 2. Batch Size Configuration: MinBatchSize() and MaxBatchSize()

To control how many operations are sent to the database in a single roundtrip, EF Core provides configuration options like `MinBatchSize()` and `MaxBatchSize()`.

```csharp
optionsBuilder.UseSqlServer(connectionString, 
    o => o.MinBatchSize(4).MaxBatchSize(100));
```

- **MinBatchSize:** Sets the minimum number of operations to include in a single batch. If the total number of operations is below this value, they might be sent together in one roundtrip.
  
- **MaxBatchSize:** Limits the number of operations sent in a single roundtrip. This helps avoid overwhelming the database with too many simultaneous operations, which could negatively impact performance.

By tweaking these settings, you can reduce the number of roundtrips and improve the efficiency of database operations.

---

### 3. Efficient Updates and Deletes in EF Core 7.0 and Later

#### Example 2: Efficient Update Using `ExecuteUpdate` and `SetProperty`

```csharp
public static void Increase_Book_Price_By_10Percent_For_Author1_EF7AnUp_Implementation()
{
    using (var context = new AppDbContext())
    {
        context.Books.Where(x => x.AuthorId == 1)
               .ExecuteUpdate(b => b.SetProperty(p => p.Price, p => p.Price * 1.1m));
    }
}
```

**Explanation:**

- **`ExecuteUpdate`:** This method is introduced in EF Core 7.0 to perform updates directly on the database without retrieving the entities into memory. This results in a single, efficient roundtrip to the database.
  
- **`SetProperty`:** Inside `ExecuteUpdate`, `SetProperty` specifies which property to update (`Price` in this case) and how to calculate the new value.

**Benefits:**

- This approach is much more efficient than the typical implementation as it minimizes database roundtrips and avoids loading unnecessary data into memory.

#### Example 3: Efficient Delete Using `ExecuteDelete`

```csharp
public static void Delete_Book_With_Title_Start_With_Book_EF7AnUp_Implementation()
{
    using (var context = new AppDbContext())
    {
        context.Books.Where(x => x.Title.StartsWith("Book")).ExecuteDelete();
    }
}
```

**Explanation:**

- **`ExecuteDelete`:** Similar to `ExecuteUpdate`, this method allows you to perform a bulk delete operation directly on the database, again reducing roundtrips and improving efficiency.

**Benefits:**

- Only a single roundtrip is required to delete all matching records, which is much faster than loading the records, marking them as deleted, and then saving changes.

---

### 4. Executing Raw SQL for Bulk Operations

#### Example 4: Raw SQL Update

```csharp
public static void Increase_Book_Price_By_10Percent_For_Author1_EF7AnUp_RawSql()
{
    using (var context = new AppDbContext())
    {
        var authorId = 1;

        context.Database.ExecuteSql($"UPDATE dbo.Books SET Price = Price * 1.1 WHERE AuthorId = {authorId}");
    }
}
```

**Explanation:**

- **`ExecuteSql`:** Directly executes a SQL command on the database. While this provides maximum control over the SQL query, it bypasses EF Core’s change tracking and other ORM features.
  
**Benefits:**

- Raw SQL is often the most efficient way to perform bulk operations, as it allows you to leverage the full power of SQL.

**Considerations:**

- This method should be used carefully, as it doesn't benefit from EF Core's safety mechanisms like parameterized queries (unless explicitly added).

---

### Summary

- **Multiple Roundtrips:** Inefficient updates (like those in the typical implementation) can result in multiple roundtrips to the database, significantly slowing down operations.
  
- **Batch Size Configuration:** Use `MinBatchSize()` and `MaxBatchSize()` to optimize how many operations are batched together in a single roundtrip.

- **Efficient Updates/Deletes:** `ExecuteUpdate`, `SetProperty`, and `ExecuteDelete` are powerful methods introduced in EF Core 7.0 that help reduce roundtrips by performing bulk operations directly on the database.

- **Raw SQL:** For maximum efficiency, raw SQL queries can be executed using `ExecuteSql`, but they should be used with caution.

This lesson has provided you with a deeper understanding of how to perform efficient updates and deletes in EF Core, ensuring your applications can handle large datasets without sacrificing performance.