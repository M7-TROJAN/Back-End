# Identity Managers in ASP.NET Core

## Introduction
ASP.NET Core Identity provides a powerful framework for managing users, roles, and authentication in web applications. Three key classes handle these tasks:

- **`UserManager<TUser>`** – Manages user creation, updates, and password handling.
- **`RoleManager<TRole>`** – Manages roles and role-based authorization.
- **`SignInManager<TUser>`** – Handles user authentication and sign-in operations.

This guide explains these identity managers in detail, with real-world scenarios and multiple examples.

---

## 1. `UserManager<TUser>`

### Overview
The `UserManager<TUser>` class provides functionalities to manage users, such as creating, updating, deleting, and validating users.

### Common Methods
| Method | Description |
|---------|-------------|
| `CreateAsync(user, password)` | Creates a new user with a password. |
| `FindByEmailAsync(email)` | Finds a user by their email address. |
| `FindByIdAsync(id)` | Finds a user by their unique identifier. |
| `UpdateAsync(user)` | Updates user information. |
| `DeleteAsync(user)` | Deletes a user. |
| `CheckPasswordAsync(user, password)` | Verifies if a password is correct. |
| `GeneratePasswordResetTokenAsync(user)` | Generates a password reset token. |

### Example: Creating a User
```csharp
public class UserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> CreateUserAsync(string email, string password)
    {
        var user = new ApplicationUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(user, password);
        
        return result.Succeeded ? "User created successfully" : "Failed to create user";
    }
}
```

### Real-World Scenario
**Scenario:** A company wants to register new employees as users in their internal portal.

- The HR team inputs employee details.
- The system automatically creates an account with a temporary password.
- The employee receives an email with a link to reset the password.

```csharp
var newUser = new ApplicationUser { UserName = "johndoe", Email = "johndoe@company.com" };
var password = "Temp@123";
var createResult = await _userManager.CreateAsync(newUser, password);
if (createResult.Succeeded)
{
    var token = await _userManager.GeneratePasswordResetTokenAsync(newUser);
    Console.WriteLine("Send this reset link to the user: " + token);
}
```

---

## 2. `RoleManager<TRole>`

### Overview
The `RoleManager<TRole>` class manages user roles in an application.

### Common Methods
| Method | Description |
|---------|-------------|
| `CreateAsync(role)` | Creates a new role. |
| `DeleteAsync(role)` | Deletes a role. |
| `FindByNameAsync(name)` | Finds a role by name. |
| `RoleExistsAsync(name)` | Checks if a role exists. |
| `AddToRoleAsync(user, roleName)` | Assigns a user to a role. |
| `GetRolesAsync(user)` | Gets all roles assigned to a user. |

### Example: Creating and Assigning Roles
```csharp
public class RoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<string> AssignRoleAsync(ApplicationUser user, string role)
    {
        if (!await _roleManager.RoleExistsAsync(role))
        {
            await _roleManager.CreateAsync(new IdentityRole(role));
        }
        await _userManager.AddToRoleAsync(user, role);
        return "Role assigned successfully";
    }
}
```

### Real-World Scenario
**Scenario:** An e-commerce platform needs to distinguish between "Admins", "Sellers", and "Customers".

- Admins have full access.
- Sellers can manage products.
- Customers can purchase items.

```csharp
await _roleManager.CreateAsync(new IdentityRole("Admin"));
await _roleManager.CreateAsync(new IdentityRole("Seller"));
await _roleManager.CreateAsync(new IdentityRole("Customer"));
```

Assigning a user to a role:
```csharp
var user = await _userManager.FindByEmailAsync("seller@example.com");
await _userManager.AddToRoleAsync(user, "Seller");
```

---

## 3. `SignInManager<TUser>`

### Overview
The `SignInManager<TUser>` class handles user authentication and sign-in operations.

### Common Methods
| Method | Description |
|---------|-------------|
| `PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure)` | Signs in a user with a password. |
| `SignInAsync(user, isPersistent)` | Signs in a user. |
| `SignOutAsync()` | Signs out the current user. |
| `TwoFactorAuthenticatorSignInAsync(token, rememberMe, rememberBrowser)` | Signs in using 2FA. |
| `GetExternalLoginInfoAsync()` | Gets external login details (e.g., Google, Facebook). |

### Example: Logging In a User
```csharp
public class AuthenticationService
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthenticationService(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<string> LoginAsync(string email, string password, bool rememberMe)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, false);
        return result.Succeeded ? "Login successful" : "Invalid credentials";
    }
}
```

### Real-World Scenario
**Scenario:** A banking app requires 2FA for sensitive transactions.

```csharp
var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(code, false, false);
if (result.Succeeded)
{
    Console.WriteLine("Access granted");
}
else
{
    Console.WriteLine("Invalid 2FA code");
}
```

---

## Conclusion
ASP.NET Core Identity provides powerful user management tools:

- `UserManager<TUser>` handles user creation, updates, and password validation.
- `RoleManager<TRole>` manages roles and permissions.
- `SignInManager<TUser>` authenticates users and handles sign-in operations.

These identity managers are essential for any secure web application.

### Official Documentation
- [UserManager Documentation](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.usermanager-1?view=aspnetcore-8.0)
- [RoleManager Documentation](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.rolemanager-1?view=aspnetcore-8.0)
- [SignInManager Documentation](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.signinmanager-1?view=aspnetcore-8.0)

