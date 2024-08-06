### Connection and Network-Related Errors

- **-2**: Timeout expired. The timeout period elapsed prior to completion of the operation or the server is not responding.
- **53**: Named Pipes Provider: Could not open a connection to SQL Server.
- **64**: A connection was successfully established with the server, but then an error occurred during the login process. (provider: TCP Provider, error: 0 - The specified network name is no longer available.)
- **233**: The client was unable to establish a connection because of an error during connection initialization process before login. Possible causes include the following: the client cannot find the server. This error can also occur if the SQL Server instance is not running.
- **10053**: A transport-level error has occurred when receiving results from the server. An established connection was aborted by the software in your host machine.
- **10054**: A transport-level error has occurred when sending the request to the server. (provider: TCP Provider, error: 0 - An existing connection was forcibly closed by the remote host.)
- **10060**: A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible.

### Resource-Related Errors

- **701**: There is insufficient system memory to run this query.
- **802**: There is insufficient memory available in the buffer pool.
- **1204**: The instance of the SQL Server Database Engine cannot obtain a LOCK resource at this time. Rerun your statement when there are fewer active users. Ask the database administrator to check the lock and memory configuration for this instance, or to check for long-running transactions.
- **1205**: Transaction (Process ID) was deadlocked on resources with another process and has been chosen as the deadlock victim. Rerun the transaction.
- **1222**: Lock request time out period exceeded.

### General and Execution Errors

- **547**: The INSERT statement conflicted with the FOREIGN KEY constraint.
- **2627**: Violation of PRIMARY KEY constraint. Cannot insert duplicate key in object.
- **2601**: Cannot insert duplicate key row in object with unique index.
- **8152**: String or binary data would be truncated.

### Authentication and Authorization Errors

- **18456**: Login failed for user.
- **4060**: Cannot open database requested by the login. The login failed.

### Backup and Restore Errors

- **3201**: Cannot open backup device. Operating system error.
- **3313**: During redoing of a logged operation in database, an error occurred at log record ID. Typically, the specific failure is logged previously as an error in the operating system error log. Restore the backup if the problem results in a failure during startup.

### High Availability and Disaster Recovery Errors

- **41106**: The operation cannot be performed on database because it is involved in a database mirroring session or an availability group.
- **41144**: Database mirroring cannot be enabled because the database is an availability database.

### Miscellaneous Errors

- **50000**: This is a user-defined error. It can be used for custom error messages defined by the application.

These are just some of the more common error numbers that you might encounter when working with SQL Server. You can find a more comprehensive list of SQL Server error numbers and messages in the SQL Server documentation or by querying the `sys.messages` system catalog view in SQL Server:

```sql
SELECT * FROM sys.messages WHERE language_id = 1033;
```

This query will return all the error messages for the language with ID 1033 (English). You can adjust the language ID if you need error messages in a different language.
