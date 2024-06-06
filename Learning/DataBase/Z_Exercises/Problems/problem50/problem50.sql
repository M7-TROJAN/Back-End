-- Problem 50: Get all Fuel Types in random order

-- Select all rows from the 'FuelTypes' table and randomize the order.
SELECT *
FROM FuelTypes
-- Use the NEWID() function to generate a random value for each row and sort the results by it.
-- This effectively randomizes the order in which the Fuel Types are displayed.
ORDER BY NEWID();