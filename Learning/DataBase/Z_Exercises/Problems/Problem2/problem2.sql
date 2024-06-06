-- Problem 2 : Get All Vehicles Made Between 1950 - 2000

SELECT * FROM VehicleDetailsView 
WHERE VehicleDetailsView.Year BETWEEN 1950 AND 2000
ORDER BY VehicleDetailsView.Year