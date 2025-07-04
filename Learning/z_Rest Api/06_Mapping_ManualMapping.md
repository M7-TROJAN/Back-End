
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
