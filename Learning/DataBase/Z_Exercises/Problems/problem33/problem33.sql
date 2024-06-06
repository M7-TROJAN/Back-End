--   Problem 33: Get Minimum Engine CC , Maximum Engine CC , and Average Engine CC of all Vehicles


SELECT
	MIN(VehicleDetails.Engine_CC) AS 'Minimum Engine CC',
	MAX(VehicleDetails.Engine_CC) AS 'Maximum Engine CC',
	AVG(VehicleDetails.Engine_CC) AS 'Average Engine CC'
FROM VehicleDetails