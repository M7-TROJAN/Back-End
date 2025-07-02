
# **S — Single Responsibility Principle (SRP)**

### "مبدأ المسؤولية الواحدة"

---

##  التعريف الرسمي:

- > **"A class should have only one reason to change."**
- > يعني: الكلاس أو الموديول يكون مسؤول عن شيء واحد فقط، ولو حصل تغيير في النظام، مفيش غير يكون سبب واحد بس الي يخلّي الكلاس ده يتغير.

---

##  المعنى ببساطة:

كل كلاس أو مكون في التطبيق لازم يكون عنده **وظيفة واحدة محددة بوضوح**.

لو الكلاس بيقوم بأكتر من وظيفة، يبقى هيتغيّر لأسباب مختلفة، وده بيخلي الكود:

* صعب يتفهم
* صعب يتعدل
* صعب يتعمله تيستنج

---

##  سيناريو الفاتورة:

تخيل إنك بتعمل برنامج لإدارة الفواتير. وكتبت كلاس اسمه `Invoice`.

```csharp
public class Invoice
{
    public void CalculateTotal() { }
    public void SaveToDatabase() { }
    public void PrintInvoice() { }
}
```

 إيه المشاكل هنا؟

* ال `CalculateTotal`: حساب الإجمالي → **مسؤولية Business Logic**
* ال `SaveToDatabase`: حفظ في قاعدة البيانات → **مسؤولية Data Access**
* ال `PrintInvoice`: طباعة → **مسؤولية Presentation/Output**

كل وظيفة من دول **بتنتمي لمجال مختلف**. لو حصل تعديل في طريقة الحساب، أو قاعدة البيانات، أو طريقة الطباعة، كلهم هيأثروا على نفس الكلاس… وده ضد SRP.

---

## الحل حسب SRP:

###  نقسم المسؤوليات:

```csharp
public class Invoice
{
    public void CalculateTotal() { }
}

public class InvoiceRepository
{
    public void Save(Invoice invoice) { }
}

public class InvoicePrinter
{
    public void Print(Invoice invoice) { }
}
```

* ال `Invoice` مسؤول عن منطق الفاتورة.
* ال `InvoiceRepository` مسؤول عن التخزين.
* ال `InvoicePrinter` مسؤول عن الطباعة.

 النتيجة: كل كلاس ممكن يتغير لسبب واحد فقط → سهل التعديل والاختبار والصيانة.

---

##  سيناريو: "UserManager"

```csharp
public class UserManager
{
    public void RegisterUser(string email, string password) { }
    public void SendWelcomeEmail(string email) { }
    public void LogActivity(string message) { }
}
```

 هنا الـ `UserManager` بيعمل:

* تسجيل المستخدم
* إرسال إيميل ترحيبي
* تسجيل اللوج

###  نطبّق SRP:

```csharp
public class UserService
{
    public void RegisterUser(string email, string password) { }
}

public class EmailService
{
    public void SendWelcomeEmail(string email) { }
}

public class Logger
{
    public void LogActivity(string message) { }
}
```

كدة كل كلاس ممكن يتغير لسبب واحد فقط:

* ال `UserService` لو منطق التسجيل اتغير
* ال `EmailService` لو نظام الإيميل اتغير
* ال `Logger` لو طريقة اللوج اتغيرت

---

## فوائد تطبيق SRP:

* سهولة الصيانة والتعديل.
* سهولة اختبار كل مكون لوحده (Unit Testing).
* تقليل الاعتماديات (Low Coupling).
* زيادة وضوح الكود.

---

## خطأ شائع:

فيه ناس بتبقي فاكرة ان ال SRP يعنى "الكلاس يكون صغير"، ولكن ده غلط.

 الصح:
**ال SRP بيتكلم عن سبب التغيير مش حجم الكلاس.**
الكلاس ممكن يكون كبير ويحقق SRP طالما بيعمل حاجة واحدةو ولو اتغير فيه حاجة هيبقي بسبب الحاجة الواحدة دي وبس
-  حط في الكلاس 1000 ميثود مش مهم مادام كلهم بيحققوا وبيخدموا نفس الحاجة الواحدة

