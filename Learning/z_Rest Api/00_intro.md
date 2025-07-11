
##  يعني إيه REST API؟

كلمة **REST** هي اختصار لـ: **Representational State Transfer**
وهو **نمط معماري** (Architectural Style) لتصميم الـ APIs، تم تقديمه لاول مرة سنة 2000 في رسالة دكتوراه لـ واحد اسمه Roy Fielding.

ال REST مش تكنولوجيا ولا بروتوكول، لكنه طريقة لتنظيم التواصل بين الأنظمة (زي الويب) باستخدام بروتوكول HTTP.

---

###  الهدف من REST:

* تسهيل التواصل بين Client (زي المتصفح أو تطبيق موبايل) و Server (اللي فيه البيانات والـ API).
* تنظيم الـ APIs بشكل منطقي وسهل الفهم.
* الاعتماد على بروتوكول HTTP وميزاته الجاهزة.

---

###  خصائص REST الأساسية (6 مبادئ):

| المبدأ                          | التفسير                                                                                |
| ------------------------------- | -------------------------------------------------------------------------------------- |
| **1. Client-Server**            | فصل تام بين الـ Frontend والـ Backend. كل واحد يشتغل مستقل.                            |
| **2. Stateless**                | كل Request بيُرسل لازم يحتوي كل المعلومات اللازمة، السيرفر مش بيفتكر حاجة من اللي فات. |
| **3. Cacheable**                | السيرفر يقدر يقول للعميل يخزن بيانات معينة مؤقتًا (Caching).                           |
| **4. Uniform Interface**        | كل الموارد (Resources) بتتجاب بنفس الطريقة عبر URI واضح.                               |
| **5. Layered System**           | الطلب ممكن يمر بعدة طبقات (مثل load balancer) بدون ما الكلاينت يحس.                    |
| **6. Code on Demand (اختياري)** | السيرفر ممكن يبعت كود يتنفذ على الكلاينت (زي JavaScript) – لكن قليل اللي بيستخدمه.     |

---

###  يعني إيه RESTful API؟

هو أي API معمول على مبادئ REST. بمعنى:

* كل حاجة ليها URI واضح.
* بنستخدم HTTP Methods (GET, POST, PUT, DELETE).
* البيانات غالبًا بتكون بصيغة JSON.
* مفيش جلسة (Stateless).
* في استجابة واضحة بحالة الـ Request (Status Code).

---

### مثال عملي:

لو عندك API لإدارة الكتب، REST هيكون بالشكل ده:

| العملية         | HTTP Method | URI            |
| --------------- | ----------- | -------------- |
| عرض كل الكتب    | GET         | `/api/books`   |
| عرض كتاب معين   | GET         | `/api/books/3` |
| إضافة كتاب جديد | POST        | `/api/books`   |
| تعديل كتاب      | PUT         | `/api/books/3` |
| حذف كتاب        | DELETE      | `/api/books/3` |

---

### ال REST مش شرط يستخدم JSON

هو يعتمد على HTTP، وبالتالي يقدر يتعامل مع:

* JSON
* XML
* HTML
* Text

لكن JSON هو الشائع حاليًا بسبب خفته وسهولة التعامل معاه.

---

###  هل REST آمن؟

نعم، لكن هو مش بيحدد طريقة معينة للحماية. عشان كده بنستخدم معاها تقنيات زي:

* HTTPS (تشفير التواصل)
* JWT (للتحقق من المستخدم)
* OAuth (للصلاحيات)

---

###  مميزات RESTful APIs:

- ✅ سهلة الفهم
- ✅ بتستخدم HTTP العادي
- ✅ سريعة وخفيفة
- ✅ مناسبة لأي نوع عميل (موبايل – Web – IoT)
---

###  عيوب RESTful APIs:

- ❌ لازم الكلاينت يطلب البيانات دايمًا (Push غير مدعوم)
- ❌بيكون Stateless يعني محتاج تحمل بيانات أكتر في كل طلب
- ❌ مفيش Standard قوي جدًا مقارنة بـ GraphQL مثلًا 

---


##  يعني إيه HTTP؟ وإيه مكوناته؟

ده حجر الأساس اللي REST مبني عليه، فلازم نفهمه بعمق عشان نبقى متحكمين 100% في الـ APIs اللي بنبنيها.


HTTP = **HyperText Transfer Protocol**
- هو البروتوكول الأساسي المستخدم في نقل البيانات بين **العميل (Client)** (زي المتصفح أو تطبيق الموبايل) وبين **الخادم (Server)** على الإنترنت.

---
ال Http هو عبارة عن بروتوكول لنقل البيانات بين الكلاينت والسيرفر انما ال Rest بتفرض مجموعة من القيوض علي الطريقة الي بيكومنيكيت بيها السيرفر مع الكلاينت يعني هما بيكلموا بعض فيه ريكويست وفيه ريسبونس وال REST بتضيف بس مجموعة من القيوض علي الكومنيكيشن الي بتحصل ما بينهم دي
---

### إزاي بيشتغل HTTP؟

هو بروتوكول بيعتمد على **الطلب والاستجابة (Request/Response Model)**:

* ال **Client** بيبعت **HTTP Request** (طلب) للسيرفر.
* ال **Server** بيرد بـ **HTTP Response** (استجابة).

كل حاجة بتتم على شكل **نصوص Text** مفهومة، غالبًا على بورت 80 (HTTP) أو 443 (HTTPS).

---

#  خصائص HTTP الأساسية (HTTP Characteristics)

> ال HTTP مش مجرد وسيلة لنقل البيانات… ليه خصائص معينة لازم تكون فاهمها جدًا.

---

##  أولًا: **Stateless** (عديم الحالة)

###  يعني إيه "Stateless"؟

معناها إن **كل Request مستقل تمامًا عن أي Request قبله أو بعده**.
بمعنى: السيرفر مبيفتكرش أي حاجة عنك.

### شرح مبسط:

لما تعمل Login وتدخل اسم المستخدم والباسورد:

* السيرفر بيتأكد منهم 
* بيرجعلك Response فيه Token أو Session ID
* **بعد كده… السيرفر بينسى إنك عملت Login!**

> كل مرة تبعت Request، لازم تبعتله بيانات التعريف (زي التوكن) من أول وجديد، لإنه **Stateless**.

---

### مثال:

```http
POST /api/login
{
  "email": "m@m.com",
  "password": "123456"
}
```

السيرفر بيرجع:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsIn..."
}
```

في كل طلب بعد كده لازم تبعت:

```http
GET /api/user/profile
Authorization: Bearer eyJhbGci...
```

لو نسيت التوكن → السيرفر مش هيعرفك أصلاً!

---

###  ليه ده بيحصل؟

لأن HTTP نفسه مش بيحتفظ بأي بيانات بين الطلبات.
فلو عايز تخلّي السيرفر "يفتكر" المستخدم:

* تستخدم **كوكيز Cookies** (البيانات تتخزن عند الكلاينت)
* أو تستخدم **Token-Based Authentication** (زي JWT)

---

### خطأ شائع:

> "أنا لسه داخل من شوية، ليه السيستم بيطلب مني تسجيل دخول تاني؟"

لأنك ما بعتش التوكن، والسيرفر مبيفتكركش — هو Stateless

---

## ثانيًا: **Connectionless** (غير متصل دائمًا)

### يعني إيه؟

بعد ما العميل يبعث طلب والسيرفر يرد، **الاتصال بينهم بيتم غلقه فورًا**.

* مفيش "جلسة اتصال دائمة".
* كل طلب بيبدأ اتصال جديد من الصفر.

---

### مثال:

```http
GET /api/products
```

* السيرفر استلم الطلب، جهّز البيانات، رجع Response
* بعدها الاتصال تقفل.
* لو فيه طلب جديد؟ → لازم اتصال جديد.

---

###  ده بيفرق في إيه؟

* ال HTTP مش زي WebSockets (اللي بتفتح قناة دائمة).
* علشان كده لو عايز اتصال مستمر (زي الشات)، محتاج تستخدم تكنولوجيا تانية (زي SignalR أو WebSockets).

---

##  ثالثًا: **Platform Independent** (مستقل عن النظام)

###  يعني إيه؟

ال HTTP بيشتغل على أي نوع جهاز، نظام تشغيل، أو لغة برمجة.

> سواء عندك:

* ال Frontend بـ JavaScript
* ال Backend بـ .NET أو Python
* أو حتى IoT جهاز صغير بيبعت بيانات…

طول ما الاتنين بيكلموا HTTP، هيتفاهموا.

---

### مثال:

* موبايل Android بيطلب: `GET /api/weather`
* السيرفر مكتوب بـ ASP.NET Core
* هيرد عادي بالـ JSON

 مفيش اعتماد على نوع الجهاز أو اللغة.

---

##  ملخص الخصائص:

| الخاصية                  | معناها                                                          |
| ------------------------ | --------------------------------------------------------------- |
| **Stateless**            | السيرفر مبيحتفظش بأي حالة، كل Request لازم يحتوي بياناته كاملة. |
| **Connectionless**       | بعد ما الطلب يتم والرد يجي، الاتصال بيُغلق.                     |
| **Platform-Independent** | HTTP شغّال على كل الأجهزة والأنظمة واللغات.                     |

---

##  أمثلة توضيحية واقعية:

| الحالة                                    | HTTP بيعمل إيه؟                    | ليه كده؟                      |
| ----------------------------------------- | ---------------------------------- | ----------------------------- |
| سجلت دخول من دقيقة، وبعدين عملت Refresh   | لو مبعتش التوكن، السيرفر مش هيعرفك | لإنه **Stateless**            |
| عملت طلب، وقطع النت قبل ما توصلك البيانات | لازم تبدأ الطلب من الأول           | لإنه **Connectionless**       |
| موبايل بـ iOS طلب بيانات من سيرفر بـ .NET | كله شغال                           | لإنه **Platform Independent** |

---

##  علاقة كل ده بالأمان:

عشان HTTP ما بيفتكرش حاجة، فأنت لازم:

* تبعت التوكن مع كل Request
* تحمي الاتصال باستخدام HTTPS
* تعمل Authorization بنفسك في كل Endpoint

---

---

## مكونات HTTP Request

لما العميل (Client) يبعت طلب للسيرفر، بيبقى الطلب مكوّن من:

---

### 1. **Request Line**

زي:

```
GET /api/books/5 HTTP/1.1
```

- *ال **GET**: هو الـ HTTP Method (اللي بنسميه Verb).
- *ال **/api/books/5**: هو الـ URI (العنوان).
- *ال **HTTP/1.1**: نسخة البروتوكول.

---

### ال **Headers**

معلومات إضافية بتتبع الطلب، زي:

```
Host: example.com
Content-Type: application/json
Authorization: Bearer eyJhbGci...
User-Agent: Mozilla/5.0
```

* بتحتوي على بيانات زي نوع المحتوى، معلومات المستخدم، صلاحيات، وغيرها.

---

### ال **Body / Payload** *(موجودة في POST، PUT فقط غالبًا)*

هي البيانات اللي العميل بيبعتها في جسم الطلب.
مثال:

```json
{
  "title": "C# In Depth",
  "author": "Jon Skeet"
}
```

---

##  مكونات HTTP Response

الرد اللي السيرفر بيرسله بيكون شكله كالتالي:

---

### 1. **Status Line**

مثال:

```
HTTP/1.1 200 OK
```

* **200**: كود الحالة (نجاح هنا).
* ال **OK**: وصف الحالة.
* ال **HTTP/1.1**: نسخة البروتوكول.

---

### 2. **Headers**

مثال:

```
Content-Type: application/json
Content-Length: 134
Cache-Control: no-cache
```

---

### **Body / Payload**

محتوى الرد – زي JSON، HTML، صورة، ملف...
مثال:

```json
{
  "id": 5,
  "title": "C# In Depth",
  "author": "Jon Skeet"
}
```

---

##  مصطلحات لازم تفهمها كويس:

### 1. URI = Uniform Resource Identifier

هو عنوان المورد – زي `/api/books/5`
بيمثل الشيء اللي انت عايز توصله (كتاب، يوزر... إلخ)

---

### 2. HTTP Verbs (Methods)

هنشرحهم تفصيليًا في الخطوة الجاية، لكن دول: `GET, POST, PUT, DELETE, PATCH...`

---

###  3. Headers

بتحمل Metadata عن الطلب أو الرد.
زي:

* `Content-Type`: نوع البيانات (`application/json`)
* `Authorization`: التوكن بتاع المستخدم
* `Accept`: العميل عايز يرد عليه بأي صيغة

---

###  4. Payload

هو جسم الرسالة (Body) اللي بيحتوي على البيانات سواء في الطلب أو الرد.

---

##  مثال كامل لطلب:

### 🔸 Request:

```
POST /api/books HTTP/1.1
Host: example.com
Content-Type: application/json
Authorization: Bearer xyz...

{
  "title": "Clean Code",
  "author": "Robert C. Martin"
}
```

###  Response:

```
HTTP/1.1 201 Created
Content-Type: application/json

{
  "id": 10,
  "title": "Clean Code",
  "author": "Robert C. Martin"
}
```

---

##  ملخص سريع:

| المصطلح      | المعنى                                               |
| ------------ | ---------------------------------------------------- |
| **HTTP**     | بروتوكول بيستخدم لنقل البيانات بين الكلاينت والسيرفر |
| **Request**  | رسالة من الكلاينت فيها: verb + uri + headers + body  |
| **Response** | رسالة من السيرفر فيها: status + headers + body       |
| **URI**      | عنوان المورد                                         |
| **Header**   | بيانات إضافية عن الطلب أو الرد                       |
| **Payload**  | جسم البيانات                                         |

---



## HTTP Methods (GET, POST, PUT, DELETE)

##  أولاً: `GET`

###  الغرض:

* جلب (قراءة) بيانات من السيرفر.

###  الشكل:

```http
GET /api/books/5 HTTP/1.1
```

###  خصائص:

* ما بيغيرش أي حاجة في البيانات (Read-only).
* مفيش Body في الطلب.
* ممكن يستخدم كاش (cacheable).
* يقدر يتكرر بأمان (idempotent).

###  مثال:

لو عندك تطبيق لقراءة الكتب:

```http
GET /api/books
```

→ يعرض كل الكتب

```http
GET /api/books/5
```

→ يعرض الكتاب رقم 5

---

##  ثانيًا: `POST`

###  الغرض:

* **إنشاء مورد جديد** (Create).

### الشكل:

```http
POST /api/books HTTP/1.1
Content-Type: application/json

{
  "title": "Clean Code",
  "author": "Robert C. Martin"
}
```

###  خصائص:

* بيحتوي على Body فيه بيانات الإنشاء.
* بيغيّر حالة النظام (State-changing).
* مش idempotent (يعني لو كررته ممكن ينشئ أكتر من نسخة).
* غالبًا بيرجع 201 Created.

###  مثال:

لما تضيف كتاب جديد:

```http
POST /api/books
```

→ يضيف كتاب جديد في قاعدة البيانات.

---

##  ثالثًا: `PUT`

###  الغرض:

* **تعديل مورد موجود بالكامل** (Update all fields).
* أو **إنشاءه لو مش موجود** (حسب التصميم).

###  الشكل:

```http
PUT /api/books/5 HTTP/1.1
Content-Type: application/json

{
  "id": 5,
  "title": "Clean Code (Updated)",
  "author": "Robert Martin"
}
```

###  خصائص:

* لازم تبعت *كل* خصائص المورد.
* بيكون Idempotent  (يعني لو بعت نفس الطلب 100 مرة النتيجة تفضل واحدة).
* لو المورد مش موجود، بعض الأنظمة بتنشئه تلقائيًا.

###  مثال:

تحديث بيانات كتاب:

```http
PUT /api/books/5
```

→ يحدّث كل بيانات الكتاب رقم 5.

---

##  رابعًا: `DELETE`

###  الغرض:

* حذف مورد من السيرفر.

###  الشكل:

```http
DELETE /api/books/5
```

###  خصائص:

* بيحذف المورد المحدد.
* Idempotent ✅
* غالبًا بيرجع `204 No Content`

###  مثال:

```http
DELETE /api/books/5
```

→ يحذف الكتاب رقم 5 من القاعدة.

---

##  مقارنة سريعة بين الأربعة:

| الخاصية              | GET     | POST        | PUT            | DELETE     |
| -------------------- | ------- | ----------- | -------------- | ---------- |
| **الغرض**            | قراءة   | إنشاء       | تعديل أو إنشاء | حذف        |
| **فيه Body؟**        | ❌       | ✅           | ✅              | ❌ (غالبًا) |
| **idempotent؟**      | ✅       | ❌           | ✅              | ✅          |
| **يغيّر البيانات؟**  | ❌       | ✅           | ✅              | ✅          |
| **Status Code شائع** | 200/304 | 201 Created | 200/204        | 204        |

---

## فين اللغبطة اللي بتحصل؟

###  ليه نستخدم `POST` مش `PUT` لإنشاء مورد جديد؟

* ال `POST` بيستخدم لإنشاء **مورد جديد** والسيرفر هو اللي بيحدد الـ ID.
* ال `PUT` ممكن يستخدم في الإنشاء **لو الـ ID معروف مسبقًا**.

 مثال يوضح الفرق:

* مثلا `POST /api/books` → السيرفر بيولد ID جديد.
* مثلا `PUT /api/books/5` → أنت بتقول للسيرفر "اعمللي أو حدّث الكتاب رقم 5".

---

###  إمتى أستخدم `PUT` وإمتى أستخدم `PATCH`؟

> (سؤال متقدم شوية بس مهم)

* ال `PUT` = استبدال **كامل** للمورد.
* ال `PATCH` = تعديل **جزئي** للمورد (Field أو اتنين).

 مثال:

```http
PUT /api/users/10
{
  "id": 10,
  "name": "Mahmoud",
  "email": "m@m.com"
}
```

```http
PATCH /api/users/10
{
  "email": "m@m.com"
}
```

---



## تخيل معايا السيناريو ده :

عندك API لإدارة المستخدمين، وكل مستخدم ليه:

* الاسم
* الإيميل
* مجموعة عناوين (List of Addresses)

```json
{
  "id": 12,
  "name": "Mahmoud",
  "email": "mahmoud@example.com",
  "addresses": [
    {
      "type": "Home",
      "city": "Cairo"
    },
    {
      "type": "Work",
      "city": "Giza"
    }
  ]
}
```

---

##  مثال باستخدام `PUT`

```http
PUT /api/users/12
Content-Type: application/json
```

```json
{
  "id": 12,
  "name": "Mahmoud M.",
  "email": "mahmoud.m@example.com",
  "addresses": [
    {
      "type": "Home",
      "city": "Alexandria"
    },
    {
      "type": "Work",
      "city": "Giza"
    }
  ]
}
```

ملاحظات:

* لازم تبعت **كل** الحقول، حتى لو انت مش غيّرت غير الاسم.
* هنا غيرنا الاسم والإيميل، وكمان غيرنا عنوان Home.
* لو نسيت حقل (زي addresses)، في بعض الأنظمة هيتم حذفهم أو استبدالهم بقيم فاضية!

---

##  مثال باستخدام `PATCH`

```http
PATCH /api/users/12
Content-Type: application/json
```

```json
{
  "name": "Mahmoud M.",
  "addresses": [
    {
      "type": "Home",
      "city": "Alexandria"
    }
  ]
}
```

 ملاحظات:

* هنا بنعدل **جزء محدد فقط** من البيانات.
* السيرفر هيفهم إن باقي البيانات تفضل زي ما هي.
* مفيد جدًا في التطبيقات اللي فيها حجم بيانات كبير أو تغيرات بسيطة متكررة.

---

##  تلخيص الفرق بالسيناريو ده:

| النقطة                | PUT                       | PATCH                            |
| --------------------- | ------------------------- | -------------------------------- |
| **تعديل جزئي؟**       | ❌ لازم تبعت كل البيانات   | ✅ تبعت اللي عايز تعدّله بس       |
| **مخاطر فقد بيانات؟** | ✅ لو نسيت حاجة ممكن تتمسح | ❌ غالبًا بيحتفظ بالباقي زي ما هو |
| **الاستخدام المثالي** | لما تعمل Replace كامل     | لما تعدل Field أو 2 فقط          |

---
##  ملخص مبسط:

| العملية  | تستخدم        | لما...               |
| -------- | ------------- | -------------------- |
| `GET`    | تقرأ بيانات   | تعرض موارد أو تفاصيل |
| `POST`   | تنشئ          | تضيف عنصر جديد       |
| `PUT`    | تعدّل بالكامل | تحدث عنصر معروف      |
| `DELETE` | تحذف          | تمسح عنصر معين       |

---

## : أشهر HTTP Status Codes

> ودي من الحاجات المهمة جدًا لأي Backend Developer، لأنها بتحدد معنى الاستجابة اللي الـ API رجّعتها سواء كانت ناجحة أو فيها خطأ.

---

##  إيه هو Status Code؟

هو كود رقمي بيرجعه السيرفر في الـ Response بيعبّر عن نتيجة تنفيذ الـ Request.

الكود بيتكون من **3 أرقام**، وكل رقم ليه دلالة:

| أول رقم | الفئة                                    |
| ------- | ---------------------------------------- |
| 1xx     | معلومات (Informational) – نادر الاستخدام |
| 2xx     | نجاح (Success)                           |
| 3xx     | تحويل (Redirection)                      |
| 4xx     | خطأ من العميل (Client Error)             |
| 5xx     | خطأ من السيرفر (Server Error)            |

---

##  أهم الأكواد اللي تهمك كمطور REST API

---

### ال `200 OK`

>  معناها: الطلب نجح، والسيرفر رجع البيانات المطلوبة.

تستخدم مع:

* ال `GET`
* ال `PUT`
* ال `PATCH`

 مثال:

```http
GET /api/books/5 → 200 OK
```

و Body فيه بيانات الكتاب.

---

### ال `201 Created`

>  معناها: تم **إنشاء مورد جديد** بنجاح.

 تستخدم مع:

* ال `POST`

 مثال:

```http
POST /api/books → 201 Created
```

و Body فيه بيانات الكتاب الجديد.

---

### ال `204 No Content`

>  معناها: الطلب تم بنجاح، لكن مفيش حاجة تتعرض.

 تستخدم مع:

* ال `DELETE`
* ال `PUT` (لو مش بيرجع بيانات)

 مثال:

```http
DELETE /api/books/5 → 204 No Content
```

---

### ال `400 Bad Request`

>  معناها: الطلب اللي بعتُه فيه مشكلة (مطلوب حقل ناقص، تنسيق غلط... إلخ)

 مثال:

```http
POST /api/books
{
  "title": ""
}
→ 400 Bad Request
```

لأن الـ title فاضي.

---

### ال `401 Unauthorized`

>  معناها: مفيش توثيق (Authentication) – المستخدم مش مسجّل دخول أو التوكن بايظ.

 مثال:

```http
GET /api/user/profile → 401 Unauthorized
```

لأنك مش حاطط JWT Token.

---

###  `403 Forbidden`

>  معناها: المستخدم **موثّق** لكن **مش عنده صلاحية** (Authorization Failed)

 مثال:

```http
DELETE /api/admin/users/5 → 403 Forbidden
```

لو بتحاول تمسح يوزر وإنت مش Admin.

---

### ال `404 Not Found`

>  معناها: المورد مش موجود

 مثال:

```http
GET /api/books/9999 → 404 Not Found
```

---

### ال `500 Internal Server Error`

>  معناها: حصل خطأ غير متوقع في السيرفر (زي Exception في الكود)

 مثال:

```http
GET /api/books → 500 Internal Server Error
```

لو الكويري ضربت أو حصل Null Reference.

---

##  ملخص الجدول:

| الكود | المعنى                | متى يُستخدم           |
| ----- | --------------------- | --------------------- |
| `200` | OK                    | طلب ناجح وفيه رد      |
| `201` | Created               | إنشاء مورد جديد       |
| `204` | No Content            | تم بنجاح، بدون بيانات |
| `400` | Bad Request           | خطأ من العميل         |
| `401` | Unauthorized          | محتاج توثيق           |
| `403` | Forbidden             | مفيش صلاحية           |
| `404` | Not Found             | المورد مش موجود       |
| `500` | Internal Server Error | خطأ في السيرفر        |

---

##  شوية لخبطة بتحصل:

### ال 401 vs 403

* `401` → محتاج تسجيل دخول.
* `403` → إنت مسجّل دخول، لكن ممنوع تدخل هنا.

---

###  400 vs 422 (Unprocessable Entity)

* `400` = غلط في الطلب نفسه.
* `422` = الطلب سليم شكليًا، بس البيانات مش منطقية (زي رقم تليفون غلط أو إيميل مش موجود)

> في REST APIs كتير الناس بتستخدم `400` بس لتسهيل الأمور.

---

###  200 vs 204

* ال `200 OK` → فيه Body بيرجع بيانات.
* ال `204 No Content` → مفيش بيانات في الرد، بس تم بنجاح.

---


---

# **RESTful URI Naming Guidelines**

> إزاي تسمي الـ URLs بتاعتك في REST API بشكل احترافي؟ وليه بنعتبر بعض الأسماء "غلط" حتى لو بتشتغل؟

---

## REST URI Naming Best Practices:

في REST:

* **الـ URI بيمثل "Resource" مش "Action"**
* والـ **HTTP Method هي اللي بتحدد نوع العملية**

يعني:
بدل ما تقول في الـ URI "اعمل كذا"...
بتحدد "أين المورد"، وتسيب الـ Method هي اللي توضح نوع العملية.

---

##  غلط شائع:

```http
GET /getOrders
POST /createOrder
DELETE /removeUser
```

* كأنك بتعامل الـ API كأنها مجموعة Functions أو أوامر.
* وده بيكسر **فلسفة REST** اللي بتقول إن كل حاجة بتتم حوالين "Resources".

---

## الطريقة الصحيحة:

```http
GET /orders
POST /orders
DELETE /users/5
```

* كأنك بتتعامل مع "مورد" معين (Orders / Users)، مش "function" تعملها.

---

##  ليه كده أفضل؟

| السبب                    | الشرح                                                                     |
| ------------------------ | ------------------------------------------------------------------------- |
| ✅ أكثر وضوحًا            | أي حد هيبص على `/orders` هيعرف إنك بتتعامل مع طلبات                       |
| ✅ أسهل في القراءة والفهم | بدون ما تحتاج توثيق إضافي                                                 |
| ✅ RESTful ومعايير موحدة  | كل لغات وتطبيقات الـ API هتفهمها بنفس الطريقة                             |
| ✅ بتسهل التوسع لاحقًا    | تقدر تبني على `/orders` بسهولة `/orders/active` – `/orders/summary` وهكذا |

---

##  سيناريوهات عملية

###  استرجاع كل الطلبات (Retrieve All Orders)

**❌ غلط:**

```http
GET /getOrders
```

**ليه؟**

* اسم فعل (Action)
* مش RESTful
* مبهم: هل Orders دي Object؟ Collection؟

** الصح:**

```http
GET /orders
```

**ليه؟**

* ال `/orders` = اسم المورد (Resource)
* ال `GET` = المعنى واضح: هات كل الطلبات

---

###  إنشاء طلب جديد (Create Order)

** غلط:**

```http
POST /createOrder
```

**ليه؟**

* انت كانك بتقولله “نفّذ دالة اسمها createOrder”، كأنك بتكلم سيرفر RPC مش REST.

** الصح:**

```http
POST /orders
```

**ليه؟**

* الفيرب `POST` = إنشاء جديد
* ال `/orders` = المجموعة اللي انت عايز تضيف فيها عنصر جديد

---

### تعديل بيانات طلب (Update Order)

** غلط:**

```http
PUT /updateOrder/10
```

**✅د الصح:**

```http
PUT /orders/10
```

* ال `/orders/10` = مورد واحد، هو الطلب رقم 10
* ال `PUT` = تعديل المورد بالكامل

---

###  حذف يوزر

** غلط:**

```http
DELETE /deleteUser/5
```

** الصح:**

```http
DELETE /users/5
```

* المورد هو `/users/5`
* ال `DELETE` = النوع يوضح إنها عملية حذف

---

##  ملحوظة عن أسماء الموارد:

###  استخدم:

* جمع (Plural) للأسماء: `/orders`, `/users`
* حروف صغيرة + dash أو بدون فواصل

```http
/products
/users/15/orders
/articles/tech-news
```

###  تجنب:

* أسماء أفعال: `/getAllUsers`
* أسماء غامضة: `/doAction`
* استخدم camelCase او snake_case زي `/GetAllUsers`, `/get_all_users`

---

##  قاعدة ذهبية:

> **فكر في الـ URI كأنك بتوصف كيان (Entity)، مش كأنك بتنادي على دالة.**

---

##  ملخص سريع:

| العملية              | RESTful URI              | HTTP Method |
| -------------------- | ------------------------ | ----------- |
| كل الطلبات           | `/orders`                | `GET`       |
| طلب واحد             | `/orders/15`             | `GET`       |
| إنشاء طلب            | `/orders`                | `POST`      |
| تعديل طلب            | `/orders/15`             | `PUT`       |
| حذف طلب              | `/orders/15`             | `DELETE`    |
| طلبات يوزر معين      | `/users/5/orders`        | `GET`       |
| فلترة أو عمليات خاصة | `/orders?status=pending` | `GET`       |

---
