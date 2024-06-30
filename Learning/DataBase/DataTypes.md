## SQL Data Types Cheat Sheet
- ![Fundamentals_of_Programming](./1.png)
### Numeric Data Types

1. **INT**
   - **Description**: Integer data type.
   - **Range**: -2,147,483,648 to 2,147,483,647.
   - **Example**: `INT` is commonly used for IDs, counts, and quantities.
   ```sql
   CREATE TABLE Products (
       ProductID INT PRIMARY KEY,
       Stock INT
   );
   ```

2. **SMALLINT**
   - **Description**: Smaller integer data type.
   - **Range**: -32,768 to 32,767.
   - **Example**: Use `SMALLINT` for fields that require less storage and have smaller ranges.
   ```sql
   CREATE TABLE Orders (
       OrderID SMALLINT PRIMARY KEY,
       Quantity SMALLINT
   );
   ```

3. **TINYINT**
   - **Description**: Very small integer data type.
   - **Range**: 0 to 255.
   - **Example**: Use `TINYINT` for small counters or status codes.
   ```sql
   CREATE TABLE SurveyResponses (
       ResponseID TINYINT PRIMARY KEY,
       Rating TINYINT
   );
   ```

4. **BIGINT**
   - **Description**: Large integer data type.
   - **Range**: -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807.
   - **Example**: Use `BIGINT` for large numbers, such as large-scale financial data.
   ```sql
   CREATE TABLE FinancialRecords (
       RecordID BIGINT PRIMARY KEY,
       Amount BIGINT
   );
   ```

5. **DECIMAL(p, s) / NUMERIC(p, s)**
   - **Description**: Fixed precision and scale numeric data type.
   - **Range**: Varies based on precision and scale.
   - **Example**: Use for financial and monetary data.
   ```sql
   CREATE TABLE Transactions (
       TransactionID INT PRIMARY KEY,
       Amount DECIMAL(10, 2)
   );
   ```

6. **FLOAT**
   - **Description**: Approximate numeric data type.
   - **Range**: -1.79E+308 to 1.79E+308.
   - **Example**: Use `FLOAT` for scientific calculations.
   ```sql
   CREATE TABLE Measurements (
       MeasurementID INT PRIMARY KEY,
       Value FLOAT
   );
   ```

7. **REAL**
   - **Description**: Approximate numeric data type.
   - **Range**: -3.40E+38 to 3.40E+38.
   - **Example**: Use `REAL` for less precise scientific calculations.
   ```sql
   CREATE TABLE TemperatureReadings (
       ReadingID INT PRIMARY KEY,
       Temperature REAL
   );
   ```

   8. **MONEY**
   - **Description**: Monetary data type.
   - **Range**: -922,337,203,685,477.5808 to 922,337,203,685,477.5807.
   - **Example**: Monetary values.
   ```sql
   CREATE TABLE Salaries (
       EmployeeID INT PRIMARY KEY,
       Salary MONEY
   );
   ```

9. **SMALLMONEY**
   - **Description**: Smaller monetary data type.
   - **Range**: -214,748.3648 to 214,748.3647.
   - **Example**: Smaller monetary values.
   ```sql
   CREATE TABLE Budget (
       BudgetID INT PRIMARY KEY,
       Amount SMALLMONEY
   );
   ```

### String Data Types

1. **CHAR(n)**
   - **Description**: Fixed length character data.
   - **Range**: 1 to 8,000 characters.
   - **Example**: Use `CHAR` for fixed-length data, like codes.
   ```sql
   CREATE TABLE CountryCodes (
       Code CHAR(3) PRIMARY KEY,
       CountryName VARCHAR(50)
   );
   ```

2. **VARCHAR(n)**
   - **Description**: Variable length character data.
   - **Range**: 1 to 8,000 characters.
   - **Example**: Use `VARCHAR` for variable length strings, like names and addresses.
   ```sql
   CREATE TABLE Customers (
       CustomerID INT PRIMARY KEY,
       FirstName VARCHAR(50),
       LastName VARCHAR(50)
   );
   ```

3. **TEXT**
   - **Description**: Variable length character data.
   - **Range**: Up to 2GB of text data.
   - **Example**: Use `TEXT` for large text data, like descriptions or articles.
   ```sql
   CREATE TABLE Articles (
       ArticleID INT PRIMARY KEY,
       Content TEXT
   );
   ```

### Unicode String Data Types

1. **NCHAR(n)**
   - **Description**: Fixed length Unicode character data.
   - **Range**: 1 to 4,000 characters.
   - **Example**: Use `NCHAR` for fixed-length Unicode data.
   ```sql
   CREATE TABLE LanguageCodes (
       Code NCHAR(3) PRIMARY KEY,
       LanguageName NVARCHAR(50)
   );
   ```

2. **NVARCHAR(n)**
   - **Description**: Variable length Unicode character data.
   - **Range**: 1 to 4,000 characters.
   - **Example**: Use `NVARCHAR` for variable length Unicode strings.
   ```sql
   CREATE TABLE Employees (
       EmployeeID INT PRIMARY KEY,
       FirstName NVARCHAR(50),
       LastName NVARCHAR(50)
   );
   ```

