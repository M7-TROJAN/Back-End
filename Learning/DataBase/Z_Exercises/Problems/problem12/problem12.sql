-- Problem 12: Count Vehicles by Make and Order Them by NumberOfVehicles from High to Low
SELECT 
    Makes.Make,
    COUNT(*) AS NumberOfVehicles
FROM VehicleDetails
JOIN Makes ON Makes.MakeID = VehicleDetails.MakeID
GROUP BY Makes.Make
ORDER BY NumberOfVehicles DESC;
