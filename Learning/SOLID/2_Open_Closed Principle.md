
# **O — Open/Closed Principle (OCP)**

### "مبدأ الفتح/الغلق"

---

##  التعريف الرسمي:

> **"Software entities (classes, modules, functions, etc.) should be open for extension, but closed for modification."**
طب يعني إيه الكلام ده؟

* ال **Open for extension:** يعني تقدر تضيف سلوك جديد للكود.
* ال **Closed for modification:** يعني من غير ما تعدّل على الكود القديم.

---

## الفكرة ببساطة:

لو عندك كلاس شغال تمام، وتم اختباره والدنيا تمام وزي الفل، ومستخدم في أكتر من حتة،
لما تيجي تضيف ميزة جديدة، المفروض **ما تلمسوش**.
وبدل ما تفتح بطن الكلاس ده وتعدّل عليه، بنعمل **امتداد ليه** (inheritance, composition, delegation... إلخ).

---

##  ليه OCP مهم جدًا؟

* التعديل على كود شغال ممكن يكسّر حاجات كانت شغالة.
* بيزود احتمال حدوث Bugs.
* بيقلل الـ Reusability.
* بيزود Coupling بين الأجزاء.

---

##  سيناريو: بدون OCP (كود قابل للكسر بسهولة)

تخيل عندك نظام حساب ضريبة:

```csharp
public class TaxCalculator
{
    public double CalculateTax(string country, double amount)
    {
        if (country == "US")
            return amount * 0.1;
        else if (country == "UK")
            return amount * 0.2;
        else if (country == "EG")
            return amount * 0.14;
        else
            throw new NotSupportedException("Country not supported");
    }
}
```

هنا لو جيت تضيف دولة جديدة، لازم تفتح الكلاس وتعدّل عليه.
يعني **الكود مش مغلق للتعديل**.

---

## الحل باستخدام OCP:

### 1. نعمل Interface:

```csharp
public interface ITaxStrategy
{
    double CalculateTax(double amount);
}
```

### 2. نعمل Implementation لكل دولة:

```csharp
public class USTax : ITaxStrategy
{
    public double CalculateTax(double amount) => amount * 0.1;
}

public class UKTax : ITaxStrategy
{
    public double CalculateTax(double amount) => amount * 0.2;
}

public class EGTaX : ITaxStrategy
{
    public double CalculateTax(double amount) => amount * 0.14;
}
```

### 3. نحقن الاستراتيجية في الكلاس الأساسي:

```csharp
public class TaxCalculator
{
    private readonly ITaxStrategy _taxStrategy;

    public TaxCalculator(ITaxStrategy taxStrategy)
    {
        _taxStrategy = taxStrategy;
    }

    public double Calculate(double amount)
    {
        return _taxStrategy.CalculateTax(amount);
    }
}
```

 دلوقتي لو عايز تضيف بلد جديدة:

* بتضيف كلاس جديد بس (مثلاً: `FranceTax`)
* **من غير ما تعدّل الكود اللي شغال**

وده بالضبط OCP.

---

## أدوات بتساعدك تطبّق OCP:

* ال **Interfaces / Abstract Classes**
* ال **Inheritance**
* ال **Composition**
* ال **Dependency Injection**
* ال **Polymorphism**

---

##  سيناريو 2: Notification System

```csharp
public class Notifier
{
    public void Notify(string type, string message)
    {
        if (type == "Email")
            SendEmail(message);
        else if (type == "SMS")
            SendSMS(message);
    }

    private void SendEmail(string msg) { /* Send Email */ }
    private void SendSMS(string msg) { /* Send SMS */ }
}
```

 كل ما تضيف وسيلة جديدة مثلا (WhatsApp, Push...) لازم تعدّل الكلاس الي اسمه `Notifier`.

### الحل: OCP via interface:

```csharp
public interface INotificationService
{
    void Send(string message);
}

public class EmailNotifier : INotificationService
{
    public void Send(string message) => Console.WriteLine("Email: " + message);
}

public class SMSNotifier : INotificationService
{
    public void Send(string message) => Console.WriteLine("SMS: " + message);
}

public class NotificationManager
{
    private readonly INotificationService _service;

    public NotificationManager(INotificationService service)
    {
        _service = service;
    }

    public void Notify(string message)
    {
        _service.Send(message);
    }
}
```

