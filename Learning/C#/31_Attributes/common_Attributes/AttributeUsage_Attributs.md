the `[AttributeUsage]` attribute and understand its purpose.

### Understanding `[AttributeUsage]`

The `[AttributeUsage]` attribute is used to specify how a custom attribute class can be used. It defines the types of program elements that the attribute can be applied to and other usage rules.

### Components of `[AttributeUsage]`

The `[AttributeUsage]` attribute accepts several parameters, but the most commonly used one is `AttributeTargets`. Here's what it does:

1. **AttributeTargets**: This parameter specifies the kinds of elements to which the attribute can be applied, such as classes, methods, properties, etc. 

### Example 

```csharp
[AttributeUsage(AttributeTargets.Property)]
```

- `AttributeUsage` is the attribute being applied.
- `AttributeTargets.Property` specifies that the custom attribute can only be applied to properties.

This means you can only use the `SkillAttribute` on properties and not on methods, classes, fields, etc.

### AttributeTargets Values

Here are some possible values you can use with `AttributeTargets`:

- `AttributeTargets.All`: Specifies that the attribute can be applied to any application element.
- `AttributeTargets.Assembly`: Attribute can be applied to an assembly.
- `AttributeTargets.Class`: Attribute can be applied to a class.
- `AttributeTargets.Constructor`: Attribute can be applied to a constructor.
- `AttributeTargets.Delegate`: Attribute can be applied to a delegate.
- `AttributeTargets.Enum`: Attribute can be applied to an enumeration.
- `AttributeTargets.Event`: Attribute can be applied to an event.
- `AttributeTargets.Field`: Attribute can be applied to a field.
- `AttributeTargets.Interface`: Attribute can be applied to an interface.
- `AttributeTargets.Method`: Attribute can be applied to a method.
- `AttributeTargets.Module`: Attribute can be applied to a module.
- `AttributeTargets.Parameter`: Attribute can be applied to a parameter.
- `AttributeTargets.Property`: Attribute can be applied to a property.
- `AttributeTargets.Struct`: Attribute can be applied to a struct.

### Example

```csharp
[AttributeUsage(AttributeTargets.Property)]
public sealed class SkillAttribute : Attribute
{
    public SkillAttribute(string name, int minimum, int maximum)
    {
        Name = name;
        Minimum = minimum;
        Maximum = maximum;
    }

    public string Name { get; }
    public int Minimum { get; }
    public int Maximum { get; }

    public bool IsValid(object value)
    {
        if (value is int intValue)
        {
            return intValue >= Minimum && intValue <= Maximum;
        }
        return false;
    }
}
```

Here, `SkillAttribute` is a custom attribute that can only be applied to properties, thanks to the `[AttributeUsage(AttributeTargets.Property)]` declaration.

