# Entity Types & Mapping in Entity Framework Core

## Introduction

Entity Framework Core (EF Core) is a powerful ORM (Object-Relational Mapper) that enables developers to work with a database using .NET objects. This document provides a detailed guide on different aspects of entity types and mapping in EF Core, including excluding entities, including entities, mapping views, and mapping table-valued functions (TVFs).

## Table of Contents

1. [Exclude Entities](#exclude-entities)
2. [Include Entities](#include-entities)
3. [Mapping Views](#mapping-views)
4. [Mapping Table-Valued Functions](#mapping-table-valued-functions)

## Exclude Entities

### What Are Excluded Entities?

In some cases, you may have classes in your model that you do not want to map to a database table. These are known as excluded entities.

### Example Scenario

Consider a class `Product` that has a nested class `Snapshot` which you do not want to map to the database.

```csharp
namespace MyNamespace.Entities
{
    public class Product
    {
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

### How to Exclude Entities

#### Using `[NotMapped]` Attribute

Mark the class with the `[NotMapped]` attribute.

```csharp
[NotMapped]
public class Snapshot
{
    public DateTime LoadAt => DateTime.Now;
    public string Version => Guid.NewGuid().ToString().Substring(0, 8);
}
```

#### Using Fluent API

In the `OnModelCreating` method of your `DbContext` class, use the `Ignore` method.

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Ignore<Snapshot>();
    base.OnModelCreating(modelBuilder);
}
```

## Include Entities

### What Are Included Entities?

Entities that you explicitly want to be mapped to a database table. These are typically your domain model classes that represent tables in your database.

### Example Scenario

Consider the following `Product` class.

```csharp
namespace MyNamespace.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; }

        // Navigation property
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
```

### How to Include Entities

#### By Convention

By default, EF Core includes all public `DbSet<TEntity>` properties that you define in your `DbContext`.

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
}
```

#### Using Fluent API

If you need more control, you can configure entity mappings using the Fluent API in the `OnModelCreating` method.

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Product>(entity =>
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
        entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
    });

    base.OnModelCreating(modelBuilder);
}
```

## Mapping Views

### What Are Database Views?

Database views are virtual tables created by a query joining one or more tables. They do not store data but provide a way to simplify complex queries.

### How to Map Views

#### Step 1: Create a View in SQL

First, create a view in your database.

```sql
CREATE VIEW OrderWithDetailsView AS
SELECT
    o.OrderId,
    o.OrderDate,
    c.Email AS CustomerEmail,
    p.ProductId,
    p.Name AS ProductName,
    od.Quantity,
    od.UnitPrice
FROM
    Orders o
JOIN Customers c ON o.CustomerId = c.CustomerId
JOIN OrderDetails od ON o.OrderId = od.OrderId
JOIN Products p ON od.ProductId = p.ProductId;
```

#### Step 2: Create a Model for the View

Define a class that matches the structure of the view.

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
        return $"#{OrderId}: {OrderDate}, {ProductName} X {Quantity} @ {UnitPrice:C}";
    }
}
```

#### Step 3: Configure the View in DbContext

Map the view to a `DbSet` in your `DbContext`.

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

## Mapping Table-Valued Functions

### What Are Table-Valued Functions (TVFs)?

TVFs are functions in SQL that return a table. They are useful for encapsulating complex queries that return a set of rows.

### How to Map TVFs

#### Step 1: Create a TVF in SQL

Define the TVF in your database.

```sql
CREATE FUNCTION GetOrdersForCustomer (@customerId INT)
RETURNS TABLE
AS
RETURN
(
    SELECT o.OrderId, o.OrderDate, c.CustomerName, p.ProductName, od.Quantity, od.UnitPrice
    FROM Orders o
    JOIN Customers c ON o.CustomerId = c.CustomerId
    JOIN OrderDetails od ON o.OrderId = od.OrderId
    JOIN Products p ON od.ProductId = p.ProductId
    WHERE o.CustomerId = @customerId
);
```

#### Step 2: Create a Model for the TVF Result

Define a class that represents the result of the TVF.

```csharp
public class OrderForCustomer
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string CustomerName { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
```

#### Step 3: Add a Method to the DbContext

Map the TVF to a method in your `DbContext`.

```csharp
public class AppDbContext : DbContext
{
    public DbSet<OrderForCustomer> OrdersForCustomer { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderForCustomer>().HasNoKey().ToTable(nameof(OrderForCustomer));

        base.OnModelCreating(modelBuilder);
    }

    public IQueryable<OrderForCustomer> GetOrdersForCustomer(int customerId)
    {
        return OrdersForCustomer.FromSqlInterpolated($"SELECT * FROM GetOrdersForCustomer({customerId})");
    }
}
```

#### Step 4: Use the Method in Your Application

Call the method from your application code to execute the TVF and retrieve the results.

```csharp
class Program
{
    static void Main(string[] args)
    {
        using (var context = new AppDbContext())
        {
            int customerId = 1;
            var orders = context.GetOrdersForCustomer(customerId).ToList();

            foreach (var order in orders)
            {
                Console.WriteLine($"Order {order.OrderId}: {order.OrderDate} - {order.CustomerName} - {order.ProductName} - {order.Quantity} - {order.UnitPrice:C}");
            }
        }
    }
}
```

## Summary

This document has provided a detailed overview of how to work with entity types and mapping in EF Core. By understanding how to exclude and include entities, map views, and map TVFs, you can effectively manage your database interactions and leverage the full power of EF Core for your applications.