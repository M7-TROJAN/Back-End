
# **Refresh Tokens**

## **أولًا: هل هي ضرورية؟**

خلينا نكون واضحين من البداية:
**Refresh Tokens (Not Mandatory)**.

يعني إنت ممكن تبني نظام Login كامل باستخدام JWT بس، من غير ما تضيف أي حاجة اسمها "Refresh Token".
ولو التطبيق بسيط ومفيش حاجة تتطلب إن اليوزر يفضل logged in لفترة طويلة، فممكن تكمل عادي من غيرها كله بيعتمد علي حسب البيزنس الي انت شغال عليه.

لكن...

لو بتبني **تطبيق حقيقي بيستخدمه ناس فعليًا**، وبيفضلوا logged in بالساعات أو الأيام أو الشهور – زي Gmail, Facebook, Netflix – يبقى ال Refresh Tokens **مش مجرد إضافة اختيارية، بل ضرورية جدًا**.

---

## **تعالى نبدأ بالمشكلة الأساسية في JWT**

كل JWT Token بيحتوي على:

* بيانات اليوزر (Claims)
* توقيع رقمي (Signature)
* مدة صلاحية (`exp`)

### يعني إيه صلاحية (Expiration)؟

يعني فيه وقت معين التوكن فيه يكون صالح **Valid**، وبعد كده خلاص يعتبر **expired**.
الـ JWT مش بيتحدث تلقائيًا، ومفيش طريقة نمد صلاحيته بعد ما يخلص.

> وده حاجة كويسة أمنيًا: الـ Token له عمر محدود، فحتى لو اتسرّب، ميشتغلش للأبد.

### طيب... بعد ما يخلص؟

لو الـ Token خلص، وجربت تبعته في أي request محمي بالسيرفر، هيجيلك:

```http
401 Unauthorized
```

يعني السيرفر رفض الطلب لأن الـ access token منتهي.

---

## **طيب ليه دي مشكلة؟**

اليوزر مش هيفهم يعني إيه JWT ولا Expiry ولا 401.
كل اللي هو شايفه إنه كان شغال، وفجأة التطبيق بيقوله:

> "You're logged out"
> "Please login again"

وده شعور سيئ جدًا، وبيأثر على الـ UX (User Experience) بشكل سلبي.
محدش عايز كل شوية يكتب الإيميل والباسورد تاني!

---

## **الحل: نضيف Refresh Token**

### إيه اللي بيحصل بقى في التطبيقات المحترمة؟

أول ما اليوزر يعمل **Login**:

 السيرفر بيبعتله **اتنين توكن**:

1- ال JWT Access Token	

* صغير وخفيف وسريع - يستخدم في كل الريكوستات المحمية

2- ال Refresh Token	

*  طويل المدى – يُستخدم بس لتجديد (تحديث) الـ JWT بعد ما يخلص

---

## **مدة الصلاحية المختلفة**

| نوع التوكن    | مدة الصلاحية النموذجية         |
| ------------- | ------------------------------ |
| Access Token  | من 5 دقايق لـ 1 ساعة (قصيرة)   |
| Refresh Token | من أيام لأسابيع أو شهور (أطول) |

---

## **طيب الـ Client بيعمل إيه؟**

### السيناريو الطبيعي:

1. اليوزر عمل login ← السيرفر رجعله access token + refresh token.
2. التطبيق بيستخدم الـ access token مع كل API request.
3. بعد فترة، الـ access token خلص (expired).
4. بدل ما يظهر للـ يوزر رسالة "اعمل login تاني"، التطبيق يعمل الآتي:

    **يبعت refresh token للسيرفر ويطلب access token جديد.**
    السيرفر يولّد JWT جديد (لو الريفرش توكن سليم) ويرجعه التطبيق.
    التطبيق يستخدم التوكن الجديد… واليوزر مش واخد باله إن حصل أي حاجة.


---

## ملحوظة في سيناريو ال Tokens ، فيه طرفين مهمين لازم نفرق بينهم:

 **اليوزر (User)**: هو الشخص الحقيقي اللي بيستخدم التطبيق – زيك لما تفتح Gmail أو Facebook. 

**العميل او التطبيق (Client App)**: الكود اللي بيشتغل عند المستخدم وبيتواصل مع الـ API – سواء موقع ويب (React, Angular) أو موبايل (Android, iOS) أو حتى Postman.

---

## **طب يعني التوكن بيتجدد إلى ما لا نهاية؟**

