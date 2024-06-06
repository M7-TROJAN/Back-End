
-- [1]
-- Create a report that shows the CategoryName and Description 
-- from the categories table sorted by CategoryName.
SELECT 
	CategoryName, 
	Description 
FROM Categories
ORDER BY CategoryName

-- [2]
-- Create a report that shows the ContactName, CompanyName, ContactTitle and Phone number 
-- from the customers table sorted by ContactTitle DESC
SELECT 
	ContactName,
	CompanyName,
	ContactTitle,
	Phone
FROM Customers
ORDER BY ContactTitle DESC

-- [3]
-- Create a report that shows the capitalized FirstName and 
-- capitalized LastName renamed as FirstName and Lastname 
-- respectively and HireDate from the employees table sorted 
-- from the newest to the oldest employee.
SELECT 
	UPPER(FirstName) AS FirstName,
	UPPER(LastName) AS LastName,
	HireDate
FROM Employees
ORDER BY HireDate DESC

-- [4]
-- Create a report that shows the top 10 OrderID, OrderDate, ShippedDate, CustomerID, Freight 
-- from the orders table sorted by Freight in descending order.
SELECT TOP 10
	OrderID, 
	OrderDate, 
	ShippedDate, 
	CustomerID, 
	Freight 
FROM Orders
ORDER BY Freight DESC

-- [5]
-- Create a report that shows all the CustomerID in lowercase 
-- letter and renamed as ID from the customers table.
SELECT 
	LOWER(CustomerID) AS ID
FROM Customers
			
-- [6]
-- Create a report that shows the CompanyName, Fax, Phone, Country, HomePage 
-- from the suppliers table sorted by the Country in descending order then by 
-- CompanyName in ascending order.
SELECT
	CompanyName, 
	Fax, 
	Phone, 
	Country, 
	HomePage
FROM Suppliers
ORDER BY  Country DESC, CompanyName ASC 

-- [7]
-- Create a report that shows CompanyName, ContactName of all customers from 'Buenos Aires' only.
 SELECT
	CompanyName, 
	ContactName
 FROM Customers
 WHERE City = 'Buenos Aires'
 ORDER BY CompanyName

-- [8]
-- Create a report showing ProductName, UnitPrice, QuantityPerUnit of 
-- products that are out of stock.
SELECT
	ProductName,
	UnitPrice,
	QuantityPerUnit
FROM Products
WHERE UnitsInStock = 0

-- [9]
-- Create a report showing all the ContactName, Address, City 
-- of all customers not from Germany, Mexico, Spain.
SELECT
	ContactName, 
	Address, 
	City
FROM Customers
WHERE Country NOT IN ('Germany', 'Mexico', 'Spain')

-- [10]
-- Create a report showing OrderDate, ShippedDate, CustomerID, Freight of 
-- all orders placed on 04 Feb 1997.
SELECT
	OrderDate, 
	ShippedDate, 
	CustomerID, 
	Freight
FROM Orders
WHERE OrderDate = '1997-02-04';
	
-- [11]
-- Create a report showing FirstName, LastName, Country from the employees not from United States.
SELECT
	FirstName, 
	LastName, 
	Country
FROM Employees
WHERE Country <> 'USA'

-- [12]
-- Create a report that shows the EmployeeID, OrderID, CustomerID, RequiredDate, 
-- ShippedDate from all orders shipped later than the required date.
SELECT
	EmployeeID, 
	OrderID, 
	CustomerID,
	OrderDate,
	RequiredDate,
	ShippedDate
FROM Orders
WHERE ShippedDate > RequiredDate

-- [13]
-- Create a report that shows the City, CompanyName, ContactName of customers from 
-- cities starting with A or B.
SELECT
	City, 
	CompanyName, 
	ContactName
FROM Customers
WHERE City LIKE 'A%' OR City LIKE 'B%'

-- [14]
-- Create a report showing all the even numbers of OrderID from the orders table
SELECT OrderID
FROM Orders
WHERE OrderID % 2 = 0;

-- [15]
-- Create a report that shows all the orders where the freight cost more than $500
 SELECT *
 FROM Orders
