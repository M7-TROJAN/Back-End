-- Problem 30: Get all Vehicle_Display_Name, NumDoors and add extra column to describe number of doors by words, 
-- and if door is null display 'Not Set'

SELECT 
	VehicleDetails.Vehicle_Display_Name,
	VehicleDetails.NumDoors,
	DoorDescription = 
	CASE
		WHEN VehicleDetails.NumDoors = 0 THEN 'Zero Doors'
		WHEN VehicleDetails.NumDoors = 1 THEN 'One Door'
		WHEN VehicleDetails.NumDoors = 2 THEN 'Two Doors'
		WHEN VehicleDetails.NumDoors = 3 THEN 'Three Doors'
		WHEN VehicleDetails.NumDoors = 4 THEN 'Four Doors'
		WHEN VehicleDetails.NumDoors = 5 THEN 'Five Doors'
		WHEN VehicleDetails.NumDoors = 6 THEN 'Six Doors'
		WHEN VehicleDetails.NumDoors = 8 THEN 'Eight Doors'
		ELSE 'Not Set'
	END
FROM VehicleDetails