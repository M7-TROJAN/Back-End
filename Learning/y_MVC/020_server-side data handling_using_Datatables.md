لتشغيل DataTables على الـ Server Side ، هتحتاج تمشي بخطوات معينة عشان تعرض البيانات بشكل فعال وتتعامل مع طلبات المستخدمين في البحث، التصفية، والفرز من خلال الخادم. دي الخطوات الأساسية:

1. إعداد DataTables في الـ Client Side

في البداية، ضيف مكتبة DataTables على الصفحة اللي عايز تعرض فيها البيانات، ممكن تضيفها من الـ CDN أو تثبتها محلياً.

مثال:
```html
<link rel="stylesheet" href="https://cdn.datatables.net/1.10.21/css/jquery.dataTables.min.css">
<script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
<script src="https://cdn.datatables.net/1.10.21/js/jquery.dataTables.min.js"></script>
```
2. إعداد جدول HTML للبيانات

ضيف جدول HTML عادي بالهيكل الأساسي فقط بدون بيانات، لأنه هيتم جلب البيانات من الـ Server Side.
```html
<table id="example" class="display" style="width:100%">
    <thead>
        <tr>
            <th>الاسم</th>
            <th>العمر</th>
            <th>الوظيفة</th>
            <th>البلد</th>
        </tr>
    </thead>
</table>
```
3. كتابة الكود الخاص بـ DataTables

في الملف الـ JavaScript، عرف DataTables وخليه يعمل طلبات للـ Server كل ما تحتاج البيانات (لما المستخدم يغير الصفحة، أو يستخدم البحث).
```javascript
$(document).ready(function() {
    $('#example').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/api/data", // غير الرابط هنا حسب API الخاصة بيك
            "type": "POST"
        }
    });
});
```
4. إعداد الـ Server Side لمعالجة الطلبات

دلوقتي على الجانب السيرفر، محتاج تتعامل مع الطلبات اللي جاية من DataTables وتستجيب بالبيانات المطلوبة بالشكل الصحيح.

الخطوات العامة للـ Server Side:

 • جلب البيانات من قاعدة البيانات مع الفلترة حسب طلب المستخدم.
 • فرز البيانات حسب العمود المطلوب.
 • تنفيذ الـ Pagination بحيث يتم جلب بيانات الصفحة المطلوبة فقط.
 • حساب عدد البيانات الكلي وإرجاعه عشان DataTables يظهر الصفحات بشكل صحيح.

مثال بلغة C# مع ASP.NET Core

هنا مثال بسيط على دالة API بتتعامل مع طلبات DataTables على الجانب السيرفر:

```csharp
[HttpPost]
public IActionResult GetData([FromBody] DataTableRequest request)
{
    // جلب البيانات من قاعدة البيانات
    var data = _context.People.AsQueryable();

    // التصفية والبحث
    if (!string.IsNullOrEmpty(request.Search.Value))
    {
        data = data.Where(p => p.Name.Contains(request.Search.Value));
    }

    // الترتيب حسب العمود
    if (request.Order != null && request.Order.Count > 0)
    {
        var orderColumn = request.Columns[request.Order[0].Column].Data;
        var isAsc = request.Order[0].Dir == "asc";
        data = isAsc ? data.OrderByDynamic(orderColumn) : data.OrderByDescendingDynamic(orderColumn);
    }

    // الـ Pagination
    var pagedData = data.Skip(request.Start).Take(request.Length).ToList();

    // النتيجة النهائية
    return Json(new {
        draw = request.Draw,
        recordsTotal = data.Count(),
        recordsFiltered = data.Count(),
        data = pagedData
    });
}
```
ملحوظات

 • لازم ترجع البيانات بصيغة JSON بنفس الشكل اللي DataTables متوقعه.
 • تأكد إن الدالة بترجع draw, recordsTotal, recordsFiltered, و data.

5. تعديل الجوانب الأخرى حسب الحاجة



مفهوم استقبال البارامترات في الـ Action بيكون أساسي عشان DataTables تقدر تبعت البيانات اللي محتاجاها زي البحث، الترتيب، والصفحات.

في DataTables، لما تعمل طلب لـ Server Side، بيبعت مجموعة من البارامترات بتحدد:
 • عدد السجلات المطلوبة في الصفحة.
 • البحث لو المستخدم كتب أي نص.
 • الترتيب حسب عمود معين.
 • رقم الصفحة المطلوب عرضها.

شكل البارامترات اللي DataTables بترسلها

