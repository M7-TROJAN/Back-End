## بعد ما عملنا ال table الخاص ب ال Refresh Tokens, إيه المطلوب نعمله دلوقتي؟

إحنا شغالين على جزئية الـ **"تجديد التوكن لما يخلص"**، وده بيحصل باستخدام **Refresh Token**.
---
إحنا عاوزين نعمل **Endpoint** (يعني API) جديدة، والـ API دي مهمتها إنها تستقبل من اليوزر حاجتين:

1. **الـ JWT Token** اللي معاه — وده ممكن يكون:

   * اما Valid (لسه شغال)
   * واما Expired (خلص وقته)
2. **الـ Refresh Token** — وده المفروض إنه صالح لفترة أطول شوية.

وبناءً على الاتنين دول، نقرر نعمل إيه:

* لو الاتنين صح → نولد JWT جديد + Refresh Token جديد.
* لو في حاجة غلط ← نرفض الطلب.

---

## الخطوة الأولى: تجهيز ميثود Validate للـ JWT Token

إحنا دلوقتي عندنا Class اسمه `JwtProvider`، وده هو المسؤول عن توليد الـ JWT من الأساس. دلوقتي عاوزين نزود عليه ميثود اسمها `ValidateToken`.

### الهدف منها:

الميثود دي المفروض تاخد JWT وتطلع لنا منه **UserId**، لو التوكن فعّال، أو حتى لو Expired بس المهم التركيب بتاعه سليم.

---

### الكود:

```csharp
public string? ValidateToken(string token)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key!));

    try
    {
        var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            IssuerSigningKey = symmetricSecurityKey,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        return principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
    }
    catch (Exception)
    {
        return null;
    }
}
```

---


1. ال **`var tokenHandler = new JwtSecurityTokenHandler();`**
   دي كلاس من مكتبة Microsoft مسؤولة عن قراءة وفهم الـ JWT Tokens.

2. ال **`var symmetricSecurityKey = ...`**

   * هنا بنجهز المفتاح اللي هنتحقق بيه من التوقيع.
   * هو نفس المفتاح السري اللي استخدمناه لما أنشأنا التوكن.

3. ال **`ValidateToken(...)`**

   * دي الميثود اللي بتحاول تفك التوكن وتتحقق من صحته.
   * بتاخد:

     * التوكن نفسه
     * إعدادات التحقق `TokenValidationParameters`
     * ال Output بيرجع فيه الـ `SecurityToken` بعد فكه

4. ال **`TokenValidationParameters`** دي الإعدادات اللي بتتحكم في الفاليديشن:

   * ال `IssuerSigningKey` ← بنقوله استخدم المفتاح ده عشان تتأكد من صحة التوقيع.
   * ال `ValidateIssuerSigningKey` ← خليناها `true` عشان يتحقق فعليًا من التوقيع.
   * ال `ValidateIssuer` ← قولناله `false` لأننا مش محتاجينها.
   * ال `ValidateAudience` ← برضو `false`، نفس السبب.
   * ال `ValidateLifetime` ← `true` = التوكن لو منتهي هيعتبره غير صالح.
   * ال `ClockSkew = TimeSpan.Zero` ← معناها مفيش سماحية فرق وقت (بعض السيرفرات بتسمح بدقائق).

5. ال **`principal.FindFirst(...).Value`**

   * لو التوكن سليم، هنرجع الـ **UserId** من الـ Claims.
   * الـ "sub" claim هو اللي بيحتوي على الـ ID حسب معايير JWT وحسب ما احنا حددنا قبل كدة واحنا بنعمله Generate.

6. ال **`catch`**

   * لو حصل أي خطأ (زي توكن مش متوقع، أو توقيع غلط، أو منتهي)، بنرجع `null`.

---

## ليه عملنا كده؟

عشان نكون قادرين نتحقق من الـ JWT Token حتى لو هو منتهي، وناخد منه الـ ID بتاع اليوزر ونبدأ خطوات التحقق من الـ Refresh Token.

---


إحنا دلوقتي عايزين لما اليوزر يعمل **تسجيل دخول (Login)**، السيرفر:

1. يولّده **JWT Access Token** صالح لفترة قصيرة (مثلاً 15 دقيقة).
2. وفي نفس الوقت يولّده **Refresh Token** صالح لفترة أطول (مثلاً 30 يوم).
3. ويحفظ الـ Refresh Token في الداتا بيز.
4. ويرجعله الاتنين في الـ response.

علشان لو الـ Access Token انتهى، يقدر اليوزر يبعته مع الـ Refresh Token عشان ياخد Access Token جديد بدون ما يعمل Login من تاني.

---

## أول حاجة: نعدّل `AuthResponse`

### ليه نعدله؟

لأننا كنا بترجع البيانات دي:

* Id
* Email
* FirstName
* LastName
* Token
* ExpiresIn

