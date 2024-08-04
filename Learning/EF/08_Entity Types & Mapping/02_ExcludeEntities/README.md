# Excluding Entities in Entity Framework Core

In Entity Framework Core (EF Core), there are scenarios where you might want to exclude certain classes from being mapped to the database. This can be done using the `[NotMapped]` attribute or the `modelBuilder.Ignore` method. This guide will cover both approaches.

## Using the `[NotMapped]` Attribute

The `[NotMapped]` attribute is used to indicate that a class or property should not be mapped to a database table or column. This attribute is typically applied to classes or properties that are part of your domain model but do not need to be persisted in the database.

### Example

Consider the following example with a `Product` class and a `Snapshot` class.

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace _01_BasicSetup.Entities
{
    public class Product
    {
        public Product()
        {
            this.Snapshot = new Snapshot();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; }

        // Navigation property
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public Snapshot Snapshot { get; set; }
    }

    [NotMapped]
    public class Snapshot
    {
        public DateTime LoadAt => DateTime.Now;
        public string Version => Guid.NewGuid().ToString().Substring(0, 8);
    }
}
```

In this example, the `Snapshot` class is marked with the `[NotMapped]` attribute. This means EF Core will ignore this class and not map it to a table in the database.

## Using the `modelBuilder.Ignore` Method

The `modelBuilder.Ignore` method is used within the `OnModelCreating` method of your DbContext to exclude a class from being mapped to the database.

### Example

The same functionality can be achieved using the `modelBuilder.Ignore` method.

```csharp
using Microsoft.EntityFrameworkCore;

namespace _01_BasicSetup.Entities
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Exclude the Snapshot class
            modelBuilder.Ignore<Snapshot>();
        }
    }
}
```

In this example, the `Snapshot` class is ignored by EF Core using the `modelBuilder.Ignore<Snapshot>();` method within the `OnModelCreating` method.

## Choosing Between `[NotMapped]` and `modelBuilder.Ignore`

### `[NotMapped]` Attribute
- **Pros**: Simple to use and understand; keeps the configuration close to the class it applies to.
- **Cons**: Must be used on each class or property that you want to exclude.

### `modelBuilder.Ignore`
- **Pros**: Centralized configuration within the `OnModelCreating` method; useful for excluding multiple classes or applying complex configurations.
- **Cons**: Requires access to the DbContext configuration, making it slightly less intuitive for beginners.

## Summary

Both the `[NotMapped]` attribute and the `modelBuilder.Ignore` method are useful tools in EF Core for excluding entities from being mapped to the database. The choice between them depends on your specific requirements and preferences for configuration.

- Use `[NotMapped]` for simple exclusions directly on the class or property.
- Use `modelBuilder.Ignore` for more centralized and potentially complex exclusion configurations.