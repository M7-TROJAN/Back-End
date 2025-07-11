## Options Pattern

The Options Pattern uses classes to provide strongly typed access to groups of related settings.  
When configuration settings are isolated by scenario into separate classes, the app adheres to two important software engineering principles:

## Principles

1. **Encapsulation**  
   Classes that depend on configuration settings depend only on the configuration settings that they use.

2. **Separation of Concerns**  
   Settings for different parts of the app aren't dependent or coupled to one another.

---

## Options Class

- Must be **non-abstract**.
- Has public **read-write properties** of types that have corresponding items in configuration files.

---

## Options Interfaces

| Interface                  | Reading Behavior                      | Registration Lifetime | Can Be Injected Into       |
|---------------------------|----------------------------------------|------------------------|-----------------------------|
| `IOptions<TOptions>`      | Once, at **application start**         | Singleton              | Any service lifetime        |
| `IOptionsSnapshot<TOptions>` | On **each request**                    | Scoped                 | **Not** into Singleton      |
| `IOptionsMonitor<TOptions>`  | On **each config file update**        | Singleton              | Any service lifetime        |

---

## أولًا: ما هو Options Pattern؟

ال **Options Pattern** هو أسلوب في ASP.NET Core بيسمحلك بإنك توصل لإعدادات (configuration settings) بطريقة **Strongly-Typed** باستخدام كائنات (classes)، بدل ما تعتمد على الـ `IConfiguration` وaccess بالسلاسل (strings).

---

## المميزات الرئيسية:

| الميزة                   | الوصف                                                           |
| ------------------------ | --------------------------------------------------------------- |
| ✅ Strongly-Typed         | توصل للإعدادات من خلال خصائص `Properties` واضحة مش بس Strings.  |
| ✅ Encapsulation          | كل كلاس بيشيل الإعدادات اللي هو محتاجها بس (مبدأ OOP حقيقي).    |
| ✅ Separation of Concerns | كل جزء في البرنامج بياخد الإعدادات الخاصة بيه فقط.              |
| ✅ Validations            | تقدر تستخدم Data Annotations زي `[Required]`, `[Range]` للتحقق. |
| ✅ Supports DI            | تقدر تحقنه في الخدمات بكل سهولة باستخدام `IOptions<T>` وأنواعه. |

---

## هيكل الإعدادات (مثال حقيقي)

### `appsettings.json`

```json
"JwtOptions": {
  "Key": "J7MfAb4WcAIMkkigVtIepIILOVJEjAcB",
  "Issuer": "SurveyBasketApp",
  "Audience": "SurveyBasketApp users",
  "ExpiryMinutes": 30
}
```

---

## كلاس الإعدادات `JwtOptions`

```csharp
public class JwtOptions
{
    public static string SectionName = "JwtOptions";

    [Required]
    public string Key { get; init; } = string.Empty;

    [Required]
    public string Issuer { get; init; } = string.Empty;

    [Required]
    public string Audience { get; init; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int ExpiryMinutes { get; init; }
}
```

### ليه استخدمنا `init` بدل `set`؟

* ال `init` معناها إن الخاصية **تقدر تتحدد وقت إنشاء الكائن بس** (Immutable بعد الإنشاء).
* ده بيخلي الكلاس **Read-only بعد البايندنج** وبيعزز الـ immutability.
* مثال:

```csharp
var opts = new JwtOptions { Key = "abc" }; // 
opts.Key = "xyz"; // Error
```

---

## ال Binding في `Program.cs`

```csharp
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.SectionName)
);
```

> بنستخدم `Configure<T>()` علشان نربط الإعدادات بكلاس strongly-typed.

---

##  استخدام الإعدادات

### 1. في الـ JWT Authentication

```csharp
var jwtSettings = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

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
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});
```

> هنا استخدمنا `Get<JwtOptions>()` علشان نقرأ القيم مباشرة من `IConfiguration`.

---

## استخدام القيم داخل class أو services

### باستخدام `IOptions<JwtOptions>`

```csharp
public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string GetIssuer() => _options.Issuer;
}
```

---

##  أنواع `IOptions` المتاحة

| النوع                 | Lifetime  | مناسب لإيه؟                                              | إمتى يتحدث؟                | Inject في؟          |
| --------------------- | --------- | -------------------------------------------------------- | -------------------------- | ------------------- |
| `IOptions<T>`         | Singleton | إعدادات ثابتة أثناء تشغيل البرنامج                       | لمرة واحدة عند بدء التشغيل | أي خدمة             |
| `IOptionsSnapshot<T>` | Scoped    | إعدادات بتتغير كل Request (مثلاً Web API)                | عند كل Request             | Scoped services فقط |
| `IOptionsMonitor<T>`  | Singleton | لو عاوز تتتبع التغيير في الإعدادات لحظيًا (File Watcher) | فور التحديث                | أي خدمة             |

---

## مثال يوضح الفرق

### Inject `IOptionsSnapshot<JwtOptions>` في Controller:

```csharp
public class AuthController : ControllerBase
{
    private readonly JwtOptions _jwtOptions;

    public AuthController(IOptionsSnapshot<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }
}
```

> هيتقرأ كل مرة بـ request جديد (مفيد لو بتغير الإعدادات أثناء التطوير).

---

## نصائح مهمة

| نصيحة                                             | ليه؟                                    |
| ------------------------------------------------- | --------------------------------------- |
| استخدم `init` بدلاً من `set`                      | يجعل الإعدادات Immutable بعد التحميل.   |
| اربط السكشن باسم الكلاس (`SectionName`)           | يخلي الكود نظيف وقابل لإعادة الاستخدام. |
| استخدم Validation Attributes                      | عشان تتحقق من صحة القيم وقت التشغيل.    |
| Inject `IOptions<T>` بدلاً من IConfiguration      | أقوى وأسهل وأوضح.                       |
| متستخدمش `IOptionsSnapshot` في Singleton services | هتديك Error.                            |

---

## الخلاصة

| العنصر                | الهدف                                                 |
| --------------------- | ----------------------------------------------------- |
| `Options Pattern`     | ربط الإعدادات بكلاسات strongly-typed.                 |
| `Configure<T>()`      | ربط الإعدادات بالـ Configuration من appsettings.json. |
| `IOptions<T>`         | قراءة الإعدادات مرة واحدة عند بدء التشغيل.            |
| `IOptionsSnapshot<T>` | قراءة متجددة مع كل HTTP request.                      |
| `IOptionsMonitor<T>`  | مراقبة حية لأي تغيير في الإعدادات.                    |
| `init` بدل `set`      | جعل الإعدادات غير قابلة للتعديل بعد الإنشاء.          |

---
