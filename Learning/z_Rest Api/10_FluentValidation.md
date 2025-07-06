## أولًا: ليه نستخدم FluentValidation؟

* أكثر مرونة وتنظيمًا من DataAnnotations.
* بيفصل منطق التحقق من الصحة عن الموديلات (Separation of Concerns).
* بيسمح بكتابة قواعد متقدمة ومعقدة بسهولة.
* بيشتغل بكفاءة مع ASP.NET Core Web API و MVC و Minimal APIs.

---

## 1. التثبيت (Installation)

ضيف الباكدج الأساسية:

```xml
<PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="12.0.0" />
```

لو هتستخدم AutoValidation (هنشرحها بعدين)، ضيف:

```xml
<PackageReference Include="SharpGrip.FluentValidation.AutoValidation.Mvc" Version="1.5.0" />
```

---

## 2. التسجيل في `Program.cs`

### لو المشروع طبقي (Monolithic):

```csharp
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
```

### لو بتستخدم Clean Architecture:

ساعتها بتسجل الـ Validators من الـ Layer اللي فيها الـ DTOs:

```csharp
builder.Services.AddValidatorsFromAssembly(typeof(CreatePollRequestValidator).Assembly);
```

أو لو عندك multiple assemblies:

```csharp
builder.Services.AddValidatorsFromAssemblyContaining<CreatePollRequestValidator>();
```

---

## 3. إنشاء Validator

```csharp
using FluentValidation;

public class CreatePollRequestValidator : AbstractValidator<CreatePollRequest>
{
    public CreatePollRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}
```

### شرح القواعد:

* ال `RuleFor(x => x.Title)`: بنحدد الخاصية اللي هنفعل عليها التحقق.
* ال `.NotEmpty()` → لازم تكون مش فاضية.
* ال `.MaximumLength(100)` → أقصى حد للحروف.
* ال `.WithMessage(...)` → رسالة الخطأ اللي هترجع لو الشرط مش متحقق.

---

## الطريقة اليدوية في الـ Controller (مش الأفضل):

```csharp
[HttpPost]
public IActionResult Add(
    [FromBody] CreatePollRequest request,
    [FromServices] IValidator<CreatePollRequest> validator)
{
    var result = validator.Validate(request);

    if (!result.IsValid)
    {
        var modelState = new ModelStateDictionary();
        result.Errors.ForEach(error =>
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        });

        return ValidationProblem(modelState);
    }

    var poll = request.Adapt<Poll>();
    var addedPoll = _pollService.Add(poll);
    return CreatedAtAction(nameof(Get), new { id = addedPoll.Id }, addedPoll);
}
```

### ليه دي مش الأفضل؟

* هتكرر الكود ده في كل الأكشنات.
* فيه boilerplate code كتير.
* صعب الحفاظ عليه مع الوقت.

---

## الأفضل: AutoValidation

### 1. ضيف الباكدج:

```xml
<PackageReference Include="SharpGrip.FluentValidation.AutoValidation.Mvc" Version="1.5.0" />
```

### 2. فعلها في `Program.cs`:

```csharp
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

builder.Services
    .AddFluentValidationAutoValidation()
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
```

---

## الشكل النهائي في الـ Controller:

```csharp
[HttpPost]
[Route("")]
public IActionResult Add([FromBody] CreatePollRequest request)
{
    var poll = request.Adapt<Poll>();
    var addedPoll = _pollService.Add(poll);
    return CreatedAtAction(nameof(Get), new { id = addedPoll.Id }, addedPoll);
}
```

* الـ Validation بيتم تلقائيًا.
* لو فيه Error → الـ API بترجع 400 تلقائيًا بالشكل التالي:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Title": [
      "Title must not exceed 100 characters."
    ]
  },
  "traceId": "..."
}
```

---

## أمثلة متقدمة على FluentValidation:

### شرط أن السن أكبر من 18:

```csharp
RuleFor(x => x.DateOfBirth)
    .Must(dob => dob <= DateTime.Today.AddYears(-18))
    .WithMessage("You must be at least 18 years old.");
```

### شرط إن قيمة واحدة تعتمد على أخرى:

```csharp
RuleFor(x => x.ConfirmPassword)
    .Equal(x => x.Password)
    .WithMessage("Passwords do not match.");
```

###  Validate child collection:

```csharp
RuleForEach(x => x.Questions)
    .SetValidator(new QuestionValidator());
```

---

##  إمتى بنحتاج ال `Validate child collection` و `RuleForEach`؟

بنحتاج `RuleForEach` لما يكون عندنا **Property عبارة عن Collection (قائمة)** جوا الـ DTO أو الـ Model، وكل عنصر جوا الـ Collection دي محتاج **Validation خاص بيه**.

مثلاً:

* عندك استطلاع (Poll) فيه مجموعة أسئلة.
* كل سؤال لازم يتراجع بنفس القواعد (مثلاً: العنوان مش فاضي، نوع السؤال صحيح،...).

---

## السيناريو العملي

### Model

```csharp
public class CreatePollRequest
{
    public string Title { get; set; } = string.Empty;
    public List<QuestionRequest> Questions { get; set; } = new();
}

