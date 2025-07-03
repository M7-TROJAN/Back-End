
##  انواع الـ Lifetime

| النوع         | دورة الحياة                                              | مثال مبسط                                        |
| ------------- | -------------------------------------------------------- | ------------------------------------------------ |
| **Transient** | كائن جديد في كل مرة يتم طلبه                             | كل مرة تنادي على السيرفيس، بيعمل `new`           |
| **Scoped**    | كائن واحد طول عمر الـ HTTP Request                       | الكائن بيتعمل أول مرة وتفضل نفس النسخة طول الطلب |
| **Singleton** | كائن واحد بيتعمل مرة واحدة عند تشغيل التطبيق ويفضل موجود | بيعيش طول عمر الـ application                    |

---

## مثال على `ApplicationDbContext`

### ليه دايمًا بنستخدم `AddScoped<ApplicationDbContext>()`؟

لأن `DbContext` لازم:

* يكون له **حالة مستقلة لكل Request**.
* وأي **تزامن (Concurrency)** في استخدامه ممكن يعمل مشاكل لو تم مشاركته بين طلبين مختلفين.

### لو استخدمته بـ Singleton

* هيتم مشاركة نفس الـ `DbContext` بين كل الطلبات ودي مشكلة كبيرة!
* هيحصل **Thread Safety Issues** لأن الـ DbContext مش مصمم يبقى مشترك.

### ولو استخدمته بـ Transient

* هيتعمل Object جديد **في كل مرة يتم فيها حقنه**.
* ده ممكن يؤدي لوجود أكثر من `DbContext` لنفس الـ request فيحصل **tracking conflict** أو **مشكلة حفظ بيانات مش متزامنة**.

>  علشان كده `Scoped` هو الخيار المثالي مع `DbContext`:
وده لأنه
* بيضمن نسخة واحدة للـ Request.
* ويحافظ على تتبع الكيانات `ChangeTracking`.
* وبيمنع تداخل الاستخدام.

---

##  طيب... إمتى أستخدم كل نوع:

### ال **Transient** امتى نستخدمه؟

لما يكون السيرفيس **stateless**، يعني:

* مفيهوش داتا بتتخزن.
* وملوش حالة لازم تتبعها.
* أو بيشتغل كأداة utility.

#### أمثلة:

* ال `EmailSenderService`
* ال `ICalculatorService`
* ال `ITokenGenerator`

#### ليه Transient هنا؟

لأن مفيش أي قيمة في إعادة استخدام نفس الكائن، وكمان مش بيخزن حالة، فكل مرة نعمله `new` ده طبيعي وآمن.

---

### ال **Scoped** امتى نستخدمه؟

لما تكون الخدمة:

* محتاجة تحتفظ بحالة خلال **طلب واحد فقط**.
* أو بتتعامل مع داتا مرتبطة بالـ Request (زي الـ User أو الـ DB).

#### أمثلة:

* ال `ApplicationDbContext`
* ال `IUserSessionService` (اللي بيجيب بيانات اليوزر من الكليمز أو التوكن)
* ال `IUnitOfWork`

#### ليه Scoped هنا؟

علشان نضمن consistency في الداتا اللي شغالين عليها داخل نفس الـ request.

---

### ال  **Singleton** امتى نستخدمه؟

لما تكون الخدمة:

* فيها بيانات **ثابتة ومش بتتغير**.
* أو فيها عمليات مكلفة وعايز تعيد استخدامها طول الوقت.
* ومفيهاش أي تعامل مع الـ HTTP أو الـ Request أو المستخدم.

#### أمثلة:

* ال `IConfiguration` (الـ appsettings)
* ال `ILogger<>`
* ال `CachingService` (لو بيستخدم in-memory static cache)

#### تحذير مهم:

لو خدت Singleton وحقنت فيه Scoped أو Transient دي مشكلة وهتدخل في **Captive Dependency Problem**.

---

## أهم قاعدة: Lifetime الأطول ميحقنش الأقصر!

| الحقن                   | هل ينفع؟ |
| ----------------------- | -------- |
| Singleton → Scoped ❌    |          |
| Singleton → Transient ❌ |          |
| Scoped → Transient ✅    |          |
| Transient → Scoped ✅    |          |

