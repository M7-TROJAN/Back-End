# Understanding External Authentication in ASP.NET Core Identity

## 📌 Understanding AspNetUserLogins Table in ASP.NET Core Identity

The `AspNetUserLogins` table is responsible for external authentication in ASP.NET Core Identity. It allows users to sign in using third-party providers like Google, Facebook, Microsoft, GitHub, etc., without needing to store their passwords in your database.

### 🔍 Why is AspNetUserLogins Important?
Instead of requiring users to create and remember a password, they can log in using an external account, such as:
- Google
- Facebook
- Microsoft
- Twitter

This is more secure because:
✅ Users don’t need to create a new password  
✅ Your app doesn’t store passwords, reducing security risks  
✅ Users can log in instantly if they are already authenticated on the provider  

### 📂 Table Structure: AspNetUserLogins
When a user logs in using an external provider, the system stores their information in `AspNetUserLogins`.

| Column Name         | Data Type      | Description                                      |
|---------------------|---------------|--------------------------------------------------|
| LoginProvider      | nvarchar(450)  | The name of the external provider (e.g., Google, Facebook) |
| ProviderKey        | nvarchar(450)  | A unique identifier for the user from the provider (e.g., Google ID) |
| ProviderDisplayName | nvarchar(256) | The display name of the provider (optional)     |
| UserId            | nvarchar(450)  | A foreign key linking to `AspNetUsers.Id`       |

### 📌 Relationships:
- `UserId` links to `AspNetUsers.Id` (One user can have multiple logins)
- `LoginProvider + ProviderKey` uniquely identify a user for a specific provider

---

## 🛠 Step-by-Step: Implementing External Login in ASP.NET Core MVC

We will configure Google authentication as an example.

### 📌 Step 1: Enable Google Authentication in `Program.cs`

Edit your `Program.cs` file to configure Google authentication:

```csharp
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Google Authentication
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = "YOUR_GOOGLE_CLIENT_ID";
        options.ClientSecret = "YOUR_GOOGLE_CLIENT_SECRET";
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

### 📌 Explanation
- `options.ClientId` → Get this from Google Developer Console
- `options.ClientSecret` → Generated when setting up OAuth in Google
- The app is now configured to accept Google login requests

---

### 📌 Step 2: Add External Login Button to UI
Modify `Views/Account/Login.cshtml` to add a Google login button:

```html
<form asp-action="ExternalLogin" method="post">
    <input type="hidden" name="provider" value="Google" />
    <button type="submit">Sign in with Google</button>
</form>
```

---

### 📌 Step 3: Implement External Login in `AccountController`
Create `AccountController.cs` to handle authentication:

```csharp
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using LibraryManagement.Models;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost]
    public IActionResult ExternalLogin(string provider)
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "Account");
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }

    public async Task<IActionResult> ExternalLoginCallback()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return RedirectToAction("Login");

        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

        if (signInResult.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (email != null)
        {
            var user = new ApplicationUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }
        }

        return RedirectToAction("Login");
    }
}
```

### 📌 Explanation
1. User clicks **Sign in with Google** → `ExternalLogin(provider)` triggers authentication
2. User is redirected to Google → If successful, Google sends user info back to `ExternalLoginCallback()`
3. Check if user exists in `AspNetUserLogins`:
   - **If yes** → Log them in
   - **If no** → Create a new user in `AspNetUsers` and link Google login
4. Save `LoginProvider` & `ProviderKey` in `AspNetUserLogins`

---

## 📂 Step 4: What Happens in the Database?

### 📌 Example Data in `AspNetUsers` Table
| Id        | UserName       | Email          |
|-----------|---------------|---------------|
| 1a2b3c4d | user@gmail.com | user@gmail.com |

### 📌 Example Data in `AspNetUserLogins` Table
| LoginProvider | ProviderKey  | UserId   |
|--------------|-------------|---------|
| Google       | 123456789   | 1a2b3c4d |

🚀 Now, every time the user logs in with Google, the app recognizes them!

---

## 🛠 Step 5: Allow Users to Link Multiple Logins

Modify `AccountController` to Allow Linking:

```csharp
[HttpPost]
public async Task<IActionResult> LinkLogin(string provider)
{
    var redirectUrl = Url.Action("LinkLoginCallback", "Account");
    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
    return new ChallengeResult(provider, properties);
}

public async Task<IActionResult> LinkLoginCallback()
{
    var info = await _signInManager.GetExternalLoginInfoAsync();
    var user = await _userManager.GetUserAsync(User);

    if (info != null && user != null)
    {
        await _userManager.AddLoginAsync(user, info);
    }

    return RedirectToAction("ManageLogins");
}
```

🎯 **Summary**
✅ `AspNetUserLogins` links external login providers to user accounts  
✅ Users don’t need passwords → Authentication handled by Google, Facebook, etc.  
✅ Faster, more secure login process  
✅ Users can link multiple providers to one account  

