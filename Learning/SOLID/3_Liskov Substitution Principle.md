
# ✅ **L — Liskov Substitution Principle (LSP)**

### "مبدأ استبدال ليسكوف"
وهو من أهم المبادئ اللي بتمنع الكوارث البرمجية اللي بتحصل لما تشتغل بـ Inheritance من غير ما تكون فاهم كويس.
---

## التعريف الرسمي:

> **“Objects of a superclass should be replaceable with objects of its subclasses without breaking the behavior of the program.”**

يعني إيه؟

> أي كود بيشتغل بكائن من الكلاس الأساسي (Base class)، المفروض يشتغل **بنفس الطريقة** لو استبدلناه بكائن من كلاس فرعي (Derived class).

---

## الفكرة ببساطة:

لما تورّث كلاس من كلاس تاني، لازم تتأكد إن **الكلاس الابن يقدر يحل مكان الأب** من غير ما "يبوّظ" سلوك التطبيق.

يعني ما ينفعش تدي كائن من `Dog` في مكان محتاج `Animal`، ويفاجئك إنه مابيهوهوش، أو بيرفرف زي العصفور!

---

## لو كسرت المبدأ ده، تحصل مصايب زي:

* ال **Runtime bugs** مش واضحة من الكود.
* **سلوك غير متوقع** في الأماكن اللي بتستخدم الـ base class.
* **اختبارات Unit Tests بتبوظ فجأة** لما تضيف Class جديد.

---

## مثال بسيط (بيكسر LSP):

### تخيل إن عندك كلاس للمستطيل:

```csharp
public class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }

    public int Area()
    {
        return Width * Height;
    }
}
```

### وقررت تعمل كلاس `Square` يورّث من `Rectangle`:

```csharp
public class Square : Rectangle
{
    public override int Width
    {
        set
        {
            base.Width = value;
            base.Height = value;
        }
    }

    public override int Height
    {
        set
        {
            base.Width = value;
            base.Height = value;
        }
    }
}
```

### تفتكر الكود ده هيشتغل كويس؟

لأ، لأنك لو كتبت كود بيستخدم `Rectangle`:

```csharp
Rectangle r = new Square();
r.Width = 5;
r.Height = 10;

Console.WriteLine(r.Area()); // الناتج؟ مش 50، لأنه هيحسب 10x10!
```

هنا حصل خرق لمبدأ LSP. ليه؟

* لأنك استخدمت كائن `Square` في مكان متوقع `Rectangle`، لكن السلوك اتغير بشكل خفي.

---

##  الحل: لا تورّث لما يكون السلوك مختلف كليًا!

لو `Square` سلوكها مختلف عن `Rectangle`، فـ **ماينفعش تورّثها منه**.

بدل كده، اعمل Interface زي:

```csharp
public interface IShape
{
    int Area();
}
```

وكل كلاس (مربع، مستطيل، دائرة...) يطبّق الـ Interface بالسلوك الخاص بيه.

---

## سيناريو حقيقي الطباعة (Printer)

### الكلاس الأساسي:

```csharp
public class DocumentPrinter
{
    public virtual void Print(Document doc)
    {
        Console.WriteLine("Printing document...");
    }
}
```

### كلاس جديد: `ReadOnlyPrinter`

```csharp
public class ReadOnlyPrinter : DocumentPrinter
{
    public override void Print(Document doc)
    {
        throw new NotSupportedException("This printer doesn't support printing");
    }
}
```

 كده كأنك بتقول:

> "أنا ورّثت كلاس، بس استخدمه غلط وهيكسر البرنامج"

### الحل؟

* ما تورّثش.
* خليه يطبّق Interface تاني، أو استخدم ** ال Composition** بدل Inheritance.

---

## إزاي أعرف إني كسرت مبدأ LSP؟

اسأل نفسك:

> لو استبدلت الـ base class بالـ derived class، هل سلوك البرنامج يفضل هو هو؟

لو لأ:
 إنت كده كسرت مبدأ LSP.

---

##  ملخص القاعدة الذهبية:

> **Don't override behavior to break assumptions of the base class.**

 الكلاس الابن لازم يلتزم بكل شروط وسلوكيات الكلاس الأب.
 ماينفعش يعدّل في السلوك الأساسي بطريقة غير متوقعة.

---

##  إزاي أطبق LSP صح في شغلي؟

| غلط                                 | صح                                           |
| ----------------------------------- | -------------------------------------------- |
| تورّث من كلاس وتكسر وظائفه          | ورّث بس لما تكون فعلاً بتوسّع مش بتكسر       |
| تعرّف كلاس ابن يغيّر السلوك الأساسي | خليه يطبّق Interface منفصل لو سلوكه مختلف    |
| تورّث كلاس عام لكل حاجة             | فكّر في Interface أو Composition بدل التوريث |

---

## مثال Web واقعي:

### عندك:

```csharp
public abstract class PaymentMethod
{
    public abstract void Pay(decimal amount);
}
```

### وبتورّث:

```csharp
public class Visa : PaymentMethod
{
    public override void Pay(decimal amount) => Console.WriteLine("Paid with Visa");
}

public class Cash : PaymentMethod
{
    public override void Pay(decimal amount) => Console.WriteLine("Paid with Cash");
}

public class Installment : PaymentMethod
{
    public override void Pay(decimal amount)
    {
        throw new NotSupportedException("Installments not supported yet");
    }
}
```

هنا فيه خرق للمبدأ.
 الحل؟ خليه Interface مختلف، أو اعمل Strategy منفصلة:

```csharp
public interface IPaymentStrategy
{
    void Pay(decimal amount);
}
```
