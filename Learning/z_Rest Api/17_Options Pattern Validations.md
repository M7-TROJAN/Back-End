
> ال **Options Pattern Validations**
> هو ضمان إن الإعدادات اللي جاية من `appsettings.json` (أو أي مصدر) تكون سليمة قبل ما الابليكيشن يشتغل.

---

## ليه نحتاج نعمل Validation للإعدادات (Options Validation)؟

تخيل معايا إن عندك إعدادات مهمة جدًا في `appsettings.json`، زي إعدادات التوكن (JWT)، او اعدادات قاعدة البيانات، او اعدادات Cloud services، إلخ.

لو نسيت تحط واحدة من القيم، أو كتبت قيمة غلط (زي رقم سالب أو نص فاضي)، إيه اللي ممكن يحصل؟
التطبيق هيشتغل، لكن:

* هيطلع **سلوك غير متوقع**
* أو الأسوأ: يوصل **للProduction** وساعتها تبقى المصيبة أكبر

---

## تخيل عندك سكشن في ال appsettings.json بالشكل ده:

### `appsettings.json`

```json
"Jwt": {
  "Key": "super-secret",
  "Issuer": "SurveyBasketApp",
  "Audience": "SurveyBasketApp users"
  // ExpiryMinutes مش موجودة
}
```

مافيش `ExpiryMinutes` هنا

---

## المشكلة

لو كتبت كلاس الـ `JwtOptions` كده:

```csharp
public class JwtOptions
{
    public static string SectionName = "Jwt";

    public string Key { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int ExpiryMinutes { get; init; }
}
```
انت هنا متوقع ان هيجيلك `Key` و `Issuer` و `ExpiryMinutes` ,  `Audience`

### هيحصل إيه؟ 
الـ `ExpiryMinutes` مش موجودة اصلا في السكشن الي جوة ال `appsettings.json`
بس لأن الـ `int` في C# قيمة الـ default بتاعته هي `0`، التطبيق هيشتغل…
لكن الـ token هيخلص فورًا! 😶

 **النتيجة:** التوكن شغال شكله تمام، لكن بيخلص قبل ما يوصل لأي EndPoint.
 **سلوك غير متوقع وصعب تكتشفه.**
1. التطبيق هيشتغل عادي.
2. هتعمل login، هيتولد التوكن، بس `expiresIn` = 0 (غلط جدًا).
3. وهنا انت مش واخد بالك إن فيه مشكلة في الإعدادات!

---

### سيناريو تاني: قيمة غير منطقية

```json
"Jwt": {
  "Key": "super-secret",
  "Issuer": "SurveyBasketApp",
  "Audience": "SurveyBasketApp users",
  "ExpiryMinutes": -30
}
```

هنا كتبنا `ExpiryMinutes = -30`.
هل التطبيق هيوقف؟ لا.
هل فيه Error؟ لا.

بس الـ expiresIn في ال response الي راجع لليوزر هيطلع بالسالب:

```json
"expiresIn": -1800
```

وده خطأ منطقي كبير جدًا.
 **يعني اليوزر بقى معاه توكن بانتهى من المستقبل**!

---

---


## الحل: Data Annotations + Options Validation

ال ASP.NET Core بيديك أدوات قوية تتأكد إن الإعدادات اللي جايالك من `appsettings.json` **صحيحة ومنطقية**، مش بس موجودة.

### نعدّل `JwtOptions` كده:

