-- Problem 19: Get total vehicles per DriveTypeName Per Make then filter only results with total > 10,000
SELECT 
	Makes.Make,
	DriveTypes.DriveTypeName,
	COUNT(*) AS TotalVehicles
FROM VehicleDetails
JOIN Makes ON Makes.MakeID = VehicleDetails.MakeID
JOIN DriveTypes On DriveTypes.DriveTypeID = VehicleDetails.DriveTypeID
GROUP BY Makes.Make , DriveTypes.DriveTypeName
HAVING COUNT(*) > 10000
ORDER BY Makes.Make ASC , TotalVehicles DESC


-- Code With Comments
SELECT
    Makes.Make,
    DriveTypes.DriveTypeName,
    COUNT(*) AS TotalVehicles
FROM VehicleDetails
JOIN Makes ON Makes.MakeID = VehicleDetails.MakeID
JOIN DriveTypes ON DriveTypes.DriveTypeID = VehicleDetails.DriveTypeID
-- Join Makes and DriveTypes tables to obtain Make and DriveTypeName information.

GROUP BY Makes.Make, DriveTypes.DriveTypeName
-- Group the results by Make and DriveTypeName to count the vehicles for each combination.

HAVING COUNT(*) > 10000
-- Filter the results to include only those with a total greater than 10,000 using the HAVING clause.

ORDER BY Makes.Make ASC, TotalVehicles DESC
-- Order the results by Make in ascending order and TotalVehicles in descending order for each Make.
