
من أول **.NET 8** ظهر مفهوم جديد في الـ Dependency Injection اسمه:

>  `AddKeyedScoped` / `AddKeyedSingleton` / `AddKeyedTransient`

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
