 -- Problem 7: Get number of vehicles made between 1950 and 2000 per make and add total vehicles column beside it, 
 -- then calculate it's percentage


 SELECT * , CAST(NumberOfVehicles AS FLOAT) / CAST (TotalVehicles AS FLOAT) AS PER FROM (
	SELECT 
		Makes.Make,
		COUNT(*) AS NumberOfVehicles,
		TotalVehicles = (
			SELECT COUNT(VehicleDetails.ID) 
			FROM VehicleDetails 
			WHERE VehicleDetails.Year BETWEEN 1950 AND 2000
		)
	FROM 
		VehicleDetails
	JOIN
		Makes ON Makes.MakeID = VehicleDetails.MakeID
	WHERE VehicleDetails.Year BETWEEN 1950 AND 2000
	GROUP BY Makes.Make
) Result
ORDER BY NumberOfVehicles DESC





-- Code With Explanation Comments:

-- Select data and calculate the percentage:
SELECT *,  -- Select data 
    CAST(NumberOfVehicles AS FLOAT) / CAST(TotalVehicles AS FLOAT) AS PER -- calculate the percentage
FROM (
    -- Step 1: Count the number of vehicles per make and get the total vehicles count.

    SELECT 
        Makes.Make,                       -- Select the make from the 'Makes' table.
        COUNT(*) AS NumberOfVehicles,     -- Count the number of vehicles for each make.
        TotalVehicles = (
            -- Subquery to calculate the total number of vehicles between 1950 and 2000.
            SELECT COUNT(VehicleDetails.ID) 
            FROM VehicleDetails 
            WHERE VehicleDetails.Year BETWEEN 1950 AND 2000
        )
    FROM 
        VehicleDetails                    -- Select data from the 'VehicleDetails' table.
    JOIN
        Makes ON Makes.MakeID = VehicleDetails.MakeID  -- Join 'Makes' and 'VehicleDetails' based on 'MakeID'.
    WHERE 
        VehicleDetails.Year BETWEEN 1950 AND 2000  -- Filter vehicles made between 1950 and 2000.
    GROUP BY Makes.Make                    -- Group the results by make.
) Result                                  -- Use 'Result' as an alias for the subquery.

ORDER BY NumberOfVehicles DESC;             -- Order the results by the number of vehicles in descending order.





-- Without Using Cast
SELECT
    Makes.Make, -- Select the make from the 'Makes' table.
    COUNT(*) AS Number, -- Count the number of vehicles for each make.
    (SELECT COUNT(VehicleDetails.ID) FROM VehicleDetails WHERE VehicleDetails.Year BETWEEN 1950 AND 2000) AS TOTAL, -- Calculate the total count of all vehicles.
    (COUNT(*) * 1.0 / (SELECT COUNT(VehicleDetails.ID) FROM VehicleDetails WHERE VehicleDetails.Year BETWEEN 1950 AND 2000)) AS Percentage
FROM VehicleDetails
LEFT JOIN Makes ON Makes.MakeID = VehicleDetails.MakeID  -- Left join 'Makes' and 'VehicleDetails' based on 'MakeID'.
WHERE VehicleDetails.Year BETWEEN 1950 AND 2000   -- Filter vehicles made between 1950 and 2000.
GROUP BY Makes.Make  -- Group the results by make.
ORDER BY Number DESC;  -- Order the results by the number of vehicles in descending order.






-- How to Calculate Percentage
-- Percentage is a way to represent a part as a proportion of the whole (total) in percentage.
-- Percentage = (Part / Whole) × 100

-- Where:
-- - Percentage is the percentage we are trying to calculate.
-- - Part is the value we want to know the percentage of.
-- - Whole is the total or the complete value we want to know how much the part represents.
-- - "× 100" is for converting the ratio to a percentage.
