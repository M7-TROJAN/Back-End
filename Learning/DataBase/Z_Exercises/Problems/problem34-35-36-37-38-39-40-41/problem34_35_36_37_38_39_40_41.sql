--   Problem 34: Get all vehicles that have the minimum Engine_CC
SELECT VehicleDetails.Vehicle_Display_Name FROM VehicleDetails
WHERE Engine_CC = ( SELECT MinimumEngineCC = MIN(VehicleDetails.Engine_CC) FROM VehicleDetails )


-- Problem 35: Get all vehicles that have the Maximum Engine_CC
SELECT VehicleDetails.Vehicle_Display_Name FROM VehicleDetails
WHERE Engine_CC = ( SELECT MaximumEngineCC = MAX(VehicleDetails.Engine_CC) FROM VehicleDetails )


-- Problem 36: Get all vehicles that have Engin_CC below average
SELECT VehicleDetails.Vehicle_Display_Name FROM VehicleDetails
WHERE Engine_CC < ( SELECT AverageEngineCC = AVG(VehicleDetails.Engine_CC) FROM VehicleDetails )


-- Problem 37: Get total vehicles that have Engin_CC above average
SELECT COUNT(*) AS NumberOfVehiclesAboveAverageEngineCC FROM (
	SELECT VehicleDetails.Vehicle_Display_Name FROM VehicleDetails
	WHERE Engine_CC > ( SELECT AverageEngineCC = AVG(VehicleDetails.Engine_CC) FROM VehicleDetails )
)Result;

--   Problem 38: Get all unique Engin_CC and sort them Desc
SELECT DISTINCT Engine_CC FROM VehicleDetails
ORDER BY Engine_CC DESC


-- Problem 39: Get the maximum 3 Engine CC
SELECT DISTINCT TOP 3 Engine_CC FROM VehicleDetails
ORDER BY Engine_CC DESC


-- Problem 40: Get all vehicles that has one of the Max 3 Engine CC
SELECT Vehicle_Display_Name FROM VehicleDetails
WHERE Engine_CC IN 
(
	SELECT DISTINCT TOP 3 Engine_CC FROM VehicleDetails
	ORDER BY Engine_CC DESC
)

-- Note: This method is the same as you would use if you had a table containing students and their grades
-- and you were asked to retrieve the top ten The top ten could be 15 because there may be duplicate tags
-- You must retrieve the ten highest grades and then retrieve the students who obtained one of these grades.


--   Problem 41: Get all Makes that manufactures one of the Max 3 Engine CC
SELECT DISTINCT 
	Makes.Make 
FROM 
	VehicleDetails
JOIN 
	Makes ON Makes.MakeID = VehicleDetails.MakeID
WHERE VehicleDetails.Engine_CC IN 
(
	SELECT DISTINCT TOP 3 Engine_CC FROM VehicleDetails
	ORDER BY Engine_CC DESC
)
ORDER BY Makes.Make;



