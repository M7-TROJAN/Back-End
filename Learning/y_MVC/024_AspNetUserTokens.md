# Understanding AspNetUserTokens in ASP.NET Core Identity

The `AspNetUserTokens` table stores tokens associated with users in ASP.NET Core Identity. These tokens are used for various purposes, such as password reset, two-factor authentication (2FA), email confirmation, and external authentication tokens (OAuth refresh tokens).

## 🔍 What is a Token?

A token is a small piece of data used for authentication and authorization. It can be used to:

- ✅ Verify a user’s identity (Authentication)
- ✅ Give permissions to a user (Authorization)
- ✅ Allow access without storing passwords (Security)

Think of a token like a ticket at an event 🎟️. If you have a ticket, you can enter the event without proving your identity every time.

## 📂 Table Structure: AspNetUserTokens

| Column Name   | Data Type      | Description |
|--------------|--------------|-------------|
| UserId       | nvarchar(450) | Links to `AspNetUsers.Id` (the user who owns the token) |
| LoginProvider| nvarchar(450) | The external provider (e.g., Google, Facebook) or Identity for internal tokens |
| Name         | nvarchar(450) | The name/type of the token (e.g., `RefreshToken`, `2FA`, `ResetPassword`) |
| Value        | nvarchar(max) | The actual token value |

### 📌 Primary Key:
The combination of `UserId + LoginProvider + Name` makes each token unique.

## 📌 How AspNetUserTokens is Used?

### 1️⃣ Password Reset Token

When a user forgets their password, a reset token is generated and sent via email.

**Example Data in AspNetUserTokens Table:**

| UserId | LoginProvider | Name          | Value       |
|--------|--------------|--------------|-------------|
| 1a2b3c | Identity     | ResetPassword | XYZ123TOKEN |

#### 📌 Code Example: Generate Password Reset Token
```csharp
public async Task<IActionResult> ForgotPassword(string email)
{
    var user = await _userManager.FindByEmailAsync(email);
    if (user == null) return BadRequest("User not found");

    string token = await _userManager.GeneratePasswordResetTokenAsync(user);

    // Save token to database (AspNetUserTokens)
    await _userManager.SetAuthenticationTokenAsync(user, "Identity", "ResetPassword", token);

    // Send token via email
    var resetLink = Url.Action("ResetPassword", "Account", new { email, token }, Request.Scheme);
    await _emailSender.SendEmailAsync(email, "Reset Password", $"Click <a href='{resetLink}'>here</a> to reset your password.");

    return Ok("Password reset link sent to email.");
}
```

---

### 2️⃣ Email Confirmation Token

When a user registers, an email confirmation token is stored in `AspNetUserTokens`.

**Example Data in AspNetUserTokens Table:**

| UserId | LoginProvider | Name              | Value       |
|--------|--------------|-------------------|-------------|
| 1a2b3c | Identity     | EmailConfirmation | XYZ123TOKEN |

#### 📌 Code Example: Generate Email Confirmation Token
```csharp
public async Task<IActionResult> SendEmailConfirmation(string email)
{
    var user = await _userManager.FindByEmailAsync(email);
    if (user == null) return BadRequest("User not found");

    string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

    // Save token to database (AspNetUserTokens)
    await _userManager.SetAuthenticationTokenAsync(user, "Identity", "EmailConfirmation", token);

    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { email, token }, Request.Scheme);
    await _emailSender.SendEmailAsync(email, "Confirm Email", $"Click <a href='{confirmationLink}'>here</a> to confirm your email.");

    return Ok("Confirmation email sent.");
}
```

---

### 3️⃣ Two-Factor Authentication (2FA) Token

If 2FA is enabled, the system generates a token when the user tries to log in.

**Example Data in AspNetUserTokens Table:**

| UserId | LoginProvider | Name | Value  |
|--------|--------------|------|--------|
| 1a2b3c | Identity     | 2FA  | 123456 |

#### 📌 Code Example: Generate 2FA Token
```csharp
public async Task<IActionResult> Send2FAToken(string userId)
{
    var user = await _userManager.FindByIdAsync(userId);
    if (user == null) return BadRequest("User not found");

    string token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

    // Save token to database (AspNetUserTokens)
    await _userManager.SetAuthenticationTokenAsync(user, "Identity", "2FA", token);

    await _emailSender.SendEmailAsync(user.Email, "2FA Code", $"Your code: {token}");

    return Ok("2FA code sent.");
}
```

