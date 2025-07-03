
## يعني إيه Middleware؟

ال **Middleware** هو **كود بيتنفذ في منتصف البايبلاين بتاع الـ Request/Response**.

بمعنى:
لما بيجي طلب (Request) من المستخدم للسيرفر (مثلاً يفتح صفحة معينة)، الطلب ده بيمشي في سلسلة خطوات (Pipeline) قبل ما يوصل للكود بتاعك اللي بيرجع الـ Response.

الخطوات دي ممكن تكون:

* فحص صلاحيات المستخدم
* تسجيل اللوجات (Logging)
* التعامل مع الأخطاء (Exception Handling)
* ضغط البيانات
* إضافة أو تعديل Headers
* وغيرها

كل خطوة من دول هي **Middleware**.

---

## شكل البايبلاين (Pipeline):

الطلب بيبدأ من المتصفح ويمر على الميدل ويرات واحدة ورا التانية لحد ما يوصل للكود اللي هيعالج الطلب، وبعدين الـ Response بيرجع من نفس السكة بس بالعكس.

```
Request ---> Middleware1 ---> Middleware2 ---> Middleware3 ---> Endpoint (Controller)
                                                           <--- Response
```

---

## في ASP.NET Core

الميدل وير بيتسجل في ملف `Program.cs` أو زمان في `Startup.cs` زي كده:

```csharp
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
```

كل واحدة من دول هي Middleware.

---

## خصائص مهمة للميدل وير:

1. **بتاخد Request، تعمل عليه حاجة، وتقرر هتكمل لباقي السلسلة ولا توقف هنا**.
2. **ممكن تعدّل في الـ Request أو الـ Response**.
3. **ممكن تكتب Middleware بنفسك (Custom Middleware)**.

---

## مثال بسيط – Middleware بيحسب وقت التنفيذ:

```csharp
app.Use(async (context, next) =>
{
    var sw = new Stopwatch();
    sw.Start();

    await next(); // يكمل لباقي الميدل ويرات

    sw.Stop();
    Console.WriteLine($"Request took {sw.ElapsedMilliseconds}ms");
});
```

---

## طيب إمتى أحتاج أعمل Middleware خاص بيا؟

لما يكون عندك منطق مش متعلق مباشرة بالـ Controller بس لازم يتنفذ مع كل أو بعض الطلبات:

* تسجّل الطلبات
* تمنع طلبات معينة
* تضيف Header معين
* تعمل Rate Limiting
* تقيس الأداء

---

## تخيل عاوزين نعمل تسجيل لكل الطلبات اللي بتيجي للموقع يعني عاوزين نعمل (Logging Middleware)

### الفكرة:

أنت عامل Web API أو موقع ASP.NET Core، وعايز **كل طلب (Request)** يدخل السيرفر:

* يتسجل اسمه (Route)
* نوعه (GET, POST)
* التاريخ والوقت
* والمدة اللي أخدها في التنفيذ

وده مطلوب مثلاً لو:

* بتراقب الأداء
* أو بتدور على مشاكل
* أو محتاج تعرف الناس بتطلب إيه أكتر

---

## طب نعمل ده فين؟ هل نحط الكود ده في كل Controller؟

اكيد لأ طبعًا، لأن ده منطق مش يخص الـ Controller نفسه. ده بيخص البنية التحتية.

 يبقى الحل هو ال **Middleware**

---

## الفكرة ببساطة:

هنكتب Middleware عبارة عن كلاس بيعمل الآتي:

1. يستقبل الـ Request
2. يسجّل وقته
3. يكمّل لباقي الميدل ويرات (أو يوصّل للـ Controller)
4. بعد ما الـ Response يرجع، يحسب الوقت
5. يطبع أو يسجّل التفاصيل

---

## الكود العملي:

### أولاً: نكتب Middleware في كلاس

```csharp
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var method = context.Request.Method;
        var path = context.Request.Path;
        var time = DateTime.Now;

        // خلي الـ request يكمل لباقي ال pipeline
        await _next(context);

        stopwatch.Stop();

        Console.WriteLine($"[{time}] {method} {path} took {stopwatch.ElapsedMilliseconds}ms");
    }
}
```

---

### ثانيًا: نسجل الـ Middleware في البايبلاين (Program.cs)

```csharp
app.UseMiddleware<RequestLoggingMiddleware>();
```

---

## شكل الطلب دلوقتي هيبقى كده:

* لما ييجي request مثلاً: `GET /books`
* الـ Middleware هيشوفه، يبدأ يعد الوقت، ويكمّل
* لما الـ Response يرجع، يحسب الزمن الي خده ويطبع في الكونسول:

```
[2025-07-03 12:00:55] GET /books took 56ms
```

---

## طب لو عندي Middleware تاني بعد كده بيمنع الناس اللي مش مسجلين؟

ممكن تحط واحد تاني بعديه:

```csharp
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<RequireAuthenticationMiddleware>();
```

يبقى الطلب يعدي على الأول، بعدين التاني، وبعدين يوصل للـ Controller.

---

## سيناريو تاني Block Requests من IP معين (حظر IPs)

###  الهدف:

تحظر طلبات جايه من IP معين (مثلاً IP بيعمل سبام أو اتاك علي الموقع).

### الكود:

```csharp
public class IpBlockingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HashSet<string> _blockedIps = new() { "192.168.1.100", "10.0.0.5" };

    public IpBlockingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString();

        if (_blockedIps.Contains(ip))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access Denied");
            return;
        }

        await _next(context);
    }
}
```

### في `Program.cs`:

```csharp
app.UseMiddleware<IpBlockingMiddleware>();
```

---

## سيناريو كمان: Rate Limiting – تحديد عدد الطلبات

### الهدف:

لو المستخدم بعت أكتر من عدد معين من الطلبات في وقت قصير (مثلاً 100 طلب في الدقيقة)، نمنعه مؤقتًا.

### الكود (بسيط ومش production-ready):

```csharp
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private static Dictionary<string, (int Count, DateTime Timestamp)> _requests = new();

    private const int LIMIT = 5;
    private static readonly TimeSpan TIME_WINDOW = TimeSpan.FromSeconds(30);

    public RateLimitingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString();
        var now = DateTime.UtcNow;

        if (_requests.ContainsKey(ip))
        {
            var (count, timestamp) = _requests[ip];

            if (now - timestamp < TIME_WINDOW)
            {
                if (count >= LIMIT)
                {
                    context.Response.StatusCode = 429; // Too Many Requests
                    await context.Response.WriteAsync("Too many requests. Please try again later.");
                    return;
                }

                _requests[ip] = (count + 1, timestamp);
            }
            else
            {
                _requests[ip] = (1, now);
            }
        }
        else
        {
            _requests[ip] = (1, now);
        }

        await _next(context);
    }
}
```

### في `Program.cs`:

```csharp
app.UseMiddleware<RateLimitingMiddleware>();
```

###  ملاحظة:

ده مثال توضيحي بس. في بيئة حقيقية محتاج تستخدم Redis أو MemoryCache مع Lock أو Semaphore علشان تتجنب مشاكل الـ Thread Safety.

---

## سيناريو كمان تعديل الـ Headers على كل Response

### الهدف:

عايز تضيف Header معين على كل Response، زي:

* `X-Powered-By: TROJAN`
* أو تفعيل CORS أو CSP

### الكود:

```csharp
public class AddCustomHeaderMiddleware
{
    private readonly RequestDelegate _next;

    public AddCustomHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Add("X-Powered-By", "LitraLand");
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            return Task.CompletedTask;
        });

        await _next(context);
    }
}
```

### في `Program.cs`:

```csharp
app.UseMiddleware<AddCustomHeaderMiddleware>();
```

---

## خلاصة سريعة

| السيناريو              | الفايدة            | هل مكانه Controller؟ | الحل       |
| ---------------------- | ------------------ | -------------------- | ---------- |
| حظر IP                 | تأمين التطبيق      | ❌                    | Middleware |
| Rate Limiting          | منع الـ Abuse      | ❌                    | Middleware |
| تعديل Response Headers | إضافة إعدادات عامة | ❌                    | Middleware |
| تسجيل الطلبات          | تتبع الأداء        | ❌                    | Middleware |

---

طب ازاي  نعملهم كـ  `extension method` يعني نستخدمهم في `Program.cs` زي كدة `app.UseCustomStuff()`

---
```csharp
app.UseRequestLogging();
app.UseIpBlocking();
app.UseRateLimiting();
```

وده الشكل اللي ASP.NET Core نفسه بيستخدمه مع الميدل ويرات الجاهزة زي:

```csharp
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
```

---

## الخطوات:

### 1. اكتب الميدل وير ككلاس عادي (زي ما عملنا قبل كده):

مثلًا: `RequestLoggingMiddleware`

```csharp
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = new Stopwatch();
        sw.Start();

        await _next(context);

        sw.Stop();
        Console.WriteLine($"[LOG] {context.Request.Method} {context.Request.Path} took {sw.ElapsedMilliseconds}ms");
    }
}
```

---

### 2. اعمل Extension Class خاص بالـ Middleware ده

في ملف منفصل مثلاً اسمه: `RequestLoggingMiddlewareExtensions.cs`

```csharp
public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}
```

---

### نفس الفكرة تتكرر مع أي Middleware تاني:

#### مثال: IP Blocking

##### `IpBlockingMiddleware.cs`:

```csharp
public class IpBlockingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HashSet<string> _blockedIps = new() { "192.168.1.100" };

    public IpBlockingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString();

        if (_blockedIps.Contains(ip))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access Denied");
            return;
        }

        await _next(context);
    }
}
```

##### `IpBlockingMiddlewareExtensions.cs`:

```csharp
public static class IpBlockingMiddlewareExtensions
{
    public static IApplicationBuilder UseIpBlocking(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<IpBlockingMiddleware>();
    }
}
```

---

### 3. تستخدمهم في `Program.cs` ببساطة:

```csharp
app.UseRequestLogging();
app.UseIpBlocking();
app.UseRateLimiting();
app.UseAddCustomHeader();
```

---

## بس خلي بالك ترتيب التسجيل مهم!

* مثلًا لو حطيت `UseIpBlocking()` بعد `UseRouting()` مش هيشتغل صح.
* دايمًا خلي الميدل ويرات اللي بتفحص الطلب أو تمنعه، **في الأول**.

---

##  فايدة الـ Extension Methods:

* بتنظف الكود في `Program.cs`
* بتخليك تفصل الميدل وير عن ملف التشغيل
* سهلة إعادة الاستخدام
* بتخلي مشروعك شكله احترافي ومنظم

---
