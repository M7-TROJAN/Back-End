
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

## 🧩 ملخص الجدول:

| المصدر         | Attribute      | النوع المناسب          | مثال على الإرسال                       |
| -------------- | -------------- | ---------------------- | -------------------------------------- |
| Route          | `[FromRoute]`  | int, string            | `/api/polls/5`                         |
| QueryString    | `[FromQuery]`  | int, string, bool      | `/api/polls/search?title=survey`       |
| Header         | `[FromHeader]` | string                 | Header: `Authorization: Bearer abc123` |
| Body (JSON)    | `[FromBody]`   | DTO class              | JSON Payload                           |
| Form (Key/Val) | `[FromForm]`   | string, int, IFormFile | Form Submission or Postman form        |

---
