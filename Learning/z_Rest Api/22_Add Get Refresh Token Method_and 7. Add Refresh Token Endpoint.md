## ✅ Goal of this Feature

We are implementing **JWT Refresh Token flow**.

When the JWT access token expires, the client sends:

* The **expired JWT token**
* The **refresh token**

And expects a **new JWT** + a **new Refresh Token** in return.

---

## ✨ Step-by-step Breakdown

---

### ✅ Step 1: Add Method Signature in `IAuthService`

```csharp
Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
```

This is the contract/interface that promises:

> I can give you a new access token and refresh token if you give me a valid pair (JWT + RefreshToken).

---

### ✅ Step 2: Implement `GetRefreshTokenAsync` in `AuthService`

```csharp
public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
```

Let’s break it down piece by piece:

---

#### 🔹 **1. Validate the JWT token (even if expired)**

```csharp
var userId = _jwtProvider.ValidateToken(token);
if (userId is null) return null;
```

* This checks if the JWT has a **valid structure and signature**, even if it’s expired.
* If the structure or signature is wrong → return `null`.

---

#### 🔹 **2. Find the user from the token**

```csharp
var user = await _userManager.FindByIdAsync(userId);
if (user is null) return null;
```

* We extract the `userId` from the token.
* Then search the DB to find that user.
* If not found → return `null`.

---

#### 🔹 **3. Check if the Refresh Token is valid and active**

```csharp
var userRefreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken && rt.IsActive);
if (userRefreshToken is null) return null;
```

* We're searching the user's stored refresh tokens.
* Must match by **token value** and also be **active**.
* If not found or revoked/expired → return `null`.

---

#### 🔹 **4. Generate new JWT + Refresh Token**

```csharp
var (newToken, expiresIn) = _jwtProvider.GenerateToken(user);
var newRefreshToken = GenerateRefreshToken();
var newRefreshTokenExpiration = DateTime.UtcNow.AddDays(_RefreshTokenExpiryDays);
```

* Fresh new access token
* Fresh new refresh token (base64 string)
* Expiration in the future (e.g. 30 days)

---

#### 🔹 **5. Revoke the old Refresh Token**

```csharp
userRefreshToken.RevokedOn = DateTime.UtcNow;
```

* Mark the current refresh token as **revoked**.
* So it can’t be reused.

---

#### 🔹 **6. Store the new Refresh Token in DB**

```csharp
user.RefreshTokens.Add(new RefreshToken
{
    Token = newRefreshToken,
    ExpiresOn = newRefreshTokenExpiration,
});
```

* Add the newly generated token to user’s refresh token list.
* So it will be available for future refresh requests.

---

#### 🔹 **7. Save changes**

```csharp
var result = await _userManager.UpdateAsync(user);
if (!result.Succeeded) return null;
```

* Persist the changes: revoke old token + save the new one.

---

#### 🔹 **8. Return the new AuthResponse**

```csharp
return new AuthResponse(...);
```

Contains:

* New Access Token
* New Refresh Token
* Expiration times
* Basic user info

---

### All Code

```csharp
public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
{
    // Validate the JWT token
    var userId = _jwtProvider.ValidateToken(token);

    if (userId is null)
        return null;

    // Find the user by ID
    var user = await _userManager.FindByIdAsync(userId);

    if (user is null)
        return null;

    // Check if the provided refresh token is valid
    var userRefreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken && rt.IsActive);

    if (userRefreshToken is null)
        return null;

    // Generate a new JWT token
    var (newToken, expiresIn) = _jwtProvider.GenerateToken(user);

    // Generate a new refresh token
    var newRefreshToken = GenerateRefreshToken();
    var newRefreshTokenExpiration = DateTime.UtcNow.AddDays(_RefreshTokenExpiryDays);

    // Mark the old refresh token as revoked (because the user can use it only once)
    userRefreshToken.RevokedOn = DateTime.UtcNow;

    user.RefreshTokens.Add(new RefreshToken
    {
        Token = newRefreshToken,
        ExpiresOn = newRefreshTokenExpiration,
    });

    // Save changes to the user entity
    var result = await _userManager.UpdateAsync(user);
    if (!result.Succeeded)
    {
        // Handle the error, e.g., log it or throw an exception
        return null;
    }

    // Return the new token response
    return new AuthResponse(
        user.Id,
        user.Email,
        user.FirstName,
        user.LastName,
        newToken,
        expiresIn,
        newRefreshToken,
        newRefreshTokenExpiration
    );
}
```

---

### ✅ Step 3: Create the DTO `RefreshTokenRequest`

```csharp
public record RefreshTokenRequest(
    string Token,
    string RefreshToken
);
```

* This is the shape of the request body the client sends to the refresh endpoint.

---

### ✅ Step 4: Add the `RefreshAsync` Endpoint in `AuthController`

```csharp
[HttpPost("refresh")]
public async Task<IActionResult> RefreshAsync(
    [FromBody] RefreshTokenRequest request,
    CancellationToken cancellationToken = default)
{
    var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
    
    return authResult switch
    {
        null => BadRequest("Invalid token or refresh token."),
        _ => Ok(authResult)
    };
}
```

* It receives a **POST** request with:

  * `Token` → the old JWT
  * `RefreshToken` → the active refresh token

* Delegates the logic to `AuthService`

* If anything fails → returns `400 Bad Request`

* If everything is valid → returns `200 OK` + new tokens

---

### ✅ Summary of the Flow

| Step | Description                             |
| ---- | --------------------------------------- |
| ✅ 1  | User sends expired JWT + refresh token  |
| ✅ 2  | Token is validated, user is found       |
| ✅ 3  | Refresh token is validated              |
| ✅ 4  | Old refresh token is revoked            |
| ✅ 5  | New JWT + refresh token are generated   |
| ✅ 6  | Changes are saved, response is returned |
