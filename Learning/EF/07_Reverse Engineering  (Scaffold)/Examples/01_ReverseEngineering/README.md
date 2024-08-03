# Reverse Engineering (Scaffold) in Entity Framework Core

Reverse engineering, or scaffolding, is the process of generating entity type classes and a `DbContext` based on the schema of an existing database. This guide covers the steps to reverse engineer a database using Entity Framework Core.

## Overview

### What is Reverse Engineering?

Reverse engineering involves:
1. Reading the database schema.
2. Generating EF Core model classes.
3. Using the EF Core model to generate code.

### How It Works
1. **Reading DB Schema**: The process starts by reading the schema of the target database, including tables, columns, constraints, indexes, and relationships.
2. **Generate EF Core Model**: EF Core generates entity classes and a `DbContext` class based on the database schema.
3. **Use EF Core Model**: The generated model is used to interact with the database using LINQ and other EF Core features.

### Limitations
1. Providers are not equal.
2. Data type support varies by provider.
3. Concurrency token issues in EF Core.

## Prerequisites

1. Package Manager Console (PMC) tool or `dotnet CLI` tool.
2. Install the `Microsoft.EntityFrameworkCore.Design` package.
3. Install the provider for the database schema (e.g., `Microsoft.EntityFrameworkCore.SqlServer`).

## Database Schema

The database used in this example is `TechTalk`, with two tables: `Speakers` and `Events`.

```sql
USE [master]
GO

CREATE DATABASE [TechTalk]
GO

USE [TechTalk]
GO

CREATE TABLE [dbo].[Speakers](
    [Id] [int] IDENTITY(1,1) NOT NULL,
      NOT NULL,
      NOT NULL, 
    PRIMARY KEY ([Id])
)
GO

CREATE TABLE [dbo].[Events](
    [Id] [int] IDENTITY(1,1) NOT NULL,
      NOT NULL,
    [StartAt] [datetime] NOT NULL,
    [EndAt] [datetime] NOT NULL,
    [SpeakerId] [int] NOT NULL,
    PRIMARY KEY ([Id]),
    FOREIGN KEY([SpeakerId]) REFERENCES [dbo].[Speakers] ([Id])
)
GO

SET IDENTITY_INSERT [dbo].[Speakers] ON 
GO
INSERT [dbo].[Speakers] ([Id], [FirstName], [LastName]) VALUES (1, N'John', N'Smith')
GO
INSERT [dbo].[Speakers] ([Id], [FirstName], [LastName]) VALUES (2, N'Peter', N'Kios')
GO
SET IDENTITY_INSERT [dbo].[Speakers] OFF
GO

SET IDENTITY_INSERT [dbo].[Events] ON 
GO
INSERT [dbo].[Events] ([Id], [Title], [StartAt], [EndAt], [SpeakerId]) VALUES (1, N'The power of Software', CAST(N'2023-01-10T10:00:00.000' AS DateTime), CAST(N'2023-01-10T11:00:00.000' AS DateTime), 1)
GO
INSERT [dbo].[Events] ([Id], [Title], [StartAt], [EndAt], [SpeakerId]) VALUES (2, N'The Rumour''s weapon', CAST(N'2023-01-10T12:00:00.000' AS DateTime), CAST(N'2023-01-10T13:00:00.000' AS DateTime), 2)
GO
SET IDENTITY_INSERT [dbo].[Events] OFF
GO
```

## Step-by-Step Guide

### Step 1: Package Manager Console (PMC)

Open the Package Manager Console from Tools -> NuGet Package Manager -> Package Manager Console.

### Step 2: Install EF Core Tools

Run the following command to install EF Core tools:

```powershell
Install-Package Microsoft.EntityFrameworkCore.Tools
```

### Step 3: Install EF Core Design Package

Install the `Microsoft.EntityFrameworkCore.Design` package:

```powershell
Install-Package Microsoft.EntityFrameworkCore.Design
```

### Step 4: Install Database Provider

Install the SQL Server provider for EF Core:

```powershell
Install-Package Microsoft.EntityFrameworkCore.SqlServer
```

### Step 5: Scaffold the Database

Run the scaffold command to generate the EF Core model from the database schema:

```powershell
Scaffold-DbContext "Data Source=M7_TROJAN;Initial Catalog=TechTalk;Integrated Security=SSPI;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer
```

## Generated Files

After running the scaffold command, Visual Studio will generate the following files:

### Event.cs

```csharp
using System;
using System.Collections.Generic;

namespace _01_ReverseEngineering
{
    public partial class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public int SpeakerId { get; set; }
        public virtual Speaker Speaker { get; set; } = null!;
    }
}
```

### Speaker.cs

```csharp
using System;
using System.Collections.Generic;

namespace _01_ReverseEngineering
{
    public partial class Speaker
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
```

### TechTalkContext.cs

```csharp
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace _01_ReverseEngineering
{
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
}
```

### Program.cs

Here's an example of how to use the generated context and entities:

```csharp
namespace _01_ReverseEngineering
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new TechTalkContext())
            {
                var events = context.Events.ToList();
                var speakers = context.Speakers.ToList();

                foreach (var speaker in speakers)
                {
                    Console.WriteLine($"Speaker: {speaker.FirstName} {speaker.LastName}");
                    foreach (var talk in speaker.Events)
                    {
                        Console.WriteLine($"\t{talk.Title}");
                    }
                }

                Console.WriteLine("\n----------------------\n");

                foreach (var talk in events)
                {
                    Console.WriteLine($"Event: {talk.Title}");
                    Console.WriteLine($"\tSpeaker: {talk.Speaker.FirstName} {talk.Speaker.LastName}");
                }
            }
        }
    }
}
```

## Explanation of Key Points

1. **Specifying Table Names**:
    ```csharp
    modelBuilder.Entity<Event>().ToTable("Events");
    ```
    - This maps the `Event` entity to the `Events` table in the database.

2. **Configuring Primary Keys**:
    ```csharp
    entity.HasKey(e => e.Id);
    ```
    - This specifies that the `Id` property is the primary key.

3. **Configuring Properties**:
    ```csharp
    entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
    ```
    - The `Title` property is required and has a maximum length of 255 characters.

4.

 **Configuring Relationships**:
    ```csharp
    entity.HasOne(d => d.Speaker).WithMany(p => p.Events).HasForeignKey(d => d.SpeakerId);
    ```
    - This configures the one-to-many relationship between `Speaker` and `Event`.

By following these steps, you can successfully reverse engineer your existing database schema into an Entity Framework Core model and use it in your application.