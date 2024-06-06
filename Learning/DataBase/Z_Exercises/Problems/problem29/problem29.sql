-- Method 1: Using 'WHERE EXISTS'
-- checks if any vehicle was made in the year 1950. 
-- If exists, 'Found' is set to 1; otherwise, it Returns Null.
SELECT Found = 1 WHERE EXISTS (
    SELECT TOP 1 ID FROM VehicleDetails
    WHERE VehicleDetails.Year = 1950
)

-- Method 2: Using 'CASE END'
-- set 'Found' to 1 if any vehicle was made in the year 1950, 
-- and 0 if no vehicle Made in this year.
SELECT 
    CASE 
        WHEN EXISTS (SELECT TOP 1 ID FROM VehicleDetails WHERE VehicleDetails.Year = 1950) 
        THEN 1 
        ELSE 0 
    END 
AS Found;


-- Remember That there is no need to fetch all columns from the table , 
-- we can only fetch one column and gets correct results which is much faster even if we fetch one record