-- Problem 16: Get all Makes that manufactures DriveTypeName = FWD

SELECT DISTINCT
	Makes.Make,
	DriveTypes.DriveTypeName  
FROM VehicleDetails
JOIN Makes ON Makes.MakeID = VehicleDetails.MakeID
JOIN DriveTypes ON DriveTypes.DriveTypeID = VehicleDetails.DriveTypeID
WHERE DriveTypes.DriveTypeName = 'FWD';