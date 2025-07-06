##  Why Custom Validation?

الـ **Data Annotations** الجاهزة (زي `[Required]`, `[EmailAddress]`, `[MaxLength]`, …) بتغطي حاجات كثيرة،
لكن أحيانًا بتكون عندك شروط خاصة جدًا مش موجودة في الـ built-in attributes.

> مثلًا: المستخدم لازم يكون عمره 18 سنة أو أكتر.
> أو: اسم المستخدم لازم يبدأ بـ "admin" في حالات معينة.

في الحالة دي، بتضطر تعمل **Custom Validation Attribute** بنفسك.

---

## Basic Example (With `bool IsValid(object? value)`)

### Step 1: Define a Model

```csharp
public class Student
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    [MinAge] // Apply custom validation here
    public DateTime DateOfBirth { get; set; }
}
```

---

### Step 2: Create `MinAge` Attribute (Basic Version)

```csharp
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class MinAgeAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not null)
        {
            if (value is DateTime dateOfBirth)
            {
                var age = DateTime.Now.Year - dateOfBirth.Year;

                // If the birthday hasn't happened yet this year
                if (dateOfBirth > DateTime.Now.AddYears(-age))
                    age--;

                if (age < 18)
                {
                    ErrorMessage = "Age must be at least 18 years.";
                    return false;
                }
            }
        }

        return true;
    }
}
```

### 🧾 Explanation (Line by Line):

| Line                                                 | Explanation                                                              |
| ---------------------------------------------------- | ------------------------------------------------------------------------ |
| `[AttributeUsage(...)]`                              | Specifies where this attribute can be used. Properties or fields only.   |
| `public class MinAgeAttribute : ValidationAttribute` | Inherit from `ValidationAttribute` to implement custom logic.            |
| `public override bool IsValid(object? value)`        | This method is automatically called during validation.                   |
| `if (value is not null)`                             | Null check before casting.                                               |
| `if (value is DateTime dateOfBirth)`                 | Ensure value is DateTime.                                                |
| `var age = ...`                                      | Calculate age by subtracting years.                                      |
| `if (dateOfBirth > DateTime.Now.AddYears(-age))`     | Check if birthday hasn’t occurred this year, and adjust age accordingly. |
| `if (age < 18)`                                      | Validate the age.                                                        |
| `ErrorMessage = ...`                                 | Set the error message.                                                   |
| `return false` / `true`                              | Return based on validation result.                                       |

---

## Limitation in the Basic Version

* Can't access model-level context (like the property name, or the full object).
* You can't pass parameters (like `minAge = 18`) unless you hardcode them.

---

## Recommended Version (Using `ValidationResult`)

### Improved `MinAgeAttribute` (with parameter + full context):

```csharp
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class MinAgeAttribute : ValidationAttribute
{
    private readonly int _minAge;

    public MinAgeAttribute(int minAge)
    {
        _minAge = minAge;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime dateOfBirth)
        {
            var age = DateTime.Now.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Now.AddYears(-age))
                age--;

            if (age < _minAge)
            {
                return new ValidationResult(
                    $"Invalid {validationContext.DisplayName}, Age must be at least {_minAge} years old.");
            }
        }

        return ValidationResult.Success;
    }
}
```

### Extra Explanation:

| Code                                                               | What it does                                           |
| ------------------------------------------------------------------ | ------------------------------------------------------ |
| `new ValidationResult(...)`                                        | This allows you to return a **custom error message**.  |
| `validationContext.DisplayName`                                    | Gets the display name of the property being validated. |
| `_minAge`                                                          | Comes from the attribute parameter.                    |
| You can write `[MinAge(18)]` on any property and reuse this logic. |                                                        |

---

### Example Usage

```csharp
public class Student
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    [MinAge(18)] // Uses parameterized version
    public DateTime DateOfBirth { get; set; }
}
```

---

## Another Custom Example: StartsWithAttribute

### Goal:

Make sure a name starts with a given prefix (like "Admin").

```csharp
public class StartsWithAttribute : ValidationAttribute
{
    private readonly string _prefix;

    public StartsWithAttribute(string prefix)
    {
        _prefix = prefix;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        if (value is string str && !str.StartsWith(_prefix))
        {
            return new ValidationResult($"{context.DisplayName} must start with '{_prefix}'.");
        }

        return ValidationResult.Success;
    }
}
```

### Usage:

```csharp
public class AdminUser
{
    [StartsWith("Admin")]
    public string Username { get; set; } = string.Empty;
}
```

---

## ✅ Summary

| Topic                                        | Covered                        |
| -------------------------------------------- | ------------------------------ |
| Why Custom Validation                        | ✅                              |
| Basic Attribute with `bool IsValid()`        | ✅                              |
| Advanced Attribute with `ValidationResult`   | ✅                              |
| Accessing context (property name)            | ✅                              |
| Passing parameters to attributes             | ✅                              |
| Real-world examples (`MinAge`, `StartsWith`) | ✅                              |
| Where to use it                              | In models, DTOs, or ViewModels |

---
