
#### **1. مبدأ عكس التبعية (Dependency Inversion Principle)**

يعتمد **Dependency Injection** على مبدأ **عكس التبعية (Dependency Inversion Principle - DIP)**، وهو أحد مبادئ **SOLID** في البرمجة الكائنية التوجه **(OOP)**. ينص هذا المبدأ على:

1. **يجب ألا تعتمد الوحدات عالية المستوى (High-Level Modules) على الوحدات منخفضة المستوى (Low-Level Modules)، بل يجب أن تعتمدا كلتاهما على التجريد (Abstraction).**
2. **يجب ألا يعتمد التجريد (Abstraction) على التفاصيل (Details)، بل يجب أن تعتمد التفاصيل على التجريد.**

 **تفسير المبدأ:**
بدلاً من أن يعتمد الكود بشكل مباشر على كائنات معينة، فإنه يعتمد على **واجهات (Interfaces) أو كائنات مجردة (Abstract Classes)**، مما يسمح بالتغيير بسهولة دون التأثير على الأجزاء الأخرى من التطبيق.

#### **2. كيف يتم تطبيق Dependency Injection؟**

عند استخدام **Dependency Injection**، يتم استبدال التبعيات المباشرة بين المكونات، مثل:

```csharp
public class BooksController
{
    private readonly BooksService _booksService;

    public BooksController()
    {
        _booksService = new BooksService(); // مشكلة: يعتمد على كائن محدد
    }
}
```

🔹 **هذه الطريقة تسبب تداخلًا قويًا (Tight Coupling)، مما يجعل الكود أقل مرونة وصعب الاختبار.**

 **بدلًا من ذلك، يمكن استخدام DI عن طريق تمرير التبعيات عبر الواجهات:**

```csharp
public class BooksController
{
    private readonly IBooksService _booksService;

    public BooksController(IBooksService booksService)
    {
        _booksService = booksService; // استخدام واجهة يجعل الكود أكثر مرونة
    }
}
```

بهذه الطريقة، يمكننا استبدال `BooksService` بأي تنفيذ آخر مثل `FakeBooksService` أثناء الاختبار دون تغيير الكود داخل `BooksController`.

---

### **3. أنواع Service Lifetime في Dependency Injection**

في **ASP.NET Core**، عند تسجيل الخدمات داخل **Container** الخاص بـ Dependency Injection، يمكن تحديد نوع "عمر الخدمة" **(Service Lifetime)**، وهناك ثلاث أنواع رئيسية:

| **النوع**     | **الوصف**                                                                                  |
| ------------- | ------------------------------------------------------------------------------------------ |
| **Transient** | يتم إنشاء كائن جديد في كل مرة يتم فيها طلبه.                                               |
| **Scoped**    | يتم إنشاء كائن واحد فقط لكل **طلب (Request)** ويب واحد، ويُعاد استخدامه داخل نفس الطلب.    |
| **Singleton** | يتم إنشاء كائن واحد فقط طوال فترة تشغيل التطبيق، ويُستخدم في جميع الأماكن التي تحتاج إليه. |

---

#### **4. الفرق بين Service Lifetime الثلاثة في ASP.NET Core**

🔹 **Transient:**

* يتم إنشاء كائن جديد **في كل مرة** يتم فيها استدعاء الخدمة.
* يُفضل استخدامه مع الخدمات التي لا تحتاج إلى مشاركة الحالة بين الطلبات.

 **مثال:**

```csharp
services.AddTransient<IBooksService, BooksService>();
```

---

🔹 **Scoped:**

* يتم إنشاء كائن واحد فقط لكل **طلب (Request)**.
* مثالي عند التعامل مع **قاعدة البيانات**، حيث يساعد في استخدام نفس الكائن داخل الطلب.

 **مثال:**

```csharp
services.AddScoped<IBooksService, BooksService>();
```

---

 **Singleton:**

* يتم إنشاء كائن واحد فقط **لجميع الطلبات** ويستمر طوال فترة تشغيل التطبيق.
* مناسب لاستخدام الكائنات التي تحتاج إلى **حالة مشتركة** مثل **ذاكرة التخزين المؤقت (Cache)، خدمات التسجيل (Logging)**.

 **مثال:**

```csharp
services.AddSingleton<IBooksService, BooksService>();
```

---

### **5. متى نستخدم كل نوع؟**

 **Transient:**

* إذا كنت بحاجة إلى كائن جديد في كل مرة يتم استدعاء الخدمة.
* مناسب للخدمات الخفيفة **(Lightweight Services)** التي لا تعتمد على تخزين بيانات طويلة الأمد.

 **Scoped:**

