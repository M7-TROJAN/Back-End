# Razor and Razor Syntax

## Table of Contents
1. [What is Razor?](#what-is-razor)
2. [Razor Syntax Overview](#razor-syntax-overview)
3. [Razor Syntax Rules](#razor-syntax-rules)
4. [Using Razor Syntax in .NET MVC](#using-razor-syntax-in-net-mvc)
5. [Razor Directives](#razor-directives)
6. [Inline Razor Expressions](#inline-razor-expressions)
7. [Razor Control Statements](#razor-control-statements)
   - [Conditionals](#conditionals)
   - [Loops](#loops)
8. [Razor Layouts and Partial Views](#razor-layouts-and-partial-views)
9. [Practical Examples](#practical-examples)
10. [Conclusion](#conclusion)

---

## What is Razor?

**Razor** is a templating language used in ASP.NET MVC to dynamically generate web pages using C# and HTML. It allows you to embed server-side code in HTML markup using a clean and easy-to-read syntax. Razor files typically have a `.cshtml` extension in .NET applications.

Razor syntax is designed to be *lightweight*, *intuitive*, and *clean*, which means you can easily switch between HTML and C# code without using complex syntax or special delimiters. Razor pages provide a seamless way to work with data from the server while building dynamic web pages.

## Razor Syntax Overview

Razor code starts with an `@` symbol and is written using C# syntax. It can be used for rendering values, working with HTML, handling loops, and conditional statements.

### Example
```html
@{
    var today = DateTime.Now;
}

<!DOCTYPE html>
<html>
<head>
    <title>Razor Syntax Demo</title>
</head>
<body>
    <h2>Today's Date is: @today.ToString("D")</h2>
</body>
</html>
```
In this example:
- The `@{ ... }` block is used to execute C# code.
- The `@` symbol is used to output the `today` variable.

## Razor Syntax Rules

1. Razor code blocks start with `@` and can be enclosed in curly braces (`{ ... }`).
2. If the code is a single statement, curly braces are optional.
3. Razor uses C# for logic, loops, and conditionals.
4. The `@` symbol is used to switch between HTML and C# code.
5. To escape the `@` symbol (e.g., `@@`), use double `@` to prevent Razor from treating it as C#.

## Using Razor Syntax in .NET MVC

In .NET MVC, Razor syntax is used in `.cshtml` files, typically inside the `Views` folder. Each view corresponds to a controller action and is used to display the HTML markup combined with dynamic data from the controller.

### Example
Suppose you have a controller action like this:

```csharp
public ActionResult Index()
{
    ViewBag.Message = "Hello, Razor!";
    return View();
}
```

The corresponding Razor view might look like this:

```html
<!DOCTYPE html>
<html>
<head>
    <title>Razor View</title>
</head>
<body>
    <h2>@ViewBag.Message</h2> <!-- Outputs "Hello, Razor!" -->
</body>
</html>
```

## Razor Directives

Razor directives are special keywords prefixed with `@` that control the behavior of the Razor view. Common directives include:

1. **@model**: Specifies the model type for the view.
   ```csharp
   @model Student
   ```
2. **@using**: Imports namespaces.
   ```csharp
   @using System.Collections.Generic
   ```
3. **@inject**: Injects services or objects into the view.
   ```csharp
   @inject ILogger<MyView> Logger
   ```
4. **@inherits**: Sets the base class for the view.
   ```csharp
   @inherits MyBaseClass
   ```

## Inline Razor Expressions

Inline Razor expressions are used to output data directly in the HTML markup.

### Examples
```html
<p>Current Year: @DateTime.Now.Year</p> <!-- Outputs: Current Year: 2024 -->
<p>@(5 + 10)</p> <!-- Outputs: 15 -->
<p>@("Hello, " + "World!")</p> <!-- Outputs: Hello, World! -->
```

## Razor Control Statements

### Conditionals
Razor allows you to use C# conditional statements such as `if`, `else`, and `switch`.

```html
@{
    var isLoggedIn = true;
}
@if (isLoggedIn)
{
    <p>Welcome back, user!</p>
}
else
{
    <p>Please log in to continue.</p>
}
```

### Loops
Razor supports standard C# loops like `for`, `foreach`, and `while`:

```html
@{
    var items = new List<string> { "Apple", "Banana", "Cherry" };
}
<ul>
@foreach (var item in items)
{
    <li>@item</li> <!-- Outputs each item as an <li> element -->
}
</ul>
```

## Razor Layouts and Partial Views

### Layouts
Razor Layouts provide a master page template for your application. They define a common structure (like header, footer, and sidebar) that other pages can reuse.

1. **Create a Layout** (`_Layout.cshtml`):
   ```html
   <!DOCTYPE html>
   <html>
   <head>
       <title>@ViewBag.Title</title>
   </head>
   <body>
       <header>
           <h1>My Website</h1>
           <nav>
               <ul>
                   <li><a href="/">Home</a></li>
                   <li><a href="/About">About</a></li>
               </ul>
           </nav>
       </header>
       <div>
           @RenderBody() <!-- Placeholder for page-specific content -->
       </div>
   </body>
   </html>
   ```

2. **Use the Layout in Views**:
   ```html
   @{
       Layout = "~/Views/Shared/_Layout.cshtml";
   }

   <h2>Welcome to Razor Views!</h2>
   ```

### Partial Views
Partial views are reusable components within a page, such as a navigation menu or footer.

- Create a partial view: `Views/Shared/_Menu.cshtml`.
  ```html
  <ul>
      <li><a href="/">Home</a></li>
      <li><a href="/About">About</a></li>
      <li><a href="/Contact">Contact</a></li>
  </ul>
  ```

- Include a partial view in a Razor file:
  ```html
  @Html.Partial("_Menu")
  ```

## Practical Examples

### Example 1: Displaying a List of Products
```html
@model List<Product>

<h2>Product List</h2>
<ul>
@foreach (var product in Model)
{
    <li>@product.ProductName - $@product.Price</li>
}
</ul>
```

### Example 2: Using Inline Expressions
```html
@{
    var firstName = "John";
    var lastName = "Doe";
}
<p>Full Name: @firstName @lastName</p> <!-- Outputs: Full Name: John Doe -->
```

## Conclusion

Razor syntax is a powerful templating language that seamlessly integrates server-side C# logic with HTML. Its simplicity, readability, and easy-to-learn nature make it a preferred choice for creating dynamic web pages in .NET MVC applications. By mastering Razor, you can effectively build robust and responsive web applications with clean, maintainable code.