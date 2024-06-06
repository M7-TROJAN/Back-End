-- Problem 9: Get all vehicles that runs with GAS


SELECT * FROM VehicleDetails
JOIN FuelTypes ON FuelTypes.FuelTypeID = VehicleDetails.FuelTypeID
WHERE FuelTypes.FuelTypeName = N'GAS'