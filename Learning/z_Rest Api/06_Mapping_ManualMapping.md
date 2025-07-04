
## أولًا: يعني إيه Mapping؟ وليه بنعمله؟

### الفكرة:

في أي تطبيق بيستخدم بنية طبقات (Layers) زي:

* ال `Domain Layer` أو `Model`
* ال `Presentation Layer` أو `API`
* ال `DTOs (Data Transfer Objects)`

إحنا بنحتاج **نحوّل البيانات من طبقة لطبقة تانية**، علشان:

1. **نحمي الداتا الحساسة** من إنها تطلع في الـ API (زي passwords أو IDs داخلية).
2. **نقلل حجم الريسبونس** (مش لازم ترجع كل الأعمدة من الجدول).
3. **نغير أسماء/شكل الخصائص** زي `FirstName` → `first_name`.
4. **نقلل الترابط بين الـ Layers** ونخلي كل طبقة مستقلة.

---

## Manual Mapping (التحويل اليدوية)

ده لما بنكتب الميثود بإيدينا عشان نحول من Model إلى DTO والعكس.

---

## مثال:

### الـ Model (اللي جاي من الـ Database):

```csharp
public class Poll
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
```

---

### ال DTO للاستجابة (اللي بنرجعه للمستخدم):

```csharp
public class PollResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
```

---

### ال DTO للإضافة (اللي بيبعته المستخدم في POST):

```csharp
public class CreatePollRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
```

---

## كتابة الـ Mapping يدويًا

```csharp
public static class ContractMapping
{
    // from Model to Response DTO
    public static PollResponse MapToResponse(this Poll poll)
    {
        return new PollResponse
        {
            Id = poll.Id,
            Title = poll.Title,
            Description = poll.Description
        };
    }

    // from List<Model> to List<Response DTO>
    public static IEnumerable<PollResponse> MapToResponse(this IEnumerable<Poll> polls)
    {
        return polls.Select(p => p.MapToResponse());
    }

    // from CreateRequest DTO to Model
    public static Poll MapToPoll(this CreatePollRequest pollRequest)
    {
        return new Poll
        {
            Title = pollRequest.Title,
            Description = pollRequest.Description
        };
    }
}
```

---

##  بنستخدم الميثودز دي في الكود؟

### مثال في الـ Controller:

```csharp
[HttpGet]
public IActionResult GetAll()
{
    var polls = _pollService.GetAll();

    var response = polls.MapToResponse(); // here we map from Model to DTO
    return Ok(response);
}
```

---

```csharp
[HttpPost]
public IActionResult Create([FromBody] CreatePollRequest request)
{
    var poll = request.MapToPoll(); // from DTO to Model
    var created = _pollService.Add(poll);

    return CreatedAtAction(nameof(Get), new { id = created.Id }, created.MapToResponse());
}
```

---

##  ليه ممكن نستخدم ال Manual Mapping بدل من AutoMapper؟

| Manual Mapping            | AutoMapper                |
| ------------------------- | ------------------------- |
|  أوضح وأسهل في التتبع   |  أوتوماتيكي وسريع       |
|  سهل الديباج (debug)     |  ممكن يخفي الأخطاء       |
|  بياخد وقت في الكتابة    |  أسرع في الكود           |
|  تقدر تتحكم في كل تفصيلة |  تحكم أقل في بعض الحالات |

---

##  مميزات Manual Mapping

* كود صريح وواضح جدًا.
* سهل تعمله `Unit Test`.
* بتقدر تضيف منطق خاص أثناء التحويل.
* مفيش مفاجآت، كل حاجة قدامك.

---

## عيوبه

* ممكن يبقى ممل لو عندك مئات الـ DTOs.
* كل لما تضيف Property جديدة، لازم تفتكر تحدث الـ Mapper.
* فيه تكرار للكود أحيانًا.

---

## Tips:

* دايمًا خليه في static class زي `ContractMapping`.
* استخدم `extension methods` عشان تكتب `.MapToResponse()` بدل ما تكتب `ContractMapping.MapToResponse(...)`.
* خليك consistent في التسمية زي: `MapToResponse`, `MapToEntity`, `MapToModel`, `MapToDto`.

---

## مثال واقعي متكامل من المشروع (SurveyBasket)

```csharp
[HttpGet("{id}")]
public IActionResult Get(int id)
{
    var poll = _pollService.Get(id);

    if (poll == null)
        return NotFound();

    return Ok(poll.MapToResponse());
}
```

```csharp
[HttpPost]
public IActionResult Create([FromBody] CreatePollRequest request)
{
    var model = request.MapToPoll();
    var created = _pollService.Add(model);

    return CreatedAtAction(nameof(Get), new { id = created.Id }, created.MapToResponse());
}
```

---

## الخلاصة:

* ال **Manual Mapping = تحويل البيانات بين Model و DTO يدويًا.**
* بنكتبه على شكل `extension methods`.
* **مفيد في التطبيقات الصغيرة والمتوسطة.**
* **تتحكم فيه بنسبة 100%.**
* ليه بديل اسمه **AutoMapper**،

---


# Implicit Mapping in C\#

---

## أولًا: يعني إيه Implicit Mapping؟

هي طريقة بنستخدم فيها **الـ `implicit operator`** علشان نخلي الكومبايلر يعمل التحويل بين كائنين (مثلاً: Model وDTO) **بشكل تلقائي وذكي** وقت الحاجة، من غير ما ننده دالة تحويل يدويًا.

---

## الفكرة ببساطة:

بدل ما تعمل:

```csharp
PollResponse response = poll.MapToResponse();
```

تقدر تعمل:

```csharp
PollResponse response = poll; // the compiler will understands on his own and converts using implicit operator
```

---

## ال (Syntax) بسيط:

```csharp
public static implicit operator TargetType(SourceType value)
{
    return new TargetType
    {
        Property1 = value.Property1,
        Property2 = value.Property2
    };
}
```

---

## مثال عملي كامل:

###  1. الـ Model:

```csharp
public class Poll
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    //implicit PollResponse
    public static implicit operator PollResponse(Poll poll)
    {
        return new PollResponse
        {
            Id = poll.Id,
            Title = poll.Title,
            Description = poll.Description
        };
    }
}
```

---

### 2. الـ DTO (اللي راجع للعميل):

```csharp
public class PollResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
```

---

### 3. الـ Request DTO:

```csharp
public class CreatePollRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // implicit mapping to Poll
    public static implicit operator Poll(CreatePollRequest request)
    {
        return new Poll
        {
            Title = request.Title,
            Description = request.Description
        };
    }
}
```

---

## إزاي تستخدمها في الكود؟

### في الـ Controller:

```csharp
[HttpPost]
public IActionResult Create([FromBody] CreatePollRequest request)
{
    Poll poll = request; // Automatically converted using implicit operator
    var createdPoll = _pollService.Add(poll);

    PollResponse response = createdPoll; // برضو بيتحول ضمنيًا
    return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
}
```

---

## طيب ليه ممكن أحب أستخدم الـ Implicit Mapping؟

### المميزات:

|                             |                                        |
| --------------------------- | -------------------------------------- |
|  **كود أنظف**              | مش محتاج تكتب `.MapTo...()`            |
|  **أسرع في الاستخدام**    | بس اكتب `Target x = source` وخلاص      |
|  **تقلل الـ Boilerplate** | أقل كود مكرر ممكن                      |
|  **باستخدام OOP محترم**   | بتحوّل الـ mapping لجزء من الكلاس نفسه |

---

###  العيوب (لازم تاخد بالك منها):

|                                                                           |   |
| ------------------------------------------------------------------------- | - |
|  لازم تكون فاهم إن التحويل بيحصل تلقائيًا                               |   |
|  ممكن يسبب **تشويش** لو فيه أكتر من طريقة تحويل بين نفس النوعين         |   |
|  مش مناسب لو فيه **منطق معقد** في التحويل (زي التحقق من شروط أو حسابات) |   |

---

##  مقارنة سريعة

| طريقة التحويل        | الشكل                      | الوضوح         | المرونة | سرعة الكتابة |
| -------------------- | -------------------------- | -------------- | ------- | ------------ |
| **Manual Mapping**   | `MapToResponse()`          | ✅ واضحة جدًا   | ✅ عالية | ❌ أطول       |
| **Implicit Mapping** | `PollResponse res = poll;` | ❌ مش واضحة أوي | ❌ أقل   | ✅ أسرع       |

---

##  إمتى أستخدم Implicit؟

1. لما **التحويل سهل ومباشر** (نفس الـ Properties تقريبًا).
2. لو بتحب تكتب **كود نظيف وقليل**.
3. في **نموذج ثابت ومش هيتغير كتير** (زي Poll → PollResponse).
4. لما تكون متأكد إن الفريق كله **فاهم Implicit Operators** كويس.

---

## ⚠ ملاحظات مهمة:

* لازم الـ operator يبقى `static`.
* لازم تحدد نوع التحويل (`Target ← Source`).
* ال C# مش بتدعم الـ *two-way implicit* تلقائيًا، لو عايز من النوعين لازم تكتب **operator في كل كلاس**.

---

## مثال نهائي من مشروع SurveyBasket:

```csharp
[HttpGet]
public IActionResult GetAll()
{
    var polls = _pollService.GetAll();

    var responseList = polls.Select(p => (PollResponse)p); // Using implicit mapping

    return Ok(responseList);
}
```

---

## الخلاصة:

> ال Implicit Mapping هو أسلوب أنيق لتحويل الكائنات بين الطبقات تلقائيًا، عن طريق تعريف `implicit operator` جوه الكلاس نفسه. بيخلي الكود أنظف وأقصر، لكن لازم تستخدمه بحكمة، خصوصًا لو فيه منطق معقد أو أنواع متعددة من التحويل.
---


# Explicit Conversion Operator in Csharp

---

ال `Explicit operator` هو طريقة بتحوّل كائن من نوع معين (SourceType) إلى نوع تاني (TargetType) **لكن لازم تطلب التحويل صراحة (explicitly)** باستخدام كاست (Cast) بالشكل ده:

```csharp
TargetType result = (TargetType)sourceObject;
```

---


## مثال:

### `Poll` Model:

```csharp
public class Poll
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // explicit mapping to PollResponse
    public static explicit operator PollResponse(Poll poll)
    {
        return new PollResponse
        {
            Id = poll.Id,
            Title = poll.Title,
            Description = poll.Description
        };
    }
}
```

### `PollResponse` DTO:

```csharp
public class PollResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
```

---

## 🧪 استخدام التحويل الصريح (Explicit Conversion):

```csharp
Poll poll = _pollService.Get(id);

PollResponse response = (PollResponse)poll; // لازم تستخدم Cast
```

---

## (Syntax):

```csharp
public static explicit operator TargetType(SourceType value)
{
    return new TargetType
    {
        Property1 = value.Property1,
        Property2 = value.Property2
    };
}
```