لكن دلوقتي محتاج ترجع كمان:

* **RefreshToken**
* **RefreshTokenExpiryDate**

### الكود بعد التعديل:

```csharp
namespace SurveyBasket.Web.Contracts.Authentication;

public record AuthResponse(
    string Id,
    string? Email,
    string FirstName,
    string LastName,
    string Token,
    int ExpiresIn,
    string RefreshToken,
    DateTime RefreshTokenExpiryDate
);
```

---

## تاني حاجة: نعدّل ميثود `GetTokenAsync` جوه `AuthService`

---

### الكود كامل:

```csharp
public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
{
    // 1. Try to find the user by email
    var user = await _userManager.FindByEmailAsync(email);
    if (user is null)
        return null;

    // 2. Check if the password is correct
    var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
    if (!isPasswordValid)
        return null;

    // 3. Generate a new JWT
    var (token, expiresIn) = _jwtProvider.GenerateToken(user);

    // 4. Generate a new Refresh Token
    var refreshToken = GenerateRefreshToken();
    var refreshTokenExpiration = DateTime.UtcNow.AddDays(_RefreshTokenExpiryDays);

    // 5. Add the Refresh Token to the user's list of tokens
    user.RefreshTokens.Add(new RefreshToken
    {
        Token = refreshToken,
        ExpiresOn = refreshTokenExpiration
    });

    // 6. Save changes to the database
    var result = await _userManager.UpdateAsync(user);
    if (!result.Succeeded)
        return null;

    // 7. Return the full auth response (JWT + Refresh Token)
    return new AuthResponse(
        user.Id,
        user.Email,
        user.FirstName,
        user.LastName,
        token,
        expiresIn,
        refreshToken,
        refreshTokenExpiration
    );
}
```

---

### `var user = await _userManager.FindByEmailAsync(email);`

* بندور في جدول الـ Users على يوزر عنده نفس الإيميل.
* لو ملقيناهش → بنرجّع `null`.

---

### `var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);`

* بنستخدم الـ Identity عشان نتأكد إن الباسورد اللي دخله المستخدم صح.
* لو غلط → برضو بنرجّع `null`.

---

### `var (token, expiresIn) = _jwtProvider.GenerateToken(user);`

* بنستدعي الميثود اللي بتولّد JWT بناءً على بيانات اليوزر.
* بترجع لنا:

  * ال `token` ← التوكن نفسه كـ string
  * ال `expiresIn` ← الوقت اللي هيكون فيه التوكن صالح (بالثواني)

---

### `var refreshToken = GenerateRefreshToken();`

* بنولّد Refresh Token جديد باستخدام كلاس داخلي.
* هنشرحه بعد شوية.

---

### `var refreshTokenExpiration = DateTime.UtcNow.AddDays(_RefreshTokenExpiryDays);`

* بنحسب تاريخ الانتهاء للـ Refresh Token.
* هنا حاطط ثابت `_RefreshTokenExpiryDays = 30` يعني التوكن هيعيش 30 يوم من لحظة الإنشاء.

---

### `user.RefreshTokens.Add(...)`

* بندخل التوكن الجديد جوه لستة التوكنات بتاعت اليوزر.
* **معلومة مهمة**: إحنا عملنا علاقة One-to-Many بين ApplicationUser وRefreshToken.

---

### `await _userManager.UpdateAsync(user)`

* لازم نحفظ التغيير في الداتا بيز.
* لو العملية فشلت → بنرجّع `null`.

---

### `return new AuthResponse(...)`

* آخر خطوة، بنرجّع الـ response الكامل لليوزر، فيه:

  * بياناته الشخصية
  * التوكن
  * وقت صلاحيته
  * الريفرش توكن
  * وتاريخ انتهاءه

---

## طيب: إزاي اتولّد الـ Refresh Token؟

الميثود الي اسمها `GenerateRefreshToken` ودي مسؤولة عن توليد **String عشوائي آمن جدًا**.

### الكود:

```csharp
private static string GenerateRefreshToken()
{
    var refreshTokenBytes = RandomNumberGenerator.GetBytes(64);
    return Convert.ToBase64String(refreshTokenBytes);
}
```


* `RandomNumberGenerator.GetBytes(64)`:

  * يولّد 64 بايت عشوائي (cryptographically secure)
  * يعني مستحيل يتوقّع أو يتكرّر بسهولة.

* `Convert.ToBase64String(...)`:

  * نحولهم لسلسلة نصية (string) يمكن تخزينها وإرسالها.

---

## كده إحنا عملنا إيه بالضبط؟

1. عدّلنا الـ AuthService.
2. بقينا بنرجع JWT + Refresh Token.
3. خزّنّا التوكن في جدول اليوزرز.
4. بقينا جاهزين ننفذ الجزء الجاي من السيناريو.