> ال  Singleton مبيفهمش request، فلو حقنت فيه Scoped، هتجيب كائن بيبص على حاجة مش موجودة أصلاً!

---

## تقدر تشبهم بكدة

* ال **Transient** = زي كوباية بلاستيك بتترمي بعد كل شرب.
* ال **Scoped** = زي كوباية زجاج بتستخدمها طول اليوم بس بتغسلها آخره.
* ال **Singleton** = زي فلتر المياه في البيت، بتستخدمه دايمًا ومش بتغيره إلا بعد شهور.

---

## مثال كامل بكود

```csharp
// Startup.cs أو Program.cs
services.AddTransient<IEmailSender, EmailSender>();
services.AddScoped<IUserService, UserService>();
services.AddSingleton<ICurrencyConverter, CurrencyConverter>();

// Example usage inside a Controller
public class HomeController : Controller
{
    private readonly IUserService _userService;
    private readonly IEmailSender _emailSender;

    public HomeController(IUserService userService, IEmailSender emailSender)
    {
        _userService = userService;
        _emailSender = emailSender;
    }

    public IActionResult Index()
    {
        var user = _userService.GetCurrentUser();
        _emailSender.SendEmail(user.Email, "Hello!");

        return View();
    }
}
```

---

## توصيات

| الحالة                                               | استخدم        |
| ---------------------------------------------------- | ------------- |
| بتتعامل مع DB أو session أو request-specific         | **Scoped**    |
| Utility أو Tool بسيطة بدون حالة                      | **Transient** |
| Config أو Services ثابتة بتحتاج أداء عالي أو caching | **Singleton** |

---


## تخيل السيناريو:

> *يوزر فتح صفحة `Products` → حصل HTTP Request → الكنترولر حقن فيه `ApplicationDbContext` → جاب أول 10 منتجات → رجعهم في الـ Response.*

### تعال نشوف إيه اللي بيحصل بالترتيب داخليًا

---

## أولاً: ايه اللي بيحصل لما اليوزر يفتح الصفحة (أول Request)؟

### 1. المتصفح بعت Request مثلاً:

```
GET /Products?page=1
```

### بعد كدة ال ASP.NET Core عمل الآتي:

* أنشأ **HTTP Request Scope جديد** (يعني بيبدأ حياة جديدة للكائنات اللي `AddScoped`).
* جوه الـ Scope ده، بيبدأ يحل الـ Dependencies المطلوبة للكنترولر.
* الكنترولر فيه `ApplicationDbContext`، فـ DI container الي هو المسؤول عن الحقن عمل:

```csharp
var dbContext = new ApplicationDbContext(...);
```

* الكائن ده بيعيش طول فترة الـ Request ده وبس.

### مهم:

> أي Service تانية في نفس الـ Scope وبتطلب `ApplicationDbContext` هتستلم **نفس النسخة** مش نسخة جديدة.

---

### 3. دلوقي الكنترولر اشتغل:

* استخدم `dbContext.Products.Skip(0).Take(10)` وجاب البيانات.
* رجّعهم في `View` أو `JSON`.

### 4. بعد ما ال response خلص وخلاص اتبعت لليوزر:

* ال ASP.NET Core **بيفنّش كل الكائنات scoped** تلقائيًا.
* يعني الـ `ApplicationDbContext` **بيتعمله Dispose تلقائيًا** (لأن `DbContext` بي implement `IDisposable`).

---

## دلوقتي اليوزر عمل سكرول في الموقع لتحت فعمل AJAX Request جديد يعني عاوز الصفحة الي بعد كدة:

### GET /Products?page=2

* ال Request الجديد = Scope جديد = `DbContext` جديد.
* كل حاجة بتبدأ تاني من الصفر.
* لكن لأننا عاملين `Scoped`، فـ ده المتوقع والمطلوب.

---

## خلاصة فهم الـ Scoped هنا:

| العنصر                       | المعنى                             |
| ---------------------------- | ---------------------------------- |
| مدة الحياة                   | من بداية الـ HTTP Request لنهايته  |
| عدد النسخ في نفس الـ Request | نسخة واحدة                         |
| لو حصل أكثر من Request؟      | كل Request بياخد نسخة جديدة تمامًا |

---

