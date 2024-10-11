Here's a detailed markdown file covering the topic **"Partial Views"** in ASP.NET MVC:

---

# Partial Views

## Table of Contents
1. [What are Partial Views?](#what-are-partial-views)
2. [Why Use Partial Views?](#why-use-partial-views)
3. [Creating a Partial View](#creating-a-partial-view)
4. [Rendering Partial Views](#rendering-partial-views)
   - [Using `@Html.Partial`](#using-htmlpartial)
   - [Using `@Html.RenderPartial`](#using-htmlrenderpartial)
   - [Using `@Html.Action`](#using-htmlaction)
   - [Using `@Html.RenderAction`](#using-htmlrenderaction)
5. [Passing Data to Partial Views](#passing-data-to-partial-views)
6. [Using Partial Views for Reusability](#using-partial-views-for-reusability)
7. [Practical Examples](#practical-examples)
8. [Conclusion](#conclusion)

---

## What are Partial Views?

**Partial Views** are reusable view components in ASP.NET MVC that allow you to render small pieces of HTML markup on a webpage. They enable you to break down complex views into smaller, more manageable sections. A partial view is a `.cshtml` file that can be rendered inside a parent view.

### Characteristics of Partial Views:
1. **Reusability**: Commonly used sections like headers, footers, or navigation menus can be encapsulated into partial views.
2. **Separation of Concerns**: Partial views help divide the page into distinct sections, making the code easier to read and maintain.
3. **Performance Optimization**: By using partial views, you can selectively render parts of a page without refreshing the entire page.

### Where to Use Partial Views:
- Navigation menus.
- Sidebar widgets.
- Login or registration forms.
- Repeated elements like product lists, comments, or user profiles.

## Why Use Partial Views?

- **Code Reusability**: Define a piece of UI once and use it across multiple views.
- **Simplified Views**: Partial views make complex views simpler by breaking them into smaller components.
- **Separation of Logic**: Keep the view logic clean and modular.
- **Easier Maintenance**: Updating one partial view will automatically reflect across all views that use it.

## Creating a Partial View

1. Right-click the **Views** folder (or a subfolder like `Shared`) and select **Add > View**.
2. In the dialog box, name the partial view (e.g., `_Menu.cshtml`) and check the **Create as a partial view** checkbox.
3. Click **Add**.

This creates a new `.cshtml` file with the typical Razor structure. By convention, partial view names start with an underscore (`_`) to indicate they are not standalone views.

### Example: `_Menu.cshtml`
```html
<ul>
    <li><a href="/">Home</a></li>
    <li><a href="/About">About</a></li>
    <li><a href="/Contact">Contact</a></li>
</ul>
```

## Rendering Partial Views

### Using `@Html.Partial`

`@Html.Partial` renders the specified partial view and returns the rendered HTML as a string.

```html
@Html.Partial("_Menu")
```

### Using `@Html.RenderPartial`

`@Html.RenderPartial` works similarly to `@Html.Partial`, but it writes directly to the output stream, making it slightly faster.

```html
@{ Html.RenderPartial("_Menu"); }
```

### Using `@Html.Action`

`@Html.Action` is used when you want to call a controller action that returns a partial view. This is useful when the partial view requires additional data from the controller.

```html
@Html.Action("Menu", "Home")
```

### Using `@Html.RenderAction`

`@Html.RenderAction` is similar to `@Html.Action`, but it writes directly to the output stream.

```html
@{ Html.RenderAction("Menu", "Home"); }
```

## Passing Data to Partial Views

You can pass data to partial views in several ways:

### 1. Using `ViewData`
You can pass values using the `ViewData` dictionary:

```csharp
ViewData["Title"] = "Navigation Menu";
```

In the partial view (`_Menu.cshtml`), access the data using:

```html
<h3>@ViewData["Title"]</h3>
```

### 2. Using `ViewBag`
Another approach is to use the `ViewBag`:

```csharp
ViewBag.Title = "Navigation Menu";
```

In the partial view:

```html
<h3>@ViewBag.Title</h3>
```

### 3. Using Strongly-Typed Models
Pass a strongly-typed model directly to the partial view:

**Controller:**

```csharp
public ActionResult Index()
{
    var items = new List<string> { "Home", "About", "Contact" };
    return View(items);
}
```

**View:**

```html
@model List<string>

@Html.Partial("_Menu", Model)
```

**Partial View (`_Menu.cshtml`):**

```html
@model List<string>
<ul>
@foreach (var item in Model)
{
    <li>@item</li>
}
</ul>
```

## Using Partial Views for Reusability

Suppose you have a list of products that you want to render in multiple pages, such as in the home page and the product page. Instead of duplicating the same code in different views, create a partial view (`_ProductCard.cshtml`) and render it wherever required.

### Example: `_ProductCard.cshtml`
```html
@model Product

<div class="product-card">
    <h3>@Model.Name</h3>
    <p>@Model.Description</p>
    <p>Price: $@Model.Price</p>
</div>
```

### Using the Partial View
In `Index.cshtml` or `Product.cshtml`:

```html
@model List<Product>

@foreach (var product in Model)
{
    @Html.Partial("_ProductCard", product)
}
```

## Practical Examples

### Example 1: Rendering a Simple Menu Partial View
1. **Create a Partial View**: `_Menu.cshtml`.
   ```html
   <ul>
       <li><a href="/">Home</a></li>
       <li><a href="/About">About</a></li>
       <li><a href="/Contact">Contact</a></li>
   </ul>
   ```
   
2. **Render the Partial View in the Main View**:
   ```html
   <div>
       @Html.Partial("_Menu")
   </div>
   ```

### Example 2: Using a Partial View for a Product Card
1. **Create a Partial View**: `_ProductCard.cshtml`.
   ```html
   @model Product

   <div class="product">
       <h2>@Model.Name</h2>
       <p>@Model.Description</p>
       <p>Price: $@Model.Price</p>
   </div>
   ```

2. **Render the Partial View with a Model**:
   ```html
   @model List<Product>

   <div class="product-list">
   @foreach (var product in Model)
   {
       @Html.Partial("_ProductCard", product)
   }
   </div>
   ```

## Conclusion

Partial views are a powerful feature in ASP.NET MVC that enable you to create reusable components and reduce code duplication. By leveraging partial views, you can build modular, maintainable applications and improve the overall structure of your views.