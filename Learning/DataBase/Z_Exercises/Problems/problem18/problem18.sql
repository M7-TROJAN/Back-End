-- Problem 18: Get total vehicles per DriveTypeName Per Make and order them per make asc then per total Desc

SELECT 
	Makes.Make,
	DriveTypes.DriveTypeName,
	COUNT(*) AS TotalVehicles
FROM VehicleDetails
JOIN Makes ON Makes.MakeID = VehicleDetails.MakeID
JOIN DriveTypes On DriveTypes.DriveTypeID = VehicleDetails.DriveTypeID
GROUP BY Makes.Make , DriveTypes.DriveTypeName
ORDER BY Makes.Make ASC , TotalVehicles DESC