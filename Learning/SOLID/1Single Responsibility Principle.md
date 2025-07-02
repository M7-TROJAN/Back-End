
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

فيه ناس بتفهم SRP بمعنى "الكلاس يكون صغير"، لكن ده غلط.

✅ الصح:
**ال SRP بيتكلم عن سبب التغيير مش حجم الكلاس.**
الكلاس ممكن يكون كبير ويحقق SRP طالما بيعمل حاجة واحدة.
-  حط في الكلاس 1000 ميثود مش مهم مادام كلهم بيحققوا وبيخدموا نفس الحاجة الواحدة

---


كل ما تحس إن الكلاس "بيتكلم أكتر من لغة" أو "بيتعامل مع أكتر من نوع كود"، اسأل نفسك:

> هل فيه أكتر من سبب ممكن يخليني أغير الكلاس ده؟

لو الإجابة "اه" → يبقي انت غالبًا بتكسر SRP.

---
