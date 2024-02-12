Immutable objects in programming refer to objects whose state (i.e., data) cannot be changed after they are created. Once an immutable object is constructed, its state remains constant throughout its lifetime.

This means that once the constructor for an immutable object has completed its execution and initialized all of its fields or properties, those fields or properties cannot be altered or modified. Any attempt to modify the object's state would result in creating a new instance of the object with the desired changes rather than altering the existing one.

```csharp
public class ImmutableObject
{
    public int Value { get; }

    // Constructor to initialize the value
    public ImmutableObject(int value)
    {
        Value = value;
    }
}
```

In example, `ImmutableObject` is an immutable class with a single property `Value`. 
Once an instance of `ImmutableObject` is created and its `Value` property is set in the constructor, 
the `Value` property cannot be changed because it has only a getter and no setter. 
Therefore, instances of `ImmutableObject` are immutable. Any attempt to modify `Value` would require creating a new instance of `ImmutableObject` 
with the desired value.
