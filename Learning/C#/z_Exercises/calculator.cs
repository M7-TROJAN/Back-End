using System;

namespace Revision
{
    public class Calculator
    {
        private double _previousResult = 0.0;
        private double _previousNumber = 0.0;
        private double _currentResult = 0.0;
        private char _previousOperation = ' ';

        private void UpdatePreviousValues(double value)
        {
            _previousResult = _currentResult;
            _previousNumber = value;
        }

        private void SetPreviousOperation(char operation)
        {
            _previousOperation = operation;
        }

        public void Add(double value)
        {
            UpdatePreviousValues(value);
            _currentResult += value;
            SetPreviousOperation('+');
        }

        public void Subtract(double value)
        {
            UpdatePreviousValues(value);
            _currentResult -= value;
            SetPreviousOperation('-');
        }

        public void Multiply(double value)
        {
            UpdatePreviousValues(value);
            _currentResult *= value;
            SetPreviousOperation('*');
        }

        public void Divide(double value)
        {
            if (value == 0)
                throw new DivideByZeroException("Cannot divide by zero.");

            UpdatePreviousValues(value);
            _currentResult /= value;
            SetPreviousOperation('/');
        }

        public double Result => _currentResult;

        public void PrintResult()
        {
            if (_previousOperation != ' ')
            {
                string operationDescription = GetOperationDescription();
                Console.WriteLine($"Result after {operationDescription} {_previousNumber} is {_currentResult}");
            }
            else
            {
                Console.WriteLine("No operation has been performed yet.");
            }
        }

        private string GetOperationDescription()
        {
            switch (_previousOperation)
            {
                case '+':
                    return "adding";
                case '-':
                    return "subtracting";
                case '*':
                    return "multiplying";
                case '/':
                    return "dividing";
                default:
                    return "performing an unknown operation";
            }
        }

        public void Clear()
        {
            _currentResult = 0.0;
            _previousOperation = ' ';
            _previousResult = 0.0;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // Example usage of the Calculator class
            Calculator calculator = new Calculator();

            Console.WriteLine("Result: " + calculator.Result);

            calculator.Add(5);
            Console.WriteLine("Result: " + calculator.Result);

            calculator.Divide(2);
            Console.WriteLine("Result: " + calculator.Result);

            calculator.Subtract(3);
            Console.WriteLine("Result: " + calculator.Result);

            calculator.Multiply(3);
            Console.WriteLine("Result: " + calculator.Result);

            calculator.Add(210);
            Console.WriteLine("Result: " + calculator.Result);

            // Access the result
            Console.WriteLine("Result: " + calculator.Result);

            calculator.PrintResult();

            try
            {
                calculator.Divide(0); // zero error
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}
