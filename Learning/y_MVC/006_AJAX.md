**الـ Ajax Request** (اختصارًا لـ Asynchronous JavaScript and XML) هو أسلوب في تطوير الويب يسمح لك بتحديث جزء من صفحة الويب بدون الحاجة إلى إعادة تحميل الصفحة بالكامل. يعني ذلك أنه يمكنك إرسال واستقبال بيانات من السيرفر في الخلفية (background) دون التأثير على تجربة المستخدم.

الـ Ajax يعتمد على JavaScript ويمكّنك من:

1. إرسال طلبات للسيرفر (GET أو POST) للحصول على بيانات أو إرسال بيانات.
2. استقبال البيانات من السيرفر بصيغ مثل JSON أو XML.
3. تحديث جزء معين من الصفحة باستخدام DOM بناءً على البيانات المستلمة.

### مثال بسيط:

باستخدام jQuery (واحدة من المكتبات الشهيرة لعمل طلبات Ajax بسهولة):

```javascript
$.ajax({
  url: "server-url",  // عنوان السيرفر
  type: "GET",        // نوع الطلب (GET أو POST)
  success: function(response) {  
    // يتم تنفيذ هذا الكود عند استلام الرد بنجاح
    console.log(response);  
    // يمكنك هنا تحديث جزء من الصفحة
    $("#result").html(response);
  },
  error: function(error) {  
    // يتم تنفيذ هذا الكود عند حدوث خطأ
    console.error("Error:", error);
  }
});
```

### مميزات Ajax:

- تحسين تجربة المستخدم بدون الحاجة لإعادة تحميل الصفحة بالكامل.
- تقليل كمية البيانات المرسلة بين المتصفح والسيرفر.
- تحسين سرعة الأداء في التفاعل مع السيرفر.

### تطبيقات Ajax:

- عرض محتوى جديد بدون إعادة تحميل الصفحة (مثل تحديث التعليقات أو رسائل الشات).
- ملء القوائم المنسدلة بناءً على الاختيارات السابقة.
- التحقق من البيانات المدخلة في النماذج فورياً (مثل البريد الإلكتروني أو اسم المستخدم).

---

الآن النص منسق بشكل أفضل ويفصل بين الإنجليزية والعربية بصورة أوضح.









```javascript
$(document).ready(function () {
    $(".js-toggle-status").on("click", function () {
        var btn = $(this);
        var id = btn.data("id");

        $.ajax({
            url: `/Categories/ToggleStatus/${id}`,
            type: "POST",
            success: function (lastUpdatedOn) {
                var status = btn.parents("tr").find(".js-status");
                var newStatus = status.text().trim() === "Available" ? "Deleted" : "Available";
                status.text(newStatus).toggleClass("badge-light-success badge-light-danger");

                var lastUpdatedOnElement = btn.parents("tr").find(".js-updated-on");
                lastUpdatedOnElement.text(lastUpdatedOn);
            },
        });
    });
});
```

Let's break this code down step by step so you can understand it fully, even if you're not familiar with jQuery.

### Overview
This script listens for clicks on elements with the class `.js-toggle-status`. When one of these elements is clicked, it sends a request to the server to toggle the status of a category. After receiving a response, it updates the page with the new status and the last updated time.

### Breakdown

1. **`$(document).ready(function() {...});`**:
   - This function ensures that the code inside it runs only after the entire HTML document (DOM) is fully loaded. It's the jQuery equivalent of `window.onload` in plain JavaScript.
   - In plain JavaScript, this could be replaced by `document.addEventListener("DOMContentLoaded", function() {...});`.

2. **`$(".js-toggle-status").on("click", function() {...});`**:
   - This line selects all elements with the class `js-toggle-status` and attaches a click event listener to each one. When an element with this class is clicked, the function inside `on("click", ...)` will be executed.
   - In jQuery, `$` is used to select elements (like `document.querySelector` in vanilla JS).
   - The `.on("click", ...)` method is how jQuery binds event listeners, similar to `addEventListener("click", ...)` in vanilla JS.

