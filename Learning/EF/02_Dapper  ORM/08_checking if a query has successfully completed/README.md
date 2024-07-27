In database operations, checking if a query has successfully completed is crucial for ensuring data integrity and handling errors. Here's a breakdown of best practices for checking if a query was successful:

### 1. **Check Affected Rows for Non-Query Operations**

For operations like `INSERT`, `UPDATE`, and `DELETE`, you can check the number of affected rows returned by the query. A non-zero value indicates that rows were affected, which implies that the operation was successful.

**Example:**

```csharp
var affectedRows = dbConnection.Execute(sqlQuery, parameters);

if (affectedRows > 0)
{
    Console.WriteLine($"{affectedRows} rows affected.");
}
else
{
    Console.WriteLine("No rows affected.");
}
```

### 2. **Check for Return Values**

For queries that return a value (e.g., `SELECT` queries), check if the returned value is valid. For instance, if you’re retrieving an identity value after an `INSERT`, ensure the returned ID is greater than zero.

**Example:**

```csharp
var newId = dbConnection.QuerySingle<int>(sqlQuery, parameters);

if (newId > 0)
{
    Console.WriteLine($"New record inserted with ID: {newId}");
}
else
{
    Console.WriteLine("Insert failed.");
}
```

### 3. **Use Transactions**

Encapsulate operations in a transaction to ensure atomicity. If any part of the transaction fails, you can roll back the entire operation. This helps in maintaining data consistency.

**Example:**

```csharp
using var transaction = dbConnection.BeginTransaction();
try
{
    var affectedRows = dbConnection.Execute(sqlQuery, parameters, transaction);

    if (affectedRows > 0)
    {
        transaction.Commit();
        Console.WriteLine($"{affectedRows} rows affected.");
    }
    else
    {
        transaction.Rollback();
        Console.WriteLine("No rows affected. Transaction rolled back.");
    }
}
catch (Exception ex)
{
    transaction.Rollback();
    Console.WriteLine($"An error occurred: {ex.Message}");
}
```

### 4. **Handle Exceptions**

Wrap database operations in try-catch blocks to handle exceptions that may occur during execution. This ensures that any errors are caught and can be logged or handled appropriately.

**Example:**

```csharp
try
{
    var affectedRows = dbConnection.Execute(sqlQuery, parameters);

    if (affectedRows > 0)
    {
        Console.WriteLine($"{affectedRows} rows affected.");
    }
    else
    {
        Console.WriteLine("No rows affected.");
    }
}
catch (SqlException ex)
{
    Console.WriteLine($"Database error: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
```

### 5. **Validate Results**

For queries that return data, validate the results to ensure they meet your expectations. For instance, if you expect a single record, make sure that only one record is returned.

**Example:**

```csharp
var result = dbConnection.QueryFirstOrDefault<Wallet>(sqlQuery, parameters);

if (result != null)
{
    Console.WriteLine($"Record found: {result}");
}
else
{
    Console.WriteLine("No record found.");
}
```

### Summary

1. **Affected Rows**: Check the number of affected rows for operations that modify data.
2. **Return Values**: Verify returned values, especially for operations that generate outputs (e.g., identity values).
3. **Transactions**: Use transactions to ensure atomic operations and maintain consistency.
4. **Exception Handling**: Wrap database operations in try-catch blocks to manage errors.
5. **Result Validation**: Validate query results to ensure they meet expected criteria.

By following these best practices, you can ensure that your database operations are reliable, consistent, and error-free.