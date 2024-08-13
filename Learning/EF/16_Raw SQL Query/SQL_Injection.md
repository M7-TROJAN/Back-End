
# Understanding SQL Injection

## Introduction

SQL Injection is a type of security vulnerability that allows an attacker to interfere with the queries that an application makes to its database. It typically occurs when an application fails to properly sanitize user input, allowing attackers to inject malicious SQL code. This can lead to unauthorized access, data leakage, data modification, and even full system compromise.

## Basic Example of SQL Injection

Let's consider a simple example where a web application checks user login credentials. Here’s how the code might look:

```csharp
string username = txtUsername.Text;
string password = txtPassword.Text;

string query = "SELECT * FROM Users WHERE Username = '" + username + "' AND Password = '" + password + "'";

// Execute the query against the database
```

In this code, user input is directly concatenated into the SQL query. If a malicious user enters the following in the username field:

```
' OR '1'='1
```

The resulting query will look like:

```sql
SELECT * FROM Users WHERE Username = '' OR '1'='1' AND Password = 'any_password'
```

This query will always return true for the condition `OR '1'='1'`, allowing the attacker to bypass authentication and access the system.

## Real-World Scenario: Extracting Data

An attacker can use SQL Injection to extract sensitive data from the database. Suppose the attacker wants to retrieve all the usernames from the database. By injecting a SQL command like:

```
' OR 1=1; SELECT Username FROM Users; --
```

The query might be manipulated as:

```sql
SELECT * FROM Users WHERE Username = '' OR 1=1; SELECT Username FROM Users; -- AND Password = 'password'
```

Here, `--` is a comment that ignores the rest of the SQL statement, allowing the injected query to execute separately, potentially exposing all usernames.

## Preventing SQL Injection

### 1. **Parameterized Queries**

Parameterized queries, also known as prepared statements, ensure that user input is treated as data rather than executable code. Here's how to use parameterized queries in C#:

```csharp
using (var connection = new SqlConnection("your_connection_string"))
{
    connection.Open();
    string query = "SELECT * FROM Users WHERE Username = @username AND Password = @password";

    using (var command = new SqlCommand(query, connection))
    {
        command.Parameters.AddWithValue("@username", txtUsername.Text);
        command.Parameters.AddWithValue("@password", txtPassword.Text);

        using (var reader = command.ExecuteReader())
        {
            if (reader.HasRows)
            {
                // Login successful
            }
            else
            {
                // Login failed
            }
        }
    }
}
```

In this example, `@username` and `@password` are placeholders for the user input. The input is automatically escaped, preventing SQL injection.

### 2. **Using ORM (Object-Relational Mapping) Frameworks**

ORM frameworks like Entity Framework in C# abstract away SQL queries, reducing the risk of SQL injection. For example:

```csharp
var user = dbContext.Users
                    .Where(u => u.Username == username && u.Password == password)
                    .FirstOrDefault();
```

Since ORM frameworks use parameterized queries internally, they provide a layer of protection against SQL injection.

### 3. **Input Validation**

Properly validate and sanitize user input. Ensure that inputs conform to expected formats (e.g., email addresses, numeric values) and reject or escape potentially dangerous characters.

### 4. **Least Privilege Principle**

The database user your application connects with should have the minimum permissions necessary. For example, if the application only needs to read data, it shouldn't have permissions to insert, update, or delete data.

## Advanced Scenario: Blind SQL Injection

In some cases, the results of an injection are not directly visible to the attacker. This is known as Blind SQL Injection. Attackers can still exploit this by inferring information based on the application's responses.

For example, an attacker can ask a series of true or false questions through SQL Injection, such as:

```sql
SELECT CASE WHEN (1=1) THEN 'true' ELSE 'false' END;
```

If the application behaves differently based on whether the condition is true or false, the attacker can slowly extract data from the database.

## Conclusion

SQL Injection is a powerful attack vector that can have severe consequences. However, it is entirely preventable through proper coding practices. Always use parameterized queries, consider using ORM frameworks, validate and sanitize all user inputs, and apply the principle of least privilege to your database users.
