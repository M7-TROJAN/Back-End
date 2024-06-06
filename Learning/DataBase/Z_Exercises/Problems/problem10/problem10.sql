-- Problem 10: Get all Makes that runs with GAS

SELECT DISTINCT
	Makes.Make,
	FuelTypes.FuelTypeName
FROM VehicleDetails
INNER JOIN Makes ON Makes.MakeID = VehicleDetails.MakeID
INNER JOIN FuelTypes ON FuelTypes.FuelTypeID = VehicleDetails.FuelTypeID
WHERE FuelTypes.FuelTypeName = N'GAS';