---


كل ما تحس إن الكلاس "بيتكلم أكتر من لغة" أو "بيتعامل مع أكتر من نوع كود"، اسأل نفسك:

> هل فيه أكتر من سبب ممكن يخليني أغير الكلاس ده؟

لو الإجابة "اه" → يبقي انت غالبًا بتكسر SRP.

---



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


# **L — Liskov Substitution Principle (LSP)**

### "مبدأ استبدال ليسكوف"
وهو من أهم المبادئ اللي بتمنع الكوارث البرمجية اللي بتحصل لما تشتغل بـ Inheritance من غير ما تكون فاهم كويس.
---

## التعريف الرسمي:

> **“Objects of a superclass should be replaceable with objects of its subclasses without breaking the behavior of the program.”**

يعني إيه؟

> أي كود بيشتغل بكائن من الكلاس الأساسي (Base class)، المفروض يشتغل **بنفس الطريقة** لو استبدلناه بكائن من كلاس فرعي (Derived class).

---

## الفكرة ببساطة:

لما تورّث كلاس من كلاس تاني، لازم تتأكد إن **الكلاس الابن يقدر يحل مكان الأب** من غير ما "يبوّظ" سلوك التطبيق.

يعني ما ينفعش تدي كائن من `Dog` في مكان محتاج `Animal`، ويفاجئك إنه مابيهوهوش، أو بيرفرف زي العصفور!

---

## لو كسرت المبدأ ده، تحصل مصايب زي:

* ال **Runtime bugs** مش واضحة من الكود.
* **سلوك غير متوقع** في الأماكن اللي بتستخدم الـ base class.
* **اختبارات Unit Tests بتبوظ فجأة** لما تضيف Class جديد.

---

## مثال بسيط (بيكسر LSP):

### تخيل إن عندك كلاس للمستطيل:

```csharp
public class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }

    public int Area()
    {
        return Width * Height;
    }
}
```

### وقررت تعمل كلاس `Square` يورّث من `Rectangle`:

```csharp
public class Square : Rectangle
{
    public override int Width
    {
        set
        {
            base.Width = value;
            base.Height = value;
        }
    }

    public override int Height
    {
        set
        {
            base.Width = value;
            base.Height = value;
        }
    }
}
```

### تفتكر الكود ده هيشتغل كويس؟

لأ، لأنك لو كتبت كود بيستخدم `Rectangle`:

```csharp
Rectangle r = new Square();
r.Width = 5;
r.Height = 10;

Console.WriteLine(r.Area()); // الناتج؟ مش 50، لأنه هيحسب 10x10!
```

هنا حصل خرق لمبدأ LSP. ليه؟

* لأنك استخدمت كائن `Square` في مكان متوقع `Rectangle`، لكن السلوك اتغير بشكل خفي.

---

##  الحل: لا تورّث لما يكون السلوك مختلف كليًا!

لو `Square` سلوكها مختلف عن `Rectangle`، فـ **ماينفعش تورّثها منه**.

بدل كده، اعمل Interface زي:

```csharp
public interface IShape
{
    int Area();
}
```

وكل كلاس (مربع، مستطيل، دائرة...) يطبّق الـ Interface بالسلوك الخاص بيه.

---

## سيناريو حقيقي الطباعة (Printer)

### الكلاس الأساسي:

```csharp
public class DocumentPrinter
{
    public virtual void Print(Document doc)
    {
        Console.WriteLine("Printing document...");
    }
}
```

### كلاس جديد: `ReadOnlyPrinter`

```csharp
public class ReadOnlyPrinter : DocumentPrinter
{
    public override void Print(Document doc)
    {
        throw new NotSupportedException("This printer doesn't support printing");
    }
}
```

 كده كأنك بتقول:

> "أنا ورّثت كلاس، بس استخدمه غلط وهيكسر البرنامج"

