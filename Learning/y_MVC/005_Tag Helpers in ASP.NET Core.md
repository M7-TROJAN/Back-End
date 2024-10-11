# Tag Helpers in ASP.NET Core

**Tag Helpers** in ASP.NET Core allow server-side code to participate in creating and rendering HTML elements. They are designed to be an easy and intuitive way to make your Razor views more dynamic by providing a declarative, HTML-like syntax for server-side operations.

## 1. **`asp-controller`**

The `asp-controller` tag helper is used to specify the target **controller** in a URL. It's typically used within anchor (`<a>`) or form (`<form>`) elements to generate links or form submissions to a specific controller.

### Example:
```html
<a asp-controller="Categories" asp-action="Index">Categories</a>
```
This will generate:
```html
<a href="/Categories/Index">Categories</a>
```
Here, the link points to the `Index` action of the `CategoriesController`.

---

## 2. **`asp-action`**

The `asp-action` tag helper specifies the **action** method in the target controller that the URL should point to. It's often used together with `asp-controller`.

### Example:
```html
<a asp-controller="Categories" asp-action="Details" asp-route-id="1">View Details</a>
```
This will generate:
```html
<a href="/Categories/Details/1">View Details</a>
```
Here, the link points to the `Details` action in the `CategoriesController` with a route parameter (`id=1`).

---

## 3. **`asp-for`**

The `asp-for` tag helper is used with form elements such as `<input>`, `<textarea>`, and `<select>` to bind them to a model property. This tag helper is commonly used within forms to strongly bind UI elements to properties in your models or view models.

### Example:
```html
<form asp-controller="Categories" asp-action="Create">
    <label asp-for="Name"></label>
    <input asp-for="Name" class="form-control" />
    <button type="submit">Submit</button>
</form>
```

Assuming the model has a `Name` property, this will generate:
```html
<form action="/Categories/Create" method="post">
    <label for="Name">Name</label>
    <input type="text" id="Name" name="Name" class="form-control" />
    <button type="submit">Submit</button>
</form>
```
Here, `asp-for="Name"` is linked to the `Name` property in your model, and the `id` and `name` attributes are generated automatically.

---

## 4. **`asp-validation-for`**

The `asp-validation-for` tag helper is used for displaying validation messages for a specific property of the model. It typically works alongside data annotations in your model to provide client-side validation.

### Example:
```html
<form asp-controller="Categories" asp-action="Create">
    <label asp-for="Name"></label>
    <input asp-for="Name" class="form-control" />
    <span asp-validation-for="Name" class="text-danger"></span>
    <button type="submit">Submit</button>
</form>
```

If there are validation errors for the `Name` property, the `asp-validation-for` tag helper will display the validation message.

This will generate:
```html
<form action="/Categories/Create" method="post">
    <label for="Name">Name</label>
    <input type="text" id="Name" name="Name" class="form-control" />
    <span class="text-danger" data-valmsg-for="Name" data-valmsg-replace="true"></span>
    <button type="submit">Submit</button>
</form>
```
The validation message will be dynamically filled when the `Name` field fails validation.

---

## 5. **`asp-route` / `asp-route-{parameter}`**

The `asp-route` tag helper is used to specify the route name or route parameters when generating URLs. You can also use `asp-route-{parameter}` to pass specific route values.

### Example (with named route):
```html
<a asp-route="default">Home</a>
```

This will generate a link to the default route, like:
```html
<a href="/">Home</a>
```

### Example (with route parameters):
```html
<a asp-controller="Categories" asp-action="Details" asp-route-id="2">Category 2</a>
```

This will generate:
```html
<a href="/Categories/Details/2">Category 2</a>
```

---

## 6. **`asp-area`**

If you are using **Areas** (a way to organize controllers and views), the `asp-area` tag helper allows you to specify the target area.

### Example:
```html
<a asp-area="Admin" asp-controller="Categories" asp-action="Index">Manage Categories</a>
```

This will generate:
```html
<a href="/Admin/Categories/Index">Manage Categories</a>
```
Here, the link points to the `CategoriesController` within the `Admin` area.

---

## 7. **`asp-page` and `asp-page-handler`**

The `asp-page` tag helper is used with **Razor Pages**, and it specifies the page that the link should point to. If you are using Razor Pages and need to specify a particular page, use `asp-page`.

The `asp-page-handler` is used to call specific **page handlers** in Razor Pages.

### Example (with `asp-page`):
```html
<a asp-page="/Contact">Contact Us</a>
```

This will generate:
```html
<a href="/Contact">Contact Us</a>
```

### Example (with `asp-page-handler`):
```html
<form asp-page="/Index" asp-page-handler="Submit">
    <button type="submit">Submit</button>
</form>
```

This will generate:
```html
<form action="/Index?handler=Submit" method="post">
    <button type="submit">Submit</button>
</form>
```
The form will submit to the `Submit` handler in the `Index` page.

---

## 8. **`asp-anti-forgery`**

The `asp-anti-forgery` tag helper automatically generates an anti-forgery token (CSRF token) to protect your form submissions. This is enabled by default on `<form>` elements with the `asp-controller` and `asp-action` tag helpers.

### Example:
```html
<form asp-controller="Categories" asp-action="Create" asp-anti-forgery="true">
    <input asp-for="Name" class="form-control" />
    <button type="submit">Submit</button>
</form>
```

This will automatically generate a hidden field for the anti-forgery token:
```html
<form action="/Categories/Create" method="post">
    <input name="__RequestVerificationToken" type="hidden" value="random-token" />
    <input type="text" id="Name" name="Name" class="form-control" />
    <button type="submit">Submit</button>
</form>
```

---

## 9. **`asp-items`**

The `asp-items` tag helper is used with `<select>` elements to bind a list of items (like a list of options) to the dropdown. This is often used to populate a `<select>` list from a collection in your model.

### Example:
```html
<select asp-for="CategoryId" asp-items="Model.CategoryList"></select>
```

This will generate:
```html
<select id="CategoryId" name="CategoryId">
    <option value="1">Fiction</option>
    <option value="2">Non-Fiction</option>
    <!-- Other options -->
</select>
```

Here, `asp-items` binds a collection (`CategoryList`) to the `<select>` element.

---

## 10. **`asp-route-*`**

The `asp-route-*` tag helper allows you to specify individual route parameters in a link. For example, you can dynamically add route values based on specific properties.

### Example:
```html
<a asp-controller="Products" asp-action="Details" asp-route-id="3" asp-route-category="Books">View Product</a>
```

This will generate:
```html
<a href="/Products/Details/3?category=Books">View Product</a>
```

---

## Conclusion

Tag Helpers in ASP.NET Core simplify how we write HTML in Razor Views by binding HTML elements to server-side code. They provide a more readable and maintainable way to handle linking, forms, routing, and validation by combining the flexibility of HTML with the power of ASP.NET Core.

By using these tag helpers, you will have a more seamless and dynamic way of working with server-side data and logic, leading to cleaner and more maintainable code.