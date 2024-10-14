In ASP.NET Core, a **Tag Helper** is a powerful feature that allows you to extend HTML tags with server-side logic, providing a more expressive and maintainable way to integrate backend logic with front-end markup.

### Steps to Create a Custom Tag Helper:

1. **Create a New Class for the Tag Helper**
2. **Inherit from `TagHelper`**
3. **Override the `Process` or `ProcessAsync` Method**
4. **Register the Tag Helper**
5. **Use the Tag Helper in Views**

Let's walk through each step.

### 1. Create a New Class for the Tag Helper

Start by creating a class for your Tag Helper in the `TagHelpers` folder or anywhere in your project. 

```csharp
using Microsoft.AspNetCore.Razor.TagHelpers;

public class CustomButtonTagHelper : TagHelper
{
    // Define any attributes you want to pass from the view (e.g., asp-button-text)
    public string Text { get; set; }
    public string CssClass { get; set; } = "btn-primary";

    // Override the Process method
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // Define which HTML tag this Tag Helper targets (in this case <button>)
        output.TagName = "button";

        // Add the CSS class to the button
        output.Attributes.SetAttribute("class", CssClass);

        // Set the inner HTML content of the button (i.e., the button text)
        output.Content.SetContent(Text);
    }
}
```

### 2. Inherit from `TagHelper`

Make sure your class inherits from `TagHelper`. This gives you access to methods like `Process` and properties like `Attributes`.

### 3. Override the `Process` or `ProcessAsync` Method

- **`Process`**: Use this method when you're creating a **synchronous** Tag Helper.
- **`ProcessAsync`**: Use this method for **asynchronous** tasks, such as retrieving data from a database or external service.

The `TagHelperOutput` object allows you to modify the HTML that is rendered for your Tag Helper.

In the above example:
- **`output.TagName = "button"`**: Specifies the tag that will be generated.
- **`output.Attributes.SetAttribute("class", CssClass)`**: Adds a CSS class to the button.
- **`output.Content.SetContent(Text)`**: Sets the inner content (button text).

### 4. Register the Tag Helper

To make sure your custom Tag Helper is available in your views, you need to register it in the `_ViewImports.cshtml` file.

Open `_ViewImports.cshtml` and add the following line:

```csharp
@addTagHelper *, [YourAssemblyName]
```

Replace `[YourAssemblyName]` with the name of your project or assembly.

For example, if your project is named `MyApp`, add:

```csharp
@addTagHelper *, MyApp
```

### 5. Use the Tag Helper in Views

Now you can use your Tag Helper in any Razor view. For example:

```html
<custom-button text="Click Me" css-class="btn-success"></custom-button>
```

This will render as:

```html
<button class="btn-success">Click Me</button>
```

### Passing Attributes to Tag Helpers

Any attributes passed from the view to the Tag Helper can be defined as properties in the Tag Helper class. The attribute names in the view should match the property names in the Tag Helper class.

For example:
- `text="Click Me"` corresponds to the `Text` property in the Tag Helper.
- `css-class="btn-success"` corresponds to the `CssClass` property.

### Asynchronous Tag Helper Example

If you need to perform asynchronous operations, you can override the `ProcessAsync` method. Here's an example:

```csharp
public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
{
    output.TagName = "button";
    output.Attributes.SetAttribute("class", CssClass);
    
    // Asynchronous content generation (e.g., fetching data)
    var content = await Task.FromResult("Async Button");
    
    output.Content.SetContent(content);
}
```

### Advanced Example with Conditional Rendering

Here's a more advanced example that conditionally renders the content of the Tag Helper:

```csharp
public class ConditionTagHelper : TagHelper
{
    public bool Show { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (!Show)
        {
            // Don't render the tag at all
            output.SuppressOutput();
        }
    }
}
```

You can use this `ConditionTagHelper` in the view to conditionally render HTML:

```html
<condition show="true">
    <p>This will only render if 'Show' is true</p>
</condition>
```

### Summary

- **Tag Helpers** extend the behavior of HTML tags with server-side logic in ASP.NET Core.
- Create a Tag Helper by inheriting from the `TagHelper` class and overriding the `Process` or `ProcessAsync` method.
- Register your Tag Helpers in the `_ViewImports.cshtml` file.
- Use the Tag Helper in your Razor views as a custom HTML element. 

## Example 

```csharp
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Bookify.Web.Helpers
{
    // 1. we need to know the html tag that we are going to apply this tag helper to
    [HtmlTargetElement("a", Attributes = "active-when")]
    public class ActiveTag : TagHelper
    {
        // 2. we need to know the value of the attribute that we are going to use
        // note that the name of the property should be the same as the attribute name but in pascal case (ActiveWhen)
        public string ActiveWhen { get; set; }

        // 3. we need to know the view context
        [ViewContext]
        [HtmlAttributeNotBound] // this attribute is not bound to any html attribute meaning it is not coming from the html tag
        public ViewContext? ViewContextData { get; set; }

        // 4. we need to override the Process method
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // 5. we need to check if the ActiveWhen property is null or empty
            if (string.IsNullOrEmpty(ActiveWhen))
                return;

            // 6. we need to check if the current controller is the same as the ActiveWhen property
            var currentController = ViewContextData?.RouteData.Values["controller"]?.ToString();

            // 7. we need to add the active class to the html tag if the current controller is the same as the ActiveWhen property
            if (ActiveWhen == currentController)
            {
                if(output.Attributes.TryGetAttribute("class", out var classAttribute))
                    output.Attributes.SetAttribute("class", $"{classAttribute.Value} active");
                else
                    output.Attributes.SetAttribute("class", "active");
            }

        }
    }
}
```

This example demonstrates the creation of a custom **Tag Helper** that adds an "active" CSS class to an anchor (`<a>`) tag when the current controller matches a specified value. This is especially useful for highlighting the currently active menu item or navigation link in a web application.

### Detailed Explanation of Each Part

#### 1. Apply the Tag Helper to Specific HTML Elements
```csharp
[HtmlTargetElement("a", Attributes = "active-when")]
```
- This attribute defines that the Tag Helper is only applied to `<a>` (anchor) elements. 
- Additionally, it specifies that this Tag Helper will only be used when the element has an `active-when` attribute. The `active-when` attribute is what we will pass in the view to determine when to make the link "active."

#### 2. Define a Property for the Custom Attribute
```csharp
public string ActiveWhen { get; set; }
```
- The property `ActiveWhen` is defined to capture the value of the `active-when` attribute that is used in the Razor view.
- The name `ActiveWhen` is in PascalCase (the C# naming convention), but it maps to the `active-when` attribute in the Razor view.

#### 3. Capture the Current View Context
```csharp
[ViewContext]
[HtmlAttributeNotBound]
public ViewContext? ViewContextData { get; set; }
```
- The `ViewContext` attribute tells the Tag Helper to inject the current view context (like the current controller and route data). This allows the Tag Helper to understand which controller and action are currently being executed.
- `HtmlAttributeNotBound` ensures that this property is not bound to any HTML attribute. It's populated automatically by ASP.NET Core when the view is rendered, rather than being provided by the Razor view itself.

#### 4. Override the `Process` Method
```csharp
public override void Process(TagHelperContext context, TagHelperOutput output)
```
- The `Process` method is overridden to define the logic for modifying the HTML tag. This method is where we will add the `active` class to the anchor tag if necessary.

#### 5. Check if the `ActiveWhen` Property is Null or Empty
```csharp
if (string.IsNullOrEmpty(ActiveWhen))
    return;
```
- Before proceeding with any logic, this check ensures that the `active-when` attribute was provided with a value in the view. If it's not provided, the method simply returns, and no changes are made to the anchor tag.

#### 6. Retrieve the Current Controller Name
```csharp
var currentController = ViewContextData?.RouteData.Values["controller"]?.ToString();
```
- This line fetches the name of the current controller from the `RouteData` of the `ViewContext`. This is important because the logic needs to check if the current controller matches the value provided by the `active-when` attribute.

#### 7. Add the `active` Class if the Controller Matches
```csharp
if (ActiveWhen == currentController)
{
    if(output.Attributes.TryGetAttribute("class", out var classAttribute))
        output.Attributes.SetAttribute("class", $"{classAttribute.Value} active");
    else
        output.Attributes.SetAttribute("class", "active");
}
```
- **Condition Check**: If the value of `ActiveWhen` matches the name of the current controller, the code inside the `if` block is executed.
- **Class Attribute Handling**: The `output.Attributes.TryGetAttribute("class", out var classAttribute)` checks if the anchor tag already has a `class` attribute. 
  - If it does, the `active` class is appended to the existing classes.
  - If it does not, a new `class` attribute is added with the value `active`.

### How to Use the `ActiveTag` Tag Helper

1. **Register the Tag Helper**: Ensure that your Tag Helper is registered in the `_ViewImports.cshtml` file.

```csharp
@addTagHelper *, Bookify.Web
```

2. **Use the Tag Helper in a Razor View**: Here's an example of how to use the `active-when` attribute in your Razor views.

```html
<a href="/Home" active-when="Home">Home</a>
<a href="/Products" active-when="Products">Products</a>
<a href="/Contact" active-when="Contact">Contact</a>
```

- When the `Home` controller is active, the first `<a>` tag will have the `active` class, making it visually distinct.
- Similarly, the second and third `<a>` tags will be "active" when the `Products` or `Contact` controller is active, respectively.

### Example Scenario

Suppose you have a navigation menu on your website, and you want to highlight the currently selected page. The `ActiveTag` helper automatically checks the current controller and applies the `active` class to the corresponding link. This is particularly useful for generating navigation menus in a DRY (Don't Repeat Yourself) manner, as you no longer need to manually check the controller in every link.

### Benefits of the Custom Tag Helper:
1. **Reusability**: Once created, you can use this Tag Helper across all your views without writing duplicate logic.
2. **Separation of Concerns**: The logic for determining which link should be active is encapsulated in the Tag Helper, keeping your Razor views clean and maintainable.
3. **Flexibility**: You can easily extend this helper to check for additional conditions (such as actions or route values) if needed.

### Full Example:

**View:**
```html
<a href="/Home" active-when="Home">Home</a>
<a href="/Products" active-when="Products">Products</a>
<a href="/Contact" active-when="Contact">Contact</a>
```

**Resulting HTML:**
When the user is on the `Products` page, the HTML will be:
```html
<a href="/Home">Home</a>
<a href="/Products" class="active">Products</a>
<a href="/Contact">Contact</a>
```

This highlights the `Products` link as active when the user is on the `Products` controller.

### Conclusion

This example demonstrates how to create a custom **Tag Helper** in ASP.NET Core to dynamically add classes to HTML elements based on the current controller. This is particularly helpful for navigation menus, allowing the "active" state to be automatically determined based on the route data. The helper can be further extended to match actions, areas, or even more complex conditions.