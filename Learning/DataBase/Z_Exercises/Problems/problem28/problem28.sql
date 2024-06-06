-- Problem 28: Get all vehicles that their body is 'Coupe' or 'Hatchback' or 'Sedan' and manufactured 
-- in year 2008 or 2020 or 2021

SELECT
	VehicleDetails.Vehicle_Display_Name,
	Bodies.BodyName,
	VehicleDetails.Year
FROM VehicleDetails
JOIN Bodies ON Bodies.BodyID = VehicleDetails.BodyID
WHERE Bodies.BodyName IN ('Coupe','Hatchback','Sedan') AND VehicleDetails.Year IN (2008, 2020, 2021)