WHERE Freight > 500;
	 
-- [16]
-- Create a report that shows ProductName, UnitsInStock, UnitsOnOrder, ReorderLevel  when 
-- inventory items on hand quantity are below the reorder level.
SELECT 
	ProductName, 
	UnitsInStock, 
	UnitsOnOrder, 
	ReorderLevel
FROM Products
WHERE ReorderLevel > UnitsInStock

-- [17]
-- Create a report that shows the CompanyName, ContactName, Phone number of 
-- all customer that have no fax number
 SELECT
	CompanyName, 
	ContactName,
	Phone
FROM Customers
WHERE Fax IS NULL;

-- [18]
--  Create a report that shows the FirstName, LastName of all employees 
-- that do not report to anybody
SELECT
	FirstName, 
	LastName
FROM Employees
WHERE ReportsTo IS NULL

-- [19]
-- Create a report showing Products that we don't sell any more
SELECT * FROM Products
WHERE Discontinued = 1;

-- [20]
-- Create a report that shows the CompanyName, ContactName
-- Fax of all customers that do not have Fax number AND Live in USA and sorted by ContactName.
SELECT
	CompanyName, 
	ContactName,
	Fax
FROM Customers
WHERE (Fax IS NULL) AND (Country = 'USA')
ORDER BY ContactName

-- [21]
-- Create a report that shows the City, CompanyName, ContactName of customers from cities that has 
-- letter L in the name sorted by ContactName.
SELECT
	ContactName,
	City, 
	CompanyName 
FROM Customers
WHERE City LIKE '%L%'
ORDER BY ContactName

-- [22]
--  Create a report that shows the FirstName, LastName, BirthDate of employees born in the 1950s.
SELECT 
	FirstName, 
	LastName, 
	BirthDate
FROM Employees
-- WHERE YEAR(BirthDate) = 1950
WHERE BirthDate BETWEEN '1950-01-01' AND '1950-12-31'

-- [23]
-- Create a report that shows the FirstName, LastName, the year of Birthdate as 
-- birth year from the employees table.
SELECT 
	FirstName, 
	LastName, 
	YEAR(BirthDate) AS 'birth year'
FROM Employees

-- [24]
-- Create a report showing OrderID, total number of items in the order as NumberofItems 
-- from the orderdetails table grouped by OrderID and sorted by NumberofOrders in descending order. 
SELECT 
	OrderID,
	Count(*) AS NumberofItems
FROM [Order Details]
GROUP BY OrderID
ORDER BY NumberofItems DESC;
 
-- [25]
-- Create a report that shows the SupplierID, ProductName, CompanyName 
-- from all product Supplied by 
-- Exotic Liquids, Specialty Biscuits, Ltd., Escargots Nouveaux sorted by the supplier ID 
 SELECT 
	S.SupplierID, 
	P.ProductName, 
	S.CompanyName
 FROM Suppliers S 
 INNER JOIN Products P ON S.SupplierID = P.SupplierID
 WHERE S.CompanyName in ('Exotic Liquids', 'Specialty Biscuits, Ltd.', 'Escargots Nouveaux')
 ORDER BY S.SupplierID;

-- [26]
-- Create a report that shows the 
-- ShipPostalCode, OrderID, OrderDate, RequiredDate, ShippedDate, ShipAddress of all orders
-- with ShipPostalCode ends with "00"
 SELECT 
	ShipPostalCode, 
	OrderID, 
	OrderDate, 
	RequiredDate,
	ShippedDate,
	ShipAddress
 FROM Orders
 WHERE ShipPostalCode LIKE '%00'

-- [27]
--  Create a report that shows the ContactName, ContactTitle, CompanyName of customers that 
-- the has no "Sales" in their ContactTitle.
SELECT 
	ContactName,
	ContactTitle, 
	CompanyName
FROM Customers
Where ContactTitle NOT LIKE '%Sales%';

-- [28]
--  Create a report that shows the LastName, FirstName, City of employees 
-- in cities other "Seattle";
SELECT
	LastName, 
	FirstName, 
	City
FROM Employees
WHERE City <> 'Seattle';

