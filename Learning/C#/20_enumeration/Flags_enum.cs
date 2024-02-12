/*
In C#, the [Flags] attribute is used to indicate that an enum is intended to be used as bit flags, where each value represents a single bit. 
This attribute provides additional information to the compiler and developers, indicating that certain operations such as bitwise AND, OR, XOR, 
and NOT can be applied to the enum values. 
*/

using System;

namespace Enumerations
{
    class Program
    {
        static void Main()
        {
            Permissions userPermissions = Permissions.Read | Permissions.Write;

            // Check if the user has write permission using the HasFlag method.
            if (userPermissions.HasFlag(Permissions.Write)) // you can achieve the same behavior using bitwise AND operation. (userPermissions & Permissions.Write) == Permissions.Write;
            {
                Console.WriteLine("User has write permission.");
            }
            else
            {
                Console.WriteLine("User does not have write permission.");
            }

            // Remove write permission from user
            userPermissions &= ~Permissions.Write;

            /*
            The ~ (tilde) operator is the bitwise complement operator.
            When applied to an integer value, it flips all of its bits, 
            changing each 0 to 1 and each 1 to 0. 
            */

            // Check if the user still has write permission
            if (userPermissions.HasFlag(Permissions.Write))
            {
                Console.WriteLine("User still has write permission.");
            }
            else
            {
                Console.WriteLine("User no longer has write permission.");
            }
        }
    }

    [Flags]
    public enum Permissions
    {
        None = 0,
        Read = 1,
        Write = 2,
        Execute = 4
    }
}
