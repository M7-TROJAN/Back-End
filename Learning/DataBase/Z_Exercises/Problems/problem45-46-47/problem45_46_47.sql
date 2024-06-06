--   Problem 45: Get Number of Models Per Make
SELECT 
	Makes.Make,
	COUNT(*) AS NumberOfModels
FROM MakeModels
JOIN Makes ON Makes.MakeID = MakeModels.MakeID
GROUP BY Makes.Make
ORDER By NumberOfModels Desc


-- Problem 46: Get the highest 3 manufacturers that make the highest number of models
SELECT TOP 3
	Makes.Make,
	COUNT(*) AS NumberOfModels
FROM MakeModels
JOIN Makes ON Makes.MakeID = MakeModels.MakeID
GROUP BY Makes.Make
ORDER By NumberOfModels Desc


-- Problem 47: Get the highest number of models manufactured

SELECT TOP 1
	Makes.Make,
	COUNT(*) AS NumberOfModels
FROM MakeModels
JOIN Makes ON Makes.MakeID = MakeModels.MakeID
GROUP BY Makes.Make
ORDER By NumberOfModels Desc