الـ DataTables بتبعت بارامترات مع كل طلب، وهنا بعض الأمثلة لأهم البارامترات:
 • draw: رقم الطلب الحالي لتمييزه (DataTables بتستخدمه داخليًا).
 • start: بداية السجلات في الصفحة (مثلاً إذا كانت الصفحة 2، بيكون start=10 لو كل صفحة فيها 10 سجلات).
 • length: عدد السجلات المطلوبة في الصفحة.
 • search[value]: النص اللي المستخدم كتبه للبحث.
 • order[i][column]: رقم العمود المطلوب ترتيبه.
 • order[i][dir]: اتجاه الترتيب (تصاعدي “asc” أو تنازلي “desc”).
 • columns[i][data]: اسم العمود اللي بيتم ترتيبه أو البحث فيه.

استقبال البارامترات في ASP.NET Core

بنستخدم كلاس DataTableRequest عشان نخزن فيه البارامترات اللي جاية من DataTables.


استخدام البارامترات داخل الـ Action

لما بنستقبل DataTableRequest request كـ بارامتر في الدالة، بنقدر نوصل للبارامترات المطلوبة بالشكل ده:
 • request.Search.Value: النص المكتوب في مربع البحث.
 • request.Order[0].Column: العمود اللي محتاجين نعمل عليه ترتيب.
 • request.Order[0].Dir: اتجاه الترتيب (تصاعدي أو تنازلي).
 • request.Start: بداية السجلات للصفحة المطلوبة.
 • request.Length: عدد السجلات المطلوبة للصفحة.
من هنا، نقدر نستخدم المعلومات دي لفلترة وترتيب البيانات حسب ما المستخدم محتاج.

الجزء ده `[FromBody] DataTableRequest request` بيستخدم لاستقبال بيانات الطلب (request data) اللي جاية من الـ Client Side، اللي في حالتنا هو DataTables، بشكل مباشر من جسم الطلب (Request Body) على هيئة JSON.

شرح [FromBody]

 • [FromBody] هي Attribute في ASP.NET Core بتستخدم عشان تحدد إن البارامترات (اللي هي البيانات المطلوبة) جاية من Body الطلب، مش من الـ Query String أو Route.
 • لما تضيف [FromBody] قبل البارامتر، ASP.NET Core بيحاول يحوّل البيانات اللي جاية من جسم الطلب إلى الكلاس المحدد، وفي حالتنا الكلاس هو DataTableRequest.

مثال للتوضيح

لما DataTables يرسل طلب للـ Server على هيئة POST، البيانات بتكون بالشكل ده داخل جسم الطلب (Request Body) كـ JSON:
```json
{
    "draw": 1,
    "start": 0,
    "length": 10,
    "search": { "value": "test" },
    "order": [ { "column": 1, "dir": "asc" } ],
    "columns": [
        { "data": "name", "name": "", "searchable": true, "orderable": true, "search": { "value": "" } },
        { "data": "age", "name": "", "searchable": true, "orderable": true, "search": { "value": "" } }
    ]
}
```
تحويل البيانات باستخدام DataTableRequest

عند استخدام ` DataTableRequest request [FromBody]`، الـ ASP.NET Core بيفهم إنك عايز البيانات اللي في جسم الطلب تتحول إلى كائن من نوع DataTableRequest. فيقوم بتحليل الـ JSON اللي جاية ويحولها تلقائيًا لخصائص الكلاس DataTableRequest زي draw, start, length، وهكذا.

ليه نستخدم [FromBody]

 • تنظيم البيانات: لما تستقبل البيانات في هيئة كائن، بتكون أسهل للتعامل خاصةً لو كانت البيانات معقدة أو متعددة المستويات.
 • تحسين الأداء: ASP.NET Core بيوفر معالجة تلقائية، وبالتالي مش محتاج تعمل تحويل يدوي لكل بارامتر.

ملخص:
• ال `[FromBody]` بيخلي ال `ASP.NET Core` يعرف ان البيانات في جسم الطلب `(request Body)` وتحديدا `JSON` وبيحاول يحولها لاوبجكت من النوع الي انت عامله `DataTableRequest`
• بدون [FromBody]، الدالة مش هتقدر تستقبل البيانات اللي جاي من DataTables بشكل صحيح. 
---

 مثال كامل لـ Controller و View مع Action بتسترجع قائمة من المستخدمين (Users) وتعرضهم باستخدام DataTables. هنستخدم بيانات المستخدمين بشكل هارد كود عشان نركز على الآلية.

1. كلاس DataTableRequest

في البداية هنحتاج كلاس لاستقبال بيانات DataTables، هنعمل كلاس DataTableRequest بالشكل التالي:
```csharp
public class DataTableRequest
{
    public int Draw { get; set; }
    public int Start { get; set; }
    public int Length { get; set; }
    public Search Search { get; set; }
    public List<Order> Order { get; set; }
    public List<Column> Columns { get; set; }
}

public class Search
{
    public string Value { get; set; }
}

public class Order
{
    public int Column { get; set; }
    public string Dir { get; set; }
}

public class Column
{
    public string Data { get; set; }
    public string Name { get; set; }
    public bool Searchable { get; set; }
    public bool Orderable { get; set; }
    public Search Search { get; set; }
}
```

