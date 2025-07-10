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

###  إيه الهدف من الكلاس ده؟
```csharp
public class JwtProvider : IJwtProvider
```

الكلاس ده مسؤول عن **توليد (Generate) الـ JWT Token**.
هو اللي بياخد بيانات اليوزر (المستخدم) و**يبني منها JSON Web Token** يحتوي على معلومات المستخدم، ويوقّع التوكن عشان يبقى موثوق.

### ليه بنستخدم Interface `IJwtProvider`؟

عشان نقدر نعمل Dependency Injection ونفصل ما بين منطق توليد التوكن (implementation) وبين استخدامه في الخدمات.
فبالتالي، `IJwtProvider` هو **Contract (اتفاق)**، و`JwtProvider` هو **اللي بيحقق الاتفاق ده**.

---

## Method: `GenerateToken`

```csharp
public (string token, int expiresIn) GenerateToken(ApplicationUser user)
```

* بيستقبل باراماتر الي هو `ApplicationUser user`: ده هو المستخدم اللي اتعمله login.
* بيرجع Tuple:
  * ال `token`: النص المشفّر الخاص بالـ JWT.
  * ال `expiresIn`: عدد الثواني اللي التوكن هيبقى صالح خلالها.

---

## Claims

```csharp
Claim[] claims =
[
    new(JwtRegisteredClaimNames.Sub, user.Id),
    new(JwtRegisteredClaimNames.Email, user.Email!),
    new(JwtRegisteredClaimNames.GivenName, user.FirstName),
    new(JwtRegisteredClaimNames.FamilyName, user.LastName),
    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
];
```

### يعني إيه Claim؟

الـ **Claim** هو معلومة بتحطها جوا الـ Token بتوضح مين المستخدم وإيه بياناته.
الـ JWT هو عبارة عن Header + Payload + Signature.
الـ Payload ده بقي هو المكان اللي بنحط فيه الـ Claims دي.


| Claim Type      | معناها             | محتواها                     |
| --------------- | ------------------ | --------------------------- |
| `Sub` (Subject) | رقم تعريف المستخدم | `user.Id`                   |
| `Email`         | الإيميل            | `user.Email!`               |
| `GivenName`     | الاسم الأول        | `user.FirstName`            |
| `FamilyName`    | الاسم الأخير       | `user.LastName`             |
| `Jti` (JWT ID)  | رقم مميز للتوكن    | `Guid.NewGuid().ToString()` |

> مهم جدًا تحط `Jti` عشان تبقى كل توكن ليه بصمة مميزة. ده ممكن تستخدمه في blacklisting أو منع التكرار.

---

## إنشاء مفتاح التوقيع

```csharp
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("J7MfAb4WcAIMkkigVtIepIILOVJEjAcB"));
```

* ده هو المفتاح السري اللي **بنستخدمه عشان نوقع التوكن**.
* ال `SymmetricSecurityKey`: يعني نفس المفتاح اللي عملنا بيه التوقيع، هو اللي هيتم التحقق بيه وقت الاستقبال (Symmetric Key Encryption).
* بنستخدم `Encoding.UTF8.GetBytes(...)` عشان نحول النص لمصفوفة بايتس.

 ملاحظة:

> المفتاح ده لازم يكون طويل ومعقد، ومتخزن في مكان آمن زي `appsettings.json` أو Secret Manager أو Azure Key Vault.

---

## Signing Credentials

```csharp
var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
```

* هنا بنقول "أنا هوقّع التوكن باستخدام المفتاح `key` وبالخوارزمية `HMAC SHA256`".
* ده بيمنع أي حد من تعديل محتوى التوكن، لأنه لازم يكون التوقيع صحيح.

---

## تعالي نشوف الكلاسات دي بتاعت ايه `SymmetricSecurityKey` و `SigningCredentials`

## أولاً: `SymmetricSecurityKey`
```csharp
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("J7MfAb4WcAIMkkigVtIepIILOVJEjAcB"));
```

###  إيه هو الكلاس ده؟

ال `SymmetricSecurityKey` هو كلاس موجود في مكتبة:

```
Microsoft.IdentityModel.Tokens
```

وده النوع اللي بنستخدمه لما نشتغل بـ **تشفير متماثل (Symmetric Encryption)**، يعني:

> **نفس المفتاح اللي بيعمل ال `Signature` بتاعت التوكن ← هو نفسه اللي بيتم استخدامه للتحقق من صحة التوكن.**

---

### ليه استخدمناه؟

لأننا لما بنبني JWT Token، لازم يكون عليه توقيع رقمي (Digital Signature)، علشان اللي يستقبله يقدر يتأكد إنه:

* **مش اتعدّل.**
* **صادر من جهة موثوقة (إحنا).**

وعلشان نوقّع التوكن ده، بنحتاج مفتاح سري ← وده هو اللي بيمثله `SymmetricSecurityKey`.

