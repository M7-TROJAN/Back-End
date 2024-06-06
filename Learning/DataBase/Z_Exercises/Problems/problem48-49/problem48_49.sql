
-- Problem 48: Get the Lowest Manufacturers who manufactured the lowest number of models

-- Step 1: Count the number of models per manufacturer and find the minimum number of models.
SELECT 
    Makes.Make,
    COUNT(*) AS NumberOfModels
FROM MakeModels
JOIN Makes ON Makes.MakeID = MakeModels.MakeID
GROUP BY Makes.Make

-- Step 2: Find the manufacturers with the minimum number of models.
HAVING COUNT(*) = (
    SELECT MIN(NumberOfModels) 
    FROM (
        -- Subquery to count the number of models per manufacturer.
        SELECT MakeID, COUNT(*) AS NumberOfModels
        FROM MakeModels
        GROUP BY MakeID
    ) R1
)



-- Problem 49: Get the highest Manufacturers manufactured the highest number of models

-- remember that they could be more than one manufacturer have the same high number of models

SELECT        
	Makes.Make, COUNT(*) AS NumberOfModels
FROM Makes 
INNER JOIN MakeModels ON Makes.MakeID = MakeModels.MakeID
GROUP BY Makes.Make
having COUNT(*) = (
	select Max(NumberOfModels) as MaxNumberOfModels
	from
	(
		SELECT MakeID, COUNT(*) AS NumberOfModels
		FROM MakeModels
		GROUP BY MakeID
	) R
)


-- or 

-- Step 1: Count the number of models per manufacturer.
SELECT Makes.Make, Count(*) as NumberOfModels
FROM Makes
INNER JOIN MakeModels ON Makes.MakeID = MakeModels.MakeID
GROUP BY Makes.Make

-- Step 2: Find the manufacturer(s) with the minimum number of models.
HAVING Count(*) = (
    SELECT TOP 1 Count(*) as TotalMadeModels
    FROM Makes
    INNER JOIN MakeModels ON Makes.MakeID = MakeModels.MakeID
    GROUP BY Makes.Make
    ORDER BY TotalMadeModels ASC
);