### الحل؟

* ما تورّثش.
* خليه يطبّق Interface تاني، أو استخدم ** ال Composition** بدل Inheritance.

---

## إزاي أعرف إني كسرت مبدأ LSP؟

اسأل نفسك:

> لو استبدلت الـ base class بالـ derived class، هل سلوك البرنامج يفضل هو هو؟

لو لأ:
 إنت كده كسرت مبدأ LSP.

---

##  ملخص القاعدة الذهبية:

> **Don't override behavior to break assumptions of the base class.**

 الكلاس الابن لازم يلتزم بكل شروط وسلوكيات الكلاس الأب.
 ماينفعش يعدّل في السلوك الأساسي بطريقة غير متوقعة.

---

##  إزاي أطبق LSP صح في شغلي؟

| غلط                                 | صح                                           |
| ----------------------------------- | -------------------------------------------- |
| تورّث من كلاس وتكسر وظائفه          | ورّث بس لما تكون فعلاً بتوسّع مش بتكسر       |
| تعرّف كلاس ابن يغيّر السلوك الأساسي | خليه يطبّق Interface منفصل لو سلوكه مختلف    |
| تورّث كلاس عام لكل حاجة             | فكّر في Interface أو Composition بدل التوريث |

---

## مثال Web واقعي:

### عندك:

```csharp
public abstract class PaymentMethod
{
    public abstract void Pay(decimal amount);
}
```

### وبتورّث:

```csharp
public class Visa : PaymentMethod
{
    public override void Pay(decimal amount) => Console.WriteLine("Paid with Visa");
}

public class Cash : PaymentMethod
{
    public override void Pay(decimal amount) => Console.WriteLine("Paid with Cash");
}

public class Installment : PaymentMethod
{
    public override void Pay(decimal amount)
    {
        throw new NotSupportedException("Installments not supported yet");
    }
}
```

هنا فيه خرق للمبدأ.
 الحل؟ خليه Interface مختلف، أو اعمل Strategy منفصلة:

```csharp
public interface IPaymentStrategy
{
    void Pay(decimal amount);
}
```
---




** طب هو فين الخرق في مبدأ Liskov Substitution Principle (LSP)** في المثال ده ؟.

---
```csharp
public abstract class PaymentMethod
{
    public abstract void Pay(decimal amount);
}
```

```csharp
public class Visa : PaymentMethod
{
    public override void Pay(decimal amount) => Console.WriteLine("Paid with Visa");
}

public class Cash : PaymentMethod
{
    public override void Pay(decimal amount) => Console.WriteLine("Paid with Cash");
}

public class Installment : PaymentMethod
{
    public override void Pay(decimal amount)
    {
        throw new NotSupportedException("Installments not supported yet");
    }
}
```
---

## نراجع مع بعض تعريف مبدأ LSP تاني:

> **"You should be able to replace an instance of the base class with any of its derived classes without altering the correctness of the program."**

يعني:

> أي كائن من الكلاس الفرعي (مثل `Visa`, `Cash`, `Installment`) المفروض يشتغل **بنفس طريقة الكلاس الأساسي `PaymentMethod`** ومن غير ما يكسر أو يغير سلوك التطبيق أو يرمي Exception غير متوقع.

---

##  إيه اللي حصل في الكود بتاعنا ؟:

أنت عامل كلاس `Installment` بيورّث من `PaymentMethod`.

بس جوّا `Installment`:

```csharp
public override void Pay(decimal amount)
{
    throw new NotSupportedException("Installments not supported yet");
}
```

يعني أنت **كأنك بتقول: "أنا ورّثت من الأب، لكن مش قادر أحقق وظيفته".**

---

## طيب فين المشكلة عمليًا؟

تخيل إن عندك كود بيشتغل مع `PaymentMethod`:

```csharp
public void ProcessPayment(PaymentMethod payment, decimal amount)
{
    payment.Pay(amount);
}
```

وانت متوقع إنه هيشتغل مع **أي نوع من أنواع الدفع**.

