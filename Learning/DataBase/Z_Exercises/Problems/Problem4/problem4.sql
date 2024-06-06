-- Problem 4: Get the number of vehicles made between 1950 and 2000 per make 
-- and order them by the number of vehicles descending.

-- Solution Using 'INNER JOIN' and GROUP BY
SELECT Makes.Make, COUNT(VehicleDetails.ID) AS NumberOfVehicles
FROM VehicleDetails 
INNER JOIN Makes ON Makes.MakeID = VehicleDetails.MakeID
WHERE VehicleDetails.Year BETWEEN 1950 AND 2000
GROUP BY Makes.Make
ORDER BY NumberOfVehicles DESC;

-- Explanation:
-- 1. We select the 'Make' from the 'Makes' table and count the number of vehicles in 'VehicleDetails.'
-- 2. We use an INNER JOIN to connect 'Makes' and 'VehicleDetails' on the 'MakeID' column.
-- 3. We filter the results to consider only vehicles made between 1950 and 2000.
-- 4. The GROUP BY clause groups the results by 'Make.'
-- 5. We order the results by the number of vehicles in descending order.

-- Solution Without Using 'INNER JOIN' and GROUP BY
SELECT Makes.Make,
    'Number of Vehicles' = (
        SELECT COUNT(*)
        FROM VehicleDetails
        WHERE MakeID = Makes.MakeID AND Year BETWEEN 1950 AND 2000
    )
FROM Makes
ORDER BY 'Number of Vehicles' DESC;

-- Explanation:
-- 1. We select the 'Make' from the 'Makes' table.
-- 2. We use a subquery to count the number of vehicles in 'VehicleDetails' for each 'Make.'
-- 3. The subquery is correlated with the outer query using 'MakeID.'
-- 4. We order the results by the 'number of vehicles' in descending order.