---

### مكونات السطر:

```csharp
Encoding.UTF8.GetBytes("J7MfAb4WcAIMkkigVtIepIILOVJEjAcB")
```

ده بيحوّل الـ string اللي هو المفتاح `"J7MfAb4..."` إلى `byte[]`، لأن الكلاس `SymmetricSecurityKey` بيشتغل على الـ bytes، مش string.

---

### ملاحظات مهمة:

1. المفتاح لازم يكون **طويل ومعقّد** (على الأقل 32 character).
2. ماينفعش تسيبه كده في الكود — الأفضل تحطه في:

   * `appsettings.json`
   * Secret Manager
   * Azure Key Vault (in production)

---

## ثانياً: `SigningCredentials`

```csharp
var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
```

### إيه هو `SigningCredentials`؟

ده كائن بيحتوي على **معلومات التوقيع (Signing Info)** اللي هنستخدمها في التوكن.

يعني هو اللي بيقول:

* **هستخدم أي مفتاح؟**
* **وهستخدم أي خوارزمية توقيع؟**

---

### البارامترز اللي بياخدها:

#### `key`:

المفتاح اللي هنوقع بيه التوكن — وده اللي عملناه بالسطر اللي قبله باستخدام `SymmetricSecurityKey`.

#### `SecurityAlgorithms.HmacSha256`:

دي اسم الخوارزمية اللي هنستخدمها للتوقيع.

> ال `HmacSha256` بتتقسم لحاجتين:
>
> *ا **HMAC**: خوارزمية توقيع تعتمد على مفتاح سري.
> * ا**SHA256**: خوارزمية الـ hashing المستخدمة.

ودي واحدة من أكتر الخوارزميات استخدامًا مع JWT بسبب أمانها وسرعتها.

---

## خلاصة الاثنين مع بعض:

| عنصر                   | المعنى                                     | الاستخدام                           |
| ---------------------- | ------------------------------------------ | ----------------------------------- |
| `SymmetricSecurityKey` | المفتاح السري المستخدم في التوقيع          | بيتم توليده من string (طويل ومعقّد) |
| `SigningCredentials`   | بيانات التوقيع: مين المفتاح؟ وأي خوارزمية؟ | بيتم تمريرها للـ JWT أثناء الإنشاء  |

---

## مثال عملي تاني:


```csharp
var keyAsString = "MySuperSecretKeyThatShouldBeInConfig";
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyAsString));

var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

// credentials.SigningKey => the key
// credentials.Algorithm => "HS256"
```

---

## زمن انتهاء التوكن

```csharp
var expiresIn = 30; // in minutes
```

* التوكن هينتهي بعد 30 دقيقة.

---

## إنشاء التوكن نفسه

```csharp
var token = new JwtSecurityToken(
    issuer: "SurveyBasketApp",
    audience: "SurveyBasketApp users",
    claims: claims,
    expires: DateTime.UtcNow.AddMinutes(expiresIn),
    signingCredentials: credentials
);
```

### كل بارامتر بيعمل إيه؟

| اسم البارامتر        | وظيفته                                                  |
| -------------------- | ------------------------------------------------------- |
| `issuer`             | الجهة اللي أصدرت التوكن. (مثلاً اسم الأبلكيشن بتاعك)    |
| `audience`           | مين المفروض يستخدم التوكن ده؟ (مثلاً الويب أو الموبايل) |
| `claims`             | البيانات الخاصة بالمستخدم اللي هتكون جوا التوكن         |
| `expires`            | وقت انتهاء صلاحية التوكن                                |
| `signingCredentials` | التوقيع الرقمي لضمان سلامة التوكن                       |

---

## تحويل التوكن لنص

```csharp
return (new JwtSecurityTokenHandler().WriteToken(token), expiresIn * 60);
```

* ال `WriteToken(token)` بيرجع string يمثل الـ JWT.
* ال `expiresIn * 60`: بنحول من دقائق لثواني.

---

* ال `JwtProvider` هو المصنع (factory) اللي بيبني التوكن.
* بيأخد معلومات المستخدم ← يركب جواها claims ← يوقع التوكن ← يرجعه كنص + زمن انتهاء.
* بنعتمد عليه في أي خدمة عاوزة تعمل توليد للتوكن.


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

## أولاً: تسجيل `JwtProvider` كسيرفيس

```csharp
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
```

### طب السطر ده معناه إيه؟

* بنقول للـ **Dependency Injection container** إنه لما حد يطلب `IJwtProvider`، إديله نسخة من `JwtProvider`.
* ال `AddSingleton` معناها: نسخة واحدة بس (Singleton) هتتعمل وتُعاد استخدامها طول عمر التطبيق.

### ليه نستخدم Singleton هنا؟

* لأن `JwtProvider`:
  * هو **Stateless** (مش بيحتفظ بحالة).
  * مفيهوش `DbContext` أو حاجة بتحتاج إدارة عمر.
  * ممكن استخدامه آمن من كذا مكان بدون تعارض.

