-- OrderID | ProductId | UnitPrice | Quantity | Discount | SubTotal

SELECT
OD.OrderID,
OD.ProductId,
OD.UnitPrice,
OD.Quantity,
OD.Discount,
dbo.CalculateSubTotal(OD.Quantity, OD.UnitPrice, OD.Discount) AS SUBTOTAL
FROM [Order Details] OD 
WHERE OrderId = 10250;    

-- OrderID | CustomerId | OrderDate | Total

SELECT 
O.OrderID,
O.CustomerID,
O.OrderDate,
CONVERT(money , SUM((1 - OD.Discount) * (OD.UnitPrice * OD.Quantity)) , 2) As Total
FROM
Orders O INNER JOIN [Order Details] OD
ON O.OrderID = OD.OrderID
Where O.OrderId = 10250
GROUP BY
O.OrderID,
O.CustomerID,
O.OrderDate;


-- FUNCTIONS

-- 1 SCALAR FUNCTIONS (single value)

-- Function to calculate subtotal based on quantity, unit price, and discount
CREATE OR ALTER FUNCTION dbo.CalculateSubTotal(@quantity int, @unitPrice money, @discount real)
RETURNS money WITH SCHEMABINDING
AS
BEGIN
    DECLARE @subTotal money;
    SELECT @subTotal = CONVERT(money, (1 - @discount) * (@unitPrice * @quantity), 2);
    RETURN @subTotal;
END
GO

-- Function to calculate total order amount based on order ID
CREATE OR ALTER FUNCTION dbo.CalculateOrderTotal(@orderId int)
RETURNS money 
AS
BEGIN
    DECLARE @total money;
    SELECT @total = SUM(SUBTOTAL) FROM dbo.GetOrderDetails(@orderId);
    RETURN @total;
END
GO

-- 2 TABLE VALUED FUNCTIONS (result set)

-- Function to retrieve order details including subtotal based on order ID
CREATE OR ALTER FUNCTION GetOrderDetails(@orderId int)
RETURNS Table
AS
RETURN
( 
    SELECT
        OD.OrderID,
        OD.ProductId,
        OD.UnitPrice,
        OD.Quantity,
        OD.Discount,
        dbo.CalculateSubTotal(OD.Quantity, OD.UnitPrice, OD.Discount) AS SUBTOTAL
    FROM [Order Details] OD 
    WHERE OrderId = @orderId  
);

-- Example usage of GetOrderDetails function for order ID 10250
SELECT * FROM dbo.GetOrderDetails(10250);

-- Function to retrieve customer orders with total amount based on customer ID
CREATE OR ALTER FUNCTION GetCustomerOrders(@customerID nvarchar(50))
RETURNS Table
AS
RETURN
(
    SELECT 
        O.OrderID,
        O.CustomerID,
        O.OrderDate,
        dbo.CalculateOrderTotal(O.OrderID) AS Total
    FROM
        Orders O 
    WHERE 
        O.CustomerID = @customerID
);

-- Example usage of GetCustomerOrders function for customer 'ALFKI'
SELECT * FROM GetCustomerOrders('ALFKI');

-- 3 SYSTEM FUNCTIONS (Built-in functions)

-- Numeric Function 
SELECT ABS(-100);

-- Date Function
SELECT GETDATE();
SELECT ISDATE('2000-10-20');

-- String Function
SELECT LEN('this is some text');
SELECT UPPER('mahmoud');

-- Conversion Function
SELECT CONVERT(money, '100', 2);
