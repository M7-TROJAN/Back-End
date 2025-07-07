

## أولًا: اي هي ال Microsoft Identity؟

ال `Microsoft Identity` هي مكتبة مبنية على ASP.NET Core تساعدك على إدارة كل ما يخص:

* إنشاء المستخدمين
* تسجيل الدخول والخروج
* إدارة كلمات المرور
* تأكيد الإيميل
* إدارة الأدوار والصلاحيات
* التحقق بخطوتين
* والربط بوسائل تسجيل دخول خارجية (Google, Facebook)

بدل ما تكتب كل ده من الصفر، `Identity` بتقدمه جاهز وقابل للتخصيص.

---

## تجهيز المشروع لاستخدام Identity

### 1. تثبيت الباكدج الأساسية

```xml
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="9.0.6" />
```

دي الباكدج الأساسية اللي بتوفرلك الكلاسات الجاهزة زي `IdentityUser`, `IdentityRole`, `IdentityDbContext`, وهتتعامل مع Entity Framework.

---

### 2. إنشاء كلاس ApplicationUser

```csharp
public sealed class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
```

* ده الكلاس اللي هيمثل المستخدم عندك في الداتابيز.
* ال `IdentityUser` ده هو الاساسي وبيحتوي على خصائص زي: Id, Email, UserName, PasswordHash, PhoneNumber
* إحنا مش هنستخدمه لاننا محتاجين columns اضافية زيادة زي `FirstName`, `LastName` علشان كدة عملنا كلاس `ApplicationUser` وخليناه يورث من `IdentityUser` علشان ياخد كل الخصائص الي جواه وهو الي هنستخدمه

---

### 3. تعديل DbContext علشان يستخدم Identity

بدل ما الكلاس بتاع الـ DbContext يورث من `DbContext` العادي، هنخليه يورث من `IdentityDbContext<ApplicationUser>`:

```csharp
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
```

> خلي بالك من استدعاء `base.OnModelCreating(modelBuilder);` ده مهم علشان Identity تكمل تكوين الجداول.

---

### 4. عمل Migration

افتح Package Manager Console ونفذ:

```bash
Add-Migration AddIdentityTables
```

بعد كده:

```bash
Update-Database
```

هتلاقي إنه أنشألك جداول زي:

* AspNetUsers
* AspNetRoles
* AspNetUserRoles
* AspNetUserClaims
* AspNetUserTokens
* وهكذا...

---
