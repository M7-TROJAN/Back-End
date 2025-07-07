# Microsoft Identity with Minimal APIs – From .NET 8+

الحوار ده ظهر بداية من `.Net 8`

في الإصدارات الجديدة من .NET، Microsoft قررت تبسط استخدام ال (Identity) من غير ما تحتاج تكتب Controllers أو Endpoints بنفسك.
عن طريق Minimal APIs، بقي تقدر تضيف كل الـ **Auth APIs (Register, Login, Forgot Password, etc)** بسطرين كود.

---

## إزاي تستخدم الـ Built-in Identity Endpoints؟

### الخطوة 1 – Registration في Program.cs

```csharp
builder.Services
    .AddIdentityApiEndpoints<ApplicationUser>() // This registers default Identity endpoints
    .AddEntityFrameworkStores<ApplicationDbContext>();
```

* ال `AddIdentityApiEndpoints<TUser>()`: بتسجّل مجموعة الـ API Endpoints الجاهزة. ومكان ال `TUser` بنحددلها الكلاس الي بيمثل ال table بتاع ال users عندنا في الداتا بيز
* ال `AddEntityFrameworkStores`: بيقول لـ Identity تشتغل باستخدام EF Core والداتا بيز بتاعتنا.

---

### الخطوة 2 – Map Endpoints

بعد ما تبني التطبيق وبعد سطر `app.UseAuthorization()`:

```csharp
var app = builder.Build();

app.UseAuthorization();

app.MapIdentityApi<ApplicationUser>();
```

* ال `MapIdentityApi<TUser>()`: بيعمل Map لكل الـ Endpoints الجاهزة زي:

  * `/register`
  * `/login`
  * `/refresh`
  * `/forgotPassword`
  * `/resetPassword`
  * `/manage/info` ... etc

---

## إيه الـ Endpoints اللي بيضيفها؟

| Method | Endpoint                   | Description                |
| ------ | -------------------------- | -------------------------- |
| POST   | `/register`                | Register a new user        |
| POST   | `/login`                   | Login user (returns token) |
| POST   | `/refresh`                 | Refresh token              |
| GET    | `/confirmEmail`            | Confirm email              |
| POST   | `/resendConfirmationEmail` | Resend confirmation email  |
| POST   | `/forgotPassword`          | Send password reset email  |
| POST   | `/resetPassword`           | Reset password             |
| GET    | `/manage/info`             | Get profile info           |
| POST   | `/manage/info`             | Update profile info        |
| POST   | `/manage/2fa`              | Enable/disable 2FA         |

---

## المشكلة في الـ Built-in Identity Endpoints

على الرغم من إنهم جاهزين وسهلين جدًا، لكن فيهم عيب قاتل يمنعنا إننا تعتمد عليهم في مشروع حقيقي:

### المشكلة:

**"مفيش طريقة تعمل Customization للـ logic الداخلي"**

يعني:

* مش هتقدر تضيف خصائص زي `FirstName`, `LastName` في عملية التسجيل.
* مش هتقدر تغيّر الـ response structure.
* مش هتقدر تضيف validation خاصة.
* مش هتقدر تعدل في عملية إرسال الإيميل أو تسجيل الدخول أو إضافة Claims خاصة.

---

## فهنضطر نعمل كل حاجة بنفسنا (Custom Identity Endpoints)


فبدل ما نستخدم الجاهز، إحنا هنكود كل حاجة يدويًا، لكن باستخدام:

* نفس الـ `ApplicationUser`
* نفس `UserManager`, `SignInManager` من Microsoft Identity

هنبني:

* `/api/auth/register`
* `/api/auth/login`
* `/api/auth/refresh`
* etc...

---

##  ليه نعملها بنفسنا؟

| Built-in Identity API              | Custom Identity API      |
| ---------------------------------- | ------------------------ |
| سريعة وسهلة الاستخدام              | مرنة جدًا                |
| غير قابلة للتخصيص                  | قابلة للتعديل حسب الحاجة |
| مشكلة في الخصائص الجديدة للمستخدم  | تقدر تضيف أي Columns     |
| Response structure غير قابل للتحكم | تقدر تعمل Response موحد  |

---