بس لو استدعيت الكود كده:

```csharp
PaymentMethod payment = new Installment();
ProcessPayment(payment, 100);
```

 يحصل إيه؟
 يرمي Exception لأن `Installment.Pay()` مش مدعومة.

يعني دلوقتي:

* كودك ما بقاش آمن.
* مبدأ **Polymorphism** اتكسر.
* المبدأ نفسه (LSP) اتكسر، لأنك ماعدتش تقدر تستبدل `PaymentMethod` بـ `Installment`.

---

## إزاي نحل ده ونلتزم بـ LSP؟

### الحل 1: ما تورّثش من الكلاس لو مش قادر تنفّذ وظيفته بالكامل

لو `Installment` مش قادر ينفّذ `Pay()`، يبقى ماينفعش يكون `PaymentMethod`.

### الحل 2: فصل الوظائف باستخدام Interfaces (وده الأفضل):

```csharp
public interface IPaymentMethod
{
    void Pay(decimal amount);
}
```

وبعدين:

```csharp
public class Visa : IPaymentMethod
{
    public void Pay(decimal amount) => Console.WriteLine("Paid with Visa");
}

public class Cash : IPaymentMethod
{
    public void Pay(decimal amount) => Console.WriteLine("Paid with Cash");
}
```

وبدل ما تعمل كلاس `Installment` دلوقتي، ممكن تستناه لما تكون جاهز تطبّق `Pay()` فعلاً.

أو تعمل كلاس مستقل مالوش علاقة بالـ Interface:

```csharp
public class InstallmentProposal
{
    public void CreateProposal(decimal amount)
    {
        // Generate a plan, but no actual payment
    }
}
```

---

## طيب، لو في المستقبل حبيت تدعم الأقساط؟

وقتها تقدر تخلي `Installment` يورّث من `IPaymentMethod` **بس بعد ما تطبّق `Pay()` بشكل حقيقي.**

---

## خلاصة:

| ❌ الكود الحالي فيه                                                    | ✅ الحل                                     |
| --------------------------------------------------------------------- | ------------------------------------------ |
| كائن فرعي (`Installment`) بيكسر وعود الكلاس الأساسي (`PaymentMethod`) | ما تورّثش لو مش هتطبّق الوظيفة بالكامل     |
| كودك بيرمي استثناء في سيناريو متوقع يشتغل فيه                         | استخدم Interface منفصل لكل سلوك            |
| خرق واضح لمبدأ LSP                                                    | طبّق مبادئ الـ Interface Segregation + LSP |

---



# **I — Interface Segregation Principle (ISP)**

### "مبدأ فصل الواجهات"

---

## التعريف الرسمي:

> **"Clients should not be forced to depend on interfaces they do not use."**

بمعنى:

> ماينفعش تجبر الكلاسات إنها تطبّق حاجات في Interface **مش محتاجينها**.

---

## المعنى ببساطة:

* خلي كل Interface مسؤول عن **وظيفة صغيرة ومحددة**.
* ماتحطش وظائف كتير في Interface واحد.
* الكلاس المفروض يطبّق **فقط** الحاجات اللي هو محتاجها.

---

## الهدف:

* منع الـ **Fat Interfaces** (اللي فيها كل حاجة ومالهاش لازمة).
* تقليل الـ **coupling** بين الكلاسات والأنظمة.
* تسهيل التعديل والصيانة والتستنج.

---

## مثال بيكسر ISP:

تخيل عندك Interface عام لكل أنواع الطابعات:

```csharp
public interface IPrinter
{
    void Print(Document doc);
    void Scan(Document doc);
    void Fax(Document doc);
}
```

### الكلاسات اللي هتطبقه:

```csharp
public class MultiFunctionPrinter : IPrinter
{
    public void Print(Document doc) { /* OK */ }
    public void Scan(Document doc) { /* OK */ }
    public void Fax(Document doc) { /* OK */ }
}

public class BasicPrinter : IPrinter
{
    public void Print(Document doc) { /* OK */ }
    public void Scan(Document doc) { throw new NotImplementedException(); }
    public void Fax(Document doc) { throw new NotImplementedException(); }
}
```

 هنا حصل خرق للمبدأ، لأن `BasicPrinter` مجبر يطبّق حاجات مش بيستخدمها (scan/fax).
