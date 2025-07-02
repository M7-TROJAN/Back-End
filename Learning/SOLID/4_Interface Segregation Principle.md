
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
