
## أولًا: هو يعني إيه Parameter Binding؟

في ASP.NET Core لما ييجي request (زي `GET`, `POST`, ...)، الفريمورك لازم يوصل البيانات اللي جت مع الريكوست للـ **Action Method Parameters**.

ال **Binding** هو العملية اللي فيها الفريمورك بيجمع البيانات من:

ال * Route
ال * Query String
ال * Headers
ال * Body
ال * Form

ويحولها للنوع المناسب (`int`, `string`, object...) ويربطها بالـ parameters اللي في الميثود.

---

## الأماكن اللي ممكن تبعت منها البيانات في HTTP Request

| المكان          | وصف                                                                           |
| --------------- | ----------------------------------------------------------------------------- |
| **Route**       | البيانات بتكون جوه الـ URL نفسه، زي `/api/polls/5`                            |
| **QueryString** | بتبقى بعد علامة الاستفهام `?` في الـ URL، زي `/api/polls?id=5`                |
| **Headers**     | بيانات في الـ HTTP Header، زي `Authorization`, `Accept`, إلخ.                 |
| **Body (JSON)** | لما تبعت JSON في جسم الريكوست (POST/PUT)، بيكون جوا الـ body.                 |
| **Form Data**   | زي ما بنبعت من HTML Form كـ `key/value`, وبيوصل بصيغة `x-www-form-urlencoded` |

---

## Attributes مهمة بتساعد ASP.NET Core يعرف يجيب البيانات منين:

| Attribute      | بياخد البيانات منين؟    |
| -------------- | ----------------------- |
| `[FromRoute]`  | من الـ Route URL        |
| `[FromQuery]`  | من Query String         |
| `[FromHeader]` | من Headers              |
| `[FromBody]`   | من JSON في جسم الريكوست |
| `[FromForm]`   | من الفورم كـ Key/Value  |

---

## أولًا: \[FromRoute]

### المعنى:

بيسحب القيمة من **جزء من الـ Route** اللي معمول له Mapping في الـ Controller.

### مثال:

```csharp
[HttpGet("{id}")]
public IActionResult GetPoll([FromRoute] int id)
{
    return Ok($"Poll ID is: {id}");
}
```

Request:

```
GET /api/polls/10
```

 اللي بيحصل:

* ال ASP.NET Core يلاقي في الـ route ال `id`
* يشوف إن الـ action عندها `FromRoute] int id]`
* يربط `10` بالـ `id`

---

##  ثانيًا: \[FromQuery]

###  المعنى:

بتسحب البيانات من **Query String** في الرابط بعد `?`.

### مثال:

```csharp
[HttpGet("search")]
public IActionResult SearchPolls([FromQuery] string title)
{
    return Ok($"Searching for poll with title: {title}");
}
```

 Request:

```
GET /api/polls/search?title=Survey
```

 النتيجة:

* `title = "Survey"`

---

##  ثالثًا: \[FromHeader]

###  المعنى:

بيسحب القيمة من الـ Request Headers.

###  مثال:

```csharp
[HttpGet("secure")]
public IActionResult SecureEndpoint([FromHeader] string authorization)
{
    return Ok($"Auth Token: {authorization}");
}
```

 Request Headers:

```
Authorization: Bearer abc123
```

 النتيجة:

* `authorization = "Bearer abc123"`

---

##  رابعًا: \[FromBody]

###  المعنى:

بيسحب القيمة من **جسم الريكوست** (body)، وغالبًا بصيغة JSON.

وده بيشتغل مع:

*ال `POST`
* ال `PUT`
* ال `PATCH`

###  مثال:

```csharp
public class PollDto
{
    public string Title { get; set; }
    public string Description { get; set; }
}

[HttpPost]
public IActionResult CreatePoll([FromBody] PollDto poll)
{
    return Ok($"Poll created with title: {poll.Title}");
}
```

Request:

```
POST /api/polls
Content-Type: application/json

{
    "title": "Tech Survey",
    "description": "Feedback about tech"
}
```

 النتيجة:

* `poll.Title = "Tech Survey"`

 **داخليًا بيحصل:**

*ال  ASP.NET Core يستخدم Serializer (زي System.Text.Json أو Newtonsoft.Json)
* يعمل **Deserialization** للـ JSON → Object
* ويربطه بالـ parameter

---

##  خامسًا: \[FromForm]

###  المعنى:

بتسحب القيمة من **Form Data** (زي لما تبعت `x-www-form-urlencoded` أو ملف).

### مثال:

```csharp
[HttpPost("submit")]
public IActionResult SubmitForm([FromForm] string name, [FromForm] int age)
{
    return Ok($"Name: {name}, Age: {age}");
}
```

Request Content-Type:

```
application/x-www-form-urlencoded

name=Mahmoud&age=25
```

 النتيجة:

* `name = "Mahmoud"`
* `age = 25`

---