-- [29]
--  Create a report that shows the CompanyName, ContactTitle, City, Country of all 
-- customers in any city in Mexico or other cities in Spain other than Madrid.
SELECT 
	CompanyName, 
	ContactTitle, 
	City, 
	Country
FROM Customers
WHERE (Country IN ('Mexico', 'Spain')) AND (City <> 'Madrid');

-- [30]
-- Create a select statement that outputs the following:
 SELECT
 CONCAT(FirstName, ' ', LastName, ' Can Be Reached At ', 'X', Extension) AS ContactInfo
 FROM Employees
   
-- [31]
-- Create a report that shows the ContactName of all customers that do not have letter A as the second alphabet 
-- in their Contactname.
 SELECT ContactName FROM Customers
 WHERE ContactName NOT LIKE '_A%';

-- [32] 
--  Create a report that shows the average UnitPrice rounded to the next whole number, 
-- total of UnitsInStock and maximum number of pending orders from the products table. 
-- All saved as AveragePrice, TotalStock and MaxPendingOrders respectively.
SELECT
	ROUND(AVG(UnitPrice),0) AS AveragePrice,
	SUM(UnitsInStock) AS TotalStock,
	MAX(UnitsOnOrder) As MaxPendingOrders
FROM Products

-- [33]
-- Create a report that shows the SupplierID, CompanyName, CategoryName, ProductName and UnitPrice 
SELECT 
	S.SupplierID,
	S.CompanyName,
	C.CategoryName,
	P.ProductName,
	P.UnitPrice
FROM Suppliers S
INNER JOIN Products P ON P.SupplierID = S.SupplierID
INNER JOIN Categories C ON C.CategoryID = P.CategoryID

-- [34]
-- Create a report that shows the sum of Freight for each CustomerID, 
-- when sum of freight greater $200
SELECT 
	CustomerID,
	SUM(Freight)
FROM Orders
GROUP BY CustomerID
HAVING SUM(Freight) > 200;

-- [35]
-- Create a report that shows the OrderID ContactName, UnitPrice, Quantity
-- with discount given on every purchase.
SELECT 
	O.OrderID,
	C.ContactName, 
	OD.UnitPrice,
	OD.Quantity,
	OD.Discount
FROM ORDERS O
INNER JOIN Customers C ON C.CustomerID = O.CustomerID
INNER JOIN [Order Details] OD ON OD.OrderID = O.OrderID
WHERE OD.Discount <> 0;

-- [36]
-- Create a report that shows the EmployeeID, the LastName and FirstName as employee, 
-- and the LastName and FirstName of who they report to as manager sorted by Employee ID
SELECT 
    Emp.EmployeeID,
    CONCAT(Emp.FirstName, ' ', Emp.LastName) AS EmployeeName,
    CONCAT(Mang.FirstName, ' ', Mang.LastName) AS ManagerName
FROM Employees Emp
LEFT JOIN Employees Mang ON Emp.ReportsTo = Mang.EmployeeID
ORDER BY Emp.EmployeeID;

-- [37]
-- Create a report that shows ProductId, ProductName, Active if discontinued is 0 otherwise 
-- InActive and name the column Status.
SELECT
	ProductId, 
	ProductName,
	CASE
	WHEN Discontinued = 0 THEN 'Active'
	ELSE 'InActive'
	END AS Status
FROM Products

-- [38]
-- Create a view named CustomerInfo that shows the 
-- CustomerID, CompanyName, ContactName, ContactTitle, Address, 
-- City, Country, Phone, OrderDate, RequiredDate, ShippedDate.
CREATE VIEW CustomerInfo AS 
    SELECT
        C.CustomerID, C.CompanyName, C.ContactName, C.ContactTitle, C.Address, 
        C.City, C.Country, C.Phone, O.OrderDate, O.RequiredDate, O.ShippedDate
    FROM Customers C
    INNER JOIN Orders O ON O.CustomerID = C.CustomerID;

-- Display the content of the created view for validation
SELECT * FROM CustomerInfo;
		
-- [39]
-- Change the name of the view you created from customerinfo to customerdetails.
EXEC sp_rename 'dbo.CustomerInfo', 'customerDetails';