3. **`var btn = $(this);`**:
   - `this` refers to the element that was clicked. In this case, it's one of the `.js-toggle-status` elements.
   - `$(this)` wraps the `this` object in jQuery, so you can use jQuery methods on it (like `.data()` or `.parents()`).
   - `var btn` stores this jQuery-wrapped element so it can be used later in the function.

4. **`var id = btn.data("id");`**:
   - This retrieves the value of the `data-id` attribute from the clicked element. 
   - jQuery's `.data()` method is a way to access `data-*` attributes (like `data-id="123"`).
   - If the element that was clicked had something like `<button data-id="123">`, this line would extract `123`.

5. **`$.ajax({...});`**:
   - This is how jQuery performs an AJAX request (asynchronous HTTP request) to the server.
   - The purpose of this AJAX call is to send a request to the server to toggle the status of the category identified by `id`.

   Let's look at the parameters inside `$.ajax()`:

   - **`url: '/Categories/ToggleStatus/${id}'`**:
     - This sets the URL where the request will be sent. It dynamically inserts the `id` into the URL.
     - In this case, the request is sent to `/Categories/ToggleStatus/[id]`, where `[id]` is the category's ID.

   - **`type: "POST"`**:
     - This specifies the HTTP method used for the request. It's a `POST` request, which means data is being sent to the server.

   - **`success: function (lastUpdatedOn)`**:
     - This function runs if the request is successful. The server is expected to send back some data (in this case, the `lastUpdatedOn` value).
     - The response from the server (here, `lastUpdatedOn`) is passed into this function as an argument.

6. **Inside the success callback**:

   - **`var status = btn.parents("tr").find(".js-status");`**:
     - This finds the status element (with the class `.js-status`) in the same table row (`<tr>`) as the clicked button (`btn`).
     - `.parents("tr")` looks for the nearest `<tr>` element (table row) that is an ancestor of `btn`.
     - `.find(".js-status")` finds the element with the class `.js-status` inside that row.

   - **`var newStatus = status.text().trim() === "Available" ? "Deleted" : "Available";`**:
     - This line checks the current status text. If the text inside the `.js-status` element is `"Available"`, it will change it to `"Deleted"`. If it's already `"Deleted"`, it will change it back to `"Available"`.
     - `.text()` is a jQuery method that retrieves or sets the text inside an element.
     - `.trim()` removes any whitespace around the text.

   - **`status.text(newStatus).toggleClass("badge-light-success badge-light-danger");`**:
     - `.text(newStatus)` updates the text of the `.js-status` element with the new status (`Available` or `Deleted`).
     - `.toggleClass("badge-light-success badge-light-danger")` switches between two CSS classes:
       - `badge-light-success`: Presumably for styling "Available" items.
       - `badge-light-danger`: Presumably for styling "Deleted" items.

   - **`var lastUpdatedOnElement = btn.parents("tr").find(".js-updated-on");`**:
     - Similar to how the status element was found, this finds the element with the class `.js-updated-on` in the same row as the button (`btn`).
     - This element is supposed to show the last time the status was updated.

   - **`lastUpdatedOnElement.text(lastUpdatedOn);`**:
     - This sets the text inside the `.js-updated-on` element to the value of `lastUpdatedOn` (which was returned from the server).
     - `lastUpdatedOn` would typically be a date or time value, indicating when the status was last changed.

### Summary
- When the page is loaded, the script attaches click listeners to buttons with the class `.js-toggle-status`.
- When a button is clicked, the script retrieves the `data-id` value of the clicked button.
- It sends a `POST` request to the server to toggle the status of the category with that ID.
- Once the server responds with the `lastUpdatedOn` timestamp, the script updates the page:
  - It changes the status text between `"Available"` and `"Deleted"`.
  - It switches the CSS classes to reflect the new status.
  - It updates the "last updated" time in the corresponding table row.

This process happens without reloading the entire page