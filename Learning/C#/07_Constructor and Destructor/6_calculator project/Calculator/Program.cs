internal class Calculator
{
    private float _result = 0;
    private float _lastNumber = 0;
    private string _lastOperation = string.Empty;

    private bool _ValidateNoneZero(float number)
    {
        return number != 0;
    }

    public void Add(float number)
    {
        _lastNumber = number;
        _lastOperation = "Adding";
        _result += number;
    }

    public void Subtract(float number)
    {
        _lastNumber = number;
        _lastOperation = "Subtracting";
        _result -= number;
    }

    public void Multiply(float number)
    {
        _lastNumber = number;
        _lastOperation = "Multiplyng";
        _result *= number;
    }

    public void Divide(float number)
    {
        if (number == 0)
        {
            Console.WriteLine("Error: Division by zero is not allowed! Result remains unchanged.");
        }
        else
        {
            _lastNumber = number;
            _lastOperation = "dividing";
            _result /= number;
        }
    }

    public float GetFinalResult()
    {
        return _result;
    }

    public void PrintResult()
    {
        if (_lastOperation != "Cleared")
        {
            if (_result != 0 || _lastNumber != 0)
            {
                Console.WriteLine($"Result After {_lastOperation} {_lastNumber} IS {_result}");
            }
            else
            {
                Console.WriteLine("No calculation has been performed yet.");
            }
        }
    }

    public void Clear()
    {
        _lastNumber = 0f;
        _result = 0f;
        _lastOperation = "Cleared";
        Console.WriteLine("Calculator has been cleared.");
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        Calculator calculator = new Calculator();

        calculator.PrintResult(); // Print initial state

        calculator.Add(10);
        calculator.PrintResult();

        calculator.Subtract(10);
        calculator.PrintResult();

        calculator.Add(100);
        calculator.PrintResult();

        calculator.Multiply(2);
        calculator.PrintResult();

        calculator.Divide(0); // Demonstrate divide by zero
        calculator.PrintResult();

        calculator.Clear();
        calculator.PrintResult(); // Show the result after clearing (no calculation to display after clearing)
    }
}