لا طبعًا.

كل Refresh Token له صلاحية كمان.
ولما تنتهي صلاحية الـ refresh token أو يتم إلغاؤه لأي سبب:

 السيرفر يرفض طلب التجديد.
 العميل (التطبيق) يضطر يعرض للـ يوزر إنه يعمل Login تاني.

---

## **طيب ليه منخليش الـ JWT صالح لفترة طويلة من الأول؟**

ده سؤال مهم جدًا، والإجابة عليه بتتلخص في الناحية الأمنية.

لو خلّيت الـ **Access Token** صالح لفترة طويلة (مثلاً: يوم كامل أو أسبوع)، هتاخد شوية راحة للمستخدم، بس هتفتح على نفسك مشاكل كبيرة أوي، وده توضيح:

### أولًا: المخاطرة لو التوكن اتسرّب

لو حد قدر يوصّل للـ JWT ده، يقدر يستخدمه طول ما هو صالح.
يعني لو التوكن صالح لمدة 7 أيام، فالهاكر معاه أسبوع كامل يعمل فيه اللي هو عايزه… ومفيش أي طريقة تمنعه طول المدة دي.

أما لو كنت شغال بـ **Access Token + Refresh Token**، فالـ access token بيخلص بسرعة (مثلاً 15 دقيقة)، ولو اتسرّب، مش هيفيده كتير. أما الـ refresh token تقدر تلغيه من عندك بسهولة.

---

### ثانيًا: صعوبة التحديث أو الإلغاء

لو عايز تلغي JWT طويل المدى (زي لما يوزر يعمل Logout أو حسابه يتقفل)، مفيش طريقة مباشرة تعمل كده.
ليه؟ لأن JWT **Stateless**، يعني مش متخزن في قاعدة البيانات – مجرد توقيع مشفر.

لكن في حالة الـ **Refresh Token**، أنت بتخزن التوكن في قاعدة البيانات. فبمجرد ما تلغيه من عندك، مهما المستخدم يحاول يستخدمه، مش هينفع.

---

### ثالثًا: مفيش سيطرة بعد الإصدار

بمجرد ما تطلع JWT، مفيش تحكم فيه. مفيش طريقة تعرف هو مستخدم ولا لأ، ولا تقدر تمنعه.

لكن مع refresh tokens، عندك جدول أو storage فيه كل التوكنات، فتقدر:

* تعمل revocation (إلغاء)
* تحدد صلاحية لكل واحد
* تشوف مين استخدم إيه
* تمنع الاستخدام المتكرر للـ same refresh token (one-time use)

---

## الخلاصة

* ال **JWT طويل المدى** = راحة للمستخدم لكن خطر أمني.
* ال **JWT قصير المدى + Refresh Token** = أفضل مزيج بين الراحة والأمان.

وده السبب إن كل التطبيقات الكبيرة زي Gmail، Facebook، Instagram بتشتغل بالموديل ده.

---

## **الخلاصة النظرية حول Refresh Tokens**

* **الفكرة الأساسية:**
  لما الـ **JWT Access Token** يخلص (expire)، بدل ما تجبر المستخدم يعمل تسجيل دخول من جديد، بتستخدم الـ **Refresh Token** عشان تطلب من السيرفر يولّد Access Token جديد بشكل تلقائي — من غير ما المستخدم يحس بأي حاجة.

---

* **الميزة الأمنية:**
  الـ **Refresh Token** عادة بيتخزن في مكان آمن زي `HttpOnly Cookie` (يعني مش Accessible من JavaScript)، وده بيقلل جدًا من خطر سرقته.
  وكمان تقدر تلغيه في أي وقت (لو المستخدم عمل Logout، أو لو حصل شك في محاولة اختراق).

---

* **تحسين تجربة المستخدم (UX):**
  المستخدم بيفضل مسجّل دخول في التطبيق حتى لو الـ Access Token خلص، لأنه بيتجدد في الخلفية من غير أي تدخل منه.

---

* **التحكم من جهة السيرفر:**
  السيرفر بيبقى عنده تحكم كامل في التوكنات، لأنه بيخزن الـ Refresh Tokens في قاعدة البيانات أو أي storage آمن.
  وده يتيح له:

  * يوقف جلسات معينة.
  * يتأكد إن التوكنات مش بتتكرر أو بتُستخدم من أكتر من جهاز بشكل مريب.
  * يحدد سياسات تجديد أو إلغاء دقيقة جدًا.

---

## النتيجة:

