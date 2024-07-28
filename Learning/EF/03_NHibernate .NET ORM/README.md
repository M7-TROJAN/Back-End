# NHibernate .NET

NHibernate is a mature, open-source object-relational mapper (ORM) for .NET. It facilitates the mapping of .NET objects to database tables, allowing developers to work with data in an object-oriented way. This document provides an in-depth look at NHibernate, including its setup, best practices, and real-world use cases.

## Table of Contents
1. [Introduction](#introduction)
2. [Setup and Configuration](#setup-and-configuration)
3. [Basic CRUD Operations](#basic-crud-operations)
4. [Mapping](#mapping)
5. [Queries](#queries)
6. [Transactions and Session Management](#transactions-and-session-management)
7. [Best Practices](#best-practices)
8. [Use Cases in Real-World Scenarios](#use-cases-in-real-world-scenarios)
9. [Resources](#resources)

## Introduction

NHibernate is designed to simplify data access in .NET applications. It abstracts the database interactions, allowing developers to focus on the business logic rather than the intricacies of SQL queries.

Key features include:
- Transparent persistence: Objects can be persisted without having to implement special interfaces.
- Powerful query capabilities: Supports HQL (Hibernate Query Language), LINQ, and Criteria API.
- Lazy loading: Improves performance by loading data only when it is actually needed.
- Caching: Built-in support for first-level and second-level caching.

## Setup and Configuration

### Installation

You can install NHibernate via NuGet:

```bash
Install-Package NHibernate
Install-Package NHibernate.Linq
Install-Package NHibernate.Caches.SysCache
```

### Configuration

NHibernate requires a configuration file (`hibernate.cfg.xml`) or programmatic configuration. Below is an example of a configuration file:

```xml
<hibernate-configuration xmlns="urn:nhibernate-configuration-2.2">
  <session-factory>
    <property name="connection.driver_class">NHibernate.Driver.SqlClientDriver</property>
    <property name="connection.connection_string">Data Source=.;Initial Catalog=YourDatabase;Integrated Security=True</property>
    <property name="dialect">NHibernate.Dialect.MsSql2008Dialect</property>
    <property name="show_sql">true</property>
    <property name="hbm2ddl.auto">update</property>
  </session-factory>
</hibernate-configuration>
```

### Programmatic Configuration

Alternatively, you can configure NHibernate programmatically:

```csharp
var cfg = new Configuration();
cfg.DataBaseIntegration(db =>
{
    db.ConnectionString = "Data Source=.;Initial Catalog=YourDatabase;Integrated Security=True";
    db.Dialect<MsSql2008Dialect>();
    db.Driver<SqlClientDriver>();
    db.LogSqlInConsole = true;
});
cfg.AddAssembly(typeof(Program).Assembly);

var sessionFactory = cfg.BuildSessionFactory();
```

## Basic CRUD Operations

### Entity Definition

```csharp
public class Product
{
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }
    public virtual decimal Price { get; set; }
}
```

> **Note:** NHibernate requires that all members that map to database columns be marked as `virtual`. This is necessary for NHibernate to create proxy objects for lazy loading.

### Mapping

Create an XML mapping file (`Product.hbm.xml`):

```xml
<hibernate-mapping xmlns="urn:nhibernate-mapping-2.2">
  <class name="YourNamespace.Product, YourAssembly" table="Products">
    <id name="Id" column="Id">
      <generator class="identity" />
    </id>
    <property name="Name" column="Name" />
    <property name="Price" column="Price" />
  </class>
</hibernate-mapping>
```

### CRUD Operations

```csharp
using (var session = sessionFactory.OpenSession())
{
    // Create
    using (var transaction = session.BeginTransaction())
    {
        var product = new Product { Name = "Sample Product", Price = 9.99M };
        session.Save(product);
        transaction.Commit();
    }

    // Read
    using (var transaction = session.BeginTransaction())
    {
        var product = session.Get<Product>(1);
        Console.WriteLine($"Product: {product.Name}, Price: {product.Price}");
    }

    // Update
    using (var transaction = session.BeginTransaction())
    {
        var product = session.Get<Product>(1);
        product.Price = 19.99M;
        session.Update(product);
        transaction.Commit();
    }

    // Delete
    using (var transaction = session.BeginTransaction())
    {
        var product = session.Get<Product>(1);
        session.Delete(product);
        transaction.Commit();
    }
}
```

## Queries

### HQL (Hibernate Query Language)

```csharp
using (var session = sessionFactory.OpenSession())
{
    var query = session.CreateQuery("from Product where Price > :price");
    query.SetParameter("price", 10.0M);
    var products = query.List<Product>();

    foreach (var product in products)
    {
        Console.WriteLine(product.Name);
    }
}
```

### LINQ

```csharp
using (var session = sessionFactory.OpenSession())
{
    var products = session.Query<Product>().Where(p => p.Price > 10.0M).ToList();

    foreach (var product in products)
    {
        Console.WriteLine(product.Name);
    }
}
```

### Criteria API

```csharp
using (var session = sessionFactory.OpenSession())
{
    var criteria = session.CreateCriteria<Product>()
                          .Add(Restrictions.Gt("Price", 10.0M));
    var products = criteria.List<Product>();

    foreach (var product in products)
    {
        Console.WriteLine(product.Name);
    }
}
```

## Transactions and Session Management

### Transactions

Ensure that database operations are atomic by using transactions:

```csharp
using (var session = sessionFactory.OpenSession())
{
    using (var transaction = session.BeginTransaction())
    {
        try
        {
            // Perform database operations
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
}
```

### Session Management

Sessions should be short-lived and tied to the request or unit of work:

```csharp
public class NHibernateSessionManager
{
    private static ISessionFactory sessionFactory;

    static NHibernateSessionManager()
    {
        var cfg = new Configuration();
        cfg.Configure();
        cfg.AddAssembly(typeof(Product).Assembly);
        sessionFactory = cfg.BuildSessionFactory();
    }

    public static ISession OpenSession()
    {
        return sessionFactory.OpenSession();
    }
}
```

## Best Practices

1. **Session Management**:
   - Open a session per request or unit of work.
   - Ensure sessions are short-lived and closed promptly.

2. **Lazy Loading**:
   - Enable lazy loading for collections to improve performance.

   ```xml
   <set name="Orders" table="Orders" lazy="true" fetch="select">
       <!-- collection mapping -->
   </set>
   ```

3. **Batching**:
   - Use batching to reduce the number of database round-trips.

   ```csharp
   var cfg = new Configuration();
   cfg.SetProperty(Environment.BatchSize, "20");
   ```

4. **Caching**:
   - Utilize first-level and second-level caching to optimize performance.

   ```csharp
   cfg.Cache(c =>
   {
       c.UseQueryCache = true;
       c.Provider<NHibernate.Caches.SysCache.SysCacheProvider>();
   });
   ```

5. **Exception Handling**:
   - Implement proper exception handling and rollback transactions on errors.

   ```csharp
   catch (Exception ex)
   {
       transaction.Rollback();
       Console.WriteLine($"Transaction failed: {ex.Message}");
   }
   ```

## Use Cases in Real-World Scenarios

1. **E-commerce Platform**:
   - NHibernate can manage product catalogs, orders, and customer information in an e-commerce application. Its support for lazy loading and caching can significantly enhance performance and scalability.

2. **Banking System**:
   - In a banking application, NHibernate can handle complex relationships and transactions between accounts, ensuring data integrity and consistency.

3. **Content Management System (CMS)**:
   - NHibernate can manage various content types and their relationships in a CMS, providing flexible querying and data manipulation capabilities.

4. **Enterprise Resource Planning (ERP)**:
   - NHibernate can handle the diverse and complex data structures found in ERP systems, offering robust ORM capabilities to manage business processes.

## Resources

- [NHibernate Official Documentation](https://nhibernate.info/doc/nhibernate-reference/index.html)
- [NHibernate GitHub Repository](https://github.com/nhibernate/nhibernate-core)
- [Dapper vs. NHibernate: Which is Better?](https://www.codeproject.com/Articles/5286638/Dapper-vs-NHibernate-Which-is-Better)
- [Best Practices for NHibernate](https://www.c-sharpcorner.com/UploadFile/ff2f08/best-practices-for-nhibernate/)