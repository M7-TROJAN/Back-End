
# System.Linq.Dynamic Package

The `System.Linq.Dynamic` package provides a way to dynamically construct LINQ queries using strings at runtime. This package is very useful when working with dynamic data, like building filters, orderings, and projections based on user input. Below, we’ll cover all the key methods, properties, and practical real-world scenarios with extensive examples.

## Getting Started

To install the package, use the following NuGet command:

```bash
Install-Package System.Linq.Dynamic.Core
```

After installing, include the namespace in your code:

```csharp
using System.Linq.Dynamic.Core;
```

## Key Features

### 1. Dynamic Query Methods

The `System.Linq.Dynamic` package extends `IQueryable` and `IEnumerable` with several powerful dynamic query methods:

- `Where`
- `OrderBy`
- `Select`
- `GroupBy`
- `Join`
- `Any` / `All`

Each of these methods accepts a string expression, allowing dynamic query construction.

### 2. Dynamic Filtering with `Where`

The `Where` method enables filtering based on a dynamic string expression.

#### Example:
```csharp
var data = new List<User>
{
    new User { Name = "Alice", Age = 25 },
    new User { Name = "Bob", Age = 30 },
    new User { Name = "Charlie", Age = 35 }
}.AsQueryable();

var filteredData = data.Where("Age > 30");
```

In this example, the filter `"Age > 30"` dynamically filters users whose age is greater than 30.

### 3. Dynamic Sorting with `OrderBy`

The `OrderBy` method lets you sort based on a string expression.

#### Example:
```csharp
var sortedData = data.OrderBy("Name DESC");
```

Here, `"Name DESC"` sorts the data in descending order by `Name`.

### 4. Dynamic Selection with `Select`

The `Select` method projects the data into a new form based on a dynamic expression.

#### Example:
```csharp
var projectedData = data.Select("new(Name, Age)");
```

This returns only the `Name` and `Age` properties of each `User`.

### 5. Dynamic Grouping with `GroupBy`

The `GroupBy` method allows grouping items by a specific property dynamically.

#### Example:
```csharp
var groupedData = data.GroupBy("Age");
```

This groups the data by the `Age` property.

## Real-World Scenarios

### Scenario 1: Building a Search Filter for a Web API

In a web application, you might need to create an endpoint where users can apply filters dynamically based on query parameters.

```csharp
public IQueryable<User> GetUsers(string filter)
{
    var data = _context.Users.AsQueryable();
    return string.IsNullOrEmpty(filter) ? data : data.Where(filter);
}
```

This method enables dynamic filtering based on the `filter` string provided.

### Scenario 2: Sorting Data Based on User Input

Suppose you have a UI where users can select a column to sort by.

```csharp
public IQueryable<User> GetUsersSorted(string sortBy)
{
    var data = _context.Users.AsQueryable();
    return string.IsNullOrEmpty(sortBy) ? data : data.OrderBy(sortBy);
}
```

Here, `sortBy` can be any valid property name, allowing dynamic sorting.

### Scenario 3: Aggregating Data

You may want to group and count records dynamically.

```csharp
var groupedData = _context.Users.GroupBy("Age")
                                .Select("new(Key as Age, Count() as UserCount)");
```

This groups users by `Age` and counts the number of users in each age group.

## List of All Methods and Their Uses

| Method   | Description                                                   |
|----------|---------------------------------------------------------------|
| `Where`  | Filters the sequence based on a dynamic string expression.    |
| `OrderBy`| Orders the sequence by a specified string expression.         |
| `Select` | Projects the sequence into a new form using a dynamic string. |
| `GroupBy`| Groups elements based on a dynamic string expression.         |
| `Join`   | Joins two sequences based on a specified key.                 |
| `Any`    | Checks if any elements satisfy the specified condition.       |
| `All`    | Checks if all elements satisfy the specified condition.       |

## Conclusion

The `System.Linq.Dynamic` package is a powerful tool for working with dynamic LINQ queries, especially when dealing with unknown data structures or user-defined filters and sorting. This flexibility makes it ideal for building APIs and data-driven applications.










# مكتبة System.Linq.Dynamic

توفر مكتبة `System.Linq.Dynamic` طريقة لبناء استعلامات LINQ بشكل ديناميكي باستخدام النصوص أثناء وقت التشغيل. تعتبر هذه المكتبة مفيدة جدًا عند العمل مع بيانات ديناميكية، مثل بناء الفلاتر، الترتيبات، والاختيارات بناءً على إدخال المستخدم. أدناه سنغطي جميع الدوال والخصائص الهامة مع سيناريوهات واقعية وأمثلة متنوعة.

## البداية

لتثبيت المكتبة، استخدم الأمر التالي في NuGet:

```bash
Install-Package System.Linq.Dynamic.Core
```

بعد التثبيت، قم بإضافة الـ namespace في الكود:

```csharp
using System.Linq.Dynamic.Core;
```

