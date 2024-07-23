
# Card and Deck Classes

This document describes the `Card` and `Deck` classes used to represent a deck of cards and demonstrates how to use LINQ with these classes to perform various operations.

## Card Class

The `Card` class represents a single playing card with properties for its value and suite, along with a few computed properties and methods for getting the card's name and color.

### Card Class Definition

```csharp
public class Card
{
    public enum Suites
    {
        HEARTS = 0,
        DIAMONDS,
        CLUBS,
        SPADES
    }

    public int Value { get; set; }
    public Suites Suite { get; set; }

    public string NamedValue
    {
        get
        {
            string name = string.Empty;
            switch (Value)
            {
                case (14):
                    name = "Ace";
                    break;
                case (13):
                    name = "King";
                    break;
                case (12):
                    name = "Queen";
                    break;
                case (11):
                    name = "Jack";
                    break;
                default:
                    name = Value.ToString();
                    break;
            }

            return name;
        }
    }

    public string Name
    {
        get
        {
            return NamedValue + " of " + Suite.ToString();
        }
    }

    public bool IsRed => Suite == Suites.HEARTS || Suite == Suites.DIAMONDS;

    public Card(int Value, Suites Suite)
    {
        this.Value = Value;
        this.Suite = Suite;
    }
}
```

### Properties and Methods

- **Value**: The numerical value of the card (2-14).
- **Suite**: The suite of the card (Hearts, Diamonds, Clubs, Spades).
- **NamedValue**: A computed property that returns the name of the card's value (e.g., "Ace" for 14).
- **Name**: A computed property that returns the full name of the card (e.g., "Ace of Spades").
- **IsRed**: A boolean property that returns true if the card is a red card (Hearts or Diamonds).

## Deck Class

The `Deck` class represents a deck of 52 playing cards. It provides methods to fill the deck, shuffle the deck, and get a sample set of cards.

### Deck Class Definition

```csharp
public class Deck
{
    private static Random rnd = new Random();

    public IEnumerable<Card> FillDeck()
    {
        for (int i = 0; i < 52; i++)
        {
            Card.Suites suite = (Card.Suites)(Math.Floor((decimal)i / 13));
            int val = i % 13 + 2;
            yield return (new Card(val, suite));
        }
    }

    internal IEnumerable<Card> GetSample()
    {
        yield return FillDeck().Single(x => x.Value == 11 && x.Suite == Card.Suites.CLUBS);
        yield return FillDeck().Single(x => x.Value == 9 && x.Suite == Card.Suites.DIAMONDS);
        yield return FillDeck().Single(x => x.Value == 4 && x.Suite == Card.Suites.HEARTS);
        yield return FillDeck().Single(x => x.Value == 10 && x.Suite == Card.Suites.SPADES);
        yield return FillDeck().Single(x => x.Value == 3 && x.Suite == Card.Suites.HEARTS);
        yield return FillDeck().Single(x => x.Value == 6 && x.Suite == Card.Suites.HEARTS);
    }

    public IEnumerable<Card> Shuffle()
    {
        return FillDeck().OrderBy(x => rnd.Next());
    }
}
```

### Methods

- **FillDeck()**: Fills the deck with 52 cards, one of each value and suite.
- **GetSample()**: Returns a sample set of cards.
- **Shuffle()**: Returns the deck of cards in a shuffled order.

## Printing the Deck

The `PrintDeck` extension method prints the cards in a deck.

### PrintDeck Method Definition

```csharp
public static void PrintDeck(this IEnumerable<Card> cards, string title)
{
    Console.WriteLine($"\n\n\n###### {title} ######");
    foreach (Card card in cards)
    {
        Console.WriteLine(card.Name);
    }
}
```
