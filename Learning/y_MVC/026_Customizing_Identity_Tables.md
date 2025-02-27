# Customizing Identity Tables in ASP.NET Core

## Introduction
In ASP.NET Core Identity, the default schema and table names for identity-related tables are predefined. However, you may need to customize these names to align with your database design requirements. This guide demonstrates how to change the schema and table names of identity tables.

## Modifying Identity Table Names and Schema
To customize the identity table names, modify the `OnModelCreating` method in `ApplicationDbContext`.

### Update `ApplicationDbContext`
Modify your `ApplicationDbContext` class to specify custom table names and schemas:

```csharp
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasSequence<int>("SerialNumber", schema: "shared")
            .StartsAt(1000001)
            .IncrementsBy(1);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(builder);

        // if you want to change the name of the table
        // this will change the name of the table from AspNetUsers to Users
        builder.Entity<IdentityUser>(entity =>
        {
            entity.ToTable("Users");
        });
        
        // if you want to change the name of the table and the schema
        // this will change the name of the table from AspNetRoles to Roles and the schema from dbo to security
        builder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable("Roles", "security");
        });
        
        // this will change the name of the table from AspNetUserRoles to UserRoles and the schema from dbo to security
        // (note that the IdentityUserRole is generic and you need to provide the type of the key, that is string in this case <string>)
        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("UserRoles", "security");
        });

        // Other identity tables can be modified using the same pattern
    }
}
```

## Applying the Changes with Migrations
Once you've updated the `ApplicationDbContext`, generate and apply a migration.

### Step 1: Add Migration
Run the following command in the Package Manager Console (PMC):

```sh
Add-Migration RenameIdentityTables
```

### Step 2: Generated Migration Code
This migration will rename and move the tables to the specified schema. The `Up` method in the migration file will look similar to this:

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.DropForeignKey("FK_AspNetRoleClaims_AspNetRoles_RoleId", "AspNetRoleClaims");
    migrationBuilder.DropForeignKey("FK_AspNetUserClaims_AspNetUsers_UserId", "AspNetUserClaims");
    migrationBuilder.DropForeignKey("FK_AspNetUserLogins_AspNetUsers_UserId", "AspNetUserLogins");
    migrationBuilder.DropForeignKey("FK_AspNetUserRoles_AspNetRoles_RoleId", "AspNetUserRoles");
    migrationBuilder.DropForeignKey("FK_AspNetUserRoles_AspNetUsers_UserId", "AspNetUserRoles");
    migrationBuilder.DropForeignKey("FK_AspNetUserTokens_AspNetUsers_UserId", "AspNetUserTokens");

    migrationBuilder.DropPrimaryKey("PK_AspNetUsers", "AspNetUsers");
    migrationBuilder.DropPrimaryKey("PK_AspNetUserRoles", "AspNetUserRoles");
    migrationBuilder.DropPrimaryKey("PK_AspNetRoles", "AspNetRoles");

    migrationBuilder.EnsureSchema("security");

    migrationBuilder.RenameTable("AspNetUsers", newName: "Users");
    migrationBuilder.RenameTable("AspNetUserRoles", newName: "UserRoles", newSchema: "security");
    migrationBuilder.RenameTable("AspNetRoles", newName: "Roles", newSchema: "security");

    migrationBuilder.AddPrimaryKey("PK_Users", "Users", "Id");
    migrationBuilder.AddPrimaryKey("PK_UserRoles", "security", "UserRoles", new[] { "UserId", "RoleId" });
    migrationBuilder.AddPrimaryKey("PK_Roles", "security", "Roles", "Id");
}
```

### Step 3: Apply the Migration
Execute the migration using:

```sh
Update-Database
```

## Customizing Identity Table Columns
You can also customize the columns of identity tables. Below are examples of different modifications you can apply.

### Removing a Column
If you want to remove the `Email` column from the `IdentityUser` table, use the following configuration:

```csharp
builder.Entity<IdentityUser>().Ignore(u => u.Email);
```

To remove multiple columns, for example, `PhoneNumber` and `PhoneNumberConfirmed`, use:

```csharp
builder.Entity<IdentityUser>()
    .Ignore(u => u.PhoneNumber)
    .Ignore(u => u.PhoneNumberConfirmed);
```

### Renaming a Column
To rename the `UserName` column to `User_Name`, use:

```csharp
builder.Entity<IdentityUser>().Property(u => u.UserName).HasColumnName("User_Name");
```

### Changing Column Type
To change the data type of the `UserName` column to `varchar(100)`, use:

```csharp
builder.Entity<IdentityUser>().Property(u => u.UserName).HasColumnType("varchar(100)");
```

## Conclusion
By following these steps, you can customize the schema and table names of ASP.NET Core Identity tables to better fit your application's needs. Additionally, you can modify individual columns to align with your specific requirements.

## Extending IdentityUser
If you want to extend the `IdentityUser` class by adding a new property, such as `FullName`, follow these steps:

### Step 1: Create a Custom Model
Create a new model that inherits from `IdentityUser`:

```csharp
using Microsoft.AspNetCore.Identity;

namespace LitraLand.Web.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string FullName { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
    }
}
```

### Step 2: Update `ApplicationDbContext`
Modify `ApplicationDbContext` to use `ApplicationUser` instead of `IdentityUser`:

```csharp
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasSequence<int>("SerialNumber", schema: "shared")
            .StartsAt(1000001)
            .IncrementsBy(1);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(builder);
    }
}
```

### Step 3: Update `Program.cs`
Modify the service registration in `Program.cs`:

```csharp
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
```

### Step 4: Apply Migrations
Run the following command in the Package Manager Console:

```sh
Add-Migration ExtendUsersTable
Update-Database
```

## Conclusion
By following these steps, you can customize the schema and table names of ASP.NET Core Identity tables to better fit your application's needs. Additionally, you can modify individual columns and extend the `IdentityUser` class to include custom properties.
