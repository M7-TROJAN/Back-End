
-- [1]
-- Create a report that shows the CategoryName and Description 
-- from the categories table sorted by CategoryName.

-- [2]
-- Create a report that shows the ContactName, CompanyName, ContactTitle and Phone number 
-- from the customers table sorted by ContactTitle DESC


-- [3]
-- Create a report that shows the capitalized FirstName and 
-- capitalized LastName renamed as FirstName and Lastname 
-- respectively and HireDate from the employees table sorted 
-- from the newest to the oldest employee.


-- [4]
-- Create a report that shows the top 10 OrderID, OrderDate, ShippedDate, CustomerID, Freight 
-- from the orders table sorted by Freight in descending order.


-- [5]
-- Create a report that shows all the CustomerID in lowercase 
-- letter and renamed as ID from the customers table.

			
-- [6]
-- Create a report that shows the CompanyName, Fax, Phone, Country, HomePage 
-- from the suppliers table sorted by the Country in descending order then by 
-- CompanyName in ascending order.


-- [7]
-- Create a report that shows CompanyName, ContactName of all customers from 'Buenos Aires' only.


-- [8]
-- Create a report showing ProductName, UnitPrice, QuantityPerUnit of 
-- products that are out of stock.

-- [9]
-- Create a report showing all the ContactName, Address, City 
-- of all customers not from Germany, Mexico, Spain.


-- [10]
-- Create a report showing OrderDate, ShippedDate, CustomerID, Freight of 
-- all orders placed on 04 Feb 1997.

	
-- [11]
-- Create a report showing FirstName, LastName, Country from the employees not from United States.


-- [12]
-- Create a report that shows the EmployeeID, OrderID, CustomerID, RequiredDate, 
-- ShippedDate from all orders shipped later than the required date.


-- [13]
-- Create a report that shows the City, CompanyName, ContactName of customers from 
-- cities starting with A or B.


-- [14]
-- Create a report showing all the even numbers of OrderID from the orders table


-- [15]
-- Create a report that shows all the orders where the freight cost more than $500

	 
-- [16]
-- Create a report that shows ProductName, UnitsInStock, UnitsOnOrder, ReorderLevel  when 
-- inventory items on hand quantity are below the reorder level.


-- [17]
-- Create a report that shows the CompanyName, ContactName, Phone number of 
-- all customer that have no fax number

-- [18]
--  Create a report that shows the FirstName, LastName of all employees 
-- that do not report to anybody


-- [19]
-- Create a report showing Products that we don't sell any more


-- [20]
-- Create a report that shows the CompanyName, ContactName
-- Fax of all customers that do not have Fax number AND Live in USA and sorted by ContactName.


-- [21]
-- Create a report that shows the City, CompanyName, ContactName of customers from cities that has 
-- letter L in the name sorted by ContactName.


-- [22]
--  Create a report that shows the FirstName, LastName, BirthDate of employees born in the 1950s.


-- [23]
-- Create a report that shows the FirstName, LastName, the year of Birthdate as 
-- birth year from the employees table.


-- [24]
-- Create a report showing OrderID, total number of items in the order as NumberofItems 
-- from the orderdetails table grouped by OrderID and sorted by NumberofOrders in descending order. 

 
-- [25]
-- Create a report that shows the SupplierID, ProductName, CompanyName 
-- from all product Supplied by 
-- Exotic Liquids, Specialty Biscuits, Ltd., Escargots Nouveaux sorted by the supplier ID 


-- [26]
-- Create a report that shows the 
-- ShipPostalCode, OrderID, OrderDate, RequiredDate, ShippedDate, ShipAddress of all orders
-- with ShipPostalCode ends with "00"


-- [27]
--  Create a report that shows the ContactName, ContactTitle, CompanyName of customers that 
-- the has no "Sales" in their ContactTitle.


-- [28]
--  Create a report that shows the LastName, FirstName, City of employees 
-- in cities other "Seattle";


-- [29]
--  Create a report that shows the CompanyName, ContactTitle, City, Country of all 
-- customers in any city in Mexico or other cities in Spain other than Madrid.


-- [30]
-- Create a select statement that outputs the following:

   
-- [31]
-- Create a report that shows the ContactName of all customers that do not have letter A as the second alphabet 
-- in their Contactname.


-- [32] 
--  Create a report that shows the average UnitPrice rounded to the next whole number, 
-- total of UnitsInStock and maximum number of pending orders from the products table. 
-- All saved as AveragePrice, TotalStock and MaxPendingOrders respectively.


-- [33]
-- Create a report that shows the SupplierID, CompanyName, CategoryName, ProductName and UnitPrice 


-- [34]
-- Create a report that shows the sum of Freight for each CustomerID, 
-- when sum of freight greater $200


-- [35]
-- Create a report that shows the OrderID ContactName, UnitPrice, Quantity
-- with discount given on every purchase.

-- [36]
-- Create a report that shows the EmployeeID, the LastName and FirstName as employee, 
-- and the LastName and FirstName of who they report to as manager sorted by Employee ID

-- [37]
-- Create a report that shows ProductId, ProductName, Active if discontinued is 0 otherwise 
-- InActive and name the column Status.


-- [38]
-- Create a view named CustomerInfo that shows the 
-- CustomerID, CompanyName, ContactName, ContactTitle, Address, 
-- City, Country, Phone, OrderDate, RequiredDate, ShippedDate.


-- Display the content of the created view for validation

		
-- [39]
-- Change the name of the view you created from customerinfo to customerdetails.


-- [40]
-- Create a view named ProductDetails that shows the 
-- ProductID, CompanyName, ProductName, CategoryName, 
-- Description, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued 


-- Display the content of the created view for validation


-- [41] 
-- Drop the customer details view.


-- [42] 
-- Create a report that fetch the first 5 character of categoryName from the category 
-- tables and renamed as ShortInfo

	 
-- [43] 
-- Create a copy of the shipper table as shippers_duplicate. 
-- Then insert a copy of shippers data into the new table HINT: 
-- reate a Table, use the LIKE Statement and INSERT INTO statement.


-- [44] 
-- ADD a cloumn to shippers_duplicate table called email of type varchar(150)
-- update each shipper email with random email to each shipper
-- and Display the data in the shippers_duplicate table


-- [45] 
-- Create a report that shows the CompanyName and ProductName from all product 
-- in the Seafood category.

		
-- [46]
-- Create a report that shows most expensive and least expensive Product list (name and unit price)

-- [47]
-- Delete the shippers_duplicate table.


-- [48]
-- Create a select statement that ouputs the following from the employees table.
-- LastName, FirstName, Title, Age


-- [49]
-- Create a report that the CompanyName and total number of orders by customer
-- renamed as number of orders since Decemeber 31, 1994. Show number of Orders greater than 10.


-- [50]
-- Create a select statement that ouputs the following from the product table 
-- {ProductName} / is {quantityperunit} and cost ${unitprice}
