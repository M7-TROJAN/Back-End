A connection string is a string that specifies information about a data source and how to connect to it. In ADO.NET, connection strings are used to define the connection details for a database. They typically include the database type, server name, database name, user credentials, and other parameters required to establish a connection.

### Example of a Connection String for SQL Server

Here's a typical connection string for a SQL Server database:

```csharp
string connectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";
```

### Components of a Connection String

- **Server**: The name or network address of the database server.
  - Example: `Server=myServerAddress;`
  - For local SQL Server instances, you can use `localhost` or `.`.

- **Database**: The name of the database to connect to.
  - Example: `Database=myDataBase;`

- **User Id**: The username to use for authentication.
  - Example: `User Id=myUsername;`

- **Password**: The password to use for authentication.
  - Example: `Password=myPassword;`

### Additional Parameters

You can include additional parameters to customize the connection behavior:

- **Trusted_Connection**: If set to `True`, uses Windows Authentication.
  - Example: `Trusted_Connection=True;`

- **Integrated Security**: Another way to specify Windows Authentication.
  - Example: `Integrated Security=SSPI;`

- **Connection Timeout**: Specifies the time in seconds to wait for a connection to open before timing out.
  - Example: `Connection Timeout=30;`

- **Encrypt**: Determines whether the SQL Server connection should be encrypted.
  - Example: `Encrypt=True;`

### Example Connection Strings

1. **SQL Server with SQL Server Authentication:**
   ```csharp
   string connectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";
   ```

2. **SQL Server with Windows Authentication:**
   ```csharp
   string connectionString = "Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;";
   ```

3. **Local SQL Server Instance:**
   ```csharp
   string connectionString = "Server=localhost;Database=myDataBase;User Id=myUsername;Password=myPassword;";
   ```

4. **Encrypted Connection:**
   ```csharp
   string connectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;Encrypt=True;";
   ```

### Using a Connection String in ADO.NET

Here's how you use a connection string in ADO.NET to open a connection to a SQL Server database:

```csharp
using System.Data.SqlClient;

string connectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";

using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    // Perform database operations
}
```

### Securing Connection Strings

To keep your connection strings secure:

- **Avoid Hardcoding**: Store connection strings in configuration files, such as `app.config` or `web.config`, rather than in your code.
- **Encrypt Configuration Files**: Use encryption to protect connection strings in configuration files.
- **Use Environment Variables**: Store sensitive information like user credentials in environment variables.

### Example from `app.config` or `web.config`

```xml
<configuration>
  <connectionStrings>
    <add name="MyConnectionString"
         connectionString="Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;"
         providerName="System.Data.SqlClient" />
  </connectionStrings>
</configuration>
```

You can then access this connection string in your code like this:

```csharp
using System.Configuration;
using System.Data.SqlClient;

string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    // Perform database operations
}
```

This approach centralizes the connection string, making it easier to manage and secure.