
## أولًا: هو ايه اصلا ال `appsettings.json`؟

ال `appsettings.json` هو ملف **تكوين (Configuration File)** في ASP.NET Core.

بيستخدم لتخزين أي بيانات أو إعدادات ممكن تتغير من بيئة لبيئة أو من مشروع لمشروع — زي:

* Connection strings
* API keys
* JWT secrets
* Timeout settings
* Feature toggles
* Logging settings
* Email configurations

---

### شكله بيكون عامل كده:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "JwtSettings": {
    "Key": "J7MfAb4WcAIMkkigVtIepIILOVJEjAcB",
    "Issuer": "SurveyBasketApp",
    "Audience": "SurveyBasketApp users",
    "ExpiresInMinutes": 30
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=SurveyBasket;Trusted_Connection=True;"
  }
}
```

---

## طب ليه نستخدم `appsettings.json` بدل ما نحط القيم في الكود؟

1. **سهولة التعديل بدون الحاجة لإعادة الكومبايل.**
2. **إدارة القيم حسب البيئة (dev/test/prod).**
3. **فصل منطق البرنامج عن الإعدادات.**
4. **أمان ومرونة، خصوصًا لما بنستخدم Secrets أو Azure App Configuration.**

---

الملف ده بنلاقيه في **الروت بتاع المشروع** (root of the project) وبيكون موجود تلقائيًا لما تعمل مشروع جديد ASP.NET Core.

---

## طب فيه كام نوع من ملفات الإعدادات؟

| الملف                          | الغرض                                               |
| ------------------------------ | --------------------------------------------------- |
| `appsettings.json`             | الإعدادات العامة الافتراضية                         |
| `appsettings.Development.json` | إعدادات مخصصة لوضع التطوير (dev environment)        |
| `appsettings.Production.json`  | إعدادات مخصصة لوضع التشغيل (production environment) |

> النظام بيقرأ أولًا من `appsettings.json` وبعدين يطبّق أي **Override** موجود في الملف الخاص بالبيئة.

---

## ترتيب قراءة الـ Configuration

ال ASP.NET Core بتقرأ الإعدادات من مصادر مختلفة بترتيب (حسب الأولوية):

1. ال `appsettings.json`
2. ال `appsettings.{Environment}.json`
3. ال Secrets Manager (لو شغال عليه)
4. ال Environment Variables (متغيرات النظام)
5. ال Command Line args
6. أي مصدر مخصص تضيفه

---

## إزاي نقرأ الإعدادات من `appsettings.json`؟

### عندنا طريقتين رئيسيتين:

---

## الطريقة الأولى: بالـ `Configuration` مباشرة

```csharp
var jwtKey = builder.Configuration["JwtSettings:Key"];
```

> دي طريقة مباشرة وسريعة، بس مش strongly typed.

---

##  الطريقة الثانية (المفضّلة): عن طريق ربط الإعدادات بكلاس `Options Pattern`

ودي اللي هنستخدمها لو هنتعامل مع إعدادات كبيرة ومتعددة زي إعدادات الـ JWT

### مثال على `JwtSettings` class:

```csharp
public class JwtSettings
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiresInMinutes { get; set; }
}
```

---

## التسجيل في `Program.cs`

```csharp
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));
```

كده إحنا قلنا: اربط `class JwtSettings` بالقسم `"JwtSettings"` الموجود في `appsettings.json`.

---

## واستخدامها في السيرفيس (مثال):

```csharp
public class JwtProvider(IOptions<JwtSettings> jwtOptions)
{
    private readonly JwtSettings _settings = jwtOptions.Value;

    public string GenerateToken(ApplicationUser user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_settings.Key));

        // the entire code
    }
}
```

> استخدام `IOptions<T>` بيخلّيك تقدر تجيب الإعدادات بشكل **Strongly Typed** ومضمون.

---

##  كده إحنا عرفنا:

| جزئية              | شرحها                                 |
| ------------------ | ------------------------------------- |
| `appsettings.json` | ملف الإعدادات الأساسي في ASP.NET Core |
| ليه نستخدمه؟       | فصل الإعدادات عن الكود وتسهيل الإدارة |
| شكله؟              | ملف JSON قابل للتقسيم لأقسام متعددة   |
| استخدامه إزاي؟     | Direct أو عبر `Options pattern`       |

---
