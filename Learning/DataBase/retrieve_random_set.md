To retrieve a random set of 10 records from a table containing 1000 records, you can use the `NEWID()` function in SQL Server. The `NEWID()` function generates a unique value for each row, which can then be used to order the rows randomly.

Here's how you can do it:

### Query

```sql
SELECT TOP 10 *
FROM YourTableName
ORDER BY NEWID();
```

### Explanation

1. **`SELECT TOP 10 *`**:
   - This part of the query selects the top 10 rows from the result set.

2. **`FROM YourTableName`**:
   - Replace `YourTableName` with the actual name of your table.

3. **`ORDER BY NEWID()`**:
   - The `ORDER BY NEWID()` clause sorts the rows in a random order because `NEWID()` generates a unique identifier for each row, which is different every time you execute the query.

### Example

Assume you have a table named `Employees`. Here’s the query:

```sql
SELECT TOP 10 *
FROM Employees
ORDER BY NEWID();
```

### Considerations

- **Performance**: For very large tables, using `NEWID()` can be resource-intensive as it requires generating and sorting unique identifiers for each row. For tables with 1000 records, this should perform reasonably well.
- **Reproducibility**: Each execution of the query will return a different set of 10 random records due to the randomness of `NEWID()`.

### Using a Subquery for Larger Data Sets

If performance is a concern, especially with significantly larger data sets, you might consider other techniques such as using a subquery or other randomization methods.

```sql
-- Assuming there is a unique column like EmployeeID
SELECT *
FROM (
    SELECT *,
           ROW_NUMBER() OVER (ORDER BY NEWID()) AS RowNum
    FROM Employees
) AS RandomizedEmployees
WHERE RowNum <= 10;
```

This query:
1. Uses `ROW_NUMBER()` with `ORDER BY NEWID()` to assign a random row number to each record.
2. Selects the top 10 records based on the randomly assigned row number.

These queries will help you retrieve a random set of 10 records from a table containing 1000 records in SQL Server.
