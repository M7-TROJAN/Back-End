# DeleteBehavior in Entity Framework Core

## Overview

Entity Framework Core provides various delete behaviors that dictate how the database will handle deleting entities when they are involved in relationships. Understanding these behaviors is crucial for maintaining data integrity and controlling the cascading effects of deletions in your database.

### Delete Behaviors

1. **Cascade**: When the principal entity is deleted, the dependent entities are also automatically deleted.
2. **Restrict**: Prevents the deletion of the principal entity if there are related dependent entities. The deletion operation is rejected.
3. **SetNull**: When the principal entity is deleted, the foreign key values in the dependent entities are set to `null`.
4. **NoAction**: The operation is ignored, and no action is taken in the database. The responsibility to handle the operation falls to the application.
5. **ClientCascade**: Similar to `Cascade`, but the deletion is handled on the client side.
6. **ClientNoAction**: Similar to `NoAction`, but the operation is handled on the client side.
7. **ClientSetNull**: Similar to `SetNull`, but the foreign key is set to `null` on the client side.

### Required vs. Optional Foreign Keys

- **Required Foreign Key**: The foreign key field is non-nullable, meaning that a relationship is required.
  ```csharp
  public int AuthorV2Id { get; set; }  // Required FK
  ```

- **Optional Foreign Key**: The foreign key field is nullable, meaning that a relationship is optional.
  ```csharp
  public int? AuthorV2Id { get; set; }  // Optional FK
  ```

## Example Entities

### Author and Book Entities

```csharp
// Principal Entity
public class Author
{
    public int Id { get; set; }
    public string FName { get; set; }
    public string LName { get; set; }

    public List<Book> Books { get; set; } = new();
}

// Principal Entity (Version 2)
public class AuthorV2
{
    public int Id { get; set; }
    public string FName { get; set; }
    public string LName { get; set; }

    public List<BookV2> BookV2s { get; set; } = new();
}

// Dependent Entity
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }

    public int AuthorId { get; set; }  // Required FK
    public Author Author { get; set; }
}

// Dependent Entity (Version 2)
public class BookV2
{
    public int Id { get; set; }
    public string Title { get; set; }

    public int? AuthorV2Id { get; set; }  // Optional FK
    public AuthorV2? AuthorV2 { get; set; }
}
```

## Example Scenarios and Explanations

### 1. Delete Principal Author With Dependent Book When FK is Required

#### Code Example

```csharp
public static void DeletePrincipalAuthor_With_Dependent_Book_FK_Required()
{
    Console.WriteLine($">>>> Sample: {nameof(DeletePrincipalAuthor_With_Dependent_Book_FK_Required)}");
    Console.WriteLine();

    DatabaseHelper.RecreateCleanDatabase();
    DatabaseHelper.PopulateDatabase();

    using (var context = new AppDbContext())
    {
        var author = context.Authors.First();

        context.Authors.Remove(author);

        context.SaveChanges();
    }

    Console.ReadKey();
}
```

#### Explanation

In this example, the `Book` entity has a required foreign key (`AuthorId`). When attempting to delete the `Author`, the behavior of the deletion depends on the configured `DeleteBehavior`:

- **Cascade**: The dependent `Book` entities will be automatically deleted along with the `Author`.
- **Restrict/NoAction**: An exception will be thrown, preventing the deletion of the `Author` since there are dependent `Book` entities.
- **SetNull**: Not applicable because the FK is required; a `null` value cannot be set.

### 2. Delete Principal Author With Dependent Book When FK is Optional

#### Code Example

```csharp
public static void DeletePrincipalAuthor_With_Dependent_Book_FK_Optional()
{
    Console.WriteLine($">>>> Sample: {nameof(DeletePrincipalAuthor_With_Dependent_Book_FK_Optional)}");
    Console.WriteLine();

    DatabaseHelper.RecreateCleanDatabase();
    DatabaseHelper.PopulateDatabase();

    using (var context = new AppDbContext())
    {
        var author = context.AuthorV2s.First();

        context.AuthorV2s.Remove(author);

        context.SaveChanges();
    }

    Console.ReadKey();
}
```

#### Explanation

In this example, the `BookV2` entity has an optional foreign key (`AuthorV2Id`). The deletion of the `AuthorV2` affects the `BookV2` entities based on the `DeleteBehavior`:

- **Cascade**: The `BookV2` entities will be deleted along with the `AuthorV2`.
- **SetNull**: The `AuthorV2Id` in `BookV2` will be set to `null`.
- **Restrict/NoAction**: An exception will be thrown, preventing the deletion of `AuthorV2` if dependent `BookV2` entities exist.

### 3. Severing Relationship: Dependent Book Set Principal to Null (FK Optional)

#### Code Example

```csharp
public static void SeveringRelationship_DependentBook_SetPrincipal_Null_FK_Optional()
{
    Console.WriteLine($">>>> Sample: {nameof(SeveringRelationship_DependentBook_SetPrincipal_Null_FK_Optional)}");
    Console.WriteLine();

    DatabaseHelper.RecreateCleanDatabase();
    DatabaseHelper.PopulateDatabase();

    using (var context = new AppDbContext())
    {
        var author = context.AuthorV2s.Include(x => x.BookV2s).First();

        author.BookV2s.Clear();

        context.SaveChanges();
    }

    Console.ReadKey();
}
```

#### Explanation

In this scenario, the relationship between `BookV2` and `AuthorV2` is severed by setting the `AuthorV2Id` in the `BookV2` entities to `null`. This is possible because the foreign key is optional. The `Clear()` method removes all references to the `AuthorV2` from the `BookV2s` collection.

### 4. Severing Relationship: Principal Author Clears Dependents, Dependent Book FK Optional

#### Code Example

```csharp
public static void SeveringRelationship_PrincipalAuthor_ClearDependents_Dependent_Book_FK_Optional()
{
    Console.WriteLine($">>>> Sample: {nameof(SeveringRelationship_PrincipalAuthor_ClearDependents_Dependent_Book_FK_Optional)}");
    Console.WriteLine();

    DatabaseHelper.RecreateCleanDatabase();
    DatabaseHelper.PopulateDatabase();

    using (var context = new AppDbContext())
    {
        var author = context.AuthorV2s.Include(x => x.BookV2s).First();

        foreach (var book in author.BookV2s)
        {
            book.AuthorV2 = null;
        }

        context.SaveChanges();
    }

    Console.ReadKey();
}
```

#### Explanation

This example demonstrates another way to sever the relationship between `AuthorV2` and `BookV2` by setting the foreign key (`AuthorV2Id`) to `null` for each dependent `BookV2`. This approach explicitly iterates through the `BookV2` entities and nullifies their reference to `AuthorV2`.

---

This markdown file provides a comprehensive lesson on `DeleteBehavior` in Entity Framework Core, with practical examples to illustrate each concept.