---

##  ثانياً: تفعيل Microsoft Identity

```csharp
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
```

### بنعمل إيه هنا؟

* بنقول للـ ASP.NET Core نستخدم **نظام الهوية (Identity)** لإدارة المستخدمين (Users) والأدوار (Roles).
* ال `ApplicationUser`: هو الكلاس اللي انت عامله، وبيمثّل المستخدم وبيحتوي على خصائص إضافية زي `FirstName`.
* ال `IdentityRole`: هو ال default class الي عملاه مايكروسوفت وبيمثّل ال Roles (زي Admin, User) وتقدر تعمل واحد غيره وتورث منه زي ما عملنا مع ال `IdentityUser` وخلينا `ApplicationUser` يورث منه وضيفنا علي الخصائص بتاعته .

* ال `AddEntityFrameworkStores<ApplicationDbContext>()`: يعني خزن البيانات دي في قاعدة البيانات باستخدام `ApplicationDbContext`.

---

## ثالثاً: إعداد Authentication Scheme

```csharp
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
```

### بنعمل إيه هنا؟

* بنقول للتطبيق:

  *  الطريقة اللي هيتحقق بيها من الهوية (Authentication) هي **JWT**.
  *  الطريقة اللي هيتحدى بيها المستخدم غير المصرّح له هي **JWT برضو**.

> ببساطة: لما حد يحاول يدخل على Endpoint محمية، السيستم هيشوف إذا كان معاه JWT، ويتحقق منه بناءً على الإعدادات اللي هنعملها تحت.

---

### من الاخر الجزء ده 
```csharp
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
```
* ببساطه علشان لما نيجي نستخدم الاتربيوت الخاص ب ال  `Authorization` مع ال `controller` او ال `Action` منضطرش في كل مرة نقوله ان احنا بنستخدم ال `Bearer` او ان ال tokens بتاعتنا نوعها `Bearer` ف انا هنا خلاص عرفته اني ال default بتاعي هو `Bearer` 
---

##  رابعاً: إعداد JWT Bearer Options

```csharp
.AddJwtBearer(options =>
{
    options.SaveToken = true;
```

* ال `SaveToken = true`: بيخزن التوكن داخل الـ HttpContext لو احتجته لاحقًا.

  * مش ضروري قوي في Web APIs، بس مفيش ضرر من تفعيله.

---

## إعداد `TokenValidationParameters`

```csharp
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
```

| الخيار                     | معناه                                                                  |
| -------------------------- | ---------------------------------------------------------------------- |
| `ValidateIssuer`           | اتحقق إن الـ JWT جه من مصدر موثوق (مثلاً تطبيقك نفسه).                 |
| `ValidateAudience`         | اتحقق إن الـ JWT موجه فعلاً للـ Audience الصح (مثلاً الويب أو موبايل). |
| `ValidateLifetime`         | اتحقق إن التوكن منتهيش.                                                |
| `ValidateIssuerSigningKey` | اتحقق إن التوقيع على التوكن تم بالمفتاح الصح.                          |

---

## إعداد القيم الفعلية للتحقق

```csharp
ValidIssuer = "SurveyBasketApp",
ValidAudience = "SurveyBasketApp users",
IssuerSigningKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes("J7MfAb4WcAIMkkigVtIepIILOVJEjAcB")
)
```

| العنصر             | وظيفته                                                                                                                |
| ------------------ | --------------------------------------------------------------------------------------------------------------------- |
| `ValidIssuer`      | هو اسم الـ "مصدر" اللي أصدر التوكن. لازم يطابق القيمة اللي اتحطت وقت إنشاء التوكن في `JwtSecurityToken`.              |
| `ValidAudience`    | هو الجمهور المستهدف. نفس الفكرة. لازم يطابق القيمة اللي في التوكن.                                                    |
| `IssuerSigningKey` | هو نفس المفتاح اللي استخدمته في التوقيع وقت إنشاء التوكن (`JwtProvider`). ولازم يكون **نفسه بالظبط** عشان يتم التحقق. |

 لو أي من القيم دي مش متطابقة، التوكن هيتعتبر **غير صالح**.

---

| الخطوة                           | معناها                                                |
| -------------------------------- | ----------------------------------------------------- |
| 1️⃣ `AddSingleton<IJwtProvider>` | تسجّل مصنع التوكنات                                   |
| 2️⃣ `AddIdentity<>()`            | إعداد الهوية وإدارة المستخدمين                        |
| 3️⃣ `AddAuthentication()`        | نحدد إننا هنستخدم JWT كطريقة تحقق                     |
| 4️⃣ `AddJwtBearer()`             | نحدد إعدادات تحقق JWT: من أصدره؟ لمن؟ ومفتاح التوقيع؟ |

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