وده ضد ISP.

---

## الحل: نفصل الـ Interface لمجموعة من الـ Interfaces الصغيرة

```csharp
public interface IPrinter
{
    void Print(Document doc);
}

public interface IScanner
{
    void Scan(Document doc);
}

public interface IFax
{
    void Fax(Document doc);
}
```

### وبعدين نطبّق اللي محتاجينه بس:

```csharp
public class BasicPrinter : IPrinter
{
    public void Print(Document doc) { /* OK */ }
}

public class MultiFunctionPrinter : IPrinter, IScanner, IFax
{
    public void Print(Document doc) { /* OK */ }
    public void Scan(Document doc) { /* OK */ }
    public void Fax(Document doc) { /* OK */ }
}
```
 كده:

* كل كلاس بيطبّق بس اللي محتاجه.
* لو حصل تغيير في `IFax` مش هيأثر على الكلاسات اللي مش بتفعلها.

---

## سيناريو حقيقي في Web Application

### تخيل Interface كبير كده:

```csharp
public interface IUserService
{
    void Register();
    void Login();
    void BanUser();
    void SendVerificationEmail();
    void ResetPassword();
}
```

 المستخدم العادي هيستخدم بس:

* `Register`, `Login`, `SendVerificationEmail`, `ResetPassword`

لكن Admin بس هو اللي بيستخدم `BanUser`

 يعني الكلاسات Client (زي Controllers) بتضطر تعتمد على حاجات ملهاش علاقة بيها.

---

##  الحل: فصل الواجهات

```csharp
public interface IUserAuthService
{
    void Register();
    void Login();
    void ResetPassword();
    void SendVerificationEmail();
}

public interface IUserAdminService
{
    void BanUser();
}
```

* ال Controller الخاص باليوزر العادي هيستخدم `IUserAuthService`
* ال Controller الخاص بالـ Admin هيستخدم `IUserAdminService`

 كل واحد واخد بس اللي محتاجه.

---

## فاكر كنا قولنا في مبدأ SRP:

> "كل كلاس يكون عنده مسؤولية واحدة"

هنا بقى في ISP بنقول:

> "كل **Interface** يكون عنده **مسؤولية واحدة** برضو"

---

## تحذير من Code Smell:

لما تلاقي Interface فيه أكتر من 5-6 دوال مش مرتبطة ببعض...
وقّف نفسك واسأل:

> "هل أنا كده بكسر ISP؟ هل في كلاس ممكن مايحتاجش كل ده؟"

---

## في ASP.NET Core:

كتير بنلاقي Service Interface فيها حاجات كتير:

```csharp
public interface ILibraryService
{
    void BorrowBook();
    void ReturnBook();
    void AddBook();
    void DeleteBook();
    void GenerateReport();
    void SendReminder();
}
```

 كده الكلاسات اللي بتتعامل مع استعارة الكتب هتضطر تشوف حاجات ملهاش لازمة.

### الحل:

* `IBorrowingService`
* `IBookAdminService`
* `IReportService`
* `IReminderService`

وكل Controller ياخد الـ Interface اللي يخصه بس.

---

## خلاصة Interface Segregation Principle:

| ❌ خطأ                                           | ✅ صح                                     |
| ----------------------------------------------- | ---------------------------------------- |
| Interface كبير يحتوي على دوال مالهاش علاقة ببعض | تقسيم الـ Interface لواجهات صغيرة متخصصة |
| إجبار كلاس يطبّق دوال مش محتاجها                | الكلاس يطبّق بس اللي يخصه                |
| صعوبة التعديل                                   | سهل تفصل وتختبر كل جزء لوحده             |
| Code Smell: Fat Interface                       | Clean Design                             |

---