```csharp
public class JwtOptions
{
    public static string SectionName = "Jwt";

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

>  كده بنقول:
>
> * لازم كل قيمة تبقى موجودة (`[Required]`)
> * ال  `ExpiryMinutes` لازم تبقى رقم موجب (`[Range(1, int.MaxValue)]`)


### ليه بنستخدم `[Required]` و `[Range(...)]`؟

* ال `Required`: تتأكد إن القيمة مش null أو فاضية (بالنسبة لـ strings).
* ال `Range`: تضمن إن القيمة في مدى منطقي (زي إن الـ ExpiryMinutes مش بالسالب أو بصفر).
---

## فين نعمل الربط + التحقق؟ ← `Program.cs`
## الخطوة الثانية: ربط الكلاس ده بالـ Configuration + تفعيل الفاليديشن

###  الطريقة القديمة:

```csharp
var jwtOptions = configuration.GetSection(JwtOptions.SectionName);
services.Configure<JwtOptions>(jwtOptions);
```

### الطريقة الصح مع الـ Validation:

```csharp
services.AddOptions<JwtOptions>()
    .BindConfiguration(JwtOptions.SectionName)
    .ValidateDataAnnotations();  // ← دي اللي بتفعل التحقق من الـ Attributes
```

---

##  شرح مفصل للسطر ده:

```csharp
services.AddOptions<JwtOptions>()                    // 1. بنسجل JwtOptions كـ Options
    .BindConfiguration(JwtOptions.SectionName)       // 2. بنربطها بسكشن "Jwt" من config
    .ValidateDataAnnotations();                      // 3. بنفعّل الـ validation على أساس الـ attributes
