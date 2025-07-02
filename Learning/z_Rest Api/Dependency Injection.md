
#### **1. مبدأ عكس التبعية (Dependency Inversion Principle)**

يعتمد **Dependency Injection** على مبدأ **عكس التبعية (Dependency Inversion Principle - DIP)**، وهو أحد مبادئ **SOLID** في البرمجة الكائنية التوجه **(OOP)**. ينص هذا المبدأ على:

1. **يجب ألا تعتمد الوحدات عالية المستوى (High-Level Modules) على الوحدات منخفضة المستوى (Low-Level Modules)، بل يجب أن تعتمدا كلتاهما على التجريد (Abstraction).**
2. **يجب ألا يعتمد التجريد (Abstraction) على التفاصيل (Details)، بل يجب أن تعتمد التفاصيل على التجريد.**

🔹 **تفسير المبدأ:**
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
