# Understanding Events in C#

## What is an Event?

An event in C# is a mechanism that enables a class or object (known as the publisher) to notify other classes or objects (known as subscribers) when something interesting happens. Events are widely used in user interface programming, asynchronous programming, and more.

## Key Concepts

1. **Publisher and Subscriber:**
   - **Publisher:** The class that declares and raises the event.
   - **Subscriber:** The class that wants to be notified when the event occurs.

2. **Event Declaration:**
   - Events are declared using the `event` keyword.
   - Events are based on delegates, specifically the `EventHandler` delegate or custom delegate types.

   ```csharp
   public delegate void MyEventHandler(object sender, EventArgs e);

   public class Publisher
   {
       public event MyEventHandler MyEvent;
   }
   ```

1. **Event Handler (Delegate):**
    - An event is associated with a delegate.
    - The delegate type specifies the method signature that event handlers must follow.
    ```csharp
    public delegate void MyEventHandler(object sender, EventArgs e);
    ```

2. **Subscribing to Events:**
    - Subscribers attach event handlers to events using the += operator.
    ```charp
    Publisher publisher = new Publisher();
    publisher.MyEvent += MyEventHandlerMethod;
    ```

3. **Raising Events:**
    - Publishers raise events using the Invoke method or directly calling the event.
    ```csharp
    public class Publisher
    {
        public event MyEventHandler MyEvent;

        public void DoSomething()
        {
            // ... (some operation)

            // Raise the event
            MyEvent?.Invoke(this, EventArgs.Empty);
        }
    }
    ```

4. **Unsubscribing from Events:**
    - Subscribers detach event handlers using the -= operator.
    ```csharp
    publisher.MyEvent -= MyEventHandlerMethod;
    ```

## Example
**Here's a simple example using a Stock class to demonstrate event usage:**

```csharp
public delegate void StockPriceChangeHandler(Stock stock, decimal oldPrice);

public class Stock
{
    public event StockPriceChangeHandler OnPriceChanged;

    private decimal price;

    public decimal Price
    {
        get => this.price;
        set
        {
            decimal oldPrice = this.price;
            this.price = value;

            // Notify subscribers about the price change
            OnPriceChanged?.Invoke(this, oldPrice);
        }
    }
}

class Program
{
    static void Main()
    {
        Stock amazonStock = new Stock();

        // Subscribe to the event
        amazonStock.OnPriceChanged += Stock_OnPriceChanged;

        // Simulate a price change
        amazonStock.Price = 120;
    }

    static void Stock_OnPriceChanged(Stock stock, decimal oldPrice)
    {
        Console.WriteLine($"Stock price changed from {oldPrice} to {stock.Price}");
    }
}
```

## Steps in Using Events
1. Declare the Event: In the publisher class, declare the event using the event keyword and a delegate type.

2. Raise the Event: In the publisher class, raise the event when the interesting action occurs.

3. Subscribe to the Event: In the subscriber class, attach event handlers to the event.

4. Handle the Event: Implement the event handlers in the subscriber class. These methods will be called when the event is raised.

5. Unsubscribe (Optional): If a subscriber no longer wants to receive notifications, it can unsubscribe from the event.

> **.** Events provide a powerful way to implement loosely coupled systems, where components can interact without having direct dependencies on each other. They are commonly used in GUI applications, where UI elements need to react to user actions or changes in the system.


## Author
- Mahmoud Mohamed
- Email: mahmoud.abdalaziz@outlook.com
- LinkedIn: [Mahmoud Mohamed Abdalaziz](https://www.linkedin.com/in/mahmoud-mohamed-abd/)

Happy learning and coding! 🚀
