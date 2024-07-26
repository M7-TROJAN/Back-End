# ParameterDirection Examples and Use Cases

## Overview

The `ParameterDirection` enumeration specifies the type of a parameter within a `SqlCommand`. It determines whether the parameter is used to send data to the database, receive data from the database, or both.

### ParameterDirection Values

1. `Input`
2. `Output`
3. `InputOutput`
4. `ReturnValue`

## Examples and Use Cases

### 1. Input

The `Input` direction specifies that the parameter is used to send data to the database. This is the default direction for parameters.

#### Example

```csharp
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ParameterDirectionExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "your_connection_string_here";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("UPDATE Wallets SET Balance = @Balance WHERE Id = @Id", connection))
                {
                    SqlParameter idParam = new SqlParameter("@Id", SqlDbType.Int)
                    {
                        Value = 1,
                        Direction = ParameterDirection.Input
                    };

                    SqlParameter balanceParam = new SqlParameter("@Balance", SqlDbType.Decimal)
                    {
                        Value = 1000.00m,
                        Direction = ParameterDirection.Input
                    };

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(balanceParam);

                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"Rows affected: {rowsAffected}");
                }
            }
        }
    }
}
```

### 2. Output

The `Output` direction specifies that the parameter is used to receive data from the database.

#### Example

```csharp
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ParameterDirectionExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "your_connection_string_here";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT @TotalBalance = SUM(Balance) FROM Wallets", connection))
                {
                    SqlParameter totalBalanceParam = new SqlParameter("@TotalBalance", SqlDbType.Decimal)
                    {
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(totalBalanceParam);

                    command.ExecuteNonQuery();
                    decimal totalBalance = (decimal)totalBalanceParam.Value;
                    Console.WriteLine($"Total Balance: {totalBalance}");
                }
            }
        }
    }
}
```

### 3. InputOutput

The `InputOutput` direction specifies that the parameter is capable of both sending data to and receiving data from the database.

#### Example

```csharp
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ParameterDirectionExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "your_connection_string_here";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("EXEC UpdateAndReturnBalance @Id, @Balance OUTPUT", connection))
                {
                    SqlParameter idParam = new SqlParameter("@Id", SqlDbType.Int)
                    {
                        Value = 1,
                        Direction = ParameterDirection.Input
                    };

                    SqlParameter balanceParam = new SqlParameter("@Balance", SqlDbType.Decimal)
                    {
                        Value = 500.00m,
                        Direction = ParameterDirection.InputOutput
                    };

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(balanceParam);

                    command.ExecuteNonQuery();
                    decimal updatedBalance = (decimal)balanceParam.Value;
                    Console.WriteLine($"Updated Balance: {updatedBalance}");
                }
            }
        }
    }
}
```

### 4. ReturnValue

The `ReturnValue` direction specifies that the parameter represents a return value from a stored procedure.

#### Example

```csharp
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ParameterDirectionExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "your_connection_string_here";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("EXEC @ReturnValue = GetTotalWallets", connection))
                {
                    SqlParameter returnValueParam = new SqlParameter("@ReturnValue", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };

                    command.Parameters.Add(returnValueParam);

                    command.ExecuteNonQuery();
                    int totalWallets = (int)returnValueParam.Value;
                    Console.WriteLine($"Total Wallets: {totalWallets}");
                }
            }
        }
    }
}
```

---

## Key Points

1. **Input**: Used to send data to the database.
2. **Output**: Used to receive data from the database.
3. **InputOutput**: Used to send and receive data.
4. **ReturnValue**: Used to retrieve return values from stored procedures.
---
