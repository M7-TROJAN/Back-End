
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

👎 كل ما تضيف وسيلة جديدة مثلا (WhatsApp, Push...) لازم تعدّل الكلاس الي اسمه `Notifier`.

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
