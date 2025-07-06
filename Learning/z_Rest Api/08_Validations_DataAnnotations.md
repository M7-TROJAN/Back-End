
## أولًا: ليه بنعمل Validation؟

### لأنك ببساطة مش ضامن:

1. إن الـ Frontend عامل Validation كويس.
2. إن الريكوست اللي وصلك جه من الـ Frontend أصلًا، ممكن يكون جاي من Postman أو جهة خبيثة.
3. حتى لو عندك Frontend قوي، ده خط الدفاع الأول، لكن لازم يبقى عندك خط دفاع في الـ Backend.
4. وممكن كمان تضيف **Validation في الداتا بيز** زي:

   * Unique Constraints
   * Not Null
   * Foreign Keys
   * Check Constraints

باختصار: "الداتا اللي داخلة السيستم مسؤوليتك الكاملة، لازم تتأكد إنها نظيفة وصحيحة."

---

## ثانيًا: الفرق في الـ Validation بين MVC و Web API

### في MVC:

* كنت بتكتب بنفسك:

```csharp
if (!ModelState.IsValid)
{
    return View(model); // ترجع الفيو تاني وتعرض الأخطاء
}
```

### في Web API:

* لما تحط `[ApiController]` فوق الـ Controller، الفريمورك تلقائيًا هيعمل:

```csharp
if (!ModelState.IsValid)
    return BadRequest(ModelState); // بيرجع 400 تلقائيًا
```

 يعني مش لازم تكتب `ModelState.IsValid` وال `ASP.NET Core` هيهندلها عنك.

---

## ثالثًا: Data Annotations (الـ Attributes الجاهزة)

هي الطريقة الأبسط والأسرع تعمل بيها Validation مباشرة على الـ DTO.

### مثال:

```csharp
public class CreatePollRequest
{
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
}
```

### أشهر الـ Attributes:

`[Required]`
`[StringLength]`
`[MaxLength] / [MinLength]`
`[Range(min, max)]`
`[EmailAddress]`
`[Phone]`
`[Url]`
`[Compare("OtherProp")]`

---

## رابعًا: مثال عملي متكامل

### DTO:

```csharp
public record RegisterRequest
{
    [Required]
    [StringLength(50)]
    public string Username { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; init; } = string.Empty;

    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; init; } = string.Empty;
}
```

### Controller:

```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        // No need for if (!ModelState.IsValid), handled automatically!
        return Ok("User registered.");
    }
}
```

### لو بعت request ناقص أو فيه مشكلة:

 السيرفر هيرجعلك:

```json
{
  "errors": {
    "Email": [ "The Email field is not a valid e-mail address." ],
    "Password": [ "The field Password must be a string or array type with a minimum length of '8'." ]
  }
}
```

---

## خامسًا: Custom Error Messages

```csharp
[Required(ErrorMessage = "Title field is required")]
[StringLength(100, ErrorMessage = "The title must not exceed 100 characters")]
public string Title { get; set; } = string.Empty;
```

---

## سادسًا: ترتيب التنفيذ

* بيتم تنفيذ كل الفاليديشنز بالتسلسل.
* الفريمورك بيرجع أول مجموعة Errors.
* لو عاوز تتحكم في الفورمات اللي بيرجع، ممكن تستخدم `InvalidModelStateResponseFactory`.

---

## سابعًا: مثال لتعديل شكل الـ Validation Response (متقدم شوية)

```csharp
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(
                e => e.Key,
                e => e.Value?.Errors.Select(er => er.ErrorMessage)
            );

        return new BadRequestObjectResult(new
        {
            Message = "Validation Failed",
            Errors = errors
        });
    };
});
```

---

## ثامنًا: Notes مهمة جدًا:

1. ال `[ApiController]` شرط أساسي لتفعيل Auto-Validation.
2. لازم تبعت JSON مظبوط حسب شكل DTO.
3. الـ Required بيشتغل مع `POST` أو `PUT` غالبًا.
4. تقدر تستخدم FluentValidation بدلًا من DataAnnotations لو حبيت شغل أكثر مرونة.

---
