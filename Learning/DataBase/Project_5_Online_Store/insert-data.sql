-- Test Scripts for Database Verification Important 
-- Note: Execute Each Script Separately to Prevent Errors; Avoid Running All at Once Take Care, Bro

USE OnlineStore;


-- Insert product categories
INSERT INTO ProductCategory (CategoryName)
VALUES
    ('Electronics'),
    ('Clothing'),
    ('Home & Kitchen'),
    ('Books & Literature'),
    ('Toys & Games'),
    ('Health & Beauty'),
    ('Sports & Outdoors'),
    ('Food & Grocery'),
    ('Furniture'),
    ('Jewelry'),
    ('Automotive'),
    ('Music & Instruments'),
    ('Pets & Animals'),
    ('Arts & Crafts'),
    ('Office Supplies'),
    ('Travel & Luggage');

---------------------------------------------------------------------------------------------------------------------

-- Insert shipping status values
INSERT INTO OrderStatus
VALUES
    ('Processing'), -- Order is being processed
    ('Out for Delivery'), -- Order is out for delivery to the customer
    ('Delivered'), -- Order has been successfully delivered
    ('Return to Sender'), -- Package is being returned to the sender
    ('On Hold'), -- Order is on hold due to some issue Or something Else
    ('Delayed'), -- Order is experiencing delays in shipping
    ('Lost'); -- Order has been lost in transit

---------------------------------------------------------------------------------------------------------------------

-- Example to insert customers

INSERT INTO Customers(CustomerName, Email, Phone, Address, Username, Password)
VALUES ('Mahmoud Mohamed', 'Mahmoud_mohamed@gmail.com', '01019060452', '123 Main St', 'M7_123', 'Mm#0123'),
	   ('Mohamed Salah', 'MOSalah11@gmail.com', '01129816608', '123 Main ST', 'MO11','MO_@123');

---------------------------------------------------------------------------------------------------------------------

-- Example to insert product catalog information

INSERT INTO ProductCatalog (ProductName, Description, Price, QuantityInStock, CategoryID)
VALUES
    ('Smartphone X1', 'High-end smartphone with advanced features', 699.99, 100, 1), -- Category: Electronics
    ('Men''s Casual Shirt', 'Comfortable and stylish men''s shirt', 39.99, 200, 2), -- Category: Clothing
    ('Kitchen Blender', 'Powerful blender for smoothies and more', 79.99, 50, 3), -- Category: Home & Kitchen
    ('The Great Gatsby (Paperback)', 'Classic novel by F. Scott Fitzgerald', 12.99, 30, 4), -- Category: Books & Literature
    ('Lego Building Set', 'Creative building blocks for kids', 29.99, 100, 5), -- Category: Toys & Games
    ('Organic Shampoo', 'Natural hair care product for shiny hair', 14.99, 80, 6); -- Category: Health & Beauty


