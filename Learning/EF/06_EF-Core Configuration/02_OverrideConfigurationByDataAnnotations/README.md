Data Annotations are a way to configure your classes in Entity Framework Core and ASP.NET Core applications using attributes. These attributes can be used to enforce validation rules, define database schema constraints, and control the behavior of the data model. By applying these attributes to your model classes and properties, you can ensure data integrity, enforce business rules, and customize the database schema without having to write extensive configuration code.

## Common Data Annotations in EF Core

### 1. `[Key]`

The `[Key]` attribute is used to specify the primary key of an entity when the convention-based primary key naming rules do not apply.

```csharp
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int CustomId { get; set; }
}
```

### 2. `[Required]`

The `[Required]` attribute is used to indicate that a property must have a value. In the database, this translates to a NOT NULL constraint.

```csharp
public class User
{
    [Required]
    public string Username { get; set; }
}
```

### 3. `[MaxLength]` and `[MinLength]`

The `[MaxLength]` and `[MinLength]` attributes specify the maximum and minimum length of a string or array property. In the database, `[MaxLength]` translates to a column with a maximum length constraint.

```csharp
public class User
{
    [MaxLength(50)]
    public string Username { get; set; }
}
```

### 4. `[StringLength]`

The `[StringLength]` attribute is similar to `[MaxLength]` but allows specifying both maximum and minimum lengths.

```csharp
public class User
{
    [StringLength(50, MinimumLength = 5)]
    public string Username { get; set; }
}
```

### 5. `[Range]`

The `[Range]` attribute is used to specify the minimum and maximum values for a numeric property.

```csharp
public class Product
{
    [Range(1, 1000)]
    public int Quantity { get; set; }
}
```

### 6. `[ConcurrencyCheck]`

The `[ConcurrencyCheck]` attribute is used to mark a property as being involved in concurrency checks.

```csharp
public class Product
{
    [ConcurrencyCheck]
    public int Stock { get; set; }
}
```

### 7. `[Timestamp]`

The `[Timestamp]` attribute is used to mark a byte array property as a timestamp, which is used for concurrency checking.

```csharp
public class Product
{
    [Timestamp]
    public byte[] RowVersion { get; set; }
}
```

### 8. `[ForeignKey]`

The `[ForeignKey]` attribute is used to specify the foreign key property for a navigation property.

```csharp
public class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; }
}
```

### 9. `[InverseProperty]`

The `[InverseProperty]` attribute is used to specify the inverse navigation property in a relationship.

```csharp
public class Customer
{
    public int CustomerId { get; set; }

    [InverseProperty("Customer")]
    public ICollection<Order> Orders { get; set; }
}

public class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; }
}
```

### 10. `[Column]`

The `[Column]` attribute is used to specify the column name and data type in the database.

```csharp
public class Product
{
    public int ProductId { get; set; }

    [Column("ProductName", TypeName = "nvarchar(100)")]
    public string Name { get; set; }
}
```

### Example of Using Data Annotations

Here's a complete example showing how to use various data annotations:

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string Username { get; set; }

    [MaxLength(100)]
    public string Email { get; set; }

    [Range(1, 120)]
    public int Age { get; set; }

    public ICollection<Post> Posts { get; set; }
}

public class Post
{
    [Key]
    public int PostId { get; set; }

    [Required]
    public string Title { get; set; }

    [Column(TypeName = "text")]
    public string Content { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}
```

### Using Data Annotations in EF Core

In your `DbContext` class, you don't need to add any additional configuration for the properties and classes with data annotations. EF Core will automatically apply the configurations specified by the attributes.

```csharp
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("YourConnectionString");
    }
}
```

In conclusion, Data Annotations provide a straightforward way to configure your entity classes and properties in Entity Framework Core, ensuring that your data model adheres to your business rules and database schema requirements.