* إذا كنت تحتاج إلى **إعادة استخدام نفس الكائن داخل نفس الطلب فقط**.
* مثالي لخدمات التعامل مع **قاعدة البيانات**.

 **Singleton:**

* عندما تحتاج إلى **كائن مشترك عبر التطبيق بالكامل**.
* مناسب **للكاش (Caching)، الـ Logging، وإدارة الإعدادات (Configurations).**

---

### **6. تطبيق عملي في ASP.NET Core**

#### **خطوات تسجيل واستخدام Dependency Injection في ASP.NET Core**

 **إضافة الخدمات داخل `Program.cs`**:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBooksService, BooksService>();

var app = builder.Build();
```

 **استخدام الخدمة داخل `BooksController`**:

```csharp
[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBooksService _booksService;

    public BooksController(IBooksService booksService)
    {
        _booksService = booksService;
    }

    [HttpGet]
    public IActionResult GetBooks()
    {
        var books = _booksService.GetAllBooks();
        return Ok(books);
    }
}
```

---

### **7. خلاصة**

* ال **Dependency Injection** يسمح بفصل التبعيات بين المكونات، مما يجعل الكود **أكثر مرونة وقابلية للاختبار**.
* يعتمد على مبدأ **عكس التبعية (DIP)** الذي يفضل الاعتماد على **الواجهات** بدلاً من الكائنات المحددة.
* هناك **ثلاثة أنواع من Service Lifetime**:

  * ال **Transient:** كائن جديد في كل مرة.
  * ال **Scoped:** كائن واحد لكل طلب.
  * ال **Singleton:** كائن واحد طوال فترة تشغيل التطبيق.
* ال **ASP.NET Core** يحتوي على **Container مدمج** لإدارة التبعيات بسهولة.


---



## الفرق العميق بين Transient / Scoped / Singleton مش بس في "العُمر"، لكن كمان في **سلوك الكائن** وسيناريوهات الـ Web

---

### ال Transient:  يعني كل مرة نسخة جديدة

* بيتم إنشاء **instance جديد في كل مرة** يطلب فيه الـ Container الخدمة.
* يعني لو Injectت `IBookService` في 3 أماكن داخل نفس الـ Controller، هتاخد 3 نسخ مختلفة.

 **مثالي لـ:**

* الخدمات الخفيفة (Lightweight)، اللي مش بتخزن state.
* الخدمات الحسابية (stateless operations) زي `ICalculatorService`.
* `ValidationService`, `EmailTemplateFormatter`.

 **مساوئه:**

* ممكن يستهلك موارد أكتر لأن كل مرة بيعمل new.
* لو فيه dependency داخلي (مثلاً Scoped)، يحصل مشاكل لو Injectته بشكل مباشر.

---

###  ال Scoped: يعني نسخة واحدة لكل Request

* بيعمل instance جديد **لكل HTTP Request**.
* داخل نفس الـ Request، كل اللي يطلب الخدمة هياخد نفس الـ instance.

**مثالي لـ:**

* التعامل مع قاعدة البيانات باستخدام `DbContext`.
* خدمات الـ Business logic اللي بتحتاج تشارك بيانات بين أجزاء الـ Request.
* `IBooksService`، `ICartService`.

 لو استخدمته في Singleton، يحصل **خطأ خطير**:

> `Cannot consume scoped service from singleton`

---

### ال  Singleton: نسخة واحدة فقط طول وقت تشغيل التطبيق

* بينشئ instance **مرة واحدة بس عند أول استخدام**.
* كل الأماكن اللي تطلبها هتاخد نفس النسخة.

 **مثالي لـ:**

* الكاش `ICacheService`.
* تسجيل الدخول `ILoggerService`.
* التحميل المسبق للإعدادات `IAppConfigService`.

⚠ انتبه:

* ممنوع تضيف فيه Scoped Services.
* مينفعش تستخدمه لو فيه state بيتغير لكل يوزر.

---

##  مثال حي داخل ASP.NET Core Request:

نفترض عندنا:

```csharp
services.AddScoped<IUserService, UserService>();
services.AddSingleton<ILoggerService, MyLogger>();
```

لو `UserService` محتاج `ILoggerService`، ده تمام.

لكن لو العكس، يعني:

```csharp
public class MyLogger : ILoggerService
{
    public MyLogger(IUserService userService) { } // ❌ Error
}
```

ده **كسر واضح لقواعد الـ Lifetime**:

* ال Singleton محتاج Scoped → مش مسموح.
* ليه؟ لأن Singleton بيعيش للأبد، لكن Scoped بينتهي مع كل Request.

---

##  ملخص العلاقة بينهم:

| Injected Into | Transient | Scoped | Singleton |
| ------------- | --------- | ------ | --------- |
| Transient     | ✅         | ✅      | ✅         |
| Scoped        | ✅         | ✅      | ✅         |
| Singleton     | ✅         | ❌      | ✅         |

---

##  سيناريوهات واقعية تساعدك تقرر:

| الحالة                           | النوع الأنسب | السبب                                 |
| -------------------------------- | ------------ | ------------------------------------- |
| حساب ناتج رياضي أو معالجة مؤقتة  | Transient    | لا يحتاج حفظ الحالة                   |
| إدارة مستخدم حالي في Request     | Scoped       | كل Request ليه مستخدم خاص             |
| خدمة إعدادات التطبيق العامة      | Singleton    | نفس الإعدادات لكل التطبيق             |
| الوصول لـ DB أو Entity Framework | Scoped       | عشان تربط كل العمليات بنفس السياق     |
| تسجيل الأحداث / Logging          | Singleton    | أداء أعلى، ولا يحتاج لكل Request نسخة |

---

##  ملاحظات مهمة جدًا:

* لو كتبت كلاس فيه `Guid` داخل Constructor علشان تراقب سلوك الـ Lifetime:

```csharp
public class BookService : IBookService
{
    private readonly Guid _id = Guid.NewGuid();
    public Guid GetId() => _id;
}
```

* واستخدمت `IBookService` في `Controller`، وشغلت التطبيق:

  * في Transient: كل مرة ID مختلف
  * في Scoped: نفس ID داخل نفس الـ Request
  * في Singleton: نفس ID طول عمر التطبيق

---

##  ازاي تفكر وتقرر تستخدم أنهي Lifetime؟

1. **هل الخدمة فيها حالة ثابتة؟** → Singleton
2. **هل الخدمة فيها بيانات تتغير في كل Request؟** → Scoped
3. **هل الخدمة خفيفة، لا تحتاج أي State؟** → Transient
4. **هل Injected dependencies فيها Scoped؟** → لازم تبعد عن Singleton
5. **هل محتاج تشارك البيانات بين أجزاء الـ Request؟** → Scoped
6. **هل الأداء مهم جدًا والتكلفة عالية؟** → Singleton

---

## يعني إيه Dependency Injection؟

** ال Dependency Injection** هو أسلوب (design pattern) بنستخدمه في البرمجة عشان نفصل الكلاسات عن بعض، ونخلي الكلاس يعتمد على **واجهات (Interfaces)** بدل ما ينشئ بنفسه كائنات من الكلاسات التانية.

---

### مثال بسيط جدًا:

عندك كلاس اسمه `OrderService` محتاج يطبع فواتير. بدل ما يعمل `new Printer()` جواه، بنخلي الـ Printer يتبعتله من برّه:

```csharp
public interface IPrinter
{
    void Print(string message);
}

