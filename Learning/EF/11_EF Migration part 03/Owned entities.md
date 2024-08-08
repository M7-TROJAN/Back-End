An owned entity in Entity Framework Core represents a type that doesn't have its own identity and is used as a property of another entity. Owned types are often used for value objects in domain-driven design. These entities are always part of the parent entity and cannot exist without it.

Here's how to configure an owned entity in the `OnModelCreating` method:

1. **Define the Owned Entity Type:**
   - Create a class for the owned entity type.
   - Create a class for the main entity that contains the owned entity.

2. **Configure the Owned Entity in `OnModelCreating`:**
   - Use the `OwnsOne` or `OwnsMany` method to configure the owned entity.

### Example

Let's say we have an `Order` entity that contains a `ShippingAddress` value object as an owned entity.

#### Step 1: Define the Classes

**Owned Entity:**

```csharp
public class ShippingAddress
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
}
```

**Main Entity:**

```csharp
public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public ShippingAddress ShippingAddress { get; set; }
}
```

#### Step 2: Configure in `OnModelCreating`

```csharp
public class ApplicationDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>(order =>
        {
            order.HasKey(o => o.Id);

            // Configure the owned entity
            order.OwnsOne(o => o.ShippingAddress, sa =>
            {
                sa.Property(p => p.Street).HasMaxLength(100).HasColumnType("VARCHAR").IsRequired();
                sa.Property(p => p.City).HasMaxLength(50).HasColumnType("VARCHAR").IsRequired();
                sa.Property(p => p.State).HasMaxLength(50).HasColumnType("VARCHAR").IsRequired();
                sa.Property(p => p.PostalCode).HasMaxLength(10).HasColumnType("VARCHAR").IsRequired();

                // Configure other properties or relationships of ShippingAddress if needed
            });

            // Configure other properties of Order if needed
        });
    }
}
```

### Explanation

- **Owned Entity Definition:** The `ShippingAddress` class is defined with properties representing the address details.
- **Main Entity Definition:** The `Order` class includes an instance of `ShippingAddress`.
- **Configuration in `OnModelCreating`:** 
  - The `OwnsOne` method is used to configure the `ShippingAddress` as an owned entity of `Order`.
  - Inside the `OwnsOne` configuration, each property of the `ShippingAddress` is further configured with constraints such as `HasMaxLength`, `HasColumnType`, and `IsRequired`.

Owned entities allow for more complex modeling scenarios where certain properties are tightly coupled to the parent entity and do not require their own table or primary key. This approach helps in maintaining a clean and organized domain model.