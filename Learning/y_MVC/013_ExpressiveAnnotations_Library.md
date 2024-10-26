### **ExpressiveAnnotations: Comprehensive Documentation**

#### **Overview**
**ExpressiveAnnotations** is an open-source library for .NET that enhances **data annotation attributes** in C# by allowing complex validation rules written in a concise and natural language expression syntax. This library simplifies validation logic by utilizing expressions rather than writing custom validation attributes or server-side checks.

It is commonly used in **ASP.NET Core MVC** and **ASP.NET MVC** projects to add client-side and server-side validation for form inputs. You can write flexible, expressive conditions directly in your data annotations, which means complex validation logic can be added in a declarative manner.

GitHub Repository: [ExpressiveAnnotations on GitHub](https://github.com/jwaliszko/ExpressiveAnnotations)

---

#### **Key Features**
1. **Declarative Validation Logic**: Write complex validation expressions using simple, readable syntax.
2. **Client-Side Validation**: Automatically translates server-side expressions into JavaScript for client-side validation.
3. **Flexible Expression Syntax**: Supports a wide range of operators (`&&`, `||`, `!`, etc.) and functions (`Len()`, `RegexMatch()`, etc.).
4. **Seamless Integration**: Integrates well with ASP.NET MVC validation pipelines for both server and client-side validation.
5. **Custom Error Messages**: Allows defining dynamic, custom error messages based on validation logic.
6. **Field Dependencies**: Validate fields based on the values of other fields in the model.

---

#### **Installation**

To install the ExpressiveAnnotations package, you can use **NuGet**. Run the following command in the **NuGet Package Manager Console**:

```bash
Install-Package ExpressiveAnnotations
```

Alternatively, for **.NET Core** projects, you can use the **dotnet CLI**:

```bash
dotnet add package ExpressiveAnnotations
```

Once installed, you can start using the expressive validation attributes in your models.

---

#### **Registering ExpressiveAnnotations in Program.cs**

To use ExpressiveAnnotations in an ASP.NET Core project, it’s essential to register the library within **Program.cs**. Without this registration, ExpressiveAnnotations will not be activated for validation purposes, resulting in errors. To register, add the following line within your service configuration in **Program.cs**:

```csharp
// Add ExpressiveAnnotations
builder.Services.AddExpressiveAnnotations();
```

This line should be placed in the **ConfigureServices** section, which sets up dependency injection and middleware for your application. Once added, ExpressiveAnnotations will be ready to handle validations specified in your models.

--- 

#### **Key Attributes**
1. **[AssertThat]**: Validates a condition that must evaluate to `true` for the validation to pass.
2. **[RequiredIf]**: Requires a field to be filled in if a certain condition evaluates to `true`.

---

#### **Usage of ExpressiveAnnotations**

##### **1. [AssertThat]**
The `[AssertThat]` attribute validates that a condition is `true` for the model to be valid. It allows the creation of complex validation logic by evaluating fields within an expression.

**Syntax:**
```csharp
[AssertThat("expression", ErrorMessage = "Your custom error message.")]
```

**Example:**
```csharp
public class ProductViewModel
{
    public decimal Price { get; set; }

    [AssertThat("Price >= 0", ErrorMessage = "The price must be a positive number.")]
    public decimal DiscountPrice { get; set; }
}
```

In this example, the `DiscountPrice` must be a positive number for the model to be valid.

##### **2. [RequiredIf]**
The `[RequiredIf]` attribute makes a field required if the condition is satisfied. This attribute is useful for making fields conditionally required based on other field values.

**Syntax:**
```csharp
[RequiredIf("expression", ErrorMessage = "This field is required if the condition is met.")]
```

**Example:**
```csharp
public class BookingViewModel
{
    public bool IsInternational { get; set; }

    [RequiredIf("IsInternational == true", ErrorMessage = "Passport number is required for international bookings.")]
    public string PassportNumber { get; set; }
}
```

In this case, the `PassportNumber` field is only required if `IsInternational` is set to `true`.

---

#### **Common Expression Syntax and Built-In Functions**

Here are some common operators and functions you can use within expressions:

- **Arithmetic Operators**: `+`, `-`, `*`, `/`
- **Comparison Operators**: `==`, `!=`, `<`, `<=`, `>`, `>=`
- **Logical Operators**: `&&`, `||`, `!`

---

#### **Built-In Functions**

ExpressiveAnnotations offers several built-in functions that can be used within expressions to create complex validation rules:

1. **Len(field)**: Returns the length of a string.
    - **Example**:
      ```csharp
      [AssertThat("Len(Name) > 5", ErrorMessage = "Name must be longer than 5 characters.")]
      public string Name { get; set; }
      ```

2. **RegexMatch(field, pattern)**: Checks if the value of a string field matches a regular expression pattern.
    - **Example**:
      ```csharp
      [AssertThat("RegexMatch(Email, '^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')", ErrorMessage = "Invalid email format.")]
      public string Email { get; set; }
      ```

3. **Now()**: Returns the current date and time.
    - **Example**:
      ```csharp
      [AssertThat("DateOfBirth <= Now()", ErrorMessage = "Date of Birth cannot be in the future.")]
      public DateTime DateOfBirth { get; set; }
      ```

4. **Date(field)**: Extracts the date part from a DateTime field, ignoring the time component.
    - **Example**:
      ```csharp
      [AssertThat("Date(BookingDate) >= Date(Now())", ErrorMessage = "Booking date cannot be in the past.")]
      public DateTime BookingDate { get; set; }
      ```

5. **IsNull(field)**: Checks if the field is null.
    - **Example**:
      ```csharp
      [AssertThat("!IsNull(Address)", ErrorMessage = "Address must be provided.")]
      public string Address { get; set; }
      ```

6. **IsEmpty(field)**: Checks if the field is an empty string.
    - **Example**:
      ```csharp
      [AssertThat("!IsEmpty(Username)", ErrorMessage = "Username cannot be empty.")]
      public string Username { get; set; }
      ```

7. **Contains(container, item)**: Checks if the container (list, array, or string) contains a specific item.
    - **Example**:
      ```csharp
      [AssertThat("Contains(UserRoles, 'Admin')", ErrorMessage = "User must have the Admin role.")]
      public List<string> UserRoles { get; set; }
      ```

8. **Count(field)**: Returns the count of items in a list or array.
    - **Example**:
      ```csharp
      [AssertThat("Count(Items) > 0", ErrorMessage = "At least one item must be selected.")]
      public List<string> Items { get; set; }
      ```

---

### **Client-Side Validation with ExpressiveAnnotations**

ExpressiveAnnotations supports **automatic client-side validation** for ASP.NET Core MVC and ASP.NET MVC projects. This feature translates the validation logic written in C# into JavaScript, allowing the validation to run in the browser. Client-side validation gives users immediate feedback, reducing the need for server requests when data is invalid.

To set up client-side validation with ExpressiveAnnotations, follow these steps:

1. **Ensure jQuery and jQuery Unobtrusive Validation Libraries Are Loaded**

   ASP.NET MVC typically includes these by default, as they’re required for client-side validation. Verify that the following libraries are referenced in your `_Layout.cshtml` or specific views where you want validation:

   ```html
   <script src="~/Scripts/jquery-1.10.2.min.js"></script>
   <script src="~/Scripts/jquery.validate.min.js"></script>
   <script src="~/Scripts/jquery.validate.unobtrusive.min.js"></script>
   ```

2. **Add ExpressiveAnnotations Client-Side Validation Library**

   - **Install the Library**: 
     1. Right-click on the `wwwroot` folder (or the folder where your client-side libraries are stored), and choose **Add > Client-Side Library**.
     2. In the **Library** field, search for `expressive-annotations-validate@2.7.0` and select it.
     3. Click **Install** to add the library to your project’s `lib` folder within `wwwroot`.

   - **Reference the Script**: After installation, add a reference to the ExpressiveAnnotations client-side validation script in `_Layout.cshtml` or the specific view that requires validation:

     ```html
     <script src="~/lib/expressive-annotations-validate/expressive.annotations.validate.min.js"></script>
     ```

3. **Enable Validation in Your View**

   To ensure the validation scripts are enabled and functioning, include the following code in the view:

   ```html
   @Html.ValidationSummary(true)
   @Scripts.Render("~/bundles/jquery")
   @Scripts.Render("~/bundles/jqueryval")
   ```

After these steps, validation will run both on the client side (in the browser) and on the server side, reducing unnecessary server requests for invalid data and providing a smoother user experience.

---

#### **Example: Complete Model Using ExpressiveAnnotations**

```csharp
public class UserViewModel
{
    public string Username { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [AssertThat("RegexMatch(Email, '^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')", ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    public bool IsAdmin { get; set; }

    [RequiredIf("IsAdmin == true", ErrorMessage = "Admin privileges require an access code.")]
    public string? AccessCode { get; set; }

    [AssertThat("Len(Password) >= 8", ErrorMessage = "Password must be at least 8 characters long.")]
    public string Password { get; set; }

    [AssertThat("Password == ConfirmPassword", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
}
```

In this example:
- The `Email` field must be a valid email format.
- The `AccessCode` field is required only if `IsAdmin` is true.
- The `Password` must be at least 8 characters long and must match the `ConfirmPassword`.

---

#### **Limitations**

While ExpressiveAnnotations provides great flexibility, there are a few limitations:
- It does not support cross-model validation (i.e., validation that spans across multiple models).
- Complex validation logic can become difficult to maintain if overused within annotations.

---

### **

Conclusion**

ExpressiveAnnotations is a powerful library for enhancing validation logic in .NET applications, especially in ASP.NET MVC and ASP.NET Core. It allows you to write complex validation rules using expressive syntax, which can significantly reduce the need for custom validation logic and simplify your codebase. The integration with client-side validation ensures that users receive instant feedback when they input invalid data, improving user experience.