* عايز تضيف وسيلة جديدة؟  بس كلاس جديد.
* الكود الأساسي؟ ما اتلمسش.

---

##  تحذير:

ال OCP مش معناها تعمل Abstract Classes لكل حاجة من أول يوم،
لكن لما تحس إن فيه احتمال كبير للتغيير، وتكرار منطقي، يبقى وقتها OCP مفيد.

---

## ملخص سريع:

|         | بدون OCP             | مع OCP                           |
| ------- | -------------------- | -------------------------------- |
| التوسعة | صعبة وبتكسر كود قديم | سهلة عن طريق إضافة Classes جديدة |
| الصيانة | عالية المخاطر        | آمنة                             |
| التستنج | معقدة                | سهلة                             |

---



## طب إزاي نطبّق مبدأ OCP في ASP.NET Core Web باستخدام Dependency Injection؟

---

### الفكرة الأساسية:

في ال Web App، بنحدد في ملف `Program.cs`:

* **الـ Interface** اللي عايزين نستخدمه.
* **والـ Implementation** (يعني أنهي كلاس فعلي يتنفذ).

---

## مثال عملي: Notification System في Web App

### أولًا: بنكتب الـ Interface والـ Implementations:

```csharp
public interface INotificationService
{
    void Send(string message);
}

public class EmailNotifier : INotificationService
{
    public void Send(string message)
    {
        Console.WriteLine("Email: " + message);
    }
}

public class SMSNotifier : INotificationService
{
    public void Send(string message)
    {
        Console.WriteLine("SMS: " + message);
    }
}
```

---

##  دلوقتي نروح لـ `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

// تسجيل ال Service في ال Dependency Injection Container
builder.Services.AddScoped<INotificationService, EmailNotifier>();
```

 هنا بتقول للـ framework:

> "لما أي Controller أو Class يطلب `INotificationService`، ابعت له نسخة من `EmailNotifier`."

لو غيرت السطر ده إلى:

```csharp
builder.Services.AddScoped<INotificationService, SMSNotifier>();
```

يبقى كده التطبيق هيشتغل بـ **SMSNotifier** بدل الـ Email، من **غير ما تعدّل سطر واحد في الكود الأساسي**.

وده هو **مبدأ OCP بالضبط**.

---

## مثال عن استخدامه في ال Controller:

```csharp
public class HomeController : Controller
{
    private readonly INotificationService _notifier;

    public HomeController(INotificationService notifier)
    {
        _notifier = notifier;
    }

    public IActionResult Index()
    {
        _notifier.Send("Hello Mahmoud!");
        return View();
    }
}
```

 هنا:

* ال ASP.NET Core عملت inject ل `INotificationService` تلقائيًا.
* بناءً على اللي سجلته في `Program.cs`، هيتبعت Email أو SMS.

---

## تلخيص:

| الملف                      | دوره                                                         |
| -------------------------- | ------------------------------------------------------------ |
| `Program.cs`               | بيحدد أنهي Implementation يشتغل مع الأنترفيس                 |
| `Controller` أو `Service`  | بيشتغل مع الأنترفيس فقط، ومش بيهتم أنهي كلاس اللي وراه       |
| التوسعة                    | ببساطة: تضيف كلاس جديد، وتغير سطر التسجيل في `Program.cs` بس |
| التعديل على الكود الأساسي؟ | ❌ لا يحصل إطلاقًا، وده هو جوهر OCP                           |

---

##  كأنك بتقول للـ ASP.NET Core:

> "أنا هتعامل دايمًا مع `INotificationService`، وإنت شوف أنهي implementation تبعته، وخليني مرن!"

---


طب إزاي نتعامل مع **أكتر من Implementation لنفس Interface** **في نفس الـ Controller أو Service؟**

هنا فيه **3 طرق احترافية** نقدر تستخدمهم علشان نبعت **Email و WhatsApp (أو SMS أو غيرهم)** في نفس الوقت:

---

## الطريقة الاولي : استخدام **عدة Interfaces مختلفة** (سهلة ولذيذة لو عندك عدد محدود وثابت من الأنواع)

### مثال:

```csharp
public interface IEmailSender
{
    void Send(string message);
}

