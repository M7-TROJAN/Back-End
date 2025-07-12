## أولًا: هو ليه اصلأ بنخزن الـ Refresh Tokens في الداتا بيز؟

زي ما اتكلمنا قبل كده، الـ **JWT** Token مش بنخزنه في الداتا بيز لأنه self-contained (بيحتوي كل البيانات اللي السيرفر محتاجها).

لكن:

* الـ **Refresh Token** عمره طويل.
* وبيسمح بإعادة توليد Access Token جديد بدون ما المستخدم يعمل login.
* فلازم السيرفر يعرف:

  * مين معاه Refresh Token حالي؟
  * امتى اتولد؟
  * امتى هينتهي؟
  * هل اتلغى (revoked) ولا لأ؟

علشان كده بنحتاج نعمله **persisting** (تخزين دائم) في جدول.

---

## الخطوة 1: إنشاء كلاس `RefreshToken`

```csharp
[Owned]
public class RefreshToken
{
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresOn { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public DateTime? RevokedOn { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresOn;

    public bool IsActive => !IsExpired && RevokedOn is null;
}
```

### شرح كل خاصية:

| الخاصية     | معناها                                                |
| ----------- | ----------------------------------------------------- |
| `Token`     | القيمة الفعلية للـ Refresh Token.                     |
| `ExpiresOn` | التاريخ اللي هينتهي فيه التوكن.                       |
| `CreatedOn` | امتى التوكن اتولد.                                    |
| `RevokedOn` | لو حصل له إلغاء (revoke) – بنسجل وقت الإلغاء.         |
| `IsExpired` | بترجع `true` لو التوكن عدى تاريخ انتهاءه.             |
| `IsActive`  | بترجع `true` لو التوكن لسه ساري (مش منتهي ولا متلغي). |

---

## يعني إيه `[Owned]`؟

الـ Attribute `[Owned]` معناها إن الكلاس ده **مش كيان مستقل** في الداتا بيز، لكن **مملوك (Owned)** من كيان تاني.

بمعنى:

* الكلاس `RefreshToken` مش هيتعامل كـ Table لوحده إلا لما نربطه بكلاس تاني.
* محتاج دايمًا يكون مملوك لحد (في حالتنا: الـ `ApplicationUser`).
* ال EF Core هيبني العلاقة دي عن طريق `OwnsMany`.

---

## الخطوة 2: تعديل `ApplicationUser`

```csharp
public sealed class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public List<RefreshToken> RefreshTokens { get; set; } = new();
}
```

ده معناه إن كل يوزر ممكن يبقى عنده List من التوكنات اللي اتولدوله على مدار الوقت، ولو احتاج يعمل revoke لأي واحد منهم نقدر نحدده بالضبط.

---

## الخطوة 3: إعداد EF Core Configuration

```csharp
builder.OwnsMany(u => u.RefreshTokens)
    .ToTable("RefreshTokens") 
    .WithOwner() 
    .HasForeignKey("UserId");
```

### نشرح كل سطر:

* ال `OwnsMany(u => u.RefreshTokens)`:

  * معناها إن `ApplicationUser` بيملك مجموعة (Many) من `RefreshToken`.

* ال `.ToTable("RefreshTokens")`:

  * بنقول لـ EF Core إنه يعمل Table فعلي في قاعدة البيانات اسمه "RefreshTokens".

* ال `.WithOwner()`:

  * معناها إن كل `RefreshToken` مملوك لـ User.

* ال `.HasForeignKey("UserId")`:

  * وهنا بنحدد اسم العمود اللي هيربط كل توكن باليوزر بتاعه (Foreign Key).

---

## الخطوة 4: إضافة الـ Migration

```powershell
Add-Migration AddRefreshTokensTable
Update-Database
```

هتلاحظ إن EF Core عمل الآتي:

```csharp
migrationBuilder.CreateTable(
    name: "RefreshTokens",
    columns: table => new
    {
        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
        Id = table.Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
        Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
        ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: false),
        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
        RevokedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_RefreshTokens", x => new { x.UserId, x.Id });
        table.ForeignKey(
            name: "FK_RefreshTokens_AspNetUsers_UserId",
            column: x => x.UserId,
            principalTable: "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    });
```

---

## طيب ليه حصل كده؟ تعال نفسر:

### 1. ليه ظهر `Id` رغم إننا معّرفنهوش في الكلاس؟

* لأنك استخدمت `OwnsMany` مش `OwnsOne`.
* يعني الكيان ده مجموعة مش عنصر واحد.
* علشان EF Core يقدر يفرّق ما بين كل عنصر في المجموعة، لازم يحط **مفتاح فريد** لكل عنصر.
* فبيضيف عمود `Id` تلقائيًا كمفتاح لكل `RefreshToken`.

### 2. ليه فيه `UserId`؟ وده جه منين؟

* ده هو الـ Foreign Key اللي بيربط الـ Refresh Token بالـ ApplicationUser.
* وده اللي أنت وضحته في `.HasForeignKey("UserId")`.

### 3. ليه البرايمري كي بقى مركّب (Composite Key) من `UserId` + `Id`؟

* لأن كل يوزر ممكن يكون عنده أكتر من توكن.
* فـ EF Core بيعمل مفتاح مركّب عشان يضمن تميّز كل توكن بالنسبة لكل يوزر.
* ال `Id` لوحده ممكن يتكرر عند أكتر من يوزر، لكن مع `UserId` هيبقى فريد.

---

## النتيجة النهائية

دلوقتي عندك جدول اسمه `RefreshTokens` مرتبط بجدول `AspNetUsers`:

* بيخزن كل التوكنات اللي السيرفر ولدها.
* يعرف كل توكن يخص مين.
* امتى اتولد.
* امتى هينتهي.
* هل اتلغى ولا لسه Active.

---
