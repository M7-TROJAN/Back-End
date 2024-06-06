-- Problem 20: Get all Vehicles that number of doors is not specified
SELECT * FROM VehicleDetails
WHERE NumDoors IS NULL;


-- Problem 21: Get Total Vehicles that number of doors is not specified
select count(*) as TotalWithNoSpecifiedDoors from VehicleDetails
where NumDoors is Null