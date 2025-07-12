
## أولًا: ليه إحنا أصلاً بنحتاج نخزن الـ Refresh Token في قاعدة البيانات؟

بعكس الـ `JWT` العادي، اللي مش بنخزنه في الداتابيز لإنه self-contained (بيحتوي كل المعلومات جواه)، **الـ Refresh Token لازم يتخزن**، لأننا بنعتمد عليه لإصدار توكنات جديدة، ولازم نتحكم فيه ونلغيه لو احتاجنا.

---

## الخطوة الأولى: إنشاء كلاس `RefreshToken` داخل فولدر `Entities`

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

### شرح الخصائص:

| الخاصية     | الغرض منها                                                         |
| ----------- | ------------------------------------------------------------------ |
| `Token`     | القيمة الفعلية للتوكن (نص عشوائي – GUID مثلًا).                    |
| `ExpiresOn` | الوقت اللي بينتهي فيه صلاحية الريفرش توكن.                         |
| `CreatedOn` | وقت إنشاء التوكن، عشان نعرف مدى عمره.                              |
| `RevokedOn` | لو حصل إلغاء للتوكن (مثلًا بعد تسجيل خروج)، بنسجل وقت الإلغاء هنا. |
| `IsExpired` | قيمة محسوبة: هل التوكن منتهي؟                                      |
| `IsActive`  | هل التوكن لسه ساري؟ (يعني مش منتهي ومش ملغي).                      |

---

## يعني إيه `[Owned]`؟

الـ attribute `[Owned]` معناها إن الـ Entity دي مش كيان مستقل في الداتابيز، لكنها **مملوكة (Owned)** لكيان آخر، وده معناه:

* الـ `RefreshToken` **مش ليه كيان منفصل بـ Primary Key خاص بيه لوحده**، لكنه مرتبط بالـ `ApplicationUser`.
* العلاقة دي اسمها **"Owned Entity Type"** أو **"Value Object Owned by Entity"**.
* ال EF Core بيشوف إن الـ RefreshToken هو "جزء" من المستخدم، وبالتالي كل ريفرش توكن لازم يكون مربوط بـ يوزر.

---

## نضيف الخاصية دي داخل الكلاس الي بيمثل جدول ال users عندنا جوة الداتا بيز `ApplicationUser`

```csharp
public sealed class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    // navigation property
    public List<RefreshToken> RefreshTokens { get; set; } = new();
}
```

* هنا عرفنا علاقة **One-to-Many** بين اليوزر والتوكنز.
* كل يوزر ممكن يكون عنده أكتر من refresh token (ده مهم لو حابب تسمح بتسجيل دخول من أجهزة متعددة).

---

## نروح على Configuration الخاص بـ ApplicationUser

```csharp
public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.OwnsMany(u => u.RefreshTokens)
            .ToTable("RefreshTokens") // اسم الجدول اللي هيخزن التوكنات
            .WithOwner() // اللي بيملك التوكن هو ApplicationUser
            .HasForeignKey("UserId"); // مفتاح أجنبي بيربط التوكن باليوزر
    }
}
```

### إيه اللي عملناه هنا:

* استخدمنا `OwnsMany` علشان نعرف إن العلاقة دي **One to Many مملوكة**.
* بـ `ToTable("RefreshTokens")` سمّينا الجدول اللي هيخزن التوكنات.
* بـ `WithOwner().HasForeignKey("UserId")` قلنا إن فيه مفتاح أجنبي بيربط كل توكن بـ يوزر معين.

---

## نعمل المايجريشن:

```bash
Add-Migration AddRefreshTokensTable
Update-Database
```

هيتولد عندك جدول اسمه `RefreshTokens`، وده جزء من الكود الخاص بإنشاؤه:

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

## ليه الجدول فيه `Id` واحنا مقلناش ليه ان فيه `Id` ولا عملنا في الكلاس أي `Id`؟

ده لأننا استخدمنا `OwnsMany`، الـ EF Core **لازم يولد مفتاح أساسي (Primary Key) لكل سجل** داخل الجدول علشان يميّز كل توكن عن التاني، فبيعمل الآتي تلقائيًا:

* بيضيف عمود اسمه `Id` (وهو Auto-Increment Identity).
* وبيضيف كمان عمود `UserId` (اللي بيربطه باليوزر) وبيمثل ال forginKey.
* وبيعمل **Composite Primary Key** مكوّن من `UserId` + `Id`.

ده بيضمن إن كل توكن فريد ومربوط بيوزر واحد، وبيمنع التكرار أو التعارض.

---

## ليه اختار يعمل Composite Key مش Key واحد بس؟

* لأن `RefreshToken` مش كيان مستقل كامل، فهو مرتبط بـ user محدد.
* الـ `UserId` بيحدد المالك.
* والـ `Id` بيميز كل توكن خاص باليوزر ده.
* فالاتنين مع بعض يضمنوا **عدم تكرار التوكنات حتى داخل نفس اليوزر**.

---

## تلخيص سريع:

* عرفنا كلاس `RefreshToken` وخليناه Owned Entity.
* ربطناه بالـ `ApplicationUser` عن طريق Navigation Property.
* عملنا إعدادات العلاقة داخل الـ `Fluent API`.
* عملنا مايجريشن طلّع جدول فيه `Id` و `UserId` كمفتاح مركب.

---
