-- Problem 24: Get all vehicles that have Engines > 3 Liters and have only 2 doors

SELECT
	VehicleDetails.Vehicle_Display_Name,
	VehicleDetails.Engine_Cylinders,
	VehicleDetails.NumDoors
FROM VehicleDetails
WHERE (Engine_Cylinders > 3) (AND NumDoors = 2)