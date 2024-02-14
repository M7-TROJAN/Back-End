using System;

public class MyBaseClass
{
    public virtual void MyMethod()
    {
        Console.WriteLine("Base class implementation");
    }

    public virtual void MyOtherMethod()
    {
        Console.WriteLine("Base class implementation of MyOtherMethod");
    }
}

public class MyDerivedClass : MyBaseClass
{
    public override void MyMethod()
    {
        Console.WriteLine("Derived class implementation using override");
    }

    // Using 'new' to shadow the method from the base class
    public new void MyOtherMethod()
    {
        Console.WriteLine("Derived class implementation of MyOtherMethod using new");
    }
}

class Program
{
    static void Main(string[] args)
    {
        MyBaseClass myBaseObj = new MyBaseClass();
        Console.WriteLine("\nBase Object:\n");
        myBaseObj.MyMethod(); // Output: "Base class implementation"
        myBaseObj.MyOtherMethod(); // Output: "Base class implementation of MyOtherMethod"

        MyDerivedClass myDerivedObj = new MyDerivedClass();
        Console.WriteLine("\nDerived Object:\n");
        myDerivedObj.MyMethod(); // Output: "Derived class implementation using override"
        myDerivedObj.MyOtherMethod(); // Output: "Derived class implementation of MyOtherMethod using new"

        MyBaseClass myDerivedObjAsBase = myDerivedObj;
        Console.WriteLine("\nAfter Casting:\n");
        myDerivedObjAsBase.MyMethod(); // Output: "Derived class implementation using override"
        myDerivedObjAsBase.MyOtherMethod(); // Output: "Base class implementation of MyOtherMethod"

        Console.ReadKey();
    }
}

// In C#, when you use the `override` keyword to redefine a method in a derived class, it indicates that the method overrides a virtual method in the base class. 
// This means that the method in the derived class will be called instead of the method in the base class when called through a reference to the derived class.
// In contrast, when you use the `new` keyword to redefine a method in a derived class, it indicates that the method is hiding the base class method, rather than overriding it. 
// This means that the method in the derived class will be called when called through a reference to the derived class, 
// but the method in the base class will still be called when called through a reference to the base class.
