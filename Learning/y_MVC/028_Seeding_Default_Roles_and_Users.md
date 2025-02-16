# 📌 **Seeding Default Roles and Users**
When running an application **for the first time**, we must **seed** (prepopulate) the database with:

- **Default roles** (`Admin`, `User`).
- **An admin user** (so we always have an administrator).

### ✅ **Step 1: Define Role Names in a Constants Class**
We create a static class `AppRoles` to store role names.

```csharp
namespace LitraLand.Web.Core.Consts
{
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
```
This prevents **hardcoding role names** throughout the project.

---

### ✅ **Step 2: Create `DefaultRols` Class**
This class ensures **roles exist** before assigning them.

```csharp
using Microsoft.AspNetCore.Identity;

namespace LitraLand.Web.Seeds
{
    public static class DefaultRols
    {
        public static async Task SeedRolsAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.Roles.AnyAsync())  // Check if roles exist
            {
                await roleManager.CreateAsync(new IdentityRole(AppRoles.Admin));
                await roleManager.CreateAsync(new IdentityRole(AppRoles.User));
            }
        }
    }
}
```

---

### ✅ **Step 3: Create `DefaultUsers` Class**
This class creates an **Admin user** and assigns it to the **Admin role**.

```csharp
using Microsoft.AspNetCore.Identity;

namespace LitraLand.Web.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@litraland.com",
                FullName = "Admin",
                EmailConfirmed = true
            };

            var user = await userManager.FindByNameAsync(admin.UserName);

            if (user is null)
            {
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, AppRoles.Admin);
            }
        }
    }
}
```

---

### ✅ **Step 4: Modify `Program.cs`**
#### 🔹 **1. Modify `AddIdentity`**
Replace:

```csharp
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true) 
    .AddEntityFrameworkStores<ApplicationDbContext>();
``` 

With:

```csharp
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();
```

**🔹 Why?**
- `AddDefaultIdentity` → Automatically registers **UserManager** but **not RoleManager**.
- `AddIdentity<ApplicationUser, IdentityRole>` → Manually configures both `UserManager` and `RoleManager`.

---

#### 🔹 **2. Seed Data on Startup**
After:
```csharp
app.UseAuthentication();
app.UseAuthorization();
```
Add:
```csharp
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

using var scope = scopeFactory.CreateScope();

var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

await DefaultRols.SeedRolsAsync(roleManager);
await DefaultUsers.SeedAdminUserAsync(userManager);
```

**🔹 Explanation:**
- `CreateScope()` → Creates a **new dependency injection scope** to get **services** (`RoleManager`, `UserManager`).
- `SeedRolsAsync(roleManager)` → Ensures **Admin & User roles** exist.
- `SeedAdminUserAsync(userManager)` → Ensures **Admin user** exists.

---

📌 **Microsoft Docs:**  
🔹 [`UserManager<TUser>`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.usermanager-1?view=aspnetcore-8.0)  
🔹 [`RoleManager<TRole>`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.rolemanager-1?view=aspnetcore-8.0)  
🔹 [`SignInManager<TUser>`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.signinmanager-1?view=aspnetcore-8.0)  

---
