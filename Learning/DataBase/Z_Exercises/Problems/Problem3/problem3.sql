-- Problem 3 : Get number vehicles made between 1950 and 2000

SELECT COUNT(ID) AS 'number vehicles made between 1950 and 2000' 
FROM VehicleDetailsView 
WHERE VehicleDetailsView.Year BETWEEN 1950 AND 2000