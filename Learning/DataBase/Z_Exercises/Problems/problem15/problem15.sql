-- Problem 15: Get all Makes with make ends with 'W'

SELECT Makes.Make 
FROM Makes
where Makes.Make like '%W';
