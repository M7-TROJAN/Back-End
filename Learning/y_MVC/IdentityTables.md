# Authentication & Authorization in ASP.NET Core Identity

Authentication and authorization are fundamental to securing applications. In ASP.NET Core, these functionalities are managed using **ASP.NET Core Identity**, which provides built-in user management, role handling, and permission management.

When you create a project and select **Individual User Accounts**, ASP.NET Core Identity automatically configures the database tables and authentication logic for you.

---

## 📌 Identity Tables in the Database

ASP.NET Core Identity creates seven main tables in the database. Below is an overview of each table and its purpose:

### 1️⃣ `AspNetUsers` (Users Table)
- Stores user details.
- Contains fields like:
  - `Id` (Primary Key)
  - `UserName`, `NormalizedUserName`
  - `Email`, `PasswordHash`
  - `PhoneNumber`, `TwoFactorEnabled`
  - `LockoutEnabled`
- Can be customized by adding additional columns (e.g., Full Name, Profile Picture).

### 2️⃣ `AspNetRoles` (Roles Table)
- Stores all system roles.
- Fields include:
  - `Id` (Primary Key)
  - `Name` (e.g., "Admin", "Manager", "User")
  - `NormalizedName`

### 3️⃣ `AspNetUserRoles` (User-Roles Relationship Table)
- Implements a many-to-many relationship between Users and Roles.
- Fields:
  - `UserId` (FK → `AspNetUsers`)
  - `RoleId` (FK → `AspNetRoles`)
- A user can have multiple roles, and a role can be assigned to multiple users.

### 4️⃣ `AspNetRoleClaims` (Role-Based Permissions Table)
- Stores **claims** (permissions) assigned to roles.
- Fields:
  - `Id` (Primary Key)
  - `RoleId` (FK → `AspNetRoles`)
  - `ClaimType` (e.g., "CanEditUsers")
  - `ClaimValue` (e.g., "true")
- Allows role-based permission inheritance.

### 5️⃣ `AspNetUserClaims` (User-Specific Claims Table)
- Stores **custom claims** assigned to users.
- Fields:
  - `Id` (Primary Key)
  - `UserId` (FK → `AspNetUsers`)
  - `ClaimType`, `ClaimValue`
- **Difference from `AspNetRoleClaims`**:
  - `AspNetRoleClaims`: Assigns permissions to roles (affects all users in that role).
  - `AspNetUserClaims`: Assigns permissions to specific users only.

### 6️⃣ `AspNetUserLogins` (External Logins Table)
- Stores login details for users authenticating via external providers (Google, Facebook, etc.).
- Fields:
  - `UserId` (FK → `AspNetUsers`)
  - `LoginProvider` (e.g., "Google", "Facebook")
  - `ProviderKey` (Unique ID from the provider)

### 7️⃣ `AspNetUserTokens` (User Token Storage)
- Stores authentication tokens.
- Used for features like "Remember Me," password resets, and two-factor authentication (2FA).

---

## 🔐 Role-Based vs. Permission-Based Authorization

### 1️⃣ **Role-Based Authorization**
- Assigns **roles** to users.
- Grants access based on roles.
- Example:
  - **Admin** role can access the Admin Dashboard.
  - **Manager** role can access Employee Management.

#### **Implementation Example (Role-Based Authorization)**

✅ **Step 1: Assign a Role to a User**
```csharp
await _userManager.AddToRoleAsync(user, "Admin");
```
✅ **Step 2: Protect a Controller/Action Based on Role**
```csharp
[Authorize(Roles = "Admin")]
public IActionResult AdminDashboard()
{
    return View();
}
```
✅ **Step 3: Allow Multiple Roles**
```csharp
[Authorize(Roles = "Admin,Manager")]
public IActionResult ManageEmployees()
{
    return View();
}
```

### 2️⃣ **Permission-Based Authorization**
- Assigns **specific permissions** instead of roles.
- Uses **claims** to provide fine-grained access.
- Example:
  - Admin & Manager roles both have the **CanViewReports** permission.
  - A user without these roles can still access reports if they have the **CanViewReports** claim.

#### **Implementation Example (Permission-Based Authorization)**

✅ **Step 1: Assign a Claim to a User**
```csharp
await _userManager.AddClaimAsync(user, new Claim("CanViewReports", "true"));
```
✅ **Step 2: Check Permission in Controller**
```csharp
[Authorize(Policy = "CanViewReports")]
public IActionResult Reports()
{
    return View();
}
```
✅ **Step 3: Define the Policy in `Program.cs`**
```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanViewReports", policy =>
        policy.RequireClaim("CanViewReports", "true"));
});
```

---

## ⚖ Role-Based vs. Permission-Based: Which One to Use?

| Feature  | Role-Based  | Permission-Based  |
|----------|------------|------------------|
| **Best For** | Simple apps | Complex apps |
| **Assignment** | Users assigned roles | Users assigned permissions |
| **Flexibility** | Less flexible | More flexible |
| **Example** | "Admin" role accesses everything | "CanEditUsers" without being an Admin |

### 🔄 **Combining Roles & Claims**
Scenario:
- Employees have a general **Employee** role.
- A specific employee needs extra permissions.

✅ **Solution:**
```csharp
await _userManager.AddToRoleAsync(user, "Employee");
await _userManager.AddClaimAsync(user, new Claim("CanApproveRequests", "true"));
```
This way, all employees have base permissions, while this user gets extra permissions.

---

## 🚀 Real-World Example: Library Management System
We will create a **Library Management System** where:
1. **Admins** can manage books and users.
2. **Librarians** can add/edit books but not manage users.
3. **Members** can borrow books.
4. **Custom Permissions** grant specific actions to users.

### 📂 Step 1: Create a New ASP.NET Core MVC Project
```sh
dotnet new mvc -n LibraryManagement
cd LibraryManagement
```
Add ASP.NET Core Identity:
```sh
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```

### 🛠 Step 2: Configure Identity in `Program.cs`
```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManageBooks", policy => policy.RequireClaim("CanManageBooks"));
    options.AddPolicy("ManageUsers", policy => policy.RequireClaim("CanManageUsers"));
});
```

### 👥 Step 3: Seed Roles & Admin User
```csharp
await _userManager.AddToRoleAsync(user, "Admin");
await _userManager.AddClaimAsync(user, new Claim("CanManageBooks", "true"));
```

---

## 🎯 Summary
✔ **Authentication** → Confirms identity (Login, Identity).
✔ **Authorization** → Controls access (Roles, Claims, Policies).
✔ **Role-Based** → Access via roles.
✔ **Permission-Based** → Access via claims.
✔ **Hybrid Approach** → Maximum flexibility.

---

