Authentication & Authorization in ASP.NET Core Identity

Authentication and authorization are two fundamental concepts in securing applications. In ASP.NET Core, we manage them using ASP.NET Core Identity, which provides a built-in system for handling user management, roles, and permissions.

When you create a project and select Individual User Accounts, ASP.NET Core Identity automatically configures everything for you, including database tables and authentication logic.

Understanding the Identity Tables in the Database

When you set up ASP.NET Core Identity, it creates seven main tables in the database. Let’s go through each one, understand its purpose, and how they relate to each other.

1. AspNetUsers (Users Table)
	•	This is the main table where all users are stored.
	•	It contains fields like:
	•	Id (Primary Key)
	•	UserName
	•	NormalizedUserName
	•	Email
	•	PasswordHash
	•	PhoneNumber
	•	TwoFactorEnabled
	•	LockoutEnabled
	•	You can customize this table by adding additional columns (e.g., Full Name, Profile Picture).

2. AspNetRoles (Roles Table)
	•	Stores all the roles available in the system.
	•	Contains:
	•	Id (Primary Key)
	•	Name (Role Name, e.g., “Admin”, “Manager”, “User”)
	•	NormalizedName

3. AspNetUserRoles (User-Roles Relationship Table)
	•	Many-to-Many relationship between Users and Roles.
	•	Contains:
	•	UserId (FK → AspNetUsers)
	•	RoleId (FK → AspNetRoles)
	•	This means that one user can have multiple roles, and one role can be assigned to multiple users.

4. AspNetRoleClaims (Role-Based Permissions Table)
	•	Stores claims (permissions) assigned to a Role.
	•	Contains:
	•	Id (Primary Key)
	•	RoleId (FK → AspNetRoles)
	•	ClaimType (e.g., “CanEditUsers”)
	•	ClaimValue (e.g., “true”)
	•	This allows all users within a role to inherit specific permissions.

5. AspNetUserClaims (User-Specific Claims Table)
	•	Stores custom claims assigned to specific users.
	•	Contains:
	•	Id (Primary Key)
	•	UserId (FK → AspNetUsers)
	•	ClaimType
	•	ClaimValue
	•	Difference from AspNetRoleClaims:
	•	AspNetRoleClaims: Assigns permissions to a role (applies to all users in that role).
	•	AspNetUserClaims: Assigns permissions to a specific user only.

6. AspNetUserLogins (External Logins Table)
	•	Stores authentication information for users who log in with external providers (Google, Facebook, etc.).
	•	Contains:
	•	UserId (FK → AspNetUsers)
	•	LoginProvider (e.g., “Google”, “Facebook”)
	•	ProviderKey (Unique ID from the external provider)

7. AspNetUserTokens (User Token Storage)
	•	Stores authentication tokens for users.
	•	Used for features like remember me, password resets, or two-factor authentication (2FA).

Role-Based Authorization vs. Permission-Based Authorization

There are two main approaches to controlling access to resources:

1️⃣ Role-Based Authorization
	•	Assigns roles to users.
	•	Grants access to resources based on roles.
	•	Example:
	•	The Admin role can access Admin Dashboard.
	•	The Manager role can access Employee Management.

Implementation Example (Role-Based Authorization)

Step 1: Assign a Role to a User

You can assign a role when creating a user:

await _userManager.AddToRoleAsync(user, "Admin");

Step 2: Protect Controller or Action Based on Role

[Authorize(Roles = "Admin")]
public IActionResult AdminDashboard()
{
    return View();
}

	•	Only users with the Admin role can access this controller/action.

Step 3: Multiple Roles Example

[Authorize(Roles = "Admin,Manager")]
public IActionResult ManageEmployees()
{
    return View();
}

	•	Only users with Admin OR Manager roles can access this.

2️⃣ Permission-Based Authorization
	•	Instead of assigning roles, we assign specific permissions.
	•	Uses claims to define fine-grained access.
	•	Example:
	•	Admin and Manager roles both have "CanViewReports" permission.
	•	A user without the role but with the claim "CanViewReports" can also access reports.

Implementation Example (Permission-Based Authorization)

Step 1: Assign a Claim to a User

await _userManager.AddClaimAsync(user, new Claim("CanViewReports", "true"));

Step 2: Check Permission in Controller

[Authorize(Policy = "CanViewReports")]
public IActionResult Reports()
{
    return View();
}

Step 3: Define the Policy in Program.cs

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanViewReports", policy =>
        policy.RequireClaim("CanViewReports", "true"));
});

Role-Based vs. Permission-Based: Which One to Use?

Feature	Role-Based	Permission-Based
Best For	Simple applications	Complex applications
Assignment	Users are assigned roles	Users get fine-grained permissions
Flexibility	Less flexible	More flexible
Example	“Admin” role can access everything	Users can have "CanEditUsers" without being Admin

Advanced Scenario: Combining Roles & Claims

Imagine:
	•	There is a role called "Employee" that has general permissions.
	•	A specific employee needs extra permissions beyond their role.

Solution:
	1.	Assign Employee role to the user.
	2.	Add a special claim to the user.

Example:

await _userManager.AddToRoleAsync(user, "Employee");
await _userManager.AddClaimAsync(user, new Claim("CanApproveRequests", "true"));

Now, even though all Employees share general permissions, only this user can approve requests.

Summary

✔ Authentication → Confirms who you are (Login, Identity).
✔ Authorization → Controls what you can access (Roles, Claims, Policies).
✔ Role-Based Authorization → Access granted based on user roles.
✔ Permission-Based Authorization → Access granted based on user claims.
✔ Combining Roles & Claims → Provides maximum flexibility.

Let’s build a real-world project to implement Authentication & Authorization in ASP.NET Core MVC using ASP.NET Core Identity.

📌 Project Overview: Library Management System

We will create a Library Management System where:
	1.	Admins can manage books and users.
	2.	Librarians can add/edit books but cannot manage users.
	3.	Members can borrow books and view their borrowed books.
	4.	Custom Permissions allow granting specific actions to individual users.

📂 Step 1: Create a New ASP.NET Core MVC Project

Open your terminal and run:

dotnet new mvc -n LibraryManagement
cd LibraryManagement

Now, add ASP.NET Core Identity:

dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

🛠 Step 2: Configure Identity in Program.cs

Edit Program.cs to set up Identity:

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Models;

var builder = WebApplication.CreateBuilder(args);

// Database Connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

// Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManageBooks", policy => policy.RequireClaim("CanManageBooks"));
    options.AddPolicy("ManageUsers", policy => policy.RequireClaim("CanManageUsers"));
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

📄 Step 3: Create Database & Identity Tables

Create AppDbContext.cs to define the database context:

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Book> Books { get; set; }
}

Define the ApplicationUser model in Models/ApplicationUser.cs:

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } // Custom field
}

Define the Book model in Models/Book.cs:

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
}

Now, apply migrations and update the database:

dotnet ef migrations add InitialCreate
dotnet ef database update

👥 Step 4: Seed Roles & Admin User

Modify Program.cs to create roles and an admin user automatically:

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

void SeedRolesAndAdmin(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    var roles = new[] { "Admin", "Librarian", "Member" };

    foreach (var role in roles)
    {
        if (!roleManager.RoleExistsAsync(role).Result)
        {
            roleManager.CreateAsync(new IdentityRole(role)).Wait();
        }
    }

    var adminUser = userManager.FindByEmailAsync("admin@library.com").Result;
    if (adminUser == null)
    {
        var user = new ApplicationUser
        {
            UserName = "admin@library.com",
            Email = "admin@library.com",
            FullName = "Library Admin",
            EmailConfirmed = true
        };
        var result = userManager.CreateAsync(user, "Admin@123").Result;
        if (result.Succeeded)
        {
            userManager.AddToRoleAsync(user, "Admin").Wait();
        }
    }
}

SeedRolesAndAdmin(app);

🚀 This ensures:
	•	Roles are created (Admin, Librarian, Member).
	•	An admin user is automatically created.

🔐 Step 5: Implement Role-Based Authorization

Protect the Book Management Controller so only Admins & Librarians can access it.

Create Controllers/BookController.cs:

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Models;
using System.Linq;

[Authorize(Roles = "Admin,Librarian")]
public class BookController : Controller
{
    private readonly AppDbContext _context;

    public BookController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View(_context.Books.ToList());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(book);
    }
}

🛑 Step 6: Implement Permission-Based Authorization

Give a specific user permission to manage books

Add a claim to a librarian:

await _userManager.AddClaimAsync(user, new Claim("CanManageBooks", "true"));

Protect Controller Action with Claims

Modify BookController.cs:

[Authorize(Policy = "ManageBooks")]
public IActionResult Manage()
{
    return View();
}

📢 Step 7: Protect Views

In Views/Shared/_Layout.cshtml, hide links based on roles:

@if (User.IsInRole("Admin") || User.IsInRole("Librarian"))
{
    <li><a asp-controller="Book" asp-action="Index">Manage Books</a></li>
}

@if (User.HasClaim("CanManageBooks", "true"))
{
    <li><a asp-controller="Book" asp-action="Manage">Book Permissions</a></li>
}

🔥 Final Testing

1️⃣ Run the application:

dotnet run

2️⃣ Log in as admin@library.com → Access all features.
3️⃣ Create a Librarian and test role-based restrictions.
4️⃣ Assign “CanManageBooks” claim to a Member and test permission-based access.

🎯 Summary

✅ Authentication handled by ASP.NET Core Identity.
✅ Role-Based Authorization → Admins & Librarians manage books.
✅ Permission-Based Authorization → Individual users get specific permissions.
✅ Secure Views & Controllers to restrict access.
