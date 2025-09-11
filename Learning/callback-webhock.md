
## 🎤 ردك في الانترفيو (بالعربي)

> "أيوة طبعًا، بوابات الدفع بيبقى ليها كذا طريقة للـ Integration على حسب نوع المشروع ودرجة الـ control اللي عايزها. غالبًا الطرق الأساسية 3:
>
> 1. **Hosted Payment Page (Redirect Integration):**
>    دي أبسط طريقة، المستخدم بيتحول لصفحة جاهزة من بوابة الدفع، يدخل بيانات الكارت هناك، وبعد كده بيتحول يرجع عندي في Success أو Fail URL.
>    ميزة الطريقة دي إنها آمنة جدًا لأن كل حاجة بتحصل عند مزود الخدمة.
>
> 2. **Embedded Checkout (iFrame/JS SDK):**
>    هنا النموذج (form) بتاع الدفع بيظهر جوا موقعي عن طريق iFrame أو JavaScript SDK.
>    كده المستخدم بيفضل في موقعي، لكن البطاقات بتتبعت مباشرة للـ Gateway.
>    دي بتدي User Experience أحسن شوية، ولسه آمنة.
>
> 3. **Direct API Integration (Server-to-Server):**
>    هنا أنا اللي بعمل Collect للبيانات وأبعتها مباشرة للـ Payment Gateway API.
>    الطريقة دي بتديني Full Control في الواجهة وتجربة المستخدم، بس أنا اللي مسؤول عن الأمان بالكامل (زي PCI DSS compliance).
>    غالبًا الشركات الكبيرة أو التطبيقات الموبايل هي اللي بتحتاجها."

---

## 🟢 التوضيح أكتر لك (عشان تفهمها بعمق):

1. **Redirect (Hosted Page):**

   * زي ما PayPal و Fawry بيعملوا.
   * العميل يتوجه برة عندهم، يدخل بياناته، يرجعلك.
   * أكتر حاجة أمان وأقل صداع.

2. **iFrame/Widget/JS Plugin:**

   * زي Fawaterk plugin اللي أنت لسه كنت بتجربه.
   * بيعرضلك الـ checkout جوا صفحتك.
   * بيديك مرونة في التصميم + تجربة مستخدم أحسن.

3. **Direct API:**

   * أنت تبني UI بتاعك بالكامل.
   * تبعت بيانات الكروت أو التوكين للـ Gateway.
   * محتاجة شهادات أمان (PCI DSS) وتشفير.
   * مناسبة للشركات اللي عايزة Full control (زي Uber أو Amazon).

---

## 📝 الخلاصة اللي تقولها في الانترفيو:

> "الـ integration ممكن يتعمل بثلاث طرق رئيسية: Redirect لصفحة الدفع، Embedded iFrame/JS SDK، أو Direct API Integration. كل طريقة ليها مميزات وعيوب، والاختيار بيعتمد على حجم المشروع ومتطلباته من حيث الأمان وتجربة المستخدم."

---



## 4️⃣ أنواع الـ Payment Integration

ا 1. **Hosted Payment Page (Redirect)**

   * العميل بيتحوّل على صفحة الدفع بتاعة الـ Gateway (زي PayPal, Fawry).
   * مميزاتها: سهلة ومأمونة.
   * عيبها: تجربة المستخدم بتطلع بره موقعك.

ا 2. **Embedded Form / iFrame**

   * فورم من الـ Gateway يتعرض جوة موقعك.
   * العميل مش بيسيب موقعك.
   * أمان متوسط (أنت مش بتخزن بيانات الكارت، بس بتعرضها).

ا 3. **Direct API Integration**

   * موقعك ياخد بيانات الكارت ويبعتها للـ Gateway API.
   * محتاج PCI DSS compliance (مسؤولية أعلى).
   * بتحكم أكتر في تجربة المستخدم.

---

## 5️⃣ خطوات عملية الدمج (Integration Steps)

1. **Sign up with Payment Provider** (Stripe, PayPal, Paymob, Fawry).
2. **Get API Keys** (Public Key, Secret Key).
3. **Test Environment (Sandbox)**: الأول بتجرب على بيئة تجريبية.
4. **Frontend**: تبني الفورم أو تدمج الـ SDK.
5. **Backend**:

   * تبعت بيانات العملية للـ Gateway API.
   * تستقبل الـ response (approved/declined).
   * تخزن النتيجة في DB.
ا 6. **Handle Webhooks**: علشان تعرف لو الدفع اتأكد أو فيه حاجة حصلت بعدين.
ا 7. **Go Live**: ترفع على الـ production بعد موافقة الـ provider.

---

## 6️⃣ أمثلة لمزودين مشهورين

* عالمي: **Stripe, PayPal, Braintree, Adyen, Authorize.net**
* محلي (مصر): **Paymob, Fawry, Accept, Vodafone Cash, Meeza**

---

## 7️⃣ Challenges بتظهر في الانترفيو

* **الأمان**: ليه منخزنش بيانات الكارت؟ (علشان PCI DSS).
* ا **Handling Failures**: لو العملية فشلت (declined) بتعمل retry إزاي؟
* ا **Idempotency**: إزاي تمنع double payment؟ (باستخدام transaction id).
* ا **Refunds/Chargebacks**: الفرق بينهم.
* ا **Currency Conversion**: التعامل مع multi-currency.
* ا **Recurring Payments**: إزاي تعمل اشتراكات بدون ما تعيد إدخال الكارت؟ (Tokenization).

---

## 8️⃣ إجابات سريعة ممكن تيجي في الانترفيو

🔹 *ليه بنستخدم Payment Gateway؟*
→ علشان يوفر أمان (Encryption, PCI DSS)، يسهل الدمج (API/SDK)، ويعمل كوسيط بين التاجر والبنوك.

🔹 *إيه الفرق بين Authorization و Capture؟*
→ا Authorization بيعمل hold للفلوس. Capture بيخصمها فعليًا.

🔹 *إيه الفرق بين Refund و Chargeback؟*
ا → Refund بيرجّع فلوس من التاجر للعميل. Chargeback العميل نفسه يعترض عند البنك، والبنك يسحب الفلوس بالقوة.

🔹 *إيه فوايد الـ Tokenization؟*
→ بدل ما تخزن card number، بتخزن token. ده بيقلل مخاطر الاختراق.

🔹 *إيه فوايد Webhooks؟*
→ تعرف تحديث حالة الدفع (success/fail/refund) من غير ما تعتمد بس على response أولي.

---
## 1️⃣ يعني إيه Callback / Webhook؟

* ال **Callback** = عملية استدعاء بيرجعلك فيها السيرفر بتاع بوابة الدفع بالنتيجة (نجح الدفع / فشل / ملغي).
* ال **Webhook** = طريقة (URL) انت بتجهزها في سيستمك → بوابة الدفع بتبعت عليها **Request (POST)** فيه تفاصيل العملية بعد ما العميل يخلص الدفع.

بمعنى: بدل ما تفضل تسأل البوابة كل شوية “هو العميل دفع ولا لسه؟” → هم بنفسهم يبعتولك إشعار (Notification) أول ما يحصل تغيير في حالة الدفع.

---

## 2️⃣ مثال واقعي

* محمود يطلب كتاب من موقعك.
* انت تعمل **Invoice** في Paymob أو PayTabs.
* محمود يدفع بكارت فيزا.
* البوابة تعمل اتنين:

  1. توري محمود رسالة “تم الدفع بنجاح ✅”.
  2. تبعتلك على **Webhook URL** اللي انت معرفه عندهم (زي: `https://yourdomain.com/api/payment/callback`) → JSON فيه:

     * ال `transaction_id`
     * ال `order_id`
     * ال `status` (paid, failed, pending)
     * ال `amount`

انت تاخد البيانات دي وتحدث الـDB عندك:

* لو **paid** → تغير حالة الطلب لـ "مدفوع".
* لو **failed** → تعمله cancel أو retry.

---

## 3️⃣ الفرق بين Callback و Redirect

* ال **Redirect URL**: بعد ما العميل يدفع، بيتحول لصفحة عندك (مثلاً `/payment/success`). ده بس عشان يوريه رسالة.
* ال **Webhook/Callback**: ده اللي عليه العُمدة → لأنه بيبعتلك من البوابة نفسها إشعار رسمي إن الفلوس اتدفعت. (ممكن العميل يقفل التاب أو النت يقطع، لكن الـWebhook هيجيلك برضه).

---

## 4️⃣ ا

 **إيه هو الـWebhook وليه مهم في الدفع الإلكتروني؟**
   – هو URL بيبعت عليه مزوّد الدفع تفاصيل العملية بعد ما تخلص، عشان أقدر أعمل update في السيستم بتاعي بشكل آلي. وهو مهم لأنه يضمن إن السيستم يسجل الدفع حتى لو العميل ما رجعش للـredirect page.

 **إزاي تأمّن الـWebhook؟**
   – أتحقق من التوقيع (signature أو HMAC) اللي بتبعت مع البيانات. كل بوابة بتديك secret key تستخدمه للتحقق إن الريكوست جاي فعلًا منهم مش من هاكر.

 **إيه اللي ممكن يحصل لو ما استخدمتش Webhook؟**
   – مش هتعرف حالة الدفع بدقة. ممكن يبقى العميل دفع لكن ما رجعش للصفحة → فتفتكر إنه ما دفعش.

---


مثال عملي في **ASP.NET Core Web API** لعمل Webhook Endpoint خاص بالدفع:

---

##  الخطوات

1. هتجهز **Controller** فيه Action تستقبل POST من بوابة الدفع.
2. هتقرأ **JSON** اللي جاي من البوابة.
3. هتتحقق من الـ **Signature** (لو البوابة بتوفره) عشان تتأكد إن الريكوست صحيح.
4. تحدث حالة الطلب في الـDatabase.

---

##  الكود

```csharp
using Microsoft.AspNetCore.Mvc;

namespace PaymentIntegrationDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentWebhookController : ControllerBase
    {
        [HttpPost("callback")]
        public IActionResult PaymentCallback([FromBody] PaymentWebhookRequest request)
        {
            // Example: Log or process the payment data
            if (request.Status == "paid")
            {
                // TODO: Update order in database
                Console.WriteLine($"Order {request.OrderId} has been paid successfully.");
            }
            else if (request.Status == "failed")
            {
                // TODO: Handle failed payment
                Console.WriteLine($"Order {request.OrderId} payment failed.");
            }

            // Return 200 OK to acknowledge receipt
            return Ok(new { message = "Webhook received successfully" });
        }
    }

    // This is a model to map the JSON body from the payment gateway
    public class PaymentWebhookRequest
    {
        public string TransactionId { get; set; }
        public string OrderId { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
    }
}
```

---

## مثال علي ال JSON ممكن يوصلنا من البوابة

```json
{
  "transactionId": "TX123456",
  "orderId": "ORD789",
  "status": "paid",
  "amount": 500.00
}
```

---

##  ملاحظات مهمة

* لازم تعرف الـ **Webhook URL** بتاعك للبوابة (مثلاً: `https://yourdomain.com/api/paymentwebhook/callback`).
* كل بوابة بتختلف في الـ **JSON Structure** → فلازم تطابقه مع الوثائق الرسمية بتاعتهم.
* غالبًا بيبعتوا **Signature Header** زي:

  ```
  X-Signature: HMACSHA256(payload, secret)
  ```

  → ساعتها انت تتحقق منه قبل ما تحدث أي بيانات في السيستم.

---


إزاي تضيف ال Signature Verification في الكود بحيث تتأكد إن الريكوست جاي فعلًا من بوابة الدفع مش من أي حد تاني؟ ودي النقطة الأهم في موضوع الـ **Webhook Security**. لأن أي حد ممكن يعرف الـ endpoint بتاعك ويحاول يبعته ريكوست مزيف يغيّر حالة الطلب.

---

## الفكرة الأساسية

1. بوابة الدفع بتبعتلك **Body** (الـ JSON).
2. معاها تبعتلك **Header** فيه توقيع (Signature).
3. التوقيع ده معمول باستخدام خوارزمية زي **HMAC-SHA256** بالـ **Secret Key** اللي أنت واخده منهم.
4. انت تعيد حساب التوقيع بنفسك → وتقارن مع اللي جالك في الـHeader.

   * لو متطابق ✅ ← يبقى الريكوست صحيح.
   * لو مختلف ❌ ← ترفض الريكوست.

---

## مثال عملي في ASP.NET Core

```csharp
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace PaymentIntegrationDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentWebhookController : ControllerBase
    {
        private const string SecretKey = "your-secret-key-from-gateway";

        [HttpPost("callback")]
        public async Task<IActionResult> PaymentCallback()
        {
            // Read raw body
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            // Get signature from headers (example: X-Signature)
            var signatureHeader = Request.Headers["X-Signature"].FirstOrDefault();

            if (!VerifySignature(body, signatureHeader))
            {
                return Unauthorized(new { message = "Invalid signature" });
            }

            // Deserialize JSON body
            var request = System.Text.Json.JsonSerializer.Deserialize<PaymentWebhookRequest>(body);

            if (request.Status == "paid")
            {
                Console.WriteLine($"✅ Order {request.OrderId} paid successfully!");
                // TODO: Update DB (mark order as paid)
            }
            else
            {
                Console.WriteLine($"❌ Payment failed for Order {request.OrderId}");
                // TODO: Handle failed payment
            }

            return Ok(new { message = "Webhook received successfully" });
        }

        private bool VerifySignature(string body, string? signatureHeader)
        {
            if (string.IsNullOrEmpty(signatureHeader))
                return false;

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(body));
            var computedSignature = Convert.ToHexString(hash).ToLower();

            return computedSignature == signatureHeader.ToLower();
        }
    }

    public class PaymentWebhookRequest
    {
        public string TransactionId { get; set; }
        public string OrderId { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
    }
}
```

---

##  شرح سريع للكود

* ال `SecretKey`: ده الـ secret اللي بوابة الدفع بتدهولك.
* ال `VerifySignature`: بتحسب HMAC-SHA256 من الـ body وتقارنه باللي في الـHeader.
* لو الـSignature صحيح ← تكمّل وتعمل Update في DB.
* لو غلط ← بترجع `401 Unauthorized`.

---

##  سؤال متوقع في الإنترفيو

**س: إزاي تضمن أمان الـWebhook؟**
ج: عن طريق التحقق من التوقيع (Signature Verification) باستخدام HMAC والـ Secret Key، وكمان بتسمح فقط لبوابة الدفع بالوصول (ممكن أفلتر بالـ IPs (الايبيهات يعني) لو الشركة بتدي رينج معين).

---

* أي Request بيوصلك على السيرفر لازم يكون جاي من عنوان إنترنت (IP Address).
* بوابة الدفع عندها **سيرفرات محددة** هي اللي هتبعتلك الـ Webhook.
* الشركات الكبيرة زي **Stripe / PayPal / Paymob** ساعات بتنشر **قائمة بالـ IP ranges** اللي الريكوستات هتيجي منها.

---

## إزاي تستخدم الـ IPs في الأمان؟

* لما يوصلك Webhook ← قبل ما تعالجه ← تشوف **IP بتاع المرسل**.
* لو الـIP مش من ضمن القائمة الموثوقة اللي البوابة أعلنتها ← برفضه فورًا .

---

## مثال عملي في ASP.NET Core

```csharp
using Microsoft.AspNetCore.Mvc;

namespace PaymentIntegrationDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentWebhookController : ControllerBase
    {
        private static readonly string[] AllowedIps = new[]
        {
            "52.11.22.33",    // Example: Gateway IP
            "34.210.55.120"   // Example: Another Gateway IP
        };

        [HttpPost("callback")]
        public IActionResult PaymentCallback([FromBody] PaymentWebhookRequest request)
        {
            var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (!AllowedIps.Contains(remoteIp))
            {
                return Unauthorized(new { message = "Unauthorized IP address" });
            }

            // TODO: Verify signature + process payment
            return Ok(new { message = "Webhook received successfully" });
        }
    }

    public class PaymentWebhookRequest
    {
        public string TransactionId { get; set; }
        public string OrderId { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
    }
}
```

---

## سؤال إنترفيو متوقع

**س: ليه ممكن تحتاج تعمل IP Filtering للـ Webhook؟**

* عشان تزود طبقة أمان إضافية. حتى لو حد عرف الـ Secret أو حاول يبعث Request مزيف، مش هيقدر يوصل إلا لو جاي من IP مسموح بيه تابع لبوابة الدفع.

---

 كده عندك خط دفاع مزدوج:

 ال **Signature Verification (HMAC)**
 وال **IP Whitelisting**

لو الاتنين صح ← الريكوست سليم.
لو واحد فيهم فشل ← ترفض.

---

## مثال 1. **Paymob (بوابة دفع مصرية شهيرة)**

### شكل Webhook بيجي من Paymob

بعد ما العميل يدفع، Paymob بتبعتلك JSON كده:

```json
{
  "id": 123456789,
  "amount_cents": 10000,
  "currency": "EGP",
  "order": {
    "id": 987654321,
    "merchant_order_id": "ORD123"
  },
  "success": true,
  "is_voided": false,
  "is_refunded": false,
  "created_at": "2025-09-09T14:30:00Z",
  "source_data": {
    "pan": "2346",
    "sub_type": "MasterCard",
    "type": "card"
  }
}
```

###  الأمان عند Paymob

* بيبعتوا مع الريكوست **HMAC Signature** في Header.
* انت لازم تعيد حساب التوقيع بنفس السرّ اللي عندك (Integration Key).
* لو متطابق ✅ → تكمل.

---

## مثال 2. **Stripe (واحدة من أشهر بوابات الدفع عالميًا)**

### شكل Webhook بيجي من Stripe

ال Stripe بتبعت Event كامل، مثلاً الدفع نجح:

```json
{
  "id": "evt_1OzOdh2eZvKYlo2Cgk0c9hfd",
  "object": "event",
  "api_version": "2025-01-01",
  "created": 1725876340,
  "data": {
    "object": {
      "id": "pi_3OzOdG2eZvKYlo2C1vZjqk8Q",
      "object": "payment_intent",
      "amount": 10000,
      "currency": "usd",
      "status": "succeeded"
    }
  },
  "type": "payment_intent.succeeded"
}
```

### الأمان عند Stripe

* بيبعتوا **Stripe-Signature Header**.
* انت لازم تستخدم مكتبتهم أو كود HMAC للتحقق.
* كمان بيقولوا تقدر تحدد **IP ranges** الخاصة بسيرفراتهم لو عايز Double Security.

---

## الفرق بين Paymob و Stripe

| العنصر              | Paymob                                | Stripe                             |
| ------------------- | ------------------------------------- | ---------------------------------- |
| **المكان**          | مصر / MENA                            | عالمي                              |
| **العملة الأساسية** | EGP                                   | USD + كل العملات                   |
| **الـWebhook**      | JSON بسيط مع HMAC                     | JSON مع Event كامل + توقيع         |
| **الأمان**          | HMAC Signature                        | Signature + IP Filtering           |
| **استخدام عملي**    | مناسب للسوق المصري (فيزا/ماستر محلية) | عالمي مع بطاقات وأبل باي وجوجل باي |

---

## سؤال إنترفيو متوقع

**س: لو العميل دفع الفلوس بس النت قطع عنده قبل ما يرجع للـ Redirect URL، إزاي تعرف إن الفلوس دخلت؟**
ج: عن طريق الـ Webhook. البوابة هتبعتلي إشعار رسمي إن حالة العملية = paid حتى لو العميل ما رجعش للصفحة.


---

## 📌 1. شكل JSON من Paymob (Webhook Event)

مثال مبسط:

```json
{
  "id": 123456789,
  "amount_cents": 10000,
  "currency": "EGP",
  "order": {
    "id": 987654321,
    "merchant_order_id": "ORD123"
  },
  "success": true,
  "is_voided": false,
  "is_refunded": false,
  "created_at": "2025-09-09T14:30:00Z",
  "source_data": {
    "pan": "2346",
    "sub_type": "MasterCard",
    "type": "card"
  }
}
```

---

## 📌 2. الكود في ASP.NET Core

```csharp
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PaymentIntegrationDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymobWebhookController : ControllerBase
    {
        private const string SecretKey = "YOUR_PAYMOB_HMAC_SECRET";

        [HttpPost("callback")]
        public async Task<IActionResult> Callback()
        {
            // Read raw body (JSON)
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            // Get HMAC signature from headers
            var signature = Request.Headers["hmac"].FirstOrDefault();

            if (!VerifyHmac(body, signature))
            {
                return Unauthorized(new { message = "Invalid HMAC signature" });
            }

            // Deserialize JSON
            var request = JsonSerializer.Deserialize<PaymobWebhookRequest>(body);

            if (request.Success)
            {
                Console.WriteLine($"✅ Order {request.Order.MerchantOrderId} paid successfully!");
                // TODO: Update order status in DB
            }
            else
            {
                Console.WriteLine($"❌ Payment failed for Order {request.Order.MerchantOrderId}");
                // TODO: Handle failed payment
            }

            return Ok(new { message = "Webhook received" });
        }

        private bool VerifyHmac(string body, string? receivedSignature)
        {
            if (string.IsNullOrEmpty(receivedSignature))
                return false;

            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(SecretKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(body));
            var computedSignature = BitConverter.ToString(hash).Replace("-", "").ToLower();

            return computedSignature == receivedSignature.ToLower();
        }
    }

    // Map Paymob Webhook JSON
    public class PaymobWebhookRequest
    {
        public long Id { get; set; }
        public int Amount_Cents { get; set; }
        public string Currency { get; set; }
        public PaymobOrder Order { get; set; }
        public bool Success { get; set; }
        public bool Is_Voided { get; set; }
        public bool Is_Refunded { get; set; }
        public DateTime Created_At { get; set; }
        public SourceData Source_Data { get; set; }
    }

    public class PaymobOrder
    {
        public long Id { get; set; }
        public string MerchantOrderId { get; set; }
    }

    public class SourceData
    {
        public string Pan { get; set; }
        public string Sub_Type { get; set; }
        public string Type { get; set; }
    }
}
```

---

## 📌 3. الملاحظات المهمة

* لازم تجيب الـ **HMAC Secret Key** من Paymob Dashboard.
* ال Paymob بيبعت الـ HMAC في Header اسمه `hmac`.
* استخدم نفس الـ Algorithm اللي Paymob بتقول عليه (في Docs بتاعتهم أغلب الوقت HMAC-SHA512).
* بعد ما تتحقق من التوقيع وتعمل Deserialize → حدّث الطلب في الـDB.

---

## سؤال إنترفيو متوقع

**س: إزاي تقدر تتأكد إن الـ Webhook اللي جالك من Paymob مش Fake؟**

* أرد:

  1. أتحقق من HMAC Signature باستخدام الـ Secret Key.
  2. أفلتر الـ IPs (لو Paymob نشرت رينج IPs).
  3. أسجل كل Webhook للـ Auditing عشان أقدر أراجع بعدين.
