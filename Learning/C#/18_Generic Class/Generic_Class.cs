// A generic class in C# is a class that can work with different data types without specifying the actual type until it is used. 
// It allows you to create classes, methods, and interfaces that operate on data types without committing to a specific one in advance.

// Why Use Generic Classes?
// 1. Reusability : Writing code that works with any data type makes your code more reusable.
// 2. Type Safety : The compiler enforces type safety, ensuring that you use the correct data types.
// 3. Code Clarity: Generics improve code clarity by avoiding unnecessary duplication.


namespace Generic_Class {
  
  public class Box<T>
  {
      private T item;
  
      public void Add(T newItem)
      {
          item = newItem;
      }
  
      public T GetItem()
      {
          return item;
      }
  }

  class Program
  {
      static void Main()
      {
          // Creating a Box for integers
          Box<int> intBox = new Box<int>();
          intBox.Add(42);
          Console.WriteLine("Item in intBox: " + intBox.GetItem());
  
          // Creating a Box for strings
          Box<string> stringBox = new Box<string>();
          stringBox.Add("Hello, M7Trojan");
          Console.WriteLine("Item in stringBox: " + stringBox.GetItem());
      }
  }

}
