using System;

class Car
{
    private string _brand;
    private decimal _price;

    // Parameterized constructor
    public Car(string brand, decimal price)
    {
        this._brand = brand;
        this._price = price;
    }

    // Properties
    public decimal Price
    {
        get { return _price; }
    }

    public string Brand
    {
        get { return _brand; }
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        // Use the parameterized constructor to create an instance of Car
        Car car1 = new Car("Toyota", 25000.50m);

        // Access the properties of the Car instance
        Console.WriteLine($"Brand: {car1.Brand}");
        Console.WriteLine($"Price: {car1.Price:C}"); // Use formatting to display currency
    }
}