-- Insert product images
INSERT INTO ProductImages (ImageURL, ProductID, ImageOrder)
VALUES
    ('/images/product1-1.jpg', 1, 1),  -- Image 1 for ProductID 1 (Smartphone X1)
    ('/images/product1-2.jpg', 1, 2),  -- Image 2 for ProductID 1 (Smartphone X1)
    ('/images/product2-1.jpg', 2, 1),  -- Image 1 for ProductID 2 (Men's Casual Shirt)
    ('/images/product3-1.jpg', 3, 1),  -- Image 1 for ProductID 3 (Kitchen Blender)
    ('/images/product4-1.jpg', 4, 1),  -- Image 1 for ProductID 4 (The Great Gatsby)
    ('/images/product5-1.jpg', 5, 1),  -- Image 1 for ProductID 5 (Lego Building Set)
    ('/images/product6-1.jpg', 6, 1);  -- Image 1 for ProductID 6 (Organic Shampoo)

---------------------------------------------------------------------------------------------------------------------

-- Example To Initiate a Transaction for Order Processing
BEGIN TRY
    BEGIN TRANSACTION;

    -- Declare and initialize variables
    DECLARE @CustomerID INT, @OrderID INT, @ProductID INT, @Quantity INT, @Price SMALLMONEY, @TotalItemsPrice SMALLMONEY, @Amount SMALLMONEY;

    -- Retrieve the CustomerID for an existing customer
    SELECT @CustomerID = CustomerID FROM Customers WHERE Username = 'M7_123';

    -- Insert a new order
    INSERT INTO Orders (CustomerID, OrderDate, TotalAmount, StatusID)
    VALUES (
        @CustomerID, -- Customer placing the order
        GETDATE(), -- Date and time of the order placement
        NULL, -- Total amount initially set to NULL (to be updated)
        1 -- Initial status: Processing
    );

    -- Get the OrderID of the newly inserted order
    SELECT @OrderID = SCOPE_IDENTITY();

    -- Add a product to the order
    -- Product: Smartphone X1
    SELECT @ProductID = ProductID, @Quantity = 2, @Price = Price
    FROM ProductCatalog
    WHERE ProductName = 'Smartphone X1';

    SELECT @TotalItemsPrice = @Quantity * @Price;

    -- Insert product into the order
    INSERT INTO OrderItems (OrderID, ProductID, Quantity, Price, TotalItemsPrice)
    VALUES (@OrderID, @ProductID, @Quantity, @Price, @TotalItemsPrice);

    -- Add another product to the order
    -- Product: Kitchen Blender
    SELECT @ProductID = ProductID, @Quantity = 4, @Price = Price
    FROM ProductCatalog
    WHERE ProductName = 'Kitchen Blender';

    SELECT @TotalItemsPrice = @Quantity * @Price;

    -- Insert the second product into the order
    INSERT INTO OrderItems (OrderID, ProductID, Quantity, Price, TotalItemsPrice)
    VALUES (@OrderID, @ProductID, @Quantity, @Price, @TotalItemsPrice);

    -- Calculate and update the TotalAmount in the Orders table
    UPDATE Orders
    SET TotalAmount = (SELECT SUM(TotalItemsPrice) FROM OrderItems WHERE OrderID = @OrderID)
    WHERE OrderID = @OrderID;

    -- Insert the payment for the order
    SELECT @Amount = TotalAmount
    FROM Orders
    WHERE OrderID = @OrderID;

    INSERT INTO Payments (OrderID, Amount, PaymentMethod)
    VALUES (@OrderID, @Amount, 'VISA CARD');

    -- Insert shipping data
    INSERT INTO Shippings (OrderID, CarrierName, TrackingNumber, ShippingStatusID, EstimatedDeliveryDate, ActualDeliveryDate)
    VALUES (
        @OrderID,
        'UPS', -- CarrierName
        'UPSTracking123', -- TrackingNumber
        2, -- ShippingStatusID => Order is out for delivery to the customer
        '2023-11-10', -- EstimatedDeliveryDate
        NULL -- ActualDeliveryDate initially NULL
    );

	-- Update the Order's StatusID in the Orders table based on ShippingStatus
    UPDATE Orders
    SET StatusID = (SELECT ShippingStatusID FROM Shippings WHERE OrderID = @OrderID)
    WHERE OrderID = @OrderID;

    -- Commit the transaction
    COMMIT;
    PRINT 'Transaction committed successfully.';
END TRY
BEGIN CATCH
    -- If an error occurs, roll back the transaction and handle the error
    ROLLBACK;
    PRINT 'Transaction rolled back due to an error. Error Message: ' + ERROR_MESSAGE();
END CATCH;


---------------------------------------------------------------------------------------------------------------------

-- Insert sample reviews into the Reviews table
INSERT INTO Reviews (ProductID, CustomerID, ReviewText, Rating, ReviewDate)
VALUES
    (1, 1, 'Great smartphone! I love it.', 4.5, '2023-10-15 09:30:00'),
    (2, 2, 'Nice casual shirt. Comfortable to wear.', 4.0, '2023-10-16 14:15:00');

---------------------------------------------------------------------------------------------------------------------


-- Retrieve and display customer information
SELECT * FROM Customers;

-- Retrieve and display ProductCategory details
SELECT * FROM ProductCategory;

-- Retrieve and display ProductCatalog details
SELECT * FROM ProductCatalog;

-- Retrieve and display ProductImages records
SELECT * FROM ProductImages;

-- Retrieve and display OrderStatus records
SELECT * FROM Orderitems;

-- Retrieve and display Orders records
SELECT * FROM Orders;

-- Retrieve and display Reviews records
SELECT * FROM Reviews;

-- Retrieve and display Shippings records
SELECT * FROM Shippings;