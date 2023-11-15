
/*
ref vs out:
    - Both 'ref' and 'out' are used for passing parameters by reference.
    - The key difference is that ref requires the variable to be initialized before passing, whereas out does not.
*/

using System;

class Program
{
    static void ModifyWithRef(ref int x)
    {
        x++;
    }

    static void ModifyWithOut(out int y)
    {
        // The variable 'y' must be assigned before leaving the method
        y = 42;
    }

    static void Main()
    {
        int a = 5;
        int b;

        ModifyWithRef(ref a);
        Console.WriteLine($"After ModifyWithRef: a = {a}");

        // Uncommenting the next line will result in a compilation error
        // ModifyWithRef(ref b);

        ModifyWithOut(out b);
        Console.WriteLine($"After ModifyWithOut: b = {b}");
    }
}


/*
Output:
    After ModifyWithRef: a = 6
    After ModifyWithOut: b = 42
*/
