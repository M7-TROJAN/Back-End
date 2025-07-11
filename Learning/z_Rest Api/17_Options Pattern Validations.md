
> **Options Pattern Validations**
> هو ضمان إن الإعدادات اللي جاية من `appsettings.json` (أو أي مصدر) تكون سليمة قبل ما الابليكيشن يشتغل.

---

## ليه نحتاج نعمل Validation للإعدادات (Options Validation)؟

تخيل معايا إنك نسيت تحط قيمة في `appsettings.json` أو كتبت قيمة غلط — التطبيق هيشتغل، لكن:

* هيطلع **سلوك غير متوقع**
* أو الأسوأ: يوصل **للProduction** وساعتها تبقى المصيبة أكبر

---

## تخيل عندك سكشن في ال appsettings.json بالشكل ده:

### `appsettings.json`

```json
"Jwt": {
  "Key": "",
  "Issuer": "SurveyBasketApp",
  "Audience": "SurveyBasketApp users"
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

1. التطبيق هيشتغل عادي.
2. هتعمل login، هيتولد التوكن، بس `expiresIn` = 0 (غلط جدًا).
3. وهنا انت مش واخد بالك إن فيه مشكلة في الإعدادات!

---

## الحل: **Data Annotation Validation**

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

---

## فين نعمل الربط + التحقق؟ ← `Program.cs`

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

| نقطة                        | فايدتها                                               |
| --------------------------- | ----------------------------------------------------- |
| `DataAnnotation`            | تتحقق من القيم المطلوبة والفورمات الصحيحة             |
| `ValidateDataAnnotations()` | تفعل التحقق من الكلاس أثناء الاستخدام                 |
| `ValidateOnStart()`         | تمنع تشغيل التطبيق أساسًا لو فيه مشكلة بالإعدادات     |
| Exceptions واضحة            | بتديك error details بسرعة بدل ما تلف حوالين المشكلة   |
| أحسن ممارسة (Best Practice) | دايمًا Validate options عشان تحمي التطبيق من المفاجآت |

---
