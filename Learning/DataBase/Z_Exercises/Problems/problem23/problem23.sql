-- Problem 23: Get MakeID , Make, SubModelName for all vehicles that have SubModelName 'Elite'

SELECT DISTINCT
	VehicleDetails.MakeID,
	Makes.Make,
	SubModels.SubModelName
FROM VehicleDetails
JOIN Makes ON Makes.MakeID = VehicleDetails.MakeID
JOIN SubModels ON SubModels.SubModelID = VehicleDetails.SubModelID
WHERE SubModels.SubModelName = N'Elite'