---

### 4️⃣ External OAuth Tokens (Google, Facebook, etc.)

When users log in using external providers (Google, Facebook, etc.), ASP.NET Core Identity stores an OAuth refresh token in `AspNetUserTokens`.

**Example Data in AspNetUserTokens Table:**

| UserId | LoginProvider | Name         | Value       |
|--------|--------------|-------------|-------------|
| 1a2b3c | Google       | RefreshToken | XYZ456TOKEN |

#### 📌 Code Example: Store OAuth Refresh Token
```csharp
public async Task<IActionResult> StoreOAuthToken(string userId, string provider, string tokenValue)
{
    var user = await _userManager.FindByIdAsync(userId);
    if (user == null) return BadRequest("User not found");

    await _userManager.SetAuthenticationTokenAsync(user, provider, "RefreshToken", tokenValue);

    return Ok("Token stored successfully.");
}
```

💡 OAuth Refresh Tokens allow users to stay logged in without re-entering credentials every time.

---

## 🎯 Summary

| Token Type       | Use Case |
|-----------------|----------|
| ResetPassword   | Used for password reset requests |
| EmailConfirmation | Used for verifying email addresses |
| 2FA             | Used for two-factor authentication |
| RefreshToken    | Used for external authentication with OAuth |

-🔹 The `AspNetUserTokens` table stores and manages these tokens securely.
-🔹 These tokens expire after a certain period for security reasons.
-🔹 You can retrieve and validate tokens using ASP.NET Core Identity methods.

---

## **📌 Implementing a Custom Token Validation System in ASP.NET Core**

### **🔹 Why Build a Custom Token System?**
While ASP.NET Core Identity provides built-in token management (`AspNetUserTokens`), a custom system allows you to:
✅ Set custom expiration times  
✅ Use stronger encryption  
✅ Add extra validation rules  
✅ Store tokens securely in a separate table  

---

## **📂 1️⃣ Creating a Custom Token Table**

Instead of using `AspNetUserTokens`, we create a **CustomUserTokens** table.

### **📌 Table Structure: `CustomUserTokens`**
| Column Name      | Data Type       | Description |
|-----------------|-----------------|-----------------|
| `Id`            | `int (PK)`       | Primary Key |
| `UserId`        | `nvarchar(450)`  | Foreign Key → `AspNetUsers.Id` |
| `Token`         | `nvarchar(max)`  | The actual token value |
| `TokenType`     | `nvarchar(100)`  | Type (ResetPassword, EmailConfirmation, 2FA, etc.) |
| `CreatedAt`     | `datetime`       | Token generation timestamp |
| `ExpiresAt`     | `datetime`       | Expiry date/time |
| `IsUsed`        | `bit`            | Marks if the token was already used |

---

## **🛠️ 2️⃣ Entity Model for CustomUserToken**
Create an **Entity Framework model** for the `CustomUserTokens` table.

```csharp
public class CustomUserToken
{
    public int Id { get; set; }  // Primary Key
    public string UserId { get; set; }  // Foreign Key to AspNetUsers
    public string Token { get; set; }  // The actual token
    public string TokenType { get; set; }  // Token type (ResetPassword, 2FA, etc.)
    public DateTime CreatedAt { get; set; }  // When the token was created
    public DateTime ExpiresAt { get; set; }  // Expiration time
    public bool IsUsed { get; set; }  // Whether the token is used

    // Navigation property for User
    public virtual IdentityUser User { get; set; }
}
```

---

## **🔹 3️⃣ Adding Token Management Logic**
Now, let's create a **service to generate, store, and validate tokens**.

### **✅ Token Generation Service**
This service will:
🔹 Generate a random secure token  
🔹 Store it in the database  
🔹 Set expiration time  

```csharp
public class CustomTokenService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;

    public CustomTokenService(UserManager<IdentityUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<string> GenerateTokenAsync(IdentityUser user, string tokenType, int expirationMinutes = 30)
    {
        string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)); // Secure token

        var userToken = new CustomUserToken
        {
            UserId = user.Id,
            Token = token,
            TokenType = tokenType,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes),
            IsUsed = false
        };

        _context.CustomUserTokens.Add(userToken);
        await _context.SaveChangesAsync();

        return token;
    }
}
```

---

### **✅ Token Validation Service**
This method will:
🔹 Check if the token exists  
🔹 Ensure it hasn't expired  
🔹 Ensure it's not already used  

