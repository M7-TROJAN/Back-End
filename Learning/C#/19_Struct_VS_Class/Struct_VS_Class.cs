using System;

namespace StructVsClass
{
    // Represents digital sizes in various units
    struct DigitalSize
    {
        private long bit;

        // Constants for conversion factors
        private const long BitsInBit = 1;
        private const long BitsInByte = 8;
        private const long BitsInKiloByte = BitsInByte * 1024;
        private const long BitsInMegaByte = BitsInKiloByte * 1024;
        private const long BitsInGigaByte = BitsInMegaByte * 1024;
        private const long BitsInTeraByte = BitsInGigaByte * 1024;

        // Properties for different units of digital size
        public string Bit => $"{(bit / BitsInBit):N0} bit";
        public string Byte => $"{(bit / BitsInByte):N0} Byte";
        public string KiloByte => $"{(bit / BitsInKiloByte):N0} Kilo Byte";
        public string MegaByte => $"{(bit / BitsInMegaByte):N0} Mega Byte";
        public string GigaByte => $"{(bit / BitsInGigaByte):N0} Giga Byte";
        public string TeraByte => $"{(bit / BitsInTeraByte):N0} Tera Byte";

        // Constructor to initialize the digital size
        public DigitalSize(long initialValue)
        {
            bit = initialValue;
        }

        // Adds the specified amount of bits to the digital size
        public DigitalSize AddBit(long value) => Add(value, BitsInBit);
        // Adds the specified amount of bytes to the digital size
        public DigitalSize AddByte(long value) => Add(value, BitsInByte);
        // Adds the specified amount of kilobytes to the digital size
        public DigitalSize AddKiloByte(long value) => Add(value, BitsInKiloByte);
        // Adds the specified amount of megabytes to the digital size
        public DigitalSize AddMegaByte(long value) => Add(value, BitsInMegaByte);
        // Adds the specified amount of gigabytes to the digital size
        public DigitalSize AddGigaByte(long value) => Add(value, BitsInGigaByte);
        // Adds the specified amount of terabytes to the digital size
        public DigitalSize AddTeraByte(long value) => Add(value, BitsInTeraByte);

        // Adds the specified value scaled by the provided scale
        private DigitalSize Add(long value, long scale)
        {
            long newBits = value * scale;
            return new DigitalSize(newBits);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            DigitalSize size = new DigitalSize(60000);

            size = size.AddGigaByte(1);
            Console.WriteLine(size.Bit);
            Console.WriteLine(size.Byte);
            Console.WriteLine(size.KiloByte);
            Console.WriteLine(size.MegaByte);
            Console.WriteLine(size.GigaByte);
            Console.WriteLine(size.TeraByte);

            Console.ReadKey();
        }
    }
}


// Immutable objects mean that once the constructor for an object has completed its execution that instance  can't be alerted.
// struct should be Immutable
/*
Class:
- A class is a reference type. It's a blueprint for creating objects that can contain fields, 
  properties, methods, events, and other members. Objects created from classes are allocated 
  on the heap, and variables of class types hold references to their memory locations.
- Supports user-defined types, constructors, parameterless constructors, fields, field initializer, 
  properties, methods, events, indexers, operator overloading, finalizers, inheritance, and implicitly 
  inherits from the Object class.
- Has reference semantics, meaning objects of class types are accessed via references.
- The new() keyword is mandatory when creating new instances.

Struct:
- A struct is a value type. It's similar to a class but with some limitations. Structs can contain fields, 
  properties, methods, and constructors, but they cannot contain destructors, which means they don't support 
  finalization. Instances of structs are typically allocated on the stack or inline in containing types, and 
  variables of struct types hold the actual data rather than references.
- Supports user-defined types, constructors, fields, properties, methods, events, indexers, operator overloading, 
  but does not support field initializer, finalizers, inheritance, and does not 
  implicitly inherit from the Object class.
- Has value semantics, meaning variables of struct types hold the actual data directly.
- The new() keyword is not required when creating new instances.


                                              class                              struct
user defined type                             True                               true
constructer                                   true                               true
support fields                                true                               true
field initializer (تدي الفيلد قيمة ابتدائية)            true                               false
support Properties                            true                               true
support Method                                true                               true
support event                                 true                               true
support indexers                              true                               true
support operator overloading                  true                               true
support finalizers                            true                               false
support inheritance                           true                               false
implicitly inherites obj class                true                               true
Value Semantic (value type)                   false                              true
Refrence Semantic (Refrence type)             true                               false
new() is mandetory?                           true                               false
*/
