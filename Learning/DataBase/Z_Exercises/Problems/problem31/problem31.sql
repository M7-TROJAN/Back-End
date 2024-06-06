-- Problem 31: Get all Vehicle_Display_Name, year and add extra column to calculate the age of the car 
-- then sort the results by age desc.

SELECT 
    VehicleDetails.Vehicle_Display_Name,
    VehicleDetails.Year,
    CarAge = YEAR(GETDATE()) - VehicleDetails.Year
FROM VehicleDetails
ORDER BY CarAge DESC;

-- Note that SQL Server's YEAR function (in capital letters) is a built-in function that extracts the year from a given date.
-- If you hover the mouse over it, you'll see this placeholder: 'YEAR(expression datetime) RETURNS INT'.
-- the year in small letters is the column name