3. **NTEXT**
   - **Description**: Variable length Unicode character data.
   - **Range**: Up to 2GB of text data.
   - **Example**: Use `NTEXT` for large Unicode text data.
   ```sql
   CREATE TABLE Documents (
       DocumentID INT PRIMARY KEY,
       Content NTEXT
   );
   ```

### Date and Time Data Types

1. **DATE**
   - **Description**: Stores date data.
   - **Range**: January 1, 0001, to December 31, 9999.
   - **Example**: Use `DATE` for dates only.
   ```sql
   CREATE TABLE Events (
       EventID INT PRIMARY KEY,
       EventDate DATE
   );
   ```

2. **TIME**
   - **Description**: Stores time data.
   - **Range**: 00:00:00.0000000 to 23:59:59.9999999.
   - **Example**: Use `TIME` for times only.
   ```sql
   CREATE TABLE Schedules (
       ScheduleID INT PRIMARY KEY,
       StartTime TIME,
       EndTime TIME
   );
   ```

3. **DATETIME**
   - **Description**: Stores date and time data.
   - **Range**: January 1, 1753, to December 31, 9999.
   - **Example**: Use `DATETIME` for combined date and time.
   ```sql
   CREATE TABLE Appointments (
       AppointmentID INT PRIMARY KEY,
       AppointmentDate DATETIME
   );
   ```

4. **DATETIME2**
   - **Description**: Stores date and time data with higher precision.
   - **Range**: January 1, 0001, to December 31, 9999.
   - **Example**: Use `DATETIME2` for higher precision date and time.
   ```sql
   CREATE TABLE Logs (
       LogID INT PRIMARY KEY,
       LogTimestamp DATETIME2
   );
   ```

5. **SMALLDATETIME**
   - **Description**: Stores date and time data with less precision.
   - **Range**: January 1, 1900, to June 6, 2079.
   - **Example**: Use `SMALLDATETIME` for date and time with less precision.
   ```sql
   CREATE TABLE Meetings (
       MeetingID INT PRIMARY KEY,
       MeetingTime SMALLDATETIME
   );
   ```

6. **DATETIMEOFFSET**
   - **Description**: Stores date and time data with time zone awareness.
   - **Range**: January 1, 0001, to December 31, 9999.
   - **Example**: Use `DATETIMEOFFSET` for date and time with time zone.
   ```sql
   CREATE TABLE GlobalEvents (
       EventID INT PRIMARY KEY,
       EventTimestamp DATETIMEOFFSET
   );
   ```

### Binary Data Types

1. **BINARY(n)**
   - **Description**: Fixed length binary data.
   - **Range**: 1 to 8,000 bytes.
   - **Example**: Use `BINARY` for fixed length binary data.
   ```sql
   CREATE TABLE SecurityKeys (
       KeyID INT PRIMARY KEY,
       KeyValue BINARY(32)
   );
   ```

2. **VARBINARY(n)**
   - **Description**: Variable length binary data.
   - **Range**: 1 to 8,000 bytes.
   - **Example**: Use `VARBINARY` for variable length binary data.
   ```sql
   CREATE TABLE Images (
       ImageID INT PRIMARY KEY,
       ImageData VARBINARY(MAX)
   );
   ```

3. **IMAGE**
   - **Description**: Variable length binary data for large objects.
   - **Range**: Up to 2GB of binary data.
   - **Example**: Use `IMAGE` for large binary objects like photos.
   ```sql
   CREATE TABLE Photos (
       PhotoID INT PRIMARY KEY,
       PhotoData IMAGE
   );
   ```

### Other Data Types

1. **BIT**
   - **Description**: Integer data type that can take a value of 1, 0, or NULL.
   - **Example**: Use `BIT` for boolean data.
   ```sql
   CREATE TABLE FeatureFlags (
       FeatureID INT PRIMARY KEY,
       IsEnabled BIT
   );
   ```

2. **UNIQUEIDENTIFIER**
   - **Description**: Stores a globally unique identifier (GUID).
   - **Example**: Use `UNIQUEIDENTIFIER` for unique IDs.
   ```sql
   CREATE TABLE Users (
       UserID UNIQUEIDENTIFIER PRIMARY KEY,
       Username VARCHAR(50)
   );

  
3. **XML**
   - **Description**: Stores XML data.
   - **Example**: Structured data in XML format.
   ```sql
   CREATE TABLE Configurations (
       ConfigID INT PRIMARY KEY,
       ConfigData XML
   );
   ```

4. **JSON** (SQL Server 2016 and later)
   - **Description**: Stores JSON data.
   - **Example**: Structured data in JSON format.
   ```sql
   CREATE TABLE JsonData (
       DataID INT PRIMARY KEY,
       JsonData NVARCHAR(MAX)
   );
   ```

5. **CURSOR**
   - **Description**: A reference to a cursor used for database operations.
   - **Example**: Advanced data manipulation.
   ```sql
   DECLARE cursor_name CURSOR FOR 
   SELECT column_name 
   FROM table_name;
   ```

6. **TABLE**
   - **Description**: Stores a result set for later processing.
   - **Example**: Used with table-valued parameters and functions.
   ```sql
   DECLARE @MyTable TABLE (
       Column1 INT,
       Column2 NVARCHAR(50)
   );
   ```
