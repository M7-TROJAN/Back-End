## JWT Authentication Setup in ASP.NET Core Web API

### Goal

Implement a secure login system using **JWT (JSON Web Token)** where:

* User sends `email` and `password`.
* If credentials are valid:

  * The system generates a JWT token.
  * Returns it with user info.
* The token is used for authorization in protected endpoints.

---

### Prerequisites

Install NuGet package:

```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

Or use `.csproj`:

```xml
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="9.0.6" />
```

---

## Define `LoginRequest` and `AuthResponse`

```csharp
public record LoginRequest(
    string Email,
    string Password
);
```

> Represents the incoming login request from the client.

```csharp
public record AuthResponse(
    string Id,
    string? Email,
    string FirstName,
    string LastName,
    string Token,
    int ExpiresIn
);
```

> Represents the response sent back after successful login.

---

## Create the `IJwtProvider` Interface

```csharp
public interface IJwtProvider
{
    (string token, int expiresIn) GenerateToken(ApplicationUser user);
}
```

> The contract for generating a JWT based on a user object.

---

## Implement `JwtProvider`

```csharp
public class JwtProvider : IJwtProvider
{
    public (string token, int expiresIn) GenerateToken(ApplicationUser user)
    {
        // JWT Claims
        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("J7MfAb4WcAIMkkigVtIepIILOVJEjAcB"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiresIn = 30; // in minutes

        var token = new JwtSecurityToken(
            issuer: "SurveyBasketApp",
            audience: "SurveyBasketApp users",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresIn),
            signingCredentials: credentials
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresIn * 60);
    }
}
```

---

## Register Identity and JWT in `Program.cs`

```csharp
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "SurveyBasketApp",
        ValidAudience = "SurveyBasketApp users",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("J7MfAb4WcAIMkkigVtIepIILOVJEjAcB"))
    };
});
```

---

## Create `IAuthService`

```csharp
public interface IAuthService
{
    Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
}
```

> Interface for the service responsible for validating credentials and generating tokens.

---

## Implement `AuthService`

```csharp
public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) return null;

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!isPasswordValid) return null;

        var (token, expiresIn) = _jwtProvider.GenerateToken(user);

        return new AuthResponse(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            token,
            expiresIn
        );
    }
}
```

---

## Register AuthService in `Program.cs`

```csharp
builder.Services.AddScoped<IAuthService, AuthService>();
```

---

## Create `AuthController`

```csharp
[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("")]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var authResult = await _authService.GetTokenAsync(
            request.Email,
            request.Password,
            cancellationToken);

        return authResult switch
        {
            null => BadRequest("Invalid email or password."),
            _ => Ok(authResult)
        };
    }
}
```

---

## Testing

* Open Postman.
* Make a POST request to: `http://localhost:xxxx/auth`
* Send JSON body:

```json
{
  "email": "user@example.com",
  "password": "123456"
}
```

### Response (Success)

```json
{
  "id": "user-id",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "token": "eyJhbGciOiJIUzI1NiIsInR...",
  "expiresIn": 1800
}
```

---

## ✅ Summary

| Step              | Purpose                        |
| ----------------- | ------------------------------ |
| 1. DTOs           | Define request/response models |
| 2. JwtProvider    | Create token logic             |
| 3. AuthService    | Validate user, return token    |
| 4. AuthController | Expose endpoint to user        |
| 5. Program.cs     | Configure DI + Auth schemes    |

---
