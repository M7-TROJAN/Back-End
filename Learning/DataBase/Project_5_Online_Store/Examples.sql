-- This File Contains Some Example Queries for Using the OnlineStore Database Schema
-- These queries demonstrate how to perform various operations with the database, such as updating customer information,
-- retrieving orders, calculating sales, and more. You can use these examples as a reference to understand and work with the database.

-- Example 1: Updating Customer Information
-- Update customer information
UPDATE Customers
SET Phone = '1234567890', Address = '456 Elm St'
WHERE CustomerName = 'Mahmoud Mohamed';


-- Example 2: Retrieving Orders for a Specific Customer
-- Retrieve orders for a specific customer
SELECT Orders.OrderID, Orders.OrderDate, ProductCatalog.ProductName, OrderItems.Quantity, OrderItems.Price
FROM Orders
JOIN OrderItems ON Orders.OrderID = OrderItems.OrderID
JOIN ProductCatalog ON OrderItems.ProductID = ProductCatalog.ProductID
WHERE Orders.CustomerID = (SELECT CustomerID FROM Customers WHERE CustomerName = 'Mahmoud Mohamed');


-- Example 3: Calculating Total Sales for Each Product Category
-- Calculate total sales for each product category
SELECT ProductCategory.CategoryName, SUM(OrderItems.Quantity) AS TotalQuantitySold, SUM(OrderItems.TotalItemsPrice) AS TotalSalesAmount
FROM ProductCategory
LEFT JOIN ProductCatalog ON ProductCategory.CategoryID = ProductCatalog.CategoryID
LEFT JOIN OrderItems ON ProductCatalog.ProductID = OrderItems.ProductID
GROUP BY ProductCategory.CategoryName;


-- Example 4: Retrieving the Latest Reviews with Customer Information
-- Retrieve the latest reviews along with customer information
SELECT Customers.CustomerName, Reviews.ReviewText, Reviews.Rating, Reviews.ReviewDate
FROM Reviews
JOIN Customers ON Reviews.CustomerID = Customers.CustomerID
ORDER BY Reviews.ReviewDate DESC
LIMIT 5; -- This limit the results to the latest 5 reviews


-- Example 5: Finding Products with Low Stock
-- Find products with low stock
SELECT ProductCatalog.ProductName, ProductCatalog.QuantityInStock
FROM ProductCatalog
WHERE ProductCatalog.QuantityInStock < 10;


-- Example 6: Retrieving Orders by Shipping Status
-- Retrieve orders by shipping status
SELECT Orders.OrderID, Orders.OrderDate, OrderStatus.StatusName
FROM Orders
JOIN OrderStatus ON Orders.StatusID = OrderStatus.StatusID;

-- Example 7: Retrieving Product Details with Images
-- Retrieve product details along with their associated images
SELECT ProductCatalog.ProductName, ProductCatalog.Description, ProductCatalog.Price, ProductImages.ImageURL
FROM ProductCatalog
JOIN ProductImages ON ProductCatalog.ProductID = ProductImages.ProductID;


-- Example 8: Updating Order Status
-- Update the order status for a specific order
UPDATE Orders
SET StatusID = 3 -- Updating to "Delivered" status
WHERE OrderID = 1;


-- Example 9: Finding Best-Selling Products
-- Find the best-selling products based on the total quantity sold
SELECT TOP 5 ProductCatalog.ProductName, SUM(OrderItems.Quantity) AS TotalQuantitySold
FROM ProductCatalog
JOIN OrderItems ON ProductCatalog.ProductID = OrderItems.ProductID
GROUP BY ProductCatalog.ProductName
ORDER BY TotalQuantitySold DESC;


-- Example 10: Retrieving Orders with Customer and Shipping Information
-- Retrieve orders along with customer and shipping information
SELECT Orders.OrderID, Orders.OrderDate, Customers.CustomerName, Shippings.CarrierName, Shippings.TrackingNumber
FROM Orders
JOIN Customers ON Orders.CustomerID = Customers.CustomerID
LEFT JOIN Shippings ON Orders.OrderID = Shippings.OrderID;


-- Example 11: Finding Customers with the Highest Total Spending
-- Find the top-spending customers based on the total amount spent
SELECT TOP 5 Customers.CustomerName, SUM(Orders.TotalAmount) AS TotalSpending
FROM Customers
JOIN Orders ON Customers.CustomerID = Orders.CustomerID
GROUP BY Customers.CustomerName
ORDER BY TotalSpending DESC;


-- Example 12: Retrieving Products by Category
-- Retrieve products within a specific category
SELECT ProductCatalog.ProductName, ProductCatalog.Description, ProductCatalog.Price
FROM ProductCatalog
WHERE ProductCatalog.CategoryID = 3; -- Change the CategoryID as needed.


-- Example 13: Finding Products with No Reviews
-- Find products that have not received any reviews
SELECT ProductCatalog.ProductName
FROM ProductCatalog
LEFT JOIN Reviews ON ProductCatalog.ProductID = Reviews.ProductID
WHERE Reviews.ReviewID IS NULL;


-- Example 14: Updating Product Price
-- Update the price of a specific product
UPDATE ProductCatalog
SET Price = 89.99
WHERE ProductName = 'Kitchen Blender';


-- Example 15: Retrieving Orders Within a Date Range
-- Retrieve orders placed within a specific date range
SELECT Orders.OrderID, Orders.OrderDate, Customers.CustomerName
FROM Orders
JOIN Customers ON Orders.CustomerID = Customers.CustomerID
WHERE Orders.OrderDate BETWEEN '2023-01-01' AND '2023-03-31';

-- Example 16: calculates the average rating for each product in the ProductCatalog Table
SELECT ProductCatalog.ProductID, AVG(Reviews.Rating) AS AverageRating
FROM ProductCatalog
JOIN Reviews ON ProductCatalog.ProductID = Reviews.ProductID
GROUP BY ProductCatalog.ProductID;


-- Add more examples and scenarios as needed, and customize the queries according to your specific requirements.
-- Best Regards, Mahmoud
