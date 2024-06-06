-- Problem 51: Get all employees that have manager along with Manager's name.

-- this will select all data from employees that are managed by someone along with their manager name, 
-- employees that have no manager will not be selected because we used inner join 
-- Note we used inner join on the same table with different alliace.

SELECT Employees.Name, Employees.ManagerID, Employees.Salary, Managers.Name AS ManagerName
FROM Employees 
INNER JOIN Employees AS Managers ON Employees.ManagerID = Managers.EmployeeID



-- Problem 52: Get all employees that have manager or does not have manager along with Manager's name, in case no manager name show null

-- this will select all data from employees regardless if they have manager or not, note here I used left outer join 

SELECT Employees.Name, Employees.ManagerID, Employees.Salary, Managers.Name AS ManagerName
FROM Employees 
LEFT JOIN Employees AS Managers ON Employees.ManagerID = Managers.EmployeeID



-- Problem 53: Get all employees that have manager or does not have manager along with Manager's name, 
-- incase no manager name the same employee name as manager to himself

-- -- this will select all data from employees regardless if they have manager or not, note here we used left outer join 
SELECT 
	Employees.Name, 
	Employees.ManagerID, 
	Employees.Salary,
	CASE
		WHEN Managers.Name IS NULL THEN Employees.Name
		ELSE Managers.Name
	END AS ManagerName
FROM Employees 
LEFT JOIN Employees AS Managers ON Employees.ManagerID = Managers.EmployeeID




-- Problem 54: Get All Employees managed by 'Mohammed'

SELECT * 
FROM Employees
WHERE ManagerID = (SELECT TOP 1 EmployeeID FROM Employees WHERE Employees.Name = 'Mohammed');

-- OR

SELECT 
	Employees.Name, 
	Employees.ManagerID, 
	Employees.Salary, 
	Managers.Name AS ManagerName
FROM Employees 
INNER JOIN Employees AS Managers ON Employees.ManagerID = Managers.EmployeeID
where Managers.Name='Mohammed'