public class QuestionRequest
{
    public string QuestionText { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
```

---

## اول حاجة نكتب Validator للسؤال الواحد

```csharp
public class QuestionValidator : AbstractValidator<QuestionRequest>
{
    public QuestionValidator()
    {
        RuleFor(x => x.QuestionText)
            .NotEmpty()
            .WithMessage("Question text is required.");

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Question type is required.");
    }
}
```

---

## تاني حاجة نستخدم `RuleForEach` في الـ Poll Validator

```csharp
public class CreatePollRequestValidator : AbstractValidator<CreatePollRequest>
{
    public CreatePollRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Poll title is required.");

        RuleForEach(x => x.Questions) // معناها: لكل سؤال جوا Questions
            .SetValidator(new QuestionValidator()); // استخدم ال Validator بتاع السؤال عليه
    }
}
```

---

##  الترجمة:

```csharp
RuleForEach(x => x.Questions)       // خد كل عنصر من List الأسئلة
    .SetValidator(new QuestionValidator()); // وطبّق عليه القواعد اللي في QuestionValidator
```

---

## مثال توضيحي على JSON Request

```json
{
  "title": "Survey about AI",
  "questions": [
    {
      "questionText": "What is your favorite AI tool?",
      "type": "text"
    },
    {
      "questionText": "",
      "type": ""
    }
  ]
}
```

* السؤال الأول  هيعدي.
* السؤال التاني هيرجع errors زي:

  ```json
  {
    "errors": {
      "Questions[1].QuestionText": [ "Question text is required." ],
      "Questions[1].Type": [ "Question type is required." ]
    }
  }
  ```

---

## ليه ده مفيد؟

* بيخليك تعمل validation منظم لكل عنصر في القائمة.
* لو عندك مثلاً:

  * ا Poll فيه أسئلة
  * او Form فيه مجموعة Inputs
  * او Invoice فيها قائمة Products

  فـ `RuleForEach` هو الحل الأنظف والأقوى.

---

## ملاحظات مهمة:

* لازم الـ controller عليه `[ApiController]` علشان الـ validation تشتغل تلقائيًا.
* تقدر تستعمل `.Cascade(CascadeMode.Stop)` لو عايز توقف أول ما أول خطأ يظهر.
* لو بتستخدم `record` بدل `class`، نفس القواعد تنطبق بدون مشاكل.

---

## الهدف من `Cascade(CascadeMode.Stop)`

بشكل افتراضي، **FluentValidation** بيكمل تنفيذ كل قواعد الفاليديشن حتى لو فيه شرط فشل.

لكن في بعض الأحيان، **مش منطقي تكمل باقي الفاليديشن لو أول شرط فشل**.
في الحالة دي بنستخدم:

```csharp
.Cascade(CascadeMode.Stop)
```

يعني: **"أول ما أول Rule تفشل، خلاص، وقف باقي الـ Rules على نفس الـ Property."**

---

## مثال عملي

نفترض إن عندنا DTO اسمه `UserRegisterRequest`:

```csharp
public record UserRegisterRequest
{
    public string Email { get; set; } = string.Empty;
}
```

وعاوزين نتحقق من الآتي:

1. الإيميل مش فاضي
2. الإيميل مش أطول من 100 حرف
3. الإيميل شكله صحيح

---

## بدون Cascade: كل القواعد هتتنفذ

```csharp
public class UserRegisterRequestValidator : AbstractValidator<UserRegisterRequest>
{
    public UserRegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}
```

لو بعت إيميل فاضي:

* هتاخد **كل التلات رسائل** لأن كل القواعد هتتفعل.

---

## مع `.Cascade(CascadeMode.Stop)`

```csharp
public class UserRegisterRequestValidator : AbstractValidator<UserRegisterRequest>
{
    public UserRegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}
```

لو بعت إيميل فاضي:

* هتاخد **رسالة واحدة فقط**: `"Email is required."`
* مش هيكمل يتحقق من الطول ولا الفورمات.

---

## شرح السطور:

```csharp
RuleFor(x => x.Email)                     // تحديد الخاصية
    .Cascade(CascadeMode.Stop)           // وقف التنفيذ عند أول خطأ
    .NotEmpty().WithMessage("...")       // الشرط الأول
    .MaximumLength(100).WithMessage("...") // الشرط الثاني
    .EmailAddress().WithMessage("...");    // الشرط الثالث
```

---

## امتى تستخدمها؟

* لما تكون القواعد **تعتمد على بعض**.
* لما تحب **تقلل الرسائل** اللي بترجع للمستخدم.
* علشان تحافظ على الأداء وتحمي السيستم من تنفيذ غير ضروري.
