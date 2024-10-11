# Passing Data From Controller to View

## Table of Contents
1. [Introduction](#introduction)
2. [Ways to Pass Data from Controller to View](#ways-to-pass-data-from-controller-to-view)
3. [1. ViewData](#1-viewdata)
   - [Passing Data Using ViewData](#passing-data-using-viewdata)
4. [2. ViewBag](#2-viewbag)
   - [Passing Data Using ViewBag](#passing-data-using-viewbag)
5. [3. TempData](#3-tempdata)
   - [Passing Data Using TempData](#passing-data-using-tempdata)
6. [4. Strongly-Typed Model](#4-strongly-typed-model)
   - [Passing Data Using Strongly-Typed Models](#passing-data-using-strongly-typed-models)
7. [Comparison of ViewData, ViewBag, and TempData](#comparison-of-viewdata-viewbag-and-tempdata)
8. [Practical Examples](#practical-examples)
9. [Conclusion](#conclusion)

---

## Introduction

In ASP.NET MVC, controllers are responsible for handling user input, retrieving or manipulating data, and passing the data to views for display. There are several ways to pass data from a controller to a view, each with its own use cases and benefits. The primary methods are:

1. **ViewData**  
2. **ViewBag**  
3. **TempData**  
4. **Strongly-Typed Models**

### Why Do We Pass Data From Controller to View?
Passing data from the controller to the view is essential for dynamic rendering of content. For example, if you want to display a list of products, a controller will fetch the product data and pass it to the view, where it will be displayed using Razor syntax.

---

## Ways to Pass Data from Controller to View

Let's explore each of these methods in detail:

## 1. ViewData

### What is ViewData?
**ViewData** is a dictionary object that stores data in key-value pairs. It is derived from the `ViewDataDictionary` class and is available for the current request. ViewData is useful for passing small amounts of data from the controller to the view.

### Syntax:
- **In Controller:**  
  `ViewData["key"] = value;`

- **In View:**  
  `@ViewData["key"]`

### Passing Data Using ViewData

**Controller:**
```csharp
public ActionResult Index()
{
    ViewData["Message"] = "Hello, welcome to ASP.NET MVC!";
    ViewData["Year"] = DateTime.Now.Year;
    return View();
}
```

**View:**
```html
<h2>@ViewData["Message"]</h2>
<p>Year: @ViewData["Year"]</p>
```

### Pros and Cons of ViewData
- **Pros**:  
  - Can pass data between controller and view easily.
  - Can pass multiple data items with different keys.

- **Cons**:  
  - Typecasting is required when extracting data.
  - Errors at runtime if the key does not exist or has a wrong type.

---

## 2. ViewBag

### What is ViewBag?
**ViewBag** is a dynamic property that provides a way to pass data from the controller to the view using the `ViewData` dictionary internally. It is simpler and more readable than `ViewData` since it does not require typecasting.

### Syntax:
- **In Controller:**  
  `ViewBag.PropertyName = value;`

- **In View:**  
  `@ViewBag.PropertyName`

### Passing Data Using ViewBag

**Controller:**
```csharp
public ActionResult Index()
{
    ViewBag.Message = "Hello, welcome to ASP.NET MVC using ViewBag!";
    ViewBag.CurrentTime = DateTime.Now.ToString();
    return View();
}
```

**View:**
```html
<h2>@ViewBag.Message</h2>
<p>Current Time: @ViewBag.CurrentTime</p>
```

### Pros and Cons of ViewBag
- **Pros**:  
  - No typecasting required.
  - Easier to use and more readable than `ViewData`.
  - Good for passing small amounts of data.

- **Cons**:  
  - It is a dynamic property, so errors can only be caught at runtime.

---

## 3. TempData

### What is TempData?
**TempData** is a dictionary object derived from `TempDataDictionary`. It is used to store data temporarily and is available for the current and subsequent requests. It is mainly used for transferring data between controllers or for passing messages like success or error notifications.

### Syntax:
- **In Controller:**  
  `TempData["key"] = value;`

- **In View:**  
  `@TempData["key"]`

### Passing Data Using TempData

**Controller:**
```csharp
public ActionResult Index()
{
    TempData["SuccessMessage"] = "Data saved successfully!";
    return RedirectToAction("Success");
}

public ActionResult Success()
{
    return View();
}
```

**View (`Success.cshtml`):**
```html
@if (TempData["SuccessMessage"] != null)
{
    <div class="alert alert-success">
        @TempData["SuccessMessage"]
    </div>
}
```

### Pros and Cons of TempData
- **Pros**:  
  - Can persist data for multiple requests.
  - Useful for redirect scenarios.

- **Cons**:  
  - Data is cleared after it is read.
  - Requires careful handling to avoid data loss.

---

## 4. Strongly-Typed Model

### What is a Strongly-Typed Model?
A **Strongly-Typed Model** is a class that represents the data structure passed from the controller to the view. Using strongly-typed models ensures type safety and makes the code more maintainable.

### Syntax:
- **In Controller:**  
  `return View(ModelObject);`

- **In View:**  
  `@model Namespace.ModelClass`

### Passing Data Using Strongly-Typed Models

**Model Class:**
```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}
```

**Controller:**
```csharp
public ActionResult Index()
{
    var person = new Person
    {
        Name = "John Doe",
        Age = 30
    };
    return View(person);
}
```

**View (`Index.cshtml`):**
```html
@model MyApp.Models.Person

<h2>Person Details</h2>
<p>Name: @Model.Name</p>
<p>Age: @Model.Age</p>
```

### Pros and Cons of Strongly-Typed Models
- **Pros**:  
  - Type safety and compile-time checking.
  - Intellisense support in the view.
  - Ideal for passing complex data.

- **Cons**:  
  - Requires creating a model class.
  - Not suitable for passing small pieces of unrelated data.

---

## Comparison of ViewData, ViewBag, and TempData

| Feature       | ViewData                       | ViewBag                | TempData                 |
|---------------|--------------------------------|------------------------|-------------------------|
| **Type**      | Dictionary                     | Dynamic                | Dictionary              |
| **Lifetime**  | Current request                | Current request        | Current and next request|
| **Type Safety** | No                            | No                     | No                      |
| **Performance** | Slower (requires boxing/unboxing) | Slightly faster        | Slower                  |
| **Use Case**  | Passing data to view           | Passing data to view   | Passing data between controllers |
| **Best For**  | Small amounts of data          | Small amounts of data  | Redirect scenarios      |

---

## Practical Examples

### Example 1: Using ViewData
**Controller:**
```csharp
public ActionResult Index()
{
    ViewData["Title"] = "Welcome to MVC!";
    return View();
}
```
**View:**
```html
<h1>@ViewData["Title"]</h1>
```

### Example 2: Using Strongly-Typed Model
**Controller:**
```csharp
public ActionResult Product()
{
    var product = new Product { Name = "Laptop", Price = 1500 };
    return View(product);
}
```
**View:**
```html
@model MyApp.Models.Product

<h2>Product Name: @Model.Name</h2>
<p>Price: @Model.Price</p>
```

---

## Conclusion

Passing data from the controller to the view is a crucial concept in ASP.NET MVC. Depending on your use case, you can choose from `ViewData`, `ViewBag`, `TempData`, or strongly-typed models. Each method has its advantages and scenarios where it is best suited.