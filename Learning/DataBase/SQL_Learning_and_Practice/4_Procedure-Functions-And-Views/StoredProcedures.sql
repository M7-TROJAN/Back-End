-- Creating a stored Procedure Syntax:

CREATE OR ALTER PROCEDURE ProcedureName
    @Parameter1 DataType, -- Define input parameter 1
    @Parameter2 DataType, -- Define input parameter 2
    @Parameter3 DataType, -- Define input parameter 3
    --... (Continue defining additional parameters as needed)
AS
BEGIN
    -- SQL statements and logic for the procedure go here
END


-- Example:

-- Creating or altering a stored procedure to retrieve sales by category and year
CREATE OR ALTER PROCEDURE dbo.SalesByCategoryAndYear (
    @categoryName nvarchar(15), -- Input parameter: Category name
    @year int, -- Input parameter: Year
    @count int OUTPUT -- Output parameter to store the count of distinct products
)
AS
BEGIN
    -- PRODUCT | TOTAL: Retrieve total purchases of products in the specified category and year
	-- total purchases Means إجمالي المشتريات
    SELECT P.ProductName,
           ROUND(SUM(OD.Quantity * (1 - OD.Discount) * OD.UnitPrice), 0) AS TotalPurchase
    FROM
        [Order Details] OD
    INNER JOIN Orders O ON O.OrderID = OD.OrderID
    INNER JOIN Products P ON P.ProductID = OD.ProductID
    INNER JOIN Categories C ON C.CategoryID = P.CategoryID
    WHERE C.CategoryName = @categoryName AND YEAR(O.OrderDate) = @year
    GROUP BY ProductName
    ORDER BY ProductName;

    -- Set the output parameter @count to the count of distinct products returned by the query
    SELECT @count = @@ROWCOUNT;
END
GO

-- Execute the stored procedure with the provided category and year, and retrieve the output parameter value
DECLARE @count int;
EXEC dbo.SalesByCategoryAndYear 'Beverages', 1998, @count OUTPUT;

-- Display the count of distinct products for the specified category and year
SELECT @count AS ' Total Distinct Products ';



-- Explanation of the script:

-- 1- The stored procedure 'SalesByCategoryAndYear' takes in three parameters: 
-- '@categoryName' for the category name, '@year' for the year, 
-- 	and '@count' as an output parameter to store the count of distinct products.

-- 2- Inside the procedure, it performs a query to retrieve the total purchase amount for products 
-- in the specified category and year. It calculates the total purchase by considering quantity, discount, 
-- and unit price, grouping the results by product name.

-- 3- The '@@ROWCOUNT' system variable is used to capture the count of rows returned by the preceding SQL statement 
-- and assigns this count to the output parameter '@count'.

-- 4- It then executes the stored procedure by providing specific inputs for the category name and year. 
-- After execution, it retrieves the count of distinct products within that category and year.

-- 5- The 'SELECT @count' statement displays the count of distinct products as a result.