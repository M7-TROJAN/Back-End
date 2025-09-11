

# 🥠 أولًا: Cookies

## 📌 يعني إيه Cookie؟

* الـ **Cookie** هي ملف صغير جدًا بيتخزن في المتصفح (عند الكلاينت).
* السيرفر هو اللي بيبعتها، والمتصفح بيخزنها تلقائيًا.
* بعد كده، أي Request تاني من المتصفح للسيرفر → المتصفح يضيف الكوكيز دي في الهيدر أوتوماتيك.

---

### 🎯 مثال بسيط (عشان توضح)

1. إنت دخلت على موقع: `www.shop.com/login`.
2. دخلت الإيميل والباسورد.
3. السيرفر اتأكد إنك صح، ورجعلك **Set-Cookie** في الهيدر:

   ```
   Set-Cookie: sessionId=abc123; Path=/; HttpOnly
   ```
4. المتصفح بيخزن الـ Cookie.
5. المرة الجاية لما تبعت Request (مثلًا `/cart`)، المتصفح يبعتها مع الهيدر:

   ```
   Cookie: sessionId=abc123
   ```
6. السيرفر يلاقي إن `sessionId=abc123` موجود عنده في قاعدة البيانات، فيعرف إنك محمود اللي عامل Login.

---

### 📌 الميزة

* المتصفح بيتعامل مع الكوكيز أوتوماتيك.
* مناسبة جدًا للتطبيقات الـ **Web التقليدية** (ASP.NET MVC, PHP, Django …).

### 📌 العيب

* أقل مرونة مع الـ APIs اللي بيستهلكها **موبايل أبلكيشن** أو **عميل غير المتصفح**.
* محتاجة إدارة Sessions على السيرفر (Memory/DB).

---

# 🔑 ثانيًا: Token-Based Authentication (زي JWT)

## 📌 يعني إيه Token؟

* الـ **Token** هو سلسلة نصية (String) بيصدرها السيرفر بعد ما تتأكد إنك Logged in.
* الكلاينت (المتصفح أو الموبايل) يخزنها (في localStorage أو memory).
* الكلاينت لازم يضيف الـ Token في الهيدر بتاع كل Request:

  ```
  Authorization: Bearer <token>
  ```

---

## 📌 يعني إيه JWT بالذات؟

ببساطة JWT = **JSON Web Token**
هو نوع من التوكينات مكتوب بشكل مشفر/مضغوط.
بيكون عبارة عن 3 أجزاء مفصولين بـ `.`
مثال:

```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9
.eyJpZCI6IjEyMyIsIm5hbWUiOiJNYWhtb3VkIn0
.sR4n8XrN3g3lW...
```

* الجزء الأول: معلومات التشفير (Header).
* الجزء الثاني: بيانات المستخدم (Payload). مثلًا:

  ```json
  {
    "id": 123,
    "name": "Mahmoud",
    "role": "Admin"
  }
  ```
* الجزء الثالث: توقيع (Signature) عشان نتأكد إنه مش متغير.

---

### 🎯 مثال عملي

1. تعمل Login:

   ```
   POST /api/login
   { "email": "m@m.com", "password": "123" }
   ```
2. السيرفر يرد عليك بـ JWT:

   ```json
   { "token": "eyJhbGciOi..." }
   ```
3. الكلاينت يخزن التوكن في **localStorage**.
4. أي Request بعد كده تبعته كده:

   ```
   GET /api/user/profile
   Authorization: Bearer eyJhbGciOi...
   ```
5. السيرفر يفك الـ JWT ويعرفك.

---

### 📌 الميزة

* بيكون **Stateless**: السيرفر مش محتاج يخزن حاجة، التوكن فيه كل البيانات.
* مناسب جدًا للـ **REST APIs** والـ **موبايل أبلكيشن**.
* سهل يتشارك بين خدمات مختلفة (Microservices).

### 📌 العيب

* لو التوكن اتسرب → أي حد يقدر يستخدمه لحد ما تنتهي صلاحيته.
* لازم يكون فيه نظام لتجديد (Refresh Token).

---

# ⚖️ طب أستخدم إيه؟

* لو عندك **Web App تقليدي** (زي ASP.NET MVC) → الكوكيز + السيشن أسهل وأأمن.
* لو عندك **Web API** هتستهلكه موبايل أو SPA (React/Angular/Vue) → التوكينات (JWT) أفضل.

---

💡 الخلاصة:

* ال **Cookies** = زي كارت تعريف بيتخزن عندك والمتصفح بيقدمه أوتوماتيك.
* ال **JWT** = زي بطاقة شخصية بتشيلها معاك في جيبك، وتقدمها بنفسك في كل مرة عشان السيرفر يتأكد إنك إنت.

---

| Feature                   | Cookies (Session-based)                                                     | Token-based (JWT)                                                              |
| ------------------------- | --------------------------------------------------------------------------- | ------------------------------------------------------------------------------ |
| **Where data is stored**  | Server (Session in DB or memory) + Client (Cookie)                          | Client only (Token stored in localStorage, memory, or cookie)                  |
| **Who sends credentials** | Browser automatically sends cookies with every request                      | Client app must manually attach token in `Authorization` header                |
| **Server state**          | **Stateful** (server must keep session data)                                | **Stateless** (server doesn’t store anything; info is in the token)            |
| **Scalability**           | Harder (sessions need to be shared between servers)                         | Easier (any server can validate the token)                                     |
| **Best suited for**       | Traditional web apps (ASP.NET MVC, PHP, Django)                             | Modern APIs, mobile apps, SPAs (React, Angular, Vue)                           |
| **Security**              | Cookies vulnerable to CSRF but protected by `HttpOnly` and `SameSite` flags | Tokens vulnerable to theft if stored insecurely, need HTTPS and Refresh Tokens |
| **Expiration**            | Managed by the server session timeout                                       | Encoded in the token (`exp` claim), expires automatically                      |
| **Revocation**            | Easy (delete session from DB = user logged out)                             | Harder (need blacklist or short expiration + refresh token)                    |
| **Ease of use**           | Easier for browsers (automatic)                                             | More flexible for cross-platform clients                                       |

---

⚡ Rule of thumb:

* **Cookies** = better for **browser-based apps** with server-side rendering.
* **JWT** = better for **REST APIs / mobile apps / microservices**.
