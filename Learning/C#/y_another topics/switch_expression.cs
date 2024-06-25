using System;

internal class Program
{
    static void Main(string[] args)
    {
        // Example: Representing a card number
        var cardNo = 13;

        // Using a switch expression to determine the card name
        string cardName = cardNo switch
        {
            1 => "ACE",      // If cardNo is 1, cardName is "ACE"
            12 => "QUEEN",   // If cardNo is 12, cardName is "QUEEN"
            13 => "KING",    // If cardNo is 13, cardName is "KING"
            10 => "JACK",    // If cardNo is 10, cardName is "JACK"
            _ => cardNo.ToString()  // For any other cardNo, use its string representation
        };

        // Display the determined card name
        Console.WriteLine($"Card Number: {cardNo}");
        Console.WriteLine($"Card Name: {cardName}");
        Console.ReadLine();
    }
}
