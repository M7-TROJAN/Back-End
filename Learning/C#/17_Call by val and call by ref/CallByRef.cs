
/*
 Call by Reference:
    - In call by reference, the memory address (reference) of the actual parameter is passed to the method.
    - Changes made to the parameter inside the method affect the original value.
    - This is achieved using the 'ref' keyword.
    Note: The 'ref' keyword is used both in the method call and within the method body.
*/

using System;

class Program
{
    static void Increment(ref int x)
    {
        x++;
        Console.WriteLine($"Inside Increment method: x = {x}");
    }

    static void Main()
    {
        int num = 5;
        Console.WriteLine($"Before method call: num = {num}");
        Increment(ref num);
        Console.WriteLine($"After method call: num = {num}");
    }
}

/*
Output:
    Before method call: num = 5
    Inside Increment method: x = 6
    After method call: num = 6
*/
