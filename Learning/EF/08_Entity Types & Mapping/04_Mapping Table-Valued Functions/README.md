### Mapping Table-Valued Functions (TVFs) in Entity Framework Core

Table-Valued Functions (TVFs) are functions in SQL that return a table. In Entity Framework Core (EF Core), you can map these functions to queryable methods in your context class. This is particularly useful for complex queries that are best handled at the database level.

#### Step-by-Step Guide

1. **Create the Table-Valued Function in SQL**

   First, create the TVF in your database. For example:

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

2. **Create a Model for the Result**

   Define a class that represents the result of the TVF. This model should have properties matching the columns returned by the function.

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

3. **Add a Method to the DbContext**

   Add a method to your `DbContext` class that maps to the TVF. Use the `FromSqlInterpolated` method to execute the function.

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

4. **Use the Method in Your Application**

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

### Summary

- **Define the TVF in SQL**: Create a table-valued function in your database that returns a table.
- **Create a Result Model**: Define a C# class that matches the columns returned by the TVF.
- **Map the TVF in DbContext**: Add a method to your `DbContext` class that uses `FromSqlInterpolated` to map the TVF.
- **Query the TVF**: Call the method from your application code to execute the function and retrieve the results.

Mapping TVFs in EF Core allows you to take advantage of complex SQL logic while keeping your application code clean and maintainable. This approach provides the benefits of both SQL performance optimizations and EF Core's powerful query capabilities.