-- [40]
-- Create a view named ProductDetails that shows the 
-- ProductID, CompanyName, ProductName, CategoryName, 
-- Description, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued 
CREATE VIEW ProductDetails AS
	SELECT
		P.ProductID, S.CompanyName, P.ProductName, C.CategoryName, 
		C.Description, P.QuantityPerUnit, P.UnitPrice, P.UnitsInStock, 
		P.UnitsOnOrder, P.ReorderLevel, P.Discontinued 
	FROM Products P
	INNER JOIN Suppliers S ON S.SupplierID = P.SupplierID
	INNER JOIN Categories C ON C.CategoryID = P.CategoryID;

-- Display the content of the created view for validation
SELECT * FROM ProductDetails;

-- [41] 
-- Drop the customer details view.
DROP VIEW customerDetails;

-- [42] 
-- Create a report that fetch the first 5 character of categoryName from the category 
-- tables and renamed as ShortInfo
SELECT
	SUBSTRING(categoryName, 1, 5) AS ShortInfo
FROM Categories;
	 
-- [43] 
-- Create a copy of the shipper table as shippers_duplicate. 
-- Then insert a copy of shippers data into the new table HINT: 
-- reate a Table, use the LIKE Statement and INSERT INTO statement.
SELECT * INTO shippers_duplicate
FROM Shippers;

SELECT * FROM shippers_duplicate;



-- [44] 
-- ADD a cloumn to shippers_duplicate table called email of type varchar(150)
-- update each shipper email with random email to each shipper
-- and Display the data in the shippers_duplicate table
ALTER TABLE shippers_duplicate
ADD Email NVARCHAR(50);

UPDATE shippers_duplicate SET Email = 'Email1@fake.com' WHERE ShipperID = 1;
UPDATE shippers_duplicate SET Email = 'Email2@fake.com' WHERE ShipperID = 2;
UPDATE shippers_duplicate SET Email = 'Email3@fake.com' WHERE ShipperID = 3;



-- [45] 
-- Create a report that shows the CompanyName and ProductName from all product 
-- in the Seafood category.
 SELECT 
	CompanyName,
	ProductName
 FROM Products P
 INNER JOIN Suppliers S ON P.SupplierID = S.SupplierID
 INNER JOIN Categories C ON C.CategoryID = P.CategoryID
 WHERE c.CategoryName = 'Seafood'
		
-- [46]
-- Create a report that shows most expensive and least expensive Product list (name and unit price)
SELECT
	ProductName,
	UnitPrice
FROM Products
WHERE UnitPrice = (SELECT MIN(UnitPrice) FROM Products) 
	  OR UnitPrice = (SELECT MAX(UnitPrice) FROM Products);

-- [47]
-- Delete the shippers_duplicate table.
DROP TABLE shippers_duplicate;

-- [48]
-- Create a select statement that ouputs the following from the employees table.
-- LastName, FirstName, Title, Age
 SELECT
	LastName, FirstName, Title, 
	-- 365 Days = 8760 Hours
	-- 365.25   = 8766 Hours 
	DATEDIFF(YEAR, BirthDate, GETDATE()) AS Age,
	CONVERT(INT, ROUND(DATEDIFF(HOUR, BirthDate, GETDATE()) / 8766.0, 0)) AS ExactAge
FROM Employees

-- [49]
-- Create a report that the CompanyName and total number of orders by customer
-- renamed as number of orders since Decemeber 31, 1994. Show number of Orders greater than 10.
SELECT
	C.CompanyName,
	COUNT(O.OrderID) AS [number of orders]
FROM Customers C INNER JOIN Orders O ON O.CustomerID = C.CustomerID
WHERE O.OrderDate >= '1994-12-31'
GROUP BY C.CompanyName
HAVING COUNT(O.OrderID) > 10
ORDER BY [number of orders] DESC

-- [50]
-- Create a select statement that ouputs the following from the product table 
-- {ProductName} / is {quantityperunit} and cost ${unitprice}
SELECT 
	CONCAT(ProductName, ' / Is ', QuantityPerUnit, ' and cost $', UnitPrice) AS ProductDescription
FROM Products