## ملاحظة مهمة عن \[FromBody]:

🔺 **فقط Parameter واحد بس في كل أكشن ممكن يكون \[FromBody]**
وده عشان Body في HTTP Request بيكون واحد فقط.

لو حطيت أكتر من واحد هتطلعلك Error:

```
InvalidOperationException: Action 'X' contains more than one parameter with [FromBody]
```

---

##  Binding بدون ما تكتب Attributes

لو ما كتبتش أي attribute، ASP.NET Core هيقرر من نفسه:

| Type                    | Default Source          |
| ----------------------- | ----------------------- |
| `Simple Types`          | From Route/Query/Header |
| `Complex Types` (class) | From Body               |

 يعني:

```csharp
public IActionResult Add(PollDto poll) // بدون attribute
```

هيتعامل معاها كأنها `[FromBody] PollDto poll`

لكن:

```csharp
public IActionResult Get(int id)
```

هيتعامل كأنه `[FromRoute]` أو `[FromQuery]`

---

##  Internal Process (Serialization)

### Serialization و Deserialization:

| العملية         | المعنى                                    |
| --------------- | ----------------------------------------- |
| Serialization   | تحويل Object → JSON (عند إرسال Response)  |
| Deserialization | تحويل JSON → Object (عند استقبال Request) |

**ASP.NET Core** بيستخدم:

* `System.Text.Json` (افتراضيًا)
* أو تقدر تبدله بـ `Newtonsoft.Json`

---


## استقبال Arrays باستخدام `[FromQuery]`

### الفكرة:

ال ASP.NET Core بيسمحلك تستقبل **مجموعة من القيم** (Array أو List) من الـ Query String، بنفس اسم البراميتر.

---

### مثال:

```csharp
[HttpGet("filter")]
public IActionResult FilterPolls([FromQuery] List<int> ids)
{
    return Ok($"Received Poll IDs: {string.Join(", ", ids)}");
}
```

Request:

```
GET /api/polls/filter?ids=1&ids=2&ids=3
```

النتيجة:

```json
"Received Poll IDs: 1, 2, 3"
```

 **ليه ده بيشتغل؟**
لأن ال ASP.NET Core من جواه internally بيـ **bind** القيم المتكررة في الكويري لنفس الاسم (`ids=1&ids=2&ids=3`) ويجمعها في List.

---

### أنواع ممكن نستخدمها:

* ال `List<string>`
* ال `List<int>`
* ال `int[]`
* ال `string[]`

كل دول شغالين بنفس الطريقة.

---

## استقبال أكثر من براميتر بـ `[FromQuery]`

أنت مش لازم تكتب `[FromQuery]` على كل باراميتر، ASP.NET Core هيـ infer تلقائيًا من الكويري، لكن نكتبه عشان يكون واضح.

---

### مثال:

```csharp
[HttpGet("search")]
public IActionResult Search(
    [FromQuery] string name,
    [FromQuery] int age)
{
    return Ok($"Name: {name}, Age: {age}");
}
```

Request:

```
GET /api/polls/search?name=Mahmoud&age=26
```

النتيجة:

```json
"Name: Mahmoud, Age: 26"
```

---

### مثال واقعي أكتر (Combo بين array و single params):

```csharp
[HttpGet("advanced-search")]
public IActionResult AdvancedSearch(
    [FromQuery] string keyword,
    [FromQuery] List<int> categoryIds,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
{
    return Ok(new
    {
        Keyword = keyword,
        Categories = categoryIds,
        Page = page,
        PageSize = pageSize
    });
}
```

Request:

```
GET /api/polls/advanced-search?keyword=tech&categoryIds=1&categoryIds=4&categoryIds=7&page=2&pageSize=5
```

النتيجة:

```json
{
  "Keyword": "tech",
  "Categories": [1, 4, 7],
  "Page": 2,
  "PageSize": 5
}
```

---

## توضيح مهم:

في الحالة دي ASP.NET Core بيربط كل اسم باراميتر بالاسم اللي جاي من الكويري، ويحوّله تلقائيًا للنوع المطلوب (بـ `ModelBinder`) باستخدام built-in converters.

لو فيه array → بيستخدم repeated keys.
لو فيه single value → بياخد أول قيمة مطابقة.

---

## ملخص:

| المصدر         | Attribute      | النوع المناسب          | مثال على الإرسال                       |
| -------------- | -------------- | ---------------------- | -------------------------------------- |
| Route          | `[FromRoute]`  | int, string            | `/api/polls/5`                         |
| QueryString    | `[FromQuery]`  | int, string, bool      | `/api/polls/search?title=survey`       |
| Header         | `[FromHeader]` | string                 | Header: `Authorization: Bearer abc123` |
| Body (JSON)    | `[FromBody]`   | DTO class              | JSON Payload                           |
| Form (Key/Val) | `[FromForm]`   | string, int, IFormFile | Form Submission or Postman form        |

---
