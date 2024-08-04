### Mapping Database Views in Entity Framework Core

Entity Framework Core (EF Core) allows you to map database views to your models. This is useful when you need to treat a database view as an entity in your application.

#### Step-by-Step Guide

1. **Create the Model**

   Define a class that corresponds to the database view. This model will have properties matching the columns of the view.

   ```csharp
   public class OrderWithDetailsView
   {
       public int OrderId { get; set; }
       public DateTime OrderDate { get; set; }
       public string CustomerEmail { get; set; }
       public int ProductId { get; set; }
       public string ProductName { get; set; }
       public int Quantity { get; set; }
       public decimal UnitPrice { get; set; }

       public override string ToString()
       {
           return $"#{OrderId}: {OrderDate}, {ProductName} X {Quantity} @ {UnitPrice.ToString("C")}";
       }
   }
   ```

2. **Add DbSet in DbContext**

   Add a `DbSet` property to your `DbContext` class for the view.

   ```csharp
   public class AppDbContext : DbContext
   {
       public DbSet<OrderWithDetailsView> OrderWithDetails { get; set; }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           modelBuilder.Entity<OrderWithDetailsView>()
               .ToView("OrderWithDetailsView")
               .HasNoKey();

           base.OnModelCreating(modelBuilder);
       }
   }
   ```

3. **Configure the Model**

   In the `OnModelCreating` method of your `DbContext`, configure the model to map to the database view.

   ```csharp
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
       modelBuilder.Entity<OrderWithDetailsView>()
           .ToView("OrderWithDetailsView")
           .HasNoKey();

       base.OnModelCreating(modelBuilder);
   }
   ```

   - `ToView("OrderWithDetailsView")`: Specifies that this entity maps to a database view named `OrderWithDetailsView`.
   - `HasNoKey()`: Indicates that this entity does not have a primary key. Views typically do not have primary keys, so this is necessary to avoid errors.

### Complete Example

Here's a complete example including the `DbContext` and model class:

```csharp
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EFCoreMappingViews
{
    public class OrderWithDetailsView
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerEmail { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public override string ToString()
        {
            return $"#{OrderId}: {OrderDate}, {ProductName} X {Quantity} @ {UnitPrice.ToString("C")}";
        }
    }

    public class AppDbContext : DbContext
    {
        public DbSet<OrderWithDetailsView> OrderWithDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderWithDetailsView>()
                .ToView("OrderWithDetailsView")
                .HasNoKey();

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Your_Connection_String_Here");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                var orders = context.OrderWithDetails.ToList();
                foreach (var order in orders)
                {
                    Console.WriteLine(order);
                }
            }
        }
    }
}
```

### Summary

- **Model Creation**: Define a model class that corresponds to the database view.
- **DbSet Configuration**: Add a `DbSet` property for the view in your `DbContext` class.
- **Model Mapping**: Use the `OnModelCreating` method to map the model to the database view using `ToView` and specify `HasNoKey` to indicate the absence of a primary key.
- **Usage**: Query the view using the DbSet just like you would with any other entity.

Mapping database views in EF Core is straightforward and allows you to leverage the power of views in your applications, providing a clean and efficient way to handle complex queries and data transformations.