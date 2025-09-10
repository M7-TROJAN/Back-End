

> "أيوة طبعًا، مبادئ الـ SOLID هي خمس مبادئ أساسية في البرمجة الكائنية الهدف منها تخلي الكود أسهل في الصيانة، قابل للتوسع، وكمان مرن.
>
> * الـ **S – Single Responsibility Principle**: يعني الكلاس يبقى ليه مسؤولية واحدة بس، وميعملش كل حاجة في نفس الوقت.
>
> * الـ **O – Open/Closed Principle**: الكود يبقى مفتوح للإضافة لكن مقفول للتعديل، يعني أقدر أزود عليه Features جديدة من غير ما أغير في الكود الأساسي.
>
> * الـ **L – Liskov Substitution Principle**: أي كلاس ابن ينفع يتحط مكان الكلاس الأب من غير ما يبوظ السلوك.
>
> * الـ **I – Interface Segregation Principle**: ميكونش عندي Interface ضخم فيه حاجات مش كل الكلاسات محتاجاها، لأ أقسمه لعدة Interfaces صغيرة.
>
> * الـ **D – Dependency Inversion Principle**: الكلاسات تعتمد على Abstractions (Interfaces) مش على Implementations، وده بيخلي الكود مرن وأسهل في الاختبار."
>
> "فأنا لما باجي أطبق الـ SOLID في مشاريعي، بلاحظ إن الكود بيبقى Organized أكتر، سهل أعمل له Maintain أو أغير فيه بعدين، وكمان Testing بيبقى أبسط."

---

ال Transient = زي كوباية بلاستيك بتترمي بعد كل شرب.
ال Scoped = زي كوباية زجاج بتستخدمها طول اليوم بس بتغسلها آخره.
ال Singleton = زي فلتر المياه في البيت، بتستخدمه دايمًا ومش بتغيره إلا بعد شهور.
## 🔹 1. Transient Example

### 🔸 Scenario:

Service بتعمل **Password Generator** أو **Email Formatter**.

* الحاجات دي بتشتغل مرة واحدة بسرعة، ومش محتاج تخزن حالتها.
* كل ما تحتاجها هتعمل Object جديد وخلاص.

```csharp
public interface IPasswordGenerator
{
    string GeneratePassword(int length);
}

public class PasswordGenerator : IPasswordGenerator
{
    public string GeneratePassword(int length)
    {
        return Guid.NewGuid().ToString().Substring(0, length);
    }
}

// Registration
services.AddTransient<IPasswordGenerator, PasswordGenerator>();
```

👉 هنا **Transient** أحسن لأنه كل استدعاء هيطلعلك Password جديد من غير ما يشيل state.

---

## 🔹 2. Scoped Example

### 🔸 Scenario:

**Shopping Cart Service** جوة E-commerce.

* طول ما الـ request شغال (المستخدم بيضيف منتجات) يفضل نفس الـ cart.
* لما يخلص request، cart جديدة تتعمل.

```csharp
public interface IShoppingCart
{
    void AddItem(string item);
    List<string> GetItems();
}

public class ShoppingCart : IShoppingCart
{
    private readonly List<string> _items = new();
    public void AddItem(string item) => _items.Add(item);
    public List<string> GetItems() => _items;
}

// Registration
services.AddScoped<IShoppingCart, ShoppingCart>();
```

👉 ال **Scoped** هنا ممتاز لأنه بيضمن إن الـ cart هتفضل ثابتة طول عمر الـ request، لكن مش هتنتقل بين المستخدمين.

---

## 🔹 3. Singleton Example

### 🔸 Scenario:

ال **Configuration Service** أو **Logging Service**.

* القيم ثابتة ومش محتاجة تتغير.
* أو logging عايز نفس الـ instance عشان يبقى مركزي.

```csharp
public interface IAppConfiguration
{
    string GetApplicationName();
}

public class AppConfiguration : IAppConfiguration
{
    private readonly string _appName;
    public AppConfiguration()
    {
        _appName = "Survey Basket App"; // fixed value, read once
    }

    public string GetApplicationName() => _appName;
}

// Registration
services.AddSingleton<IAppConfiguration, AppConfiguration>();
```

👉 هنا **Singleton** منطقي جدًا، لأنه مش محتاج كل شوية يعمل Object جديد عشان يجيب نفس الاسم.

---

## 🟢 الخلاصة:

* ال **Transient:** Utility services اللي مش بتخزن حالة – زي Password Generator, Email Sender Helper.
* ال **Scoped:** Services اللي ليها علاقة بالـ request – زي ShoppingCart, DbContext.
* ال **Singleton:** Services الثابتة/المركزية – زي Configurations, Cache, Logger.
