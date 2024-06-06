-- Problem 11: Get the Total Number of Makes that Run on GAS

-- Method 1: Using a Subquery
SELECT COUNT(*) AS 'Total Makes runs with GAS'
FROM (
    -- Select distinct Makes and their associated FuelType (GAS).
    SELECT DISTINCT
        Makes.Make,
        FuelTypes.FuelTypeName
    FROM
        VehicleDetails
    INNER JOIN
        Makes ON Makes.MakeID = VehicleDetails.MakeID
    INNER JOIN
        FuelTypes ON FuelTypes.FuelTypeID = VehicleDetails.FuelTypeID
    WHERE
        FuelTypes.FuelTypeName = N'GAS'
) Result;

-- Method 2: Another Approach Without Using a Subquery
SELECT COUNT(DISTINCT Makes.Make) AS 'Total Makes With GAS'
FROM VehicleDetails
INNER JOIN Makes ON Makes.MakeID = VehicleDetails.MakeID
INNER JOIN FuelTypes ON FuelTypes.FuelTypeID = VehicleDetails.FuelTypeID
WHERE FuelTypes.FuelTypeName = N'GAS';
