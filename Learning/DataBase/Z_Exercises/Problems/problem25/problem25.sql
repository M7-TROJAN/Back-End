--   Problem 25: Get make and vehicles that the engine contains 'OHV' and have Cylinders = 4
SELECT
	Makes.Make,
	VehicleDetails.Vehicle_Display_Name,
	VehicleDetails.Engine,
	VehicleDetails.Engine_Cylinders
FROM VehicleDetails
JOIN Makes ON Makes.MakeID = VehicleDetails.MakeID
WHERE (VehicleDetails.Engine LIKE '%OHV%') AND (VehicleDetails.Engine_Cylinders = 4)