## طيب تعال نفس السيناريو بس نخلي الـ DbContext عبارة عن Transient:

* كل ما الـ DI يحاول يحقنه → بيعمل `new ApplicationDbContext()`.
* لو الكنترولر فيه 2 سيرفيس بيستخدموا DbContext فكدة كل واحد فيهم هيستلم **نسخة مختلفة**.
* وممكن يحصل:

  * تضارب في الـ Tracking
  * ال Queries مش متزامنة
  * مشاكل في الحفظ

>  لذلك: ال **`DbContext` بـ Transient = خطر إلا في حالات محددة جدًا (غالبًا Testing).**

---

### طب تعالي نجرب ال  Singleton

* هنا الخطورة أكبر جدًا!
* نسخة واحدة بس من `DbContext` لكل التطبيق.
* كل Request وكل مستخدم بيشاركوا نفس الـ Instance.

ده معناه:

* ممكن ريكويستين يقروا أو يعدلوا نفس الكائن في نفس الوقت!
* ا **Concurrency violations**
* ا **Dirty data**
* ا **Memory leaks**

> وعلشان كدة Singleton مع DbContext = **مصيبة في الإنتاج!**

---

## طيب نطبق ده على Service تانية مش DbContext

### مثلا: `ICurrencyConverter`

* خدمة بتحول العملات.
* بتستخدم معدلات من API خارجي وبتحفظهم في الذاكرة.

---

### تستخدمها كـ Singleton؟

* الاجابة آه.
* ليه؟ لأن:

  * معدلات العملات مش بتتغير كتير.
  * الخدمة مفيهاش State متعلق بـ Request.
  * أفضل أعملها Cache ومشاركها بين كل الـ Requests.

---

### تستخدمها كـ Scoped أو Transient؟

* ممكن، بس ده Overkill.
* لو Transient فكل Request بيجيب نسخة جديدة وده يعني تحميل زيادة على الذاكرة.
* لو Scoped مالوش فايدة، لأن مفيش State محتاج يتغير.

---

## المقارنة النهائية بين الأنواع:

| الحالة                     | Transient | Scoped          | Singleton         |
| -------------------------- | --------- | --------------- | ----------------- |
| مناسب لـ DbContext         | ❌         | ✅               | ❌                 |
| مناسب لـ Utility Service   | ✅         | ✅               | ✅                 |
| بيعمل نسخة جديدة كل مرة؟   | ✅         | ❌ (لكل Request) | ❌ (مرة واحدة فقط) |
| يشارك الكائن بين Requests؟ | ❌         | ❌               | ✅                 |

---

## إزاي تعرف تختار؟

### اسأل نفسك:

1. هل الخدمة بتحتاج تحتفظ بحالة خلال الـ Request؟ → استخدم Scoped.
2. هل الخدمة Stateless؟ ومفيهاش أي Request-specific data؟ → استخدم Transient.
3. هل الخدمة فيها بيانات ثابتة وبتخدم كل المستخدمين؟ → Singleton.

---

من أول **.NET 8** ظهر مفهوم جديد في الـ Dependency Injection اسمه:

> ✅ `AddKeyedScoped` / `AddKeyedSingleton` / `AddKeyedTransient`

وده بيُستخدم مع حاجة جديدة اسمها **Keyed Services**.

---

## السؤال: إيه الفرق بين

```csharp
builder.Services.AddScoped<IMyService, MyService>();
```

و

```csharp
builder.Services.AddKeyedScoped<IMyService, MyService>("my-key");
```

الفرق الأساسي هو:

> ال `AddScoped` = بتسجل **نسخة واحدة فقط** من السيرفيس لكل الإنترفيس (type).
> ال `AddKeyedScoped` = بتسجل **أكثر من نسخة** من نفس السيرفيس، لكن **بمفتاح مختلف (Key)**، وتقدر تطلب واحدة معينة وقت ما تحتاجها.

---

## ليه أصلاً نحتاج Keyed Services؟

في بعض الأحيان، بتكون عندك **أكثر من Implementaion لنفس الـ Interface**، وبتحتاج تختار واحدة فيهم **حسب السياق**.

مثلاً: عندك 3 طرق لإرسال الرسائل:

```csharp
public interface IMessageSender
{
    void Send(string message);
}

public class EmailSender : IMessageSender
{
    public void Send(string message) => Console.WriteLine($"Email: {message}");
}

public class SmsSender : IMessageSender
{
    public void Send(string message) => Console.WriteLine($"SMS: {message}");
}

public class PushNotificationSender : IMessageSender
{
    public void Send(string message) => Console.WriteLine($"Push: {message}");
}
```

في القديم (قبل .NET 8) كنت ممكن تعمل:

```csharp
services.AddTransient<EmailSender>();
services.AddTransient<SmsSender>();
services.AddTransient<PushNotificationSender>();
```

لكن لو حقنت `IMessageSender` لازم تختار `Implementation` واحد بس منهم الي تحقنه
-ولو فكرت تحقن التلاتة وتعمل كدة 
```csharp
builder.Services.AddTransient<IMessageSender, EmailSender>();
builder.Services.AddTransient<IMessageSender, SmsSender>();
builder.Services.AddTransient<IMessageSender, PushNotificationSender>();
```
 هنا هيحصل Error لأن فيه أكثر من واحدة!

---

## الحل في .NET 8: استخدم `AddKeyed`

```csharp
builder.Services.AddKeyedTransient<IMessageSender, EmailSender>("email");
builder.Services.AddKeyedTransient<IMessageSender, SmsSender>("sms");
builder.Services.AddKeyedTransient<IMessageSender, PushNotificationSender>("push");
```

---

## إزاي تـ inject النسخة اللي عليها Key معين؟

### باستخدام `[FromKeyedServices("key")]` في الكنترولر أو السيرفيس:

```csharp
public class NotificationController : Controller
{
    private readonly IMessageSender _smsSender;

    public NotificationController([FromKeyedServices("sms")] IMessageSender smsSender)
    {
        _smsSender = smsSender;
    }

    public IActionResult Send()
    {
        _smsSender.Send("Your code is 1234");
        return Ok();
    }
}
```

---

###  أو تعمل `Inject` ل `IKeyedServiceProvider` وتجيب منه اللي انت عايزه زي كدة

```csharp
public class NotificationService
{
    private readonly IKeyedServiceProvider _provider;

    public NotificationService(IKeyedServiceProvider provider)
    {
        _provider = provider;
    }

    public void Notify(string method)
    {
        var sender = _provider.GetRequiredKeyedService<IMessageSender>(method);
        sender.Send("Your message");
    }
}
```

---

## مقارنة سريعة بين العادي والـ Keyed:

| الخاصية                              | AddScoped / AddTransient / AddSingleton | AddKeyedScoped / Keyed Services         |
| ------------------------------------ | --------------------------------------- | --------------------------------------- |
| كم نسخة ممكن تسجل؟                   | واحدة لكل Type                          | أكتر من واحدة بنفس الـ Type             |
| تقدر تختار أي نسخة منهم وقت التشغيل؟ | ❌ لأ                                    | ✅ أيوه                                  |
| الدعم في .NET قبل 8؟                 | ✅ مدعوم                                 | ❌ فقط في .NET 8+                        |
| ينفع تستخدمه في الـ Controller؟      | ✅ مباشرة                                | ✅ باستخدام `[FromKeyedServices("key")]` |

---

## ملاحظات مهمة:

* ال `AddKeyed...` بيديك **تحكم أكبر** لما تحتاج أكتر من Version من نفس الخدمة.
* المفتاح ممكن يكون `string` أو `enum` أو أي نوع آخر.
* مفيدة جدًا في سيناريوهات زي:

  * استدعاء API مختلف حسب العميل.
  * إرسال الرسائل بوسيلة مختلفة.
  * لو عندك أكتر من نسخة من نفس السيرفيس ببيانات إعداد مختلفة.

---

## مثال تطبيقي كامل:

```csharp
// Program.cs
builder.Services.AddKeyedScoped<IMessageSender, EmailSender>("email");
builder.Services.AddKeyedScoped<IMessageSender, SmsSender>("sms");

// Controller
public class MessageController : Controller
{
    private readonly IMessageSender _emailSender;

    public MessageController([FromKeyedServices("email")] IMessageSender emailSender)
    {
        _emailSender = emailSender;
    }

    public IActionResult Send()
    {
        _emailSender.Send("Hello via Email");
        return Ok();
    }
}
```

