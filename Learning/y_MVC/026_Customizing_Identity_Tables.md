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

## Conclusion
By following these steps, you can customize the schema and table names of ASP.NET Core Identity tables to better fit your application's needs. The same approach can be applied to other identity tables by specifying new names and schemas in the `OnModelCreating` method.

