-- Problem 6: Get the number of vehicles made between 1950 and 2000 per make and add a total vehicles column.

-- Method 1: Subquery Approach
SELECT
    Makes.Make,
    COUNT(ID) AS VhichalesNumber,
    (SELECT COUNT(ID) FROM VehicleDetails WHERE Year BETWEEN 1950 AND 2000) AS TotalVhichales
FROM VehicleDetails
INNER JOIN Makes ON VehicleDetails.MakeID = Makes.MakeID
WHERE Year BETWEEN 1950 AND 2000
GROUP BY Makes.Make
ORDER BY VhichalesNumber DESC;

-- Explanation:
-- This query calculates the number of vehicles made between 1950 and 2000 for each make
-- and also provides the total number of vehicles within that time frame.
-- 1. We select the 'Make' from the 'Makes' table and count the number of vehicles in 'VehicleDetails' for each make.
-- 2. An INNER JOIN connects 'Makes' and 'VehicleDetails' based on the 'MakeID' column.
-- 3. We filter the results to consider only vehicles made between 1950 and 2000 using the WHERE clause.
-- 4. The GROUP BY clause groups the results by 'Make.'
-- 5. The query includes a subquery to calculate the total number of vehicles between 1950 and 2000.
-- 6. The results are ordered by the number of vehicles per make in descending order.


-------------------------------------------------------------------------------------------------------------------------------------

-- Method 2: Common Table Expression (CTE) Approach

-- Step 1: Create a CTE to calculate counts
WITH VehicleCounts AS (
    SELECT
        Makes.Make,
        COUNT(*) AS NumOfVehicles
    FROM
        VehicleDetails
    JOIN
        Makes ON VehicleDetails.MakeID = Makes.MakeID
    WHERE
        Year BETWEEN 1950 AND 2000
    GROUP BY
        Makes.Make
)

-- Step 2: Get the total count of vehicles within the specified time frame
SELECT
    Make,
    NumOfVehicles
FROM
    VehicleCounts
-- Step 3: Add the row for the total vehicles
UNION ALL
SELECT
    'Total Vehicles',
    SUM(NumOfVehicles)
FROM
    VehicleCounts;

-- Explanation:
-- Method 2 employs a cleaner and more organized approach using a Common Table Expression (CTE) for better readability.
-- Step 1: We create a CTE named 'VehicleCounts' to calculate the counts of vehicles made by each make between 1950 and 2000.
-- Step 2: The main query selects results from the CTE and displays the count for each make.
-- Step 3: We add an additional row labeled 'Total Vehicles' to show the sum of all vehicles made within the specified time frame.