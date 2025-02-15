# Implementing Authentication & Authorization in ASP.NET Core MVC

## 📌 Project Overview: Library Management System

We will create a **Library Management System** where:
- **Admins** can manage books and users.
- **Librarians** can add/edit books but cannot manage users.
- **Members** can borrow books and view their borrowed books.
- **Custom Permissions** allow granting specific actions to individual users.

## 📂 Step 1: Create a New ASP.NET Core MVC Project

Open your terminal and run:

```sh
dotnet new mvc -n LibraryManagement
cd LibraryManagement
```

Now, add ASP.NET Core Identity:

```sh
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

## 🛠 Step 2: Configure Identity in `Program.cs`

Edit `Program.cs` to set up Identity:

```csharp
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
```

## 📄 Step 3: Create Database & Identity Tables

Create `AppDbContext.cs` to define the database context:

```csharp
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Book> Books { get; set; }
}
```

Define the `ApplicationUser` model in `Models/ApplicationUser.cs`:

```csharp
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } // Custom field
}
```

Define the `Book` model in `Models/Book.cs`:

```csharp
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
}
```

Now, apply migrations and update the database:

```sh
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 👥 Step 4: Seed Roles & Admin User

Modify `Program.cs` to create roles and an admin user automatically:

```csharp
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
```

🚀 This ensures:
- Roles are created (**Admin, Librarian, Member**).
- An **admin user** is automatically created.

## 🔐 Step 5: Implement Role-Based Authorization

Protect the **Book Management Controller** so only **Admins & Librarians** can access it.

Create `Controllers/BookController.cs`:

```csharp
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
```

## 🛑 Step 6: Implement Permission-Based Authorization

Give a specific user permission to manage books:

```csharp
await _userManager.AddClaimAsync(user, new Claim("CanManageBooks", "true"));
```

Protect Controller Action with Claims:

Modify `BookController.cs`:

```csharp
[Authorize(Policy = "ManageBooks")]
public IActionResult Manage()
{
    return View();
}
```

## 📢 Step 7: Protect Views

In `Views/Shared/_Layout.cshtml`, hide links based on roles:

```html
@if (User.IsInRole("Admin") || User.IsInRole("Librarian"))
{
    <li><a asp-controller="Book" asp-action="Index">Manage Books</a></li>
}

@if (User.HasClaim("CanManageBooks", "true"))
{
    <li><a asp-controller="Book" asp-action="Manage">Book Permissions</a></li>
}
```

## 🔥 Final Testing

1️⃣ Run the application:

```sh
dotnet run
```

2️⃣ Log in as `admin@library.com` → Access all features.

3️⃣ Create a **Librarian** and test role-based restrictions.

4️⃣ Assign `CanManageBooks` claim to a **Member** and test permission-based access.

---
