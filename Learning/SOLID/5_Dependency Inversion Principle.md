
# **D — Dependency Inversion Principle (DIP)**

### "مبدأ قلب الاعتمادية"

---

## التعريف الرسمي:

> **"High-level modules should not depend on low-level modules. Both should depend on abstractions. Abstractions should not depend on details. Details should depend on abstractions."**

---

## المعنى ببساطة:

* **المستوى العالي** (High-level module): كود بيمثل منطق التطبيق (زي Services، Controllers).
* **المستوى المنخفض** (Low-level module): كود بيتعامل مع التفاصيل (زي قواعد البيانات، APIs، Email).

### المشكلة:

المستوى العالي بيرتبط بالمستوى المنخفض **بشكل مباشر**.

### الحل:

نخلي **اللي اتنين** (العالي والمنخفض) يعتمدوا على \*\* abstraction (interface)\*\*، مش على بعض.

---

## مثال عملي قبل ما نطبّق المبدأ:

### كود بيكسر المبدأ:

```csharp
public class UserService
{
    private readonly EmailSender _emailSender;

    public UserService()
    {
        _emailSender = new EmailSender(); // ❌ ارتباط مباشر بكلاس التفاصيل
    }

    public void Register(string email)
    {
        // Save user in DB ...
        _emailSender.Send(email, "Welcome!");
    }
}
```

### كلاس `EmailSender`:

```csharp
public class EmailSender
{
    public void Send(string to, string message)
    {
        Console.WriteLine("Sending email to " + to);
    }
}
```

 المشكلة هنا:

* `UserService` مش ممكن تشتغل من غير `EmailSender`.
* لو حبيت تغيّر طريقة الإرسال (مثلاً تستخدم Twilio أو SendGrid)، هتضطر **تعدل في `UserService` نفسه**.

وده كسر:

* ال  **OCP** (Open/Closed)
* ال **DIP** (اعتماد عالي المستوى على التفاصيل).

---

## الحل الصحيح باستخدام DIP:

### 1. نبدأ بإنشاء abstraction (interface):

```csharp
public interface IMessageSender
{
    void Send(string to, string message);
}
```

### 2. نخلّي `EmailSender` يطبّق الـ Interface:

```csharp
public class EmailSender : IMessageSender
{
    public void Send(string to, string message)
    {
        Console.WriteLine("Email to " + to + ": " + message);
    }
}
```

### 3. نغيّر `UserService` عشان يعتمد على الـ abstraction:

```csharp
public class UserService
{
    private readonly IMessageSender _messageSender;

    public UserService(IMessageSender messageSender)
    {
        _messageSender = messageSender;
    }

    public void Register(string email)
    {
        // Save user...
        _messageSender.Send(email, "Welcome!");
    }
}
```

---

## العلاقة الجديدة (بعد تطبيق DIP):

```
UserService      -->        IMessageSender       <--      EmailSender
(High-level)                (Abstraction)                  (Low-level)
```

* دلوقتي `UserService` مش مهتم بـ التفاصيل.
* ممكن تغير `EmailSender` إلى `WhatsAppSender` أو `SMSService` من غير ما تلمس `UserService`.

---

##  وده بيتحقق تلقائي في ASP.NET Core عن طريق Dependency Injection:

### في `Program.cs`:

```csharp
builder.Services.AddScoped<IMessageSender, EmailSender>();
builder.Services.AddScoped<UserService>();
```

### ال ASP.NET Core بتعمل inject  ل `EmailSender` تلقائي في `UserService`.

---

## سيناريو Web Application كامل:

### عندك كلاس `OrderService` بيعمل طلب شراء، وبيستخدم Logger:

#### الطريقة الغلط:

```csharp
public class OrderService
{
    private readonly FileLogger _logger;

    public OrderService()
    {
        _logger = new FileLogger(); // ❌ اعتماد مباشر
    }

    public void PlaceOrder()
    {
        _logger.Log("Order placed");
    }
}
```

#### ✅ الحل:

1. Interface:

```csharp
public interface ILoggerService
{
    void Log(string message);
}
```

2. Implementations:

```csharp
public class FileLogger : ILoggerService
{
    public void Log(string message)
    {
        File.AppendAllText("log.txt", message + Environment.NewLine);
    }
}

public class ConsoleLogger : ILoggerService
{
    public void Log(string message)
    {
        Console.WriteLine("[LOG] " + message);
    }
}
```

3. `OrderService`:

```csharp
public class OrderService
{
    private readonly ILoggerService _logger;

    public OrderService(ILoggerService logger)
    {
        _logger = logger;
    }

    public void PlaceOrder()
    {
        _logger.Log("Order placed successfully.");
    }
}
```

4. `Program.cs`:

```csharp
builder.Services.AddScoped<ILoggerService, FileLogger>();
builder.Services.AddScoped<OrderService>();
```

---

##  مقارنة قبل وبعد:

| قبل DIP                              | بعد DIP                       |
| ------------------------------------ | ----------------------------- |
| الكلاس بيعتمد على كلاس تاني مباشر    | الكلاس بيعتمد على abstraction |
| تغييرات التفاصيل بتكسر الكود الأساسي | ممكن تغيّر التفاصيل بحرية     |
| Testability ضعيفة                    | تقدر تعمل Mock بسهولة         |
| High coupling                        | Low coupling                  |

---

## 🧪 فوائد Dependency Inversion Principle:

✅ تقدر تستبدل التفاصيل بسهولة
✅ تقدر تعمل Test بسهولة باستخدام Mocks
✅ بتسهّل التوسعة والتعديل
✅ بتخلي الكود مرن ومحافظ على باقي مبادئ SOLID

---

## فرق مهم:

| مبدأ                           | بيركز على                                              |
| ------------------------------ | ------------------------------------------------------ |
| **DIP**                        | العلاقة بين الكلاسات (High vs Low Level)               |
| **DI (Dependency Injection)**  | طريقة لتطبيق DIP باستخدام Injection                    |
| **IoC (Inversion of Control)** | مفهوم أوسع يخلّي الـ framework هو اللي يدير الاعتمادات |

---
