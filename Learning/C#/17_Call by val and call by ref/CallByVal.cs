
/*
 Call by Value:
    - In call by value, a copy of the actual parameter's value is passed to the method.
    - Changes made to the parameter inside the method do not affect the original value.
    - Most basic data types (all value types), like int, float, char, etc., are passed by value.
*/

using System;

class Program
{
    static void Increment(int x)
    {
        x++;
        Console.WriteLine($"Inside Increment method: x = {x}");
    }

    static void Main()
    {
        int num = 5;
        Console.WriteLine($"Before method call: num = {num}");
        Increment(num);
        Console.WriteLine($"After method call: num = {num}");
    }
}

/*
Output:
    Before method call: num = 5
    Inside Increment method: x = 6
    After method call: num = 5
*/
