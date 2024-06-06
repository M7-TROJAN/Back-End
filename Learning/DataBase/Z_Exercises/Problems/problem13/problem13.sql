-- Problem 13: Get all Makes/Count Of Vehicles that manufactures more than 20K Vehicles

SELECT 
	Makes.Make,
	Count(*) AS NumberOFVehicle
FROM VehicleDetails
JOIN Makes ON Makes.MakeID = VehicleDetails.MakeID
GROUP BY Makes.Make
HAVING COUNT(*) > 20000
ORDER BY NumberOFVehicle DESC;