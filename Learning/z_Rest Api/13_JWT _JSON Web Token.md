
## في الاول عاوزين نعرف إيه هو الـ Authentication اصلا؟


> ال **Authentication** معناها: "إثبات إن الشخص ده فعلاً هو الي بيقول عليه".

يعني لما اليوزر يبعت username و password، لازم النظام يتأكد إنه شخص فعلاً عنده حساب، ويدي له صلاحية إنه يستخدم الـ API.

---

## في حاجة اسمها ال (Basic Authentication)

* بيشتغل بإن اليوزر يبعت الـ **username\:password** مشفرين بطريقة Base64 في كل Request.
* الهيدر اسمه `Authorization`، وقيمته تبقى:

```http
Authorization: Basic dXNlcm5hbWU6cGFzc3dvcmQ=
```

> (ده مجرد **Encoding** مش **تشفير حقيقي**، أي حد يقدر يفكه بسهولة)

### ليه Basic Auth مش آمن؟

| العيب                        | الشرح                                                               |
| ---------------------------- | ------------------------------------------------------------------- |
| إرسال الباسورد مع كل (Request) | كل ريكويست بيبعت بيانات الدخول، حتى لو أنت بالفعل سجلت دخول قبل كدة |
| ضعف في الحماية               | لو حد قدر يشوف الهيدر، هيقدر يعرف الـ credentials                   |
| مفيش Token                   | مفيش جلسة (session) آمنة، ولا صلاحيات                               |
| صعب التوسع                   | صعب تضيف roles أو claims                                            |

---

## علشان كدة بنلجا ل JWT (JSON Web Token)

---

## إيه هو JWT؟

ال **JWT** هو اختصار لـ **JSON Web Token**، وده عبارة عن **Token مشفّر بيتبعت من السيرفر للعميل بعد تسجيل الدخول**.

### بيحتوي على:

1. **معلومات عن المستخدم** (Claims)
2. **توقيع رقمي** (Signature) علشان نضمن إن الـ token مش متزوّر

---

##  JWT Structure

يتكون الـ JWT من **3 أجزاء** مفصولين بـ نقط (`.`):

```text
xxxxx.yyyyy.zzzzz
Header.Payload.Signature
```

### 1. ال **Header**

* بيحتوي على معلومات عن نوع التوكن وطريقة التوقيع:

```json
{
  "alg": "HS256", 
  "typ": "JWT"
}
```

- > ال `alg`: الخوارزمية المستخدمة للتوقيع، زي `HS256`
- > ال `typ`: نوع التوكن – دايمًا "JWT"

---

### 2. ال **Payload** (البيانات أو الـ Claims)

دي المعلومات اللي جوه التوكن، زي:

* الـ id بتاع المستخدم
* اسمه
* صلاحياته (roles)
* صلاحية التوكن

```json
{
  "sub": "1234567890",
  "name": "Mahmoud Mattar",
  "role": "Admin",
  "exp": 1723742491
}
```

> دي البيانات اللي هتُستخدم من طرف السيرفر

**ملحوظة مهمة**:
الـ Payload مش مشفر! هو مجرد **Base64-encoded**. يعني أي حد معاه التوكن يقدر يشوف البيانات، بس **مينفعش يغيرها** لأن التوقيع هيبوظ.

---

### 3. ال **Signature** (التوقيع)

وده الجزء الأهم، بيتم إنشاؤه باستخدام:

```
HMACSHA256(
  base64UrlEncode(header) + "." + base64UrlEncode(payload), 
  secret
)
```

وده بيمنع أي حد من تعديل محتوى الـ token بدون معرفة الـ secret.

---

## Encoding & Decoding

### Encoding

* معناها تحويل البيانات العادية إلى **Base64-encoded** علشان تبقى قابلة للنقل في URL أو HTTP Header.
* ده بيحصل مع الـ Header و Payload.

### Decoding

* عكس الـ encoding، بيرجع البيانات الأصلية.
* أي حد يقدر يعمل Decoding لـ Header و Payload، بس مش لـ Signature.

المواقع زي [jwt.io](https://jwt.io) بتخليك تشوف تفاصيل أي JWT.

---

## مثال JWT حقيقي:

```text
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9
.
eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6Ik1haG1vdWQiLCJpYXQiOjE1MTYyMzkwMjIsInJvbGUiOiJBZG1pbiJ9
.
SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
```

---

## ليه JWT أفضل؟

| المقارنة         | Basic Auth             | JWT                             |
| ---------------- | ---------------------- | ------------------------------- |
| الأمان           | ضعيف – الباسورد بيتبعت | قوي – توكن مشفر وموقع           |
| التوسع           | صعب                    | سهل تضيف Claims و Roles         |
| الجلسات          | لأ                     | فيه صلاحية + تاريخ انتهاء       |
| قابلية الاستخدام | محدود                  | يدعم Single Sign-On، APIs       |
| التشفير          | لأ (Base64 بس)         | نعم – Signature لحماية البيانات |

---

## هل JWT له مدة صلاحية؟

أيوه ليه صلاحية

* عن طريق claim اسمه `exp`
* مثلًا: `exp = DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds()`

---

## أنواع Claims شائعة:

| Claim   | الوصف                |
| ------- | -------------------- |
| `sub`   | Subject (User ID)    |
| `name`  | User name            |
| `email` | Email address        |
| `role`  | Role like Admin/User |
| `exp`   | Expiration time      |
| `iat`   | Issued At            |

---

## باختصار المفهوم:

1. اليوزر يعمل Login → السيرفر يتأكد → السيرفر يبعت JWT.
2. اليوزر يحتفظ بالتوكن ويبعتو في كل Request في الهيدر.
3. السيرفر بيتحقق من التوكن (الـ Signature والـ Expiration).
4. لو التوكن سليم → الطلب بيتنقل للـ Controller.

---
