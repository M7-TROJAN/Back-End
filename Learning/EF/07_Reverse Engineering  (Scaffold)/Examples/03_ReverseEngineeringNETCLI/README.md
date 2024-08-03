# Reverse Engineering Using .NET CLI

Reverse engineering in Entity Framework Core (EF Core) involves scaffolding a DbContext and entity classes from an existing database schema. This guide will walk you through the process of using the .NET CLI to perform reverse engineering.

## Prerequisites

Before you begin, ensure you have the following:
1. .NET Core SDK installed on your machine.
2. An existing database schema to reverse engineer.

## Steps

### Step 1: Open Windows Terminal (Command Prompt)
Open your terminal or command prompt to execute the necessary commands.

### Step 2: Install EF Core Tool Globally
You need to install the EF Core CLI tool globally on your machine.

**Install EF Core Tool:**
```sh
dotnet tool install --global dotnet-ef
```

**Update EF Core Tool (if already installed):**
```sh
dotnet tool update --global dotnet-ef
```

### Step 3: Install Provider in the Project
Navigate to your project directory and install the EF Core provider for your database.

**Example for SQL Server:**
```sh
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

Ensure you also have the design package installed:
```sh
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### Step 4: Run the Scaffold Command
Use the `dotnet ef dbcontext scaffold` command to generate the DbContext and entity classes.

**Command Syntax:**
```sh
dotnet ef dbcontext scaffold "[Connection String]" [Provider] [Options]
```

**Example:**
```sh
dotnet ef dbcontext scaffold "Data Source=M7_TROJAN;Initial Catalog=TechTalk;Integrated Security=SSPI;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer
```

### Step 5: Explore Generated Code
After running the scaffold command, EF Core will generate the DbContext and entity classes in your project. These files represent your database schema in code.

## Detailed Explanation of Options

### Commonly Used Options

#### 1. `-o` or `--output-dir`
Specifies the directory where the entity classes should be placed.

**Usage:**
```sh
dotnet ef dbcontext scaffold "[Connection String]" [Provider] -o Models
```

#### 2. `-c` or `--context`
Specifies a custom name for the DbContext class.

**Usage:**
```sh
dotnet ef dbcontext scaffold "[Connection String]" [Provider] -c CustomContextName
```

#### 3. `-d` or `--data-annotations`
Use data annotations to configure the model (as opposed to using the Fluent API).

**Usage:**
```sh
dotnet ef dbcontext scaffold "[Connection String]" [Provider] -d
```

#### 4. `--context-dir`
Specifies the directory where the DbContext class should be placed.

**Usage:**
```sh
dotnet ef dbcontext scaffold "[Connection String]" [Provider] --context-dir Data
```

#### 5. `--schemas`
Specifies the schemas of tables to generate models for. If this option is omitted, all schemas are included.

**Usage:**
```sh
dotnet ef dbcontext scaffold "[Connection String]" [Provider] --schemas dbo,auth
```

#### 6. `--tables`
Specifies the tables to generate models for. If this option is omitted, all tables are included.

**Usage:**
```sh
dotnet ef dbcontext scaffold "[Connection String]" [Provider] --tables Users,Orders
```

#### 7. `-f` or `--force`
Overwrite existing files without prompting for confirmation.

**Usage:**
```sh
dotnet ef dbcontext scaffold "[Connection String]" [Provider] -f
```

### Example with Multiple Options

Here’s an example that uses multiple options to customize the scaffolding process:

```sh
dotnet ef dbcontext scaffold "Data Source=M7_TROJAN;Initial Catalog=TechTalk;Integrated Security=SSPI;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models -c TechTalkContext --context-dir Data -d -f
```

## Example Database Schema and Generated Code

Here is an example of a database schema and the corresponding generated entity and DbContext classes.

### Example Database Schema

```sql
USE [master];
GO

CREATE DATABASE [TechTalk];
GO

USE [TechTalk];
GO

CREATE TABLE [dbo].[Speakers](
    [Id] [int] IDENTITY(1,1) NOT NULL,
      NOT NULL,
      NOT NULL, 
    PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[Events](
    [Id] [int] IDENTITY(1,1) NOT NULL,
      NOT NULL,
    [StartAt] [datetime] NOT NULL,
    [EndAt] [datetime] NOT NULL,
    [SpeakerId] [int] NOT NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([SpeakerId]) REFERENCES [dbo].[Speakers] ([Id])
);
GO

SET IDENTITY_INSERT [dbo].[Speakers] ON;
GO
INSERT [dbo].[Speakers] ([Id], [FirstName], [LastName]) VALUES (1, N'John', N'Smith');
INSERT [dbo].[Speakers] ([Id], [FirstName], [LastName]) VALUES (2, N'Peter', N'Kios');
GO
SET IDENTITY_INSERT [dbo].[Speakers] OFF;
GO

SET IDENTITY_INSERT [dbo].[Events] ON;
GO
INSERT [dbo].[Events] ([Id], [Title], [StartAt], [EndAt], [SpeakerId]) VALUES (1, N'The power of Software', CAST(N'2023-01-10T10:00:00.000' AS DateTime), CAST(N'2023-01-10T11:00:00.000' AS DateTime), 1);
INSERT [dbo].[Events] ([Id], [Title], [StartAt], [EndAt], [SpeakerId]) VALUES (2, N'The Rumour''s weapon', CAST(N'2023-01-10T12:00:00.000' AS DateTime), CAST(N'2023-01-10T13:00:00.000' AS DateTime), 2);
GO
SET IDENTITY_INSERT [dbo].[Events] OFF;
GO
```

### Generated Code

#### Event.cs

```csharp
using System;
using System.Collections.Generic;

namespace YourNamespace;

public partial class Event
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public int SpeakerId { get; set; }

    public virtual Speaker Speaker { get; set; } = null!;
}
```

#### Speaker.cs

```csharp
using System;
using System.Collections.Generic;

namespace YourNamespace;

public partial class Speaker
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
```

#### TechTalkContext.cs

```csharp
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace YourNamespace;

public partial class TechTalkContext : DbContext
{
    public TechTalkContext()
    {
    }

    public TechTalkContext(DbContextOptions<TechTalkContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }
    public virtual DbSet<Speaker> Speakers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=M7_TROJAN;Initial Catalog=TechTalk;Integrated Security=SSPI;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Events__3214EC07F03D657F");

            entity.Property(e => e.EndAt).HasColumnType("datetime");
            entity.Property(e => e.StartAt).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Speaker).WithMany(p => p.Events)
                .HasForeignKey(d => d.SpeakerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Events__SpeakerI__398D8EEE");
        });

        modelBuilder.Entity<Speaker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Speakers__3214EC078679A6C4");

            entity.Property(e => e.FirstName).HasMaxLength(25);
            entity.Property(e => e.LastName).HasMaxLength(25);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
```

```sh
dotnet ef dbcontext scaffold "Data Source=M7_TROJAN;Initial Catalog=TechTalk;Integrated Security=SSPI;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --data-annotations --context AppDbContext --output-dir Models --context-dir Data
```

## Conclusion

Using the .NET CLI for reverse engineering in EF

 Core is a straightforward process that can save you a lot of time when working with an existing database schema. By understanding and using the available options, you can customize the scaffolding process to fit your project's needs.