
# SQL Stored Procedures and Functions

## Stored Procedures

A **Stored Procedure** in SQL is a set of SQL statements that can be stored in the database and executed as a single unit. They are used to perform a variety of tasks like inserting, updating, or deleting records. Stored procedures allow for complex operations, including loops, conditions, and error handling.

### Example of a Stored Procedure
```sql
CREATE PROCEDURE AddNewEmployee
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @DepartmentID INT
AS
BEGIN
    INSERT INTO Employees (FirstName, LastName, DepartmentID)
    VALUES (@FirstName, @LastName, @DepartmentID);
END;
```

In this example, the stored procedure `AddNewEmployee` takes three parameters (`@FirstName`, `@LastName`, and `@DepartmentID`) and inserts a new employee record into the `Employees` table.

## Functions

A **Function** in SQL is a routine that returns a value or a table. Unlike stored procedures, functions are typically used for calculations and can be included in a `SELECT` statement. SQL functions can be categorized into two main types:

### 1. Scalar Functions

A scalar function returns a single value of a specified data type. This value is usually computed based on input parameters.

#### Example of a Scalar Function
```sql
CREATE FUNCTION GetEmployeeFullName
    (@EmployeeID INT)
RETURNS NVARCHAR(100)
AS
BEGIN
    DECLARE @FullName NVARCHAR(100);
    SELECT @FullName = FirstName + ' ' + LastName
    FROM Employees
    WHERE EmployeeID = @EmployeeID;
    
    RETURN @FullName;
END;
```

In this example, the function `GetEmployeeFullName` takes an `EmployeeID` as input and returns the full name of the employee by concatenating the `FirstName` and `LastName`.

### 2. Table-Valued Functions

A table-valued function returns a table, which can then be queried as a regular table.

#### Example of a Table-Valued Function
```sql
CREATE FUNCTION GetEmployeesByDepartment
    (@DepartmentID INT)
RETURNS TABLE
AS
RETURN
(
    SELECT EmployeeID, FirstName, LastName
    FROM Employees
    WHERE DepartmentID = @DepartmentID
);
```

In this example, the function `GetEmployeesByDepartment` returns a table of employees that belong to the specified department.

## Key Differences Between Stored Procedures and Functions

- **Usage:** Stored procedures are used for performing tasks (like data modification), while functions are primarily used for computations and returning data.
- **Return Type:** A stored procedure can return multiple values or no value at all, while a function always returns a single value or a table.
- **Calling:** Stored procedures are called using the `EXEC` statement, whereas functions are used within SQL statements like `SELECT`.
- **Transactions:** Stored procedures can contain multiple transactions and manage them, but functions do not support transaction management.