---

## ملخص:

| الحالة                                            | استخدم                            |
| ------------------------------------------------- | --------------------------------- |
| عندك خدمة واحدة بس لكل Interface                  | `AddScoped` / `AddTransient`      |
| عندك أكتر من نسخة من نفس الخدمة وعايز تختار بينهم | `AddKeyedScoped` أو `AddKeyed...` |

---

## مثال كامل 


## السيناريو: نظام إرسال إشعارات Notification System

عندنا تلات أنواع إرسال:

* Email
* SMS
* Push Notification

وإحنا عايزين نختار وسيلة الإرسال حسب المستخدم، أو حسب إعداد معين جاي من قاعدة البيانات أو الإعدادات.

---

## الخطوة الاولي: إنشاء Interface مشترك

```csharp
public interface INotificationSender
{
    void Send(string message);
}
```

---

## الخطوة التانية: إنشاء التلاتة Implementations

```csharp
public class EmailNotificationSender : INotificationSender
{
    public void Send(string message)
    {
        Console.WriteLine($" Email sent: {message}");
    }
}

public class SmsNotificationSender : INotificationSender
{
    public void Send(string message)
    {
        Console.WriteLine($" SMS sent: {message}");
    }
}

public class PushNotificationSender : INotificationSender
{
    public void Send(string message)
    {
        Console.WriteLine($" Push Notification: {message}");
    }
}
```

---

##  الخطوة التالتة: تسجيلهم في الـ DI Container باستخدام `AddKeyedScoped`

```csharp
builder.Services.AddKeyedScoped<INotificationSender, EmailNotificationSender>("email");
builder.Services.AddKeyedScoped<INotificationSender, SmsNotificationSender>("sms");
builder.Services.AddKeyedScoped<INotificationSender, PushNotificationSender>("push");
```

---

## الخطوة الرابعة: نعمل خدمة وسيطة NotificationManager بتاخد الـ Key وتنفذ الإرسال

```csharp
public class NotificationManager
{
    private readonly IKeyedServiceProvider _provider;

    public NotificationManager(IKeyedServiceProvider provider)
    {
        _provider = provider;
    }

    public void NotifyUser(string channel, string message)
    {
        var sender = _provider.GetRequiredKeyedService<INotificationSender>(channel);
        sender.Send(message);
    }
}
```

ونسجلها كده:

```csharp
builder.Services.AddScoped<NotificationManager>();
```

---

## الخطوة الخامسة: ال Controller أو ال Endpoint يستخدم `NotificationManager`

```csharp
[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly NotificationManager _manager;

    public NotificationsController(NotificationManager manager)
    {
        _manager = manager;
    }

    [HttpPost]
    public IActionResult SendNotification(string channel, string message)
    {
        try
        {
            _manager.NotifyUser(channel, message);
            return Ok($"Message sent via {channel}");
        }
        catch (Exception)
        {
            return BadRequest($"Invalid channel: {channel}");
        }
    }
}
```

---

## مثال Call:

```http
POST /api/notifications?channel=email&message=Welcome
```

 النتيجة:

```
Email sent: Welcome
```

```http
POST /api/notifications?channel=sms&message=Your code is 1234
```

 النتيجة:

```
 SMS sent: Your code is 1234
```

---

## ملاحظات هامة:

| نقطة                    | شرح                                                                  |
| ----------------------- | -------------------------------------------------------------------- |
| `AddKeyedScoped`        | بتسجل نسخة مختلفة لكل Key                                            |
| `IKeyedServiceProvider` | بيسمح لك تطلب الـ service حسب الـ Key وقت التشغيل                    |
| الأداء                  | فعال جدًا لما يكون عندك أكتر من نسخة من service واحدة وعايز تحكم مرن |
| الآمان                  | لو الـ key غلط → `GetRequiredKeyedService` هيعمل Exception           |

---

## الملخص:

* ال Keyed Services أداة قوية من .NET 8 بتنفع في السيناريوهات اللي فيها تعدد للـ Implementations.
* بتسهل الكود وبتخليك متستخدمش `if/else` أو `switch` عشان تختار نسخة معينة.
* دي أفضل ممارسة بدل ما تحقن كل الـ implementations وتختار بينهم يدويًا.