```csharp
public async Task<bool> ValidateTokenAsync(string userId, string token, string tokenType)
{
    var userToken = await _context.CustomUserTokens
        .FirstOrDefaultAsync(t => t.UserId == userId && t.Token == token && t.TokenType == tokenType);

    if (userToken == null || userToken.IsUsed || userToken.ExpiresAt < DateTime.UtcNow)
        return false; // Token is invalid

    return true; // Token is valid
}
```

---

### **✅ Mark Token as Used**
For security, mark tokens as **used** after validation.

```csharp
public async Task<bool> UseTokenAsync(string userId, string token, string tokenType)
{
    var userToken = await _context.CustomUserTokens
        .FirstOrDefaultAsync(t => t.UserId == userId && t.Token == token && t.TokenType == tokenType);

    if (userToken == null || userToken.IsUsed || userToken.ExpiresAt < DateTime.UtcNow)
        return false;

    userToken.IsUsed = true;
    await _context.SaveChangesAsync();
    return true;
}
```

---

## **📩 4️⃣ Implementing Token Use Cases**
Now, let's integrate this system into **Password Reset** and **Email Confirmation**.

### **🔹 Password Reset Flow**
1️⃣ User requests a password reset.  
2️⃣ The system generates and stores a token.  
3️⃣ The token is sent via email.  
4️⃣ User clicks the link, and the system validates the token.  

#### **📌 Generate and Send Reset Token**
```csharp
public async Task<IActionResult> ForgotPassword(string email)
{
    var user = await _userManager.FindByEmailAsync(email);
    if (user == null) return BadRequest("User not found");

    var token = await _customTokenService.GenerateTokenAsync(user, "ResetPassword", 60); // 60 mins expiration

    var resetLink = Url.Action("ResetPassword", "Account", new { email, token }, Request.Scheme);
    await _emailSender.SendEmailAsync(email, "Reset Password", $"Click <a href='{resetLink}'>here</a> to reset your password.");

    return Ok("Password reset link sent to email.");
}
```

#### **📌 Validate and Reset Password**
```csharp
public async Task<IActionResult> ResetPassword(string email, string token, string newPassword)
{
    var user = await _userManager.FindByEmailAsync(email);
    if (user == null) return BadRequest("User not found");

    if (!await _customTokenService.ValidateTokenAsync(user.Id, token, "ResetPassword"))
        return BadRequest("Invalid or expired token");

    var resetResult = await _userManager.ResetPasswordAsync(user, token, newPassword);
    if (!resetResult.Succeeded) return BadRequest(resetResult.Errors);

    await _customTokenService.UseTokenAsync(user.Id, token, "ResetPassword");

    return Ok("Password reset successful.");
}
```

---

### **🔹 Email Confirmation Flow**
1️⃣ User registers.  
2️⃣ A token is generated and stored in `CustomUserTokens`.  
3️⃣ The system sends an email with a confirmation link.  
4️⃣ User clicks the link, and the system validates the token.  

#### **📌 Generate and Send Email Confirmation Token**
```csharp
public async Task<IActionResult> SendEmailConfirmation(string email)
{
    var user = await _userManager.FindByEmailAsync(email);
    if (user == null) return BadRequest("User not found");

    var token = await _customTokenService.GenerateTokenAsync(user, "EmailConfirmation", 1440); // 24-hour expiration

    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { email, token }, Request.Scheme);
    await _emailSender.SendEmailAsync(email, "Confirm Email", $"Click <a href='{confirmationLink}'>here</a> to confirm your email.");

    return Ok("Confirmation email sent.");
}
```

#### **📌 Validate and Confirm Email**
```csharp
public async Task<IActionResult> ConfirmEmail(string email, string token)
{
    var user = await _userManager.FindByEmailAsync(email);
    if (user == null) return BadRequest("User not found");

    if (!await _customTokenService.ValidateTokenAsync(user.Id, token, "EmailConfirmation"))
        return BadRequest("Invalid or expired token");

    user.EmailConfirmed = true;
    await _userManager.UpdateAsync(user);
    await _customTokenService.UseTokenAsync(user.Id, token, "EmailConfirmation");

    return Ok("Email confirmed successfully.");
}
```

---

## **🎯 Summary**
✔ We built a **custom token system** instead of using `AspNetUserTokens`.  
✔ We created a **CustomUserTokens table** with expiration and usage tracking.  
✔ We implemented **token generation, validation, and consumption**.  
✔ We integrated **password reset and email confirmation** flows.  