## الخصائص الرئيسية

### 1. دوال الاستعلام الديناميكي

توفر مكتبة `System.Linq.Dynamic` توسيعات لـ `IQueryable` و `IEnumerable` مع عدة دوال قوية للاستعلام الديناميكي:

- `Where`
- `OrderBy`
- `Select`
- `GroupBy`
- `Join`
- `Any` / `All`

كل واحدة من هذه الدوال تقبل تعبير نصي، مما يسمح ببناء استعلامات ديناميكية.

### 2. التصفية الديناميكية باستخدام `Where`

تمكنك دالة `Where` من التصفية بناءً على تعبير نصي ديناميكي.

#### مثال:
```csharp
var data = new List<User>
{
    new User { Name = "Alice", Age = 25 },
    new User { Name = "Bob", Age = 30 },
    new User { Name = "Charlie", Age = 35 }
}.AsQueryable();

var filteredData = data.Where("Age > 30");
```

في هذا المثال، يتم تصفية المستخدمين الذين تتجاوز أعمارهم 30 باستخدام التعبير `"Age > 30"`.

### 3. الترتيب الديناميكي باستخدام `OrderBy`

تمكنك دالة `OrderBy` من الترتيب بناءً على تعبير نصي.

#### مثال:
```csharp
var sortedData = data.OrderBy("Name DESC");
```

هنا، `"Name DESC"` يقوم بترتيب البيانات تنازليًا حسب `Name`.

### 4. الاختيار الديناميكي باستخدام `Select`

تمكنك دالة `Select` من عرض البيانات بشكل جديد بناءً على تعبير نصي ديناميكي.

#### مثال:
```csharp
var projectedData = data.Select("new(Name, Age)");
```

يتم هنا إرجاع خصائص `Name` و `Age` فقط من كل مستخدم.

### 5. التجميع الديناميكي باستخدام `GroupBy`

تمكنك دالة `GroupBy` من تجميع العناصر بناءً على خاصية معينة ديناميكيًا.

#### مثال:
```csharp
var groupedData = data.GroupBy("Age");
```

هذا المثال يقوم بتجميع البيانات حسب خاصية `Age`.

## سيناريوهات من العالم الواقعي

### السيناريو 1: بناء فلتر بحث في API ويب

في تطبيق ويب، قد تحتاج إلى إنشاء واجهة برمجية حيث يتمكن المستخدمون من تطبيق الفلاتر ديناميكيًا بناءً على معايير استعلام.

```csharp
public IQueryable<User> GetUsers(string filter)
{
    var data = _context.Users.AsQueryable();
    return string.IsNullOrEmpty(filter) ? data : data.Where(filter);
}
```

تمكن هذه الدالة من التصفية الديناميكية بناءً على النص `filter` المدخل.

### السيناريو 2: ترتيب البيانات بناءً على إدخال المستخدم

لنفترض أن لديك واجهة مستخدم تسمح للمستخدمين باختيار عمود للترتيب.

```csharp
public IQueryable<User> GetUsersSorted(string sortBy)
{
    var data = _context.Users.AsQueryable();
    return string.IsNullOrEmpty(sortBy) ? data : data.OrderBy(sortBy);
}
```

هنا، يمكن أن يكون `sortBy` أي اسم خاصية صحيح، مما يسمح بالترتيب الديناميكي.

### السيناريو 3: تجميع البيانات

قد ترغب في تجميع وحساب السجلات بشكل ديناميكي.

```csharp
var groupedData = _context.Users.GroupBy("Age")
                                .Select("new(Key as Age, Count() as UserCount)");
```

يقوم هذا المثال بتجميع المستخدمين حسب `Age` وحساب عدد المستخدمين في كل فئة عمرية.

## قائمة بجميع الدوال واستخداماتها

| الدالة       | الوصف                                                       |
|--------------|-------------------------------------------------------------|
| `Where`      | تصفية التسلسل بناءً على تعبير نصي ديناميكي.                 |
| `OrderBy`    | ترتيب التسلسل بناءً على تعبير نصي معين.                     |
| `Select`     | عرض التسلسل في شكل جديد باستخدام تعبير نصي ديناميكي.        |
| `GroupBy`    | تجميع العناصر بناءً على تعبير نصي ديناميكي.                 |
| `Join`       | ربط تسلسلين بناءً على مفتاح محدد.                           |
| `Any`        | التحقق مما إذا كانت بعض العناصر تفي بالشرط المحدد.          |
| `All`        | التحقق مما إذا كانت جميع العناصر تفي بالشرط المحدد.          |

## الخاتمة

تعتبر مكتبة `System.Linq.Dynamic` أداة قوية للعمل مع استعلامات LINQ الديناميكية، خاصةً عند التعامل مع هياكل بيانات غير معروفة أو فلاتر وترتيب معتمد على المستخدم. هذه المرونة تجعلها مثالية لبناء APIs وتطبيقات معتمدة على البيانات.