public interface IWhatsAppSender
{
    void Send(string message);
}
```

### بعد كدة نعمل ال Implementations:

```csharp
public class EmailSender : IEmailSender
{
    public void Send(string message)
    {
        Console.WriteLine("Sending Email: " + message);
    }
}

public class WhatsAppSender : IWhatsAppSender
{
    public void Send(string message)
    {
        Console.WriteLine("Sending WhatsApp: " + message);
    }
}
```

###  نسجّلهم في `Program.cs`:

```csharp
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IWhatsAppSender, WhatsAppSender>();
```

### وأخيرًا تستخدمهم في الـ Controller:

```csharp
public class NotificationController : Controller
{
    private readonly IEmailSender _emailSender;
    private readonly IWhatsAppSender _whatsAppSender;

    public NotificationController(IEmailSender emailSender, IWhatsAppSender whatsAppSender)
    {
        _emailSender = emailSender;
        _whatsAppSender = whatsAppSender;
    }

    public IActionResult NotifyUser()
    {
        _emailSender.Send("Welcome Email");
        _whatsAppSender.Send("WhatsApp Message");

        return Ok();
    }
}
```

 بسيطة وواضحة... لكن مش مرنة لو عندك أنواع كتير من الوسائل.

---

##  الطريقة التانية : استخدام **نفس ال Interface** مع `IEnumerable<INotificationService>`

لو عندك Interface موحّد:

```csharp
public interface INotificationService
{
    string Channel { get; }
    void Send(string message);
}
```

### ونعمل أكتر من Implementation:

```csharp
public class EmailNotifier : INotificationService
{
    public string Channel => "Email";

    public void Send(string message)
    {
        Console.WriteLine("Sending Email: " + message);
    }
}

public class WhatsAppNotifier : INotificationService
{
    public string Channel => "WhatsApp";

    public void Send(string message)
    {
        Console.WriteLine("Sending WhatsApp: " + message);
    }
}
```

###  نسجّلهم في `program.cs`:

```csharp
builder.Services.AddScoped<INotificationService, EmailNotifier>();
builder.Services.AddScoped<INotificationService, WhatsAppNotifier>();
```

### ونستخدمهم كـ `IEnumerable<INotificationService>`:

```csharp
public class NotificationController : Controller
{
    private readonly IEnumerable<INotificationService> _notifiers;

    public NotificationController(IEnumerable<INotificationService> notifiers)
    {
        _notifiers = notifiers;
    }

    public IActionResult NotifyUser()
    {
        foreach (var notifier in _notifiers)
        {
            notifier.Send("Message to user");
        }

        return Ok();
    }
}
```

 كده هيتبعت Email و WhatsApp تلقائيًا.

---

##  الطريقة التالتة: باستخدام Factory (لو عايز تختار حسب شرط معين)

```csharp
public interface INotificationFactory
{
    INotificationService GetNotifier(string channel);
}
```

###  تنفيذ الـ Factory:

```csharp
public class NotificationFactory : INotificationFactory
{
    private readonly IEnumerable<INotificationService> _notifiers;

    public NotificationFactory(IEnumerable<INotificationService> notifiers)
    {
        _notifiers = notifiers;
    }

    public INotificationService GetNotifier(string channel)
    {
        return _notifiers.FirstOrDefault(n => n.Channel == channel);
    }
}
```

### نسجّلها:

```csharp
builder.Services.AddScoped<INotificationService, EmailNotifier>();
builder.Services.AddScoped<INotificationService, WhatsAppNotifier>();
builder.Services.AddScoped<INotificationFactory, NotificationFactory>();
```

### ونستخدمها كده:

```csharp
public class NotificationController : Controller
{
    private readonly INotificationFactory _factory;

    public NotificationController(INotificationFactory factory)
    {
        _factory = factory;
    }

    public IActionResult NotifyUser()
    {
        var email = _factory.GetNotifier("Email");
        var whatsapp = _factory.GetNotifier("WhatsApp");

        email?.Send("Welcome Email");
        whatsapp?.Send("WhatsApp Message");

        return Ok();
    }
}
```

 كده بتختار الـ Implementation حسب اسم أو شرط runtime.

---