public class ConsolePrinter : IPrinter
{
    public void Print(string message)
    {
        Console.WriteLine(message);
    }
}

public class OrderService
{
    private readonly IPrinter _printer;

    public OrderService(IPrinter printer)
    {
        _printer = printer;
    }

    public void ProcessOrder()
    {
        // Process logic...
        _printer.Print("Order processed successfully.");
    }
}
```

### تحليل الكود:

| الكود                      | معناه                                                               |
| -------------------------- | ------------------------------------------------------------------- |
| `IPrinter`                 | واجهة تمثل الطابعة، مش شرط تكون Console ممكن تبقى Email أو PDF      |
| `ConsolePrinter`           | تطبيق حقيقي للواجهة                                                 |
| `OrderService`             | بيعتمد على `IPrinter` بدل ما ينشئه بنفسه                            |
| `public OrderService(...)` | هنا بنطبق مفهوم **Constructor Injection** – أهم وأشهر نوع من الـ DI |

---

##  ليه نستخدم Dependency Injection؟

| الفائدة                    | الشرح                                                                  |
| -------------------------- | ---------------------------------------------------------------------- |
| ❌ تفادي الـ tight coupling | الكلاسات مش مربوطة ببعض بشكل مباشر                                     |
| ✅ سهولة الـ Testing        | تقدر تبعت Mock Objects بسهولة في الـ Unit Tests                        |
| 🔁 سهولة التبديل           | ممكن تغيّر من `ConsolePrinter` لـ `EmailPrinter` من غير ما تغيّر الكود |
| ♻️ إدارة عمر الكائنات      | باستخدام Service Lifetimes (هنتكلم عنها دلوقتي)                        |

---

## ثالثًا: تطبيق Dependency Injection في .NET Core

في .NET Core، فيه built-in **Dependency Injection Container**.

### هيكل التطبيق:

```plaintext
- Interfaces/
   - IPrinter.cs
