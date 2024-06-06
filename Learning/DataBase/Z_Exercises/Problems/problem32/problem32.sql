-- Problem 32: Get all Vehicle_Display_Name, year, Age for vehicles that their age between 15 and 25 years old

SELECT * FROM (
	SELECT 
		VehicleDetails.Vehicle_Display_Name,
		VehicleDetails.Year,
		CarAge = YEAR(GETDATE()) - VehicleDetails.Year
	FROM VehicleDetails
) AS Result
WHERE CarAGE BETWEEN 15 AND  25
ORDER BY CarAge DESC
