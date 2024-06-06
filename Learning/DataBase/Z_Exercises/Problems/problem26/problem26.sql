--   Problem 26: Get all vehicles that their body is 'Sport Utility' and Year > 2020

SELECT
	VehicleDetails.Vehicle_Display_Name,
	Bodies.BodyName,
	VehicleDetails.Year
FROM VehicleDetails
JOIN Bodies ON Bodies.BodyID = VehicleDetails.BodyID
WHERE (Bodies.BodyName LIKE '%Sport Utility%') AND (VehicleDetails.Year = 2020)