- Services/
   - ConsolePrinter.cs
   - OrderService.cs
- Program.cs
```

### Program.cs (.NET 6/7/8/9):

```csharp
builder.Services.AddScoped<IPrinter, ConsolePrinter>();
builder.Services.AddScoped<OrderService>();
```

### استدعاء الخدمة في Controller:

```csharp
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public IActionResult CreateOrder()
    {
        _orderService.ProcessOrder();
        return Ok("Order created.");
    }
}
```

---

## رابعًا: أنواع Service Lifetimes

| النوع       | بيعيش قد إيه؟                          | الاستخدام الأنسب                                       |
| ----------- | -------------------------------------- | ------------------------------------------------------ |
| `Transient` | بيتعمل Instance جديد مع كل استخدام     | لو الخدمة خفيفة ومفيش state – زي Validation أو Logging |
| `Scoped`    | بيعيش طول عمر الـ HTTP Request الواحد  | لو فيه عمليات مرتبطة بالـ Request زي الـ DbContext     |
| `Singleton` | بيتعمل مرة واحدة ويعيش طول عمر التطبيق | لما يكون فيه Cache أو خدمة ثابتة لكل الـ Requests      |

---

### مثال تطبيقي عملي على الفرق بينهم:

```csharp
public interface IGuidService
{
    string GetGuid();
}

public class GuidService : IGuidService
{
    private readonly string _guid = Guid.NewGuid().ToString();
    public string GetGuid() => _guid;
}
```

وسجّل الخدمات:

```csharp
builder.Services.AddTransient<IGuidService, GuidService>();   // جربها مع Scoped و Singleton كمان
```

واستخدمها في كنترولر:

```csharp
[ApiController]
[Route("api/[controller]")]
public class DemoController : ControllerBase
{
    private readonly IGuidService _guidService1;
    private readonly IGuidService _guidService2;

    public DemoController(IGuidService guidService1, IGuidService guidService2)
    {
        _guidService1 = guidService1;
        _guidService2 = guidService2;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            First = _guidService1.GetGuid(),
            Second = _guidService2.GetGuid()
        });
    }
}
```

### النتيجة هتبين فرق الـ Lifetime:

| النوع     | النتيجة                                |
| --------- | -------------------------------------- |
| Transient | قيمتين مختلفتين كل مرة                 |
| Scoped    | نفس القيمة داخل نفس الـ Request        |
| Singleton | نفس القيمة حتى بعد إعادة تشغيل المتصفح |

---

## إمتى أستخدم كل نوع؟

| الحالة                               | النوع المناسب        |
| ------------------------------------ | -------------------- |
| Logging, Lightweight Helper Classes  | `Transient`          |
| Services بتتعامل مع البيانات (Db...) | `Scoped` ✅ الافتراضي |
| Cache, Configuration, HttpClient     | `Singleton`          |

---

## خامسًا: أنواع Injection في .NET

| النوع                     | الشرح                                                              |
| ------------------------- | ------------------------------------------------------------------ |
| **Constructor Injection** | أكثر الأنواع شيوعًا – بنمرر الـ dependency عن طريق الـ constructor |
| **Method Injection**      | نمرر الـ dependency كـ parameter في ميثود معينة فقط                |
| **Property Injection**    | نستخدم خاصية `public set` لإدخال الـ dependency                    |

---

## هل ممكن Inject خدمة جوه خدمة؟

أكيد! وده بيحصل دايمًا:

```csharp
public class OrderService
{
    private readonly IPrinter _printer;
    private readonly IEmailSender _emailSender;

    public OrderService(IPrinter printer, IEmailSender emailSender)
    {
        _printer = printer;
        _emailSender = emailSender;
    }

    public void ProcessOrder()
    {
        _printer.Print("Processing...");
        _emailSender.Send("order@example.com", "Order processed.");
    }
}
```

بس لازم تسجل كل الخدمات في `Program.cs`.

---

## تلخيص سريع:

| العنصر             | النقطة الرئيسية                                                      |
| ------------------ | -------------------------------------------------------------------- |
| DI                 | بيفصل الكلاسات عن بعض – بنحقن Dependencies بدل ما نعملها جوه الكلاس  |
| Lifetimes          | Transient = جديد كل مرة، Scoped = جديد لكل Request، Singleton = ثابت |
| Injection Methods  | الأفضل دايمًا Constructor Injection                                  |
| Service to Service | ممكن Inject أي خدمة جوه التانية بسهولة                               |
| Testing            | تسهّل الـ Unit Testing جدًا باستخدام Mocks                           |

---
