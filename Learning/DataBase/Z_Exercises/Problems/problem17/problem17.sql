-- Problem 17: Get the total number of makes that manufacture vehicles with DriveTypeName 'FWD'

-- Method 1: Using a subquery with DISTINCT
SELECT COUNT(*) AS MakeWithFWD FROM (
    -- This subquery selects distinct makes with DriveTypeName 'FWD'
    SELECT DISTINCT
        Makes.Make,
        DriveTypes.DriveTypeName
    FROM VehicleDetails
    JOIN Makes ON Makes.MakeID = VehicleDetails.MakeID
    JOIN DriveTypes ON DriveTypes.DriveTypeID = VehicleDetails.DriveTypeID
    WHERE DriveTypes.DriveTypeName = N'FWD'
) Result;


-- Method 2: Using a single query 
SELECT COUNT(distinct Makes.Make) as MakeWithFWD
FROM VehicleDetails
INNER JOIN Makes ON Makes.MakeID = VehicleDetails.MakeID
INNER JOIN DriveTypes ON DriveTypes.DriveTypeID = VehicleDetails.DriveTypeID
WHERE DriveTypes.DriveTypeName = N'FWD';
