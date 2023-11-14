using System;

// Base class A with a virtual Print method
class BaseClassA
{
    public virtual void Print()
    {
        Console.WriteLine("Hi, I am Print from the Base Class A");
    }
}

// Derived class B inheriting from BaseClassA
class DerivedClassB : BaseClassA
{
    // Override the Print method in the derived class
    public override void Print()
    {
        Console.WriteLine("Hi, I am Print from the Derived Class B");
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        // Create an instance of BaseClassA
        BaseClassA instanceA = new BaseClassA();

        // Call the Print method on the instance of BaseClassA
        instanceA.Print();

        // Create an instance of DerivedClassB
        DerivedClassB instanceB = new DerivedClassB();

        // Call the Print method on the instance of DerivedClassB
        instanceB.Print();
    }
}