2. إنشاء الـ Controller

الخطوة الثانية هي عمل Controller. هنا بنعمل UsersController يحتوي على Action بترجع بيانات المستخدمين للـ DataTables.
```csharp
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

public class UsersController : Controller
{
    // بيانات المستخدمين بشكل هارد كود
    private static readonly List<User> Users = new List<User>
    {
        new User { Id = 1, Name = "Mahmoud", Age = 25, Job = "Developer" },
        new User { Id = 2, Name = "Ali", Age = 30, Job = "Designer" },
        new User { Id = 3, Name = "Sara", Age = 28, Job = "Manager" },
        new User { Id = 4, Name = "Nora", Age = 32, Job = "Tester" }
    };

    [HttpPost]
    public IActionResult GetUsers([FromBody] DataTableRequest request)
    {
        // فلترة البيانات بناءً على البحث
        var filteredUsers = Users.AsQueryable();
        if (!string.IsNullOrEmpty(request.Search?.Value))
        {
            filteredUsers = filteredUsers.Where(u => u.Name.Contains(request.Search.Value));
        }

        // تنفيذ الترتيب
        if (request.Order != null && request.Order.Count > 0)
        {
            var orderColumn = request.Columns[request.Order[0].Column].Data;
            var isAsc = request.Order[0].Dir == "asc";
            filteredUsers = orderColumn switch
            {
                "name" => isAsc ? filteredUsers.OrderBy(u => u.Name) : filteredUsers.OrderByDescending(u => u.Name),
                "age" => isAsc ? filteredUsers.OrderBy(u => u.Age) : filteredUsers.OrderByDescending(u => u.Age),
                "job" => isAsc ? filteredUsers.OrderBy(u => u.Job) : filteredUsers.OrderByDescending(u => u.Job),
                _ => filteredUsers
            };
        }

        // تنفيذ Pagination (التقسيم إلى صفحات)
        var pagedUsers = filteredUsers
            .Skip(request.Start)
            .Take(request.Length)
            .ToList();

        // تجهيز النتيجة المطلوبة لـ DataTables
        return Json(new
        {
            draw = request.Draw,
            recordsTotal = Users.Count,
            recordsFiltered = filteredUsers.Count(),
            data = pagedUsers
        });
    }
}

// كلاس المستخدم
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Job { get; set; }
}
```
3. إعداد الـ View

دلوقتي نعمل View نعرض فيه البيانات باستخدام DataTables.

Index.cshtml في View

```cshtml
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Users</title>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.21/css/jquery.dataTables.min.css">
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://cdn.datatables.net/1.10.21/js/jquery.dataTables.min.js"></script>
</head>
<body>
    <h1>Users List</h1>
    <table id="usersTable" class="display" style="width:100%">
        <thead>
            <tr>
                <th>Name</th>
                <th>Age</th>
                <th>Job</th>
            </tr>
        </thead>
    </table>

    <script>
        $(document).ready(function () {
          $('#usersTable').DataTable({
                          "processing": true,
                          "serverSide": true,
                          "ajax": {
                              "url": "/Users/GetUsers",
                              "type": "POST",
                              "contentType": "application/json",
                              "data": function (d) {
                                  return JSON.stringify(d);
                              }
                          },
                          "columns": [
                              { "data": "name" },
                              { "data": "age" },
                              { "data": "job" }
                          ]
            });
        });
    </script>
</body>
</html>

شرح الخطوات بالتفصيل

 1. كلاس `DataTableRequest`: الكلاس ده بيحتوي على البارامترات اللي بيبعتها `DataTables` زي `draw, start, length` إلخ.
 2. الـ UsersController:
ال `GetUsers` دي بنسنقبل طلبات ال `Datatables` وبتتعامل معاها

 • فلترة البيانات حسب النص المكتوب في البحث.
 • ترتيب البيانات حسب العمود اللي المستخدم اختاره في DataTables.
 • تقسيم البيانات لصفحات حسب start و length.
 • في النهاية بترجع JSON فيه البيانات اللي DataTables متوقعاه.
 3. الـ `View (index.cshtml)`:
 • بنستخدم مكتبة DataTables لعرض جدول المستخدمين.
 • بنعمل تهيئة لـ DataTables بحيث تتصل بالـ API /Users/GetUsers وتبعت الطلبات بشكل Server Side.
 • عمود columns بيعرف الأعمدة اللي عايز تعرضها في الجدول مع أسماء الخصائص اللي موجودة في بيانات المستخدمين.
