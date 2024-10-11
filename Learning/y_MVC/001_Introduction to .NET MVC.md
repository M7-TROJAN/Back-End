# Introduction to .NET MVC

## Table of Contents
1. [What is MVC?](#what-is-mvc)
2. [Benefits of MVC Architecture](#benefits-of-mvc-architecture)
3. [Understanding the MVC Structure](#understanding-the-mvc-structure)
   - [Model](#model)
   - [View](#view)
   - [Controller](#controller)
4. [MVC Flow: How it Works](#mvc-flow-how-it-works)
5. [Setting up a .NET MVC Project](#setting-up-a-net-mvc-project)
6. [Example Project](#example-project)
7. [Conclusion](#conclusion)

---

## What is MVC?

MVC stands for **Model-View-Controller**. It is a software architectural pattern that separates an application into three main logical components:

1. **Model**: Represents the application data and business logic.
2. **View**: Represents the UI elements of the application (HTML, CSS, etc.).
3. **Controller**: Acts as an intermediary between Model and View, processing incoming requests, handling user input, and updating the data model.

.NET MVC (Model-View-Controller) is a web application framework developed by Microsoft that provides a powerful, pattern-based way to build dynamic websites. It's a core part of ASP.NET and focuses on separation of concerns, making the application easier to test and maintain.

## Benefits of MVC Architecture

- **Separation of Concerns**: Each component (Model, View, Controller) has a specific role, making the codebase more organized.
- **Easier Testing**: Due to separation, unit testing becomes easier.
- **Reusability**: Components can be reused across multiple projects.
- **Scalability**: The design pattern makes it easier to scale the application.
- **Support for TDD (Test-Driven Development)**: Promotes better testing methodologies.

## Understanding the MVC Structure

The MVC architecture is divided into three main components:

### Model
- **Definition**: Represents the application's data structure. It directly manages the data, logic, and rules of the application.
- **Purpose**: Retrieve, store, and manipulate the data.
- **Example**:
  ```csharp
  public class Student
  {
      public int Id { get; set; }
      public string Name { get; set; }
      public int Age { get; set; }
  }
  ```

### View
- **Definition**: Represents the user interface elements of the application.
- **Purpose**: Displays data to the user and sends UI events (like button clicks) back to the controller.
- **Example** (Razor View in .NET MVC):
  ```html
  @model Student

  <h2>Student Details</h2>
  <p>Name: @Model.Name</p>
  <p>Age: @Model.Age</p>
  ```

### Controller
- **Definition**: Manages user requests, processes them, and sends responses back to the view.
- **Purpose**: Acts as a mediator between the View and the Model.
- **Example**:
  ```csharp
  public class StudentController : Controller
  {
      public ActionResult Details(int id)
      {
          Student student = new Student { Id = id, Name = "John Doe", Age = 22 };
          return View(student);
      }
  }
  ```

## MVC Flow: How it Works

1. **User Request**: The user makes a request via the browser (e.g., `/Student/Details/1`).
2. **Controller Action**: The request is routed to the appropriate action method of a controller (e.g., `StudentController.Details`).
3. **Model Interaction**: The controller interacts with the Model to fetch or manipulate data.
4. **View Rendering**: The controller then selects a View and passes the model data to it.
5. **Response**: The View is rendered as an HTML response, which is sent back to the browser.

![MVC Flow](https://www.tutorialsteacher.com/Content/images/mvc/mvc.png)

## Setting up a .NET MVC Project

### Prerequisites
- Visual Studio or Visual Studio Code
- .NET SDK installed

### Steps to Create a Basic .NET MVC Project
1. **Open Visual Studio** and select **Create a New Project**.
2. Choose **ASP.NET Web Application (.NET Framework)**.
3. Name the project (e.g., `MVCDemo`).
4. Select **MVC Template**.
5. Click **Create**.

### Folder Structure
When you create an MVC project, you get the following folders by default:

- **Controllers**: Contains controller files like `HomeController.cs`.
- **Models**: Contains classes that represent the data model.
- **Views**: Contains Razor View files like `Index.cshtml`, `About.cshtml`.
- **wwwroot**: Contains static files like CSS, JavaScript, and images.

## Example Project

### Step 1: Create a Model
Create a new class called `Product` in the `Models` folder:

```csharp
public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }
}
```

### Step 2: Create a Controller
Add a new controller called `ProductController` in the `Controllers` folder:

```csharp
public class ProductController : Controller
{
    public ActionResult Index()
    {
        var products = new List<Product>
        {
            new Product{ ProductId=1, ProductName="Laptop", Price=1200.50 },
            new Product{ ProductId=2, ProductName="Tablet", Price=300.75 }
        };
        return View(products);
    }
}
```

### Step 3: Create a View
Add a new Razor View called `Index.cshtml` in the `Views/Product` folder:

```html
@model List<Product>

<h2>Product List</h2>
<table class="table">
    <thead>
        <tr>
            <th>Product ID</th>
            <th>Product Name</th>
            <th>Price</th>
        </tr>
    </thead>
    <tbody>
        @foreach(var product in Model)
        {
            <tr>
                <td>@product.ProductId</td>
                <td>@product.ProductName</td>
                <td>@product.Price</td>
            </tr>
        }
    </tbody>
</table>
```

### Step 4: Run the Application
- Press `F5` to run the application.
- Navigate to `/Product/Index` to view the list of products.

## Conclusion

The MVC architecture is a powerful design pattern that makes it easier to build, test, and maintain web applications. By separating the application into three components—Model, View, and Controller—developers can create organized, maintainable code. .NET MVC leverages this pattern to deliver robust web applications efficiently.