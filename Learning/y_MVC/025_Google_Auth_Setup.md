# Google Authentication in ASP.NET Core

This guide provides a step-by-step approach to integrating Google authentication in an ASP.NET Core application, storing credentials securely, and implementing authentication logic.

---

## 🔥 **Step 1: Set Up Google OAuth Credentials in Google Console**

### 1️⃣ **Go to Google Cloud Console**  
- Navigate to [Google Cloud Console](https://console.cloud.google.com/).
- Sign in with your Google account.

### 2️⃣ **Create a New Project**  
- Click **Select a project** dropdown at the top.
- Click **New Project** → Enter a name (e.g., "LitraLandAuth").
- Click **Create**.

### 3️⃣ **Enable OAuth Consent Screen**  
- In the left menu, go to **APIs & Services → OAuth Consent Screen**.
- Choose **External** for public users.
- Fill in **App Name**, **User Support Email**, and **Developer Contact Information**.
- Click **Save and Continue**.

### 4️⃣ **Create OAuth Credentials**  
- Go to **APIs & Services → Credentials**.
- Click **Create Credentials** → Select **OAuth 2.0 Client ID**.
- Choose **Application Type: Web Application**.
- Enter a **Name** (e.g., "LitraLandAuth").
- Under **Authorized JavaScript origins**, enter:
  - `http://localhost:5000`
  - Your production URL (if applicable).
- Under **Authorized redirect URIs**, enter:
  - `http://localhost:5000/signin-google`
  - Your production redirect URI.
- Click **Create** and copy **Client ID** and **Client Secret**.

---

## 🔧 **Step 2: Configure Google Authentication in ASP.NET Core**

### 📂 **1️⃣ Store Credentials in `secrets.json`**
Modify `secrets.json`:

```json
{
  "GoogleAuthSettings:ClientId": "YOUR_GOOGLE_CLIENT_ID",
  "GoogleAuthSettings:ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
}
```

🔹 **Tip:** Never hardcode secrets in `appsettings.json`. Use `secrets.json` or environment variables.

---

### 📂 **2️⃣ Create `GoogleAuthSettings.cs` Class**
Inside `Settings/GoogleAuthSettings.cs`:

```csharp
namespace LitraLand.Web.Settings
{
    public class GoogleAuthSettings
    {
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
    }
}
```

---

### 🔧 **3️⃣ Load GoogleAuthSettings in `Program.cs`**
Modify `Program.cs`:

```csharp
// Load Google Auth Settings
var googleAuthSection = builder.Configuration.GetSection(nameof(GoogleAuthSettings))
    ?? throw new InvalidOperationException("GoogleAuthSettings section not found.");
builder.Services.Configure<GoogleAuthSettings>(googleAuthSection);
```

---

### 🔑 **4️⃣ Configure Google Authentication Middleware**
Modify `Program.cs`:

```csharp
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Options;
using LitraLand.Web.Settings;

var app = builder.Build();

// Configure Authentication
var googleAuthSettings = app.Services.GetRequiredService<IOptions<GoogleAuthSettings>>().Value;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Cookies";
    options.DefaultSignInScheme = "Cookies";
    options.DefaultChallengeScheme = "Google";
})
.AddCookie("Cookies")
.AddGoogle("Google", options =>
{
    options.ClientId = googleAuthSettings.ClientId;
    options.ClientSecret = googleAuthSettings.ClientSecret;
    options.SignInScheme = "Cookies";
});

app.UseAuthentication();
app.UseAuthorization();
```

---

Let me explain this part in detail and break it down step by step.  

---

In the above step, we are configuring authentication in `Program.cs` to use **Google OAuth** along with **cookie authentication** for session management. 

---

### **🔎 Breakdown of the Code**
#### **1️⃣ Retrieve GoogleAuthSettings from Dependency Injection**
```csharp
var googleAuthSettings = app.Services.GetRequiredService<IOptions<GoogleAuthSettings>>().Value;
```
- This retrieves the **Google authentication settings** (Client ID and Client Secret) from the `GoogleAuthSettings` class, which we previously bound to `appsettings.json` or `secrets.json`.  
- `IOptions<GoogleAuthSettings>` is used to **inject configuration settings** into the application.

---

#### **2️⃣ Configure Authentication Schemes**
```csharp
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Cookies";
    options.DefaultSignInScheme = "Cookies";
    options.DefaultChallengeScheme = "Google";
})
```
- **`options.DefaultAuthenticateScheme = "Cookies"`**  
  - This tells ASP.NET Core **how to authenticate users**.  
  - We use `"Cookies"` to persist authentication via browser cookies.
  
- **`options.DefaultSignInScheme = "Cookies"`**  
  - This defines the method **for signing in the user** after authentication.

- **`options.DefaultChallengeScheme = "Google"`**  
  - When a user tries to **log in**, the system redirects them to **Google's OAuth login page**.

---

#### **3️⃣ Add Cookie Authentication**
```csharp
.AddCookie("Cookies")
```
- We add **cookie-based authentication** so the user **remains logged in** after authenticating with Google.
- When Google successfully authenticates the user, a **cookie is issued** to track the session.

---

#### **4️⃣ Add Google Authentication Provider**
```csharp
.AddGoogle("Google", options =>
{
    options.ClientId = googleAuthSettings.ClientId;
    options.ClientSecret = googleAuthSettings.ClientSecret;
    options.SignInScheme = "Cookies";
});
```
- **`AddGoogle("Google", options => { ... })`**  
  - This registers **Google as an authentication provider**.

- **`options.ClientId = googleAuthSettings.ClientId;`**  
  - Fetches the `ClientId` from our **GoogleAuthSettings class**.

- **`options.ClientSecret = googleAuthSettings.ClientSecret;`**  
  - Fetches the `ClientSecret` from **GoogleAuthSettings**.

- **`options.SignInScheme = "Cookies";`**  
  - After Google **successfully authenticates** the user, ASP.NET Core stores the login session in a cookie.

---

#### **5️⃣ Enable Authentication & Authorization Middleware**
```csharp
app.UseAuthentication();
app.UseAuthorization();
```
- **`app.UseAuthentication();`**  
  - Enables the authentication system, allowing users to sign in.
  
- **`app.UseAuthorization();`**  
  - Enables role-based access control (if applicable).

---

### **📌 Why Use Cookies for Google Authentication?**
By default, Google provides a token upon successful login. However, to keep users logged in, we need **session persistence**. Instead of requiring re-authentication on every request, we store a **cookie** containing the session details.

---

### **🛠 Summary of What This Code Does**
1️⃣ Retrieves the **Google Client ID and Secret** from the settings file.  
2️⃣ Configures **authentication schemes** (`Cookies` for session tracking, `Google` for login).  
3️⃣ Enables **cookie authentication** so users remain signed in.  
4️⃣ Registers **Google as an authentication provider** and passes `ClientId` and `ClientSecret`.  
5️⃣ Enables **middleware** for authentication and authorization.  

---

---

## 🛠️ **Step 3: Implement Google Authentication Controller**

### 📂 **1️⃣ Create `AuthController.cs`**
Inside `Controllers/AuthController.cs`:

```csharp
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LitraLand.Web.Controllers
{
    public class AuthController : Controller
    {
        // Login with Google
        public IActionResult Login()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // Google Callback
        public async Task<IActionResult> GoogleResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync("Cookies");
            if (!authenticateResult.Succeeded)
                return RedirectToAction("Login");

            var claims = authenticateResult.Principal?.Identities
                .FirstOrDefault()?.Claims.Select(c => new { c.Type, c.Value });

            return Json(claims);
        }

        // Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
```

---

## 🎨 **Step 4: Create Views for Login & Logout**

### 📂 **1️⃣ Add Login Button in `Views/Shared/_Layout.cshtml`**
Modify `_Layout.cshtml`:

```html
<ul class="navbar-nav">
    @if (User.Identity.IsAuthenticated)
    {
        <li class="nav-item">
            <a class="nav-link" asp-controller="Auth" asp-action="Logout">Logout</a>
        </li>
    }
    else
    {
        <li class="nav-item">
            <a class="nav-link" asp-controller="Auth" asp-action="Login">Login with Google</a>
        </li>
    }
</ul>
```

---

## 🏁 **Step 5: Run & Test the Application**
1. Run your application:
   ```sh
   dotnet run
   ```
2. Open `http://localhost:5000`
3. Click **Login with Google**.
4. Authenticate with Google.
5. You should see your claims in JSON format.
6. Click **Logout** to sign out.

---

## 🎯 **Bonus: Protect Routes with Authentication**
Modify `Controllers/HomeController.cs`:

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LitraLand.Web.Controllers
{
    [Authorize] // Protects this controller
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
```
Now, only authenticated users can access the **HomeController**.

---

## 🚀 **Final Thoughts**
✅ **Google authentication is now fully integrated!**  
This approach keeps credentials secure using `secrets.json` and follows **best practices** for authentication in ASP.NET Core.