استخدام **JWT + Refresh Token** بيجمع بين:

* الأمان العالي
* الراحة في تجربة المستخدم
* المرونة في التحكم من جهة السيرفر

وده السبب إن معظم الأنظمة الكبيرة والمتقدمة بتستخدم التركيبة دي بدل الاعتماد على Access Tokens طويلة الأجل فقط.

---

هل نكمل بقى على **الـ Flow التقني الكامل** بتاع refresh token من أول ما اليوزر يعمل login لحد ما يحصل تجديد للتوكنات؟

---

## خطوات عمل Refresh Token System (نجهزها للـ Implementation)

1.  عند تسجيل الدخول:

   * السيرفر يولّد Access Token + Refresh Token
   * يخزن الريفرش توكن في قاعدة البيانات + يرجعه للعميل (عميل يعني ال Client App)

2.  عند انتهاء صلاحية الـ Access Token:

   * العميل يرسل الريفرش توكن في API مخصصة (مثلاً `/auth/refresh`)
   * السيرفر يتحقق منه:

     * لو صالح → يولّد توكن جديد ويرجعه
     * لو غير صالح → يرجع `401` ويطلب تسجيل الدخول

3.  عند تسجيل الخروج أو إلغاء الجلسة:

   * السيرفر يحذف أو يبطل الريفرش توكن
   * عشان مايتستخدمش بعد كده

---

##  فين بنخزن الريفرش توكن؟

*  **مش جوه الـ JWT نفسه**
*  غالبًا في:

  * قاعدة البيانات (مع UserId و ExpirationDate)
  * ال LocalStorage أو HttpOnly Cookie على العميل

---
## **إمتى تستخدم Refresh Tokens فعلًا؟**

مش كل تطبيق محتاج يستخدم Refresh Tokens. فيه حالات معينة بيكون استخدامها مهم، وحالات تانية بيكون الموضوع مش ضروري وممكن يبقى Overkill.

تعالى نشوف أمثلة واقعية توضح إمتى تحتاجها وإمتى لأ:

---

### **تستخدم Refresh Token في الحالات التالية:**

1. **تطبيق ويب (Web App) بيحتاج صلاحية دخول لفترة طويلة:**

   * زي Gmail أو Facebook، المستخدم بيفضل داخل التطبيق بالشهور.
   * ال Refresh Token هيساعدك تحافظ على الجلسة بدون ما تطلب منه login تاني كل شوية.

2. **تطبيق موبايل بيشتغل Offline في بعض الأوقات:**

   * الموبايل ممكن يكون من غير نت، فلو الـ JWT Token خلص وهو offline، تقدر تستخدم الـ Refresh Token لما يرجع متصل.
   * كمان الموبايل مش دايمًا آمن زي الويب، فالأفضل تخلي الـ access token قصير وتجدده بالـ refresh.

3.  **لو عندك API بتتعامل مع مايكرو سيرفيسز كتير (Microservices):**

   * لما يكون عندك Distributed System، ممكن تحب تقلل التوكنز الطويلة المفعول وتتحكم في تجديدها عبر refresh tokens.
   * ده بيساعدك في السيطرة على كل session حتى في الأنظمة الموزعة.

---

### **غالبًا مش هتحتاج Refresh Token في الحالات دي:**

1. **لو عندك Dashboard داخلي (Admin Panel) أو نظام دخول مرة واحدة فقط:**

   * مثلاً لو التطبيق بيستخدمه موظفين في مكان آمن، وجلساتهم قصيرة ومحدودة.
   * هنا ممكن تخلي التوكن صالح لمدة أطول شوية بدل ما تعقد الأمور بريفرش توكن.

2. **لو الجلسة المفروض عمرها قصير جدًا:**

   * زي بعض الأنظمة الحساسة اللي بتقفل تلقائي بعد دقيقة أو اتنين.
   * مفيش داعي تجدد توكن طول ما الجلسة قصيرة جدًا من الأساس.

---

## الخلاصة:

**استخدم Refresh Tokens لما يكون عندك:**

* تجربة مستخدم مهمة جدًا (ماينفعش تطرده من السيستم بسبب انتهاء التوكن).
* بيانات حساسة محتاجة حماية متقدمة.
* بيئة فيها احتمالات اختراق أو أجهزة غير آمنة.

**ومتستخدمهاش لو:**

* الجلسة قصيرة جدًا.
* أو النظام داخلي وآمن، والـ login بيتكرر قليل جدًا.
