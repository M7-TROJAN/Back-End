-- Problem 8: Get Make, FuelTypeName, and the Number of Vehicles per FuelType per Make

SELECT 
    M.Make AS Make,
    F.FuelTypeName AS FuelType,
    COUNT(VD.ID) AS NumberOfVehicles
FROM 
    VehicleDetails AS VD
JOIN 
    Makes AS M ON M.MakeID = VD.MakeID
JOIN 
    FuelTypes AS F ON F.FuelTypeID = VD.FuelTypeID
GROUP BY 
    M.Make, F.FuelTypeName
ORDER BY 
    M.Make;
