--    Problem 27: Get all vehicles that their Body is 'Coupe' or 'Hatchback' or 'Sedan'

SELECT
	VehicleDetails.Vehicle_Display_Name,
	Bodies.BodyName,
	VehicleDetails.Year
FROM VehicleDetails
JOIN Bodies ON Bodies.BodyID = VehicleDetails.BodyID
WHERE (Bodies.BodyName LIKE '%Hatchback%') OR (Bodies.BodyName LIKE '%Coupe%') OR (Bodies.BodyName LIKE '%Sedan%');
-- Or WHERE Bodies.BodyName IN ('Coupe','Hatchback','Sedan')