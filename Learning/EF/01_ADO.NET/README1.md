### Introduction to ADO.NET

ADO.NET (ActiveX Data Objects .NET) is a set of classes in the .NET Framework that provides access to data sources such as databases and XML files. It is part of the .NET Framework and offers a rich set of components for creating distributed, data-sharing applications. 

### Key Components of ADO.NET

1. Connection: Establishes a connection to a data source.
2. Command: Executes commands to the data source (e.g., SQL queries).
3. DataReader: Provides a way to read a forward-only stream of data from a data source.
4. DataAdapter: Acts as a bridge between a DataSet and a data source for retrieving and saving data.
5. DataSet: Represents an in-memory cache of data.

### Steps to Work with ADO.NET

1. Establishing a Connection:
    - Use the SqlConnection class to connect to a SQL Server database.
       ```csharp
        using System.Data.SqlClient;

        string connectionString = "your_connection_string_here";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            // Connection is open and ready to use
        }
        ```
    
2. Executing Commands:
    - Use the SqlCommand class to execute SQL queries.

    ```csharp
    string query = "SELECT * FROM YourTable";
    using (SqlCommand command = new SqlCommand(query, connection))
    {
        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine(reader["ColumnName"].ToString());
        }
    }
    ```
    
3. Reading Data:
    - Use SqlDataReader to read data from a SQL Server database.
    ```csharp
    while (reader.Read())
    {
        Console.WriteLine(reader["ColumnName"].ToString());
    }
    reader.Close();
    ```
    
4. Using DataAdapter and DataSet:
    - Use SqlDataAdapter and DataSet to fill data and work with it offline.
    ```csharp
    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
    DataSet dataSet = new DataSet();
    adapter.Fill(dataSet);
    
    foreach (DataRow row in dataSet.Tables[0].Rows)
    {
        Console.WriteLine(row["ColumnName"].ToString());
    }
    ```

### Example: Basic CRUD Operations

#### 1. Create (Insert)
```csharp
string insertQuery = "INSERT INTO YourTable (Column1, Column2) VALUES (@value1, @value2)";
using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
{
    insertCommand.Parameters.AddWithValue("@value1", "Value1");
    insertCommand.Parameters.AddWithValue("@value2", "Value2");
    insertCommand.ExecuteNonQuery();
}
```

#### 2. Read (Select)
```csharp
string selectQuery = "SELECT * FROM YourTable";
using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
{
    SqlDataReader reader = selectCommand.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine(reader["ColumnName"].ToString());
    }
    reader.Close();
}
```

#### 3. Update
```csharp
string updateQuery = "UPDATE YourTable SET Column1 = @newValue WHERE Column2 = @criteria";
using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
{
    updateCommand.Parameters.AddWithValue("@newValue", "NewValue");
    updateCommand.Parameters.AddWithValue("@criteria", "Criteria");
    updateCommand.ExecuteNonQuery();
}
```

#### 4. Delete
```csharp
string deleteQuery = "DELETE FROM YourTable WHERE Column1 = @criteria";
using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
{
    deleteCommand.Parameters.AddWithValue("@criteria", "Criteria");
    deleteCommand.ExecuteNonQuery();
}
```
