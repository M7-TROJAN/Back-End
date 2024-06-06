
-- Problem 22: Get the percentage of vehicles that have no doors specified

SELECT
    PercOfNoSpecifiedDoors = 
    CAST(
        (CAST(TotalWithNoSpecifiedDoors AS Float) / CAST(TotalVehicles AS Float)) * 100
        AS NVARCHAR
    ) + '%'
FROM (
    -- Calculate the total number of vehicles and the number of vehicles with no specified doors.
    SELECT
        COUNT(*) AS TotalVehicles,
        TotalWithNoSpecifiedDoors = (
            SELECT COUNT(*) AS TotalWithNoSpecifiedDoors 
            FROM VehicleDetails 
            WHERE NumDoors IS NULL
        )
    FROM VehicleDetails
) Result;

-- Method 2 without the need for a subquery (Best Performance)
SELECT 
    (CAST(
        (SELECT COUNT(*) AS TotalWithNoSpecifiedDoors FROM VehicleDetails WHERE NumDoors IS NULL) 
        AS FLOAT) 
     / 
     CAST((SELECT COUNT(*) FROM VehicleDetails) AS FLOAT)) 
    AS PercOfNoSpecifiedDoors;
