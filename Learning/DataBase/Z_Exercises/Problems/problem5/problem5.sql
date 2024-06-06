-- Problem 5: Get all makes that have manufactured more than 12,000 vehicles between 1950 and 2000.

-- Solution Using HAVING
SELECT Makes.Make, COUNT(VehicleDetails.ID) AS NumberOfVehicles
FROM VehicleDetails 
INNER JOIN Makes ON Makes.MakeID = VehicleDetails.MakeID
WHERE VehicleDetails.Year BETWEEN 1950 AND 2000
GROUP BY Makes.Make
HAVING COUNT(VehicleDetails.ID) > 12000
ORDER BY NumberOfVehicles DESC;

-- Explanation:
-- 1. We select the 'Make' from the 'Makes' table and count the number of vehicles in 'VehicleDetails.'
-- 2. We use an INNER JOIN to connect 'Makes' and 'VehicleDetails' on the 'MakeID' column.
-- 3. We filter the results to consider only vehicles made between 1950 and 2000.
-- 4. The GROUP BY clause groups the results by 'Make.'
-- 5. The HAVING clause filters the results to include only those with more than 12000 vehicles.
-- 6. We order the results by the number of vehicles in descending order.

-- Solution Without Using HAVING

-- Method 1
SELECT * FROM (
    SELECT Makes.Make, COUNT(VehicleDetails.ID) AS NumberOfVehicles
    FROM VehicleDetails 
    INNER JOIN Makes ON Makes.MakeID = VehicleDetails.MakeID
    WHERE VehicleDetails.Year BETWEEN 1950 AND 2000
    GROUP BY Makes.Make
) AS Result WHERE Result.NumberOfVehicles > 12000 ORDER BY Result.NumberOfVehicles DESC;


-- Method 2
-- Create a common table expression to calculate the count and order by it
WITH VehicleCounts AS (
    SELECT Makes.Make, COUNT(VehicleDetails.ID) AS NumberOfVehicles
    FROM VehicleDetails 
    INNER JOIN Makes ON Makes.MakeID = VehicleDetails.MakeID
    WHERE VehicleDetails.Year BETWEEN 1950 AND 2000
    GROUP BY Makes.Make
)
-- Select results from the CTE and apply the ORDER BY clause
SELECT *
FROM VehicleCounts
WHERE NumberOfVehicles > 12000
ORDER BY NumberOfVehicles DESC;


-- Method 1 Explanation:
-- This method uses a subquery and a WHERE clause to filter and count vehicles made between 1950 and 2000 for each make.
-- 1. We select the 'Make' from the 'Makes' table and count the number of vehicles in 'VehicleDetails' for each make.
-- 2. An INNER JOIN connects 'Makes' and 'VehicleDetails' based on the 'MakeID' column.
-- 3. We filter the results to consider only vehicles made between 1950 and 2000 using the WHERE clause.
-- 4. The GROUP BY clause groups the results by 'Make,' ensuring counts are calculated for each make.
-- 5. In the outer query, we filter the results again, selecting only those with more than 12,000 vehicles.
-- 6. Finally, we order the results by the number of vehicles in descending order.

-- Method 2 Explanation: Common Table Expression (CTE)
-- In this method, a Common Table Expression (CTE) is used to calculate the count of vehicles manufactured between 1950 and 2000 for each make.
-- 1. We select the 'Make' from the 'Makes' table and count the number of vehicles in 'VehicleDetails' for each make.
-- 2. An INNER JOIN connects 'Makes' and 'VehicleDetails' based on the 'MakeID' column.
-- 3. We filter the results to consider only vehicles made between 1950 and 2000 using the WHERE clause.
-- 4. The GROUP BY clause groups the results by 'Make,' ensuring counts are calculated for each make.
-- 5. A CTE named 'VehicleCounts' is created to store these results for better readability.
-- 6. In the final query, we select results from the CTE and apply the ORDER BY clause.
-- 7. We filter the results to include only those with more than 12,000 vehicles.
-- 8. Finally, we order the results by the number of vehicles in descending order.