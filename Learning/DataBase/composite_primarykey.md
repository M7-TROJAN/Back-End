To modify the existing table `OrderDetails` and remove the `Id` column while making `OrderId` and `ProductId` a composite primary key, you can follow these steps:

1. **Drop the existing primary key constraint (if any).**
2. **Drop the `Id` column.**
3. **Add a composite primary key consisting of `OrderId` and `ProductId`.**

Here is the SQL script to achieve this:

```sql
-- Step 1: Drop the existing primary key constraint
ALTER TABLE [Sales].[OrderDetails]
DROP CONSTRAINT [PK_OrderDetails]; -- Adjust the constraint name if necessary

-- Step 2: Drop the Id column
ALTER TABLE [Sales].[OrderDetails]
DROP COLUMN [Id];

-- Step 3: Add a composite primary key
ALTER TABLE [Sales].[OrderDetails]
ADD CONSTRAINT [PK_OrderDetails] PRIMARY KEY (OrderId, ProductId);
```

Here is a complete example:

```sql
-- Begin transaction
BEGIN TRANSACTION;

-- Step 1: Drop the existing primary key constraint
-- If you don't know the constraint name, you can find it using the query below
-- SELECT name
-- FROM sys.key_constraints
-- WHERE type = 'PK'
-- AND OBJECT_NAME(parent_object_id) = 'OrderDetails';

-- Assuming the primary key constraint name is PK_OrderDetails
ALTER TABLE [Sales].[OrderDetails]
DROP CONSTRAINT [PK_OrderDetails];

-- Step 2: Drop the Id column
ALTER TABLE [Sales].[OrderDetails]
DROP COLUMN [Id];

-- Step 3: Add a composite primary key
ALTER TABLE [Sales].[OrderDetails]
ADD CONSTRAINT [PK_OrderDetails] PRIMARY KEY (OrderId, ProductId);

-- Commit transaction
COMMIT;
```

### Important Considerations:
1. **Backup Your Data**: Always ensure you have a backup of your data before performing schema modifications.
2. **Foreign Key Constraints**: If other tables reference the `Id` column as a foreign key, you'll need to update those tables and relationships accordingly.
3. **Application Impact**: Ensure that your application logic does not depend on the `Id` column before making this change.

### Example with Transaction Handling:

```sql
BEGIN TRANSACTION;

BEGIN TRY
    -- Step 1: Drop the existing primary key constraint
    ALTER TABLE [Sales].[OrderDetails]
    DROP CONSTRAINT [PK_OrderDetails];

    -- Step 2: Drop the Id column
    ALTER TABLE [Sales].[OrderDetails]
    DROP COLUMN [Id];

    -- Step 3: Add a composite primary key
    ALTER TABLE [Sales].[OrderDetails]
    ADD CONSTRAINT [PK_OrderDetails] PRIMARY KEY (OrderId, ProductId);

    -- Commit the transaction if everything is successful
    COMMIT;
    PRINT 'Table structure updated successfully.';
END TRY
BEGIN CATCH
    -- Rollback the transaction in case of error
    ROLLBACK;
    PRINT 'Error occurred: ' + ERROR_MESSAGE();
END CATCH;
```

 updated SQL script:

```sql
BEGIN TRANSACTION;

BEGIN TRY
    -- Step 1: Get the name of the existing primary key constraint
    DECLARE @PkConstraint NVARCHAR(128);
    SELECT @PkConstraint = name
    FROM sys.key_constraints
    WHERE type = 'PK'
    AND OBJECT_NAME(parent_object_id) = 'OrderDetails';

    -- Step 2: Drop the existing primary key constraint using dynamic SQL
    DECLARE @sql NVARCHAR(MAX);
    SET @sql = 'ALTER TABLE [Sales].[OrderDetails] DROP CONSTRAINT ' + @PkConstraint;
    EXEC sp_executesql @sql;

    -- Step 3: Drop the Id column
    ALTER TABLE [Sales].[OrderDetails]
    DROP COLUMN [Id];

    -- Step 4: Add a composite primary key
    ALTER TABLE [Sales].[OrderDetails]
    ADD CONSTRAINT [PK_OrderDetails] PRIMARY KEY (OrderId, ProductId);

    -- Commit the transaction if everything is successful
    COMMIT;
    PRINT 'Table structure updated successfully.';
END TRY
BEGIN CATCH
    -- Rollback the transaction in case of error
    ROLLBACK;
    PRINT 'Error occurred: ' + ERROR_MESSAGE();
END CATCH;
```

### Explanation:
1. **Get the Name of the Existing Primary Key Constraint**: The script retrieves the name of the existing primary key constraint on the `OrderDetails` table and stores it in a variable `@PkConstraint`.
2. **Drop the Existing Primary Key Constraint Using Dynamic SQL**: Since the constraint name is stored in a variable, the `ALTER TABLE` statement must be constructed dynamically and executed using `sp_executesql`.
3. **Drop the `Id` Column**: The script drops the `Id` column from the `OrderDetails` table.
4. **Add a Composite Primary Key**: A new composite primary key is added on the `OrderId` and `ProductId` columns.
5. **Transaction Handling**: The entire operation is wrapped in a transaction to ensure atomicity. If any step fails, the transaction is rolled back to maintain data integrity.
6. **Error Handling**: The `BEGIN TRY...END TRY` and `BEGIN CATCH...END CATCH` blocks catch any errors that occur during the execution and print an error message.

This script ensures that the primary key constraint is dropped, the `Id` column is removed, and the composite primary key on `OrderId` and `ProductId` is added, all within a single transaction.


This script will help you modify your `OrderDetails` table by removing the `Id` column and creating a composite primary key on `OrderId` and `ProductId`.