```

> وده معناه إن **كل Request** هيقرا الـ Options ويتأكد من القيم، ولو فيها مشاكل مش هيبعت Error في البداية، لكن ممكن تحصل أثناء التشغيل.

---

##  طيب إيه المشكلة هنا؟

لو شغّلت التطبيق وفيه Error في الـ config:

* التطبيق هيشتغل
* لكن أول مرة تستخدم الـ `JwtOptions` (مثلاً جوه `JwtProvider`)
  → هيترمي Exception: `OptionsValidationException`

ودي مصيبة لو ظهرت وانت شغال على Production

---

## الحل النهائي: منع التشغيل أساسًا لو الإعدادات غلط

```csharp
services.AddOptions<JwtOptions>()
    .BindConfiguration(JwtOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart(); // ← هنا بنقول: متشغلش التطبيق لو فيه غلط
```

### إيه اللي بيحصل مع `ValidateOnStart()`؟

* وقت تشغيل التطبيق (startup) بيتحقق من الإعدادات

* لو فيها Missing أو Invalid values → يرمي `OptionsValidationException` مباشرة

* الـ Visual Studio يطلعلك Error:

  >  *"unable to connect to web server https, the web server is no longer running"*

* ولما تروع علي التيرمنال هتلاقيه طبع:

  ```bash
  System.OptionsValidationException: DataAnnotation validation failed for 'JwtOptions'
  ```

---

## طيب نلخص كده الفرق بين التلت مراحل؟

| طريقة التهيئة                | الفعل                                                    |
| ---------------------------- | -------------------------------------------------------- |
| `Configure<JwtOptions>()`    | ربط الإعدادات بدون تحقق                                  |
| `.ValidateDataAnnotations()` | تحقق وقت الاستخدام بس، مش وقت التشغيل                    |
| `.ValidateOnStart()`         | تحقق فوري وقت الـ Startup — ميخلّيش التطبيق يشتغل لو غلط |

---

## سؤال: إيه الفرق بين دي ودي؟

```csharp
// الطريقة القديمة
services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

// الطريقة الأفضل
services.AddOptions<JwtOptions>()
        .BindConfiguration("Jwt")
        .ValidateDataAnnotations();
```

### الفرق:

* الأولى بتربط القيم بس، لكن لا بتتحقق منها ولا بتدعم Runtime Monitoring.
* التانية بتديك قوة أكبر، Validation، وممكن تضيف `.ValidateOnStart()` كمان.

---

## ممكن كمان تضيف Validation مخصصة:

لو عاوز تعمل Validation مش ممكن تحققها بـ DataAnnotation، زي:

```csharp
services.AddOptions<JwtOptions>()
    .BindConfiguration("Jwt")
    .Validate(options => options.Key.StartsWith("J7M"), "Key must start with J7M")
    .ValidateDataAnnotations()
    .ValidateOnStart();
```

---

## النتيجة النهائية:

* الإعدادات تتربط من `appsettings.json`
* يحصل **Validation حقيقي**
* مايشتغلش التطبيق لو فيه config ناقص
* تضمن إن التوكن بيتولد دايمًا بالقيم الصحيحة

---

## تجربة سريعة:

### `appsettings.json` صح:

```json
"Jwt": {
  "Key": "SuperSecretKey",
  "Issuer": "SurveyBasketApp",
  "Audience": "SurveyBasketApp users",
  "ExpiryMinutes": 30
}
```

تشغل التطبيق ←  يشتغل عادي.

---

###  `ExpiryMinutes` ناقصة:

```json
"Jwt": {
  "Key": "SuperSecretKey",
  "Issuer": "SurveyBasketApp",
  "Audience": "SurveyBasketApp users"
}
```

تشغل التطبيق ← Error: `OptionsValidationException`

---

###  `ExpiryMinutes` = -30  "قيمة غلط":

```json
"Jwt": {
  "Key": "SuperSecretKey",
  "Issuer": "SurveyBasketApp",
  "Audience": "SurveyBasketApp users",
  "ExpiryMinutes": -30
}
```

تشغل التطبيق ← Error: `OptionsValidationException`

---

## الخلاصة:

## **المشكلة الأساسية**

تخيل معايا إن عندك إعدادات مهمة جدًا في `appsettings.json`، زي إعدادات التوكن (JWT)، قاعدة البيانات، Cloud services، إلخ.

لو نسيت تحط واحدة من القيم، أو كتبت قيمة غلط (زي رقم سالب أو نص فاضي)، إيه اللي ممكن يحصل؟

### سيناريو 1: إعداد ناقص

```json
"Jwt": {
  "Key": "super-secret",
  "Issuer": "SurveyBasketApp",
  "Audience": "SurveyBasketApp users"
  // ExpiryMinutes مش موجودة
}
```

الـ `ExpiryMinutes` مش موجودة!
بس لأن الـ `int` في C# قيمة الـ default بتاعته هي `0`، التطبيق هيشتغل…
لكن الـ token هيخلص فورًا! 

 **النتيجة:** التوكن شغال شكله تمام، لكن بيخلص قبل ما يوصل لأي EndPoint.
 **سلوك غير متوقع وصعب تكتشفه.**

---

### سيناريو 2: قيمة غير منطقية

```json
"Jwt": {
  "Key": "super-secret",
  "Issuer": "SurveyBasketApp",
  "Audience": "SurveyBasketApp users",
  "ExpiryMinutes": -30
}
```

هنا كتبنا `ExpiryMinutes = -30`.
هل التطبيق هيوقف؟ لا.
هل فيه Error؟ لا.

بس الـ expiresIn هيطلع بالسالب:

```json
"expiresIn": -1800
```

وده خطأ منطقي كبير جدًا.
 **يعني العميل بقى معاه توكن بانتهى من المستقبل**!

---

## الحل: Data Annotations + Options Validation

ال ASP.NET Core بيديك أدوات قوية تتأكد إن الإعدادات اللي جايالك من `appsettings.json` **صحيحة ومنطقية**، مش بس موجودة.

---

## الخطوة الأولى: تعريف كلاس الإعدادات + الفاليديشن

```csharp
public class JwtOptions
{
    public static string SectionName = "Jwt";

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

### ليه بنستخدم `[Required]` و `[Range(...)]`؟

* ال `Required`: تتأكد إن القيمة مش null أو فاضية (بالنسبة لـ strings).
* ال `Range`: تضمن إن القيمة في مدى منطقي (زي إن الـ ExpiryMinutes مش بالسالب أو بصفر).

---

## الخطوة الثانية: ربط الكلاس ده بالـ Configuration + تفعيل الفاليديشن

```csharp
builder.Services.AddOptions<JwtOptions>()
    .BindConfiguration(JwtOptions.SectionName)
    .ValidateDataAnnotations();
```

### السطر ده بيعمل إيه تحديدًا؟

| كود                         | الوظيفة                                                |
| --------------------------- | ------------------------------------------------------ |
| `AddOptions<JwtOptions>()`  | بنسجل الـ Options class في الـ DI container            |
| `BindConfiguration(...)`    | بنربطه ببيانات `appsettings.json`                      |
| `ValidateDataAnnotations()` | بنقوله فعّل كل الفاليديشنز اللي حطيناها على البروبرتيز |

---

## الخطوة الثالثة (المهمة جدًا): `ValidateOnStart()`

```csharp
builder.Services.AddOptions<JwtOptions>()
    .BindConfiguration(JwtOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();
```

### إيه اللي بيحصل هنا؟

أول ما التطبيق يشتغل (في `Program.cs`)، الفاليديشن بتشتغل فورًا.
لو فيه أي مشكلة، مش هيسمح للتطبيق يشتغل أصلًا.

 **وده مهم جدًا في بيئة Production**
بدل ما تكتشف بعد النشر إن التطبيق شغال بـ config بايظ، لأ، التطبيق يرفض يشتغل أساسًا ويديك:

```
OptionsValidationException
```

---

## مثال كامل في `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<JwtOptions>()
    .BindConfiguration(JwtOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();
```

---

## كده إحنا ضمنا:

* أي config ناقص → هيتم اكتشافه.
* أي config غلط (سالب، فارغ، غير منطقي) → هيتم رفضه.
* نمنع أي احتمال إن التطبيق "يشتغل شكله تمام" لكن "فيه حاجة جوه مش مظبوطة".

---

## طيب لو حصلت مشكلة فعلًا؟ هيحصل إيه؟

لو شغلت التطبيق وفيه خطأ في الإعدادات، هتلاقي:

* ال Visual Studio كتبلك:

  ```
  Unable to connect to web server 'https', the web server is no longer running
  ```

* وفي الـ terminal أو output log:

  ```
  Unhandled exception. Microsoft.Extensions.Options.OptionsValidationException: DataAnnotation validation failed for 'JwtOptions' members: ['ExpiryMinutes' must be between 1 and 2147483647.']
  ```

---

## خلاصة السيناريوهات اللي لازم تحمي نفسك منها

| السيناريو                          | النتيجة                         | طريقة الحماية                     |
| ---------------------------------- | ------------------------------- | --------------------------------- |
| نسيت key في appsettings            | قيمة default (null أو 0) هتتطبق | `[Required] + ValidateOnStart()`  |
| كتبت قيمة غلط (زي -30)             | سلوك منطقي خاطئ                 | `[Range] + ValidateOnStart()`     |
| غلط في اسم السكشن في config        | الكلاس مش هيتبند أصلاً (null)   | تأكد من `.BindConfiguration(...)` |
| نسيت تعمل Bind أصلاً في Program.cs | القيم مش هتوصل للـ Service      | لازم تسجل `AddOptions().Bind()`   |

---



| نقطة                        | فايدتها                                               |
| --------------------------- | ----------------------------------------------------- |
| `DataAnnotation`            | تتحقق من القيم المطلوبة والفورمات الصحيحة             |
| `ValidateDataAnnotations()` | تفعل التحقق من الكلاس أثناء الاستخدام                 |
| `ValidateOnStart()`         | تمنع تشغيل التطبيق أساسًا لو فيه مشكلة بالإعدادات     |
| Exceptions واضحة            | بتديك error details بسرعة بدل ما تلف حوالين المشكلة   |
| أحسن ممارسة (Best Practice) | دايمًا Validate options عشان تحمي التطبيق من المفاجآت |

---
