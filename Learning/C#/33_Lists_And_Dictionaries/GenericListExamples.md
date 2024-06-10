
# Generic List Examples

## Country Class

The `Country` class represents a country with an ISO code and name. It implements the `IComparable<Country>` interface to allow sorting.

```csharp
public class Country : IComparable<Country>
{
    public string ISOCode { get; set; }
    public string Name { get; set; }

    public Country(string isoCode, string name)
    {
        ISOCode = isoCode;
        Name = name;
    }

    public override string ToString()
    {
        return $"{Name} - ({ISOCode})"; // e.g., India - (IN)
    }

    public int CompareTo(Country? other)
    {
        if (other == null) return 1;
        return String.Compare(this.Name, other.Name, StringComparison.OrdinalIgnoreCase);
    }
}
```

## Example Operations on List<Country>

### Initial Setup

Create a list of countries.

```csharp
var egypt = new Country("EGY", "Egypt");
var india = new Country("IN", "India");
var usa = new Country("US", "United States of America");
var iraq = new Country("IQ", "Iraq");
var jordan = new Country("JOR", "Jordan");

List<Country> countries = new List<Country> { egypt, india, usa, iraq, jordan };

// Print all countries
PrintCountries(countries);
```

### Add

Add a new country to the list.

```csharp
countries.Add(new Country("SA", "Saudi Arabia"));
PrintCountries(countries);
```

### Insert

Insert a new country at a specified index.

```csharp
countries.Insert(2, new Country("UK", "United Kingdom"));
PrintCountries(countries);
```

### Remove

Remove a specific country from the list.

```csharp
countries.Remove(usa);
PrintCountries(countries);
```

### RemoveAt

Remove a country at a specified index.

```csharp
countries.RemoveAt(3);
PrintCountries(countries);
```

### Find

Find a country by its name.

```csharp
var country = countries.Find(c => c.Name == "India");
if (country is null)
    Console.WriteLine("\nCountry not found");
else
    Console.WriteLine($"\nFound country: {country}");
```

### Sort

Sort the list of countries.

```csharp
countries.Sort();
PrintCountries(countries);
```

### Reverse

Reverse the order of countries in the list.

```csharp
countries.Reverse();
PrintCountries(countries);
```

### Clear

Clear all countries from the list.

```csharp
countries.Clear();
PrintCountries(countries);
```

### TrimExcess

Set the capacity to the actual number of elements in the list.

```csharp
countries.TrimExcess();
PrintCountries(countries);
```

### Convert Array to List

Convert an array of countries to a list.

```csharp
Country[] countriesArray = new Country[5];
countriesArray[0] = egypt;
countriesArray[1] = india;
countriesArray[2] = usa;
countriesArray[3] = iraq;
countriesArray[4] = jordan;

List<Country> countriesList = countriesArray.ToList();
PrintCountries(countriesList);
```

#### IndexOf

Find the index of a specific country.

```csharp
int index = countries.IndexOf(india);
Console.WriteLine($"Index of India: {index}");
```

#### AddRange

Add a range of countries to the list.

```csharp
var moreCountries = new List<Country>
{
    new Country("FR", "France"),
    new Country("DE", "Germany")
};
countries.AddRange(moreCountries);
PrintCountries(countries);
```

#### InsertRange

Insert a range of countries at a specified index.

```csharp
countries.InsertRange(2, moreCountries);
PrintCountries(countries);
```

#### RemoveRange

Remove a range of countries from the list.

```csharp
countries.RemoveRange(2, 2);
PrintCountries(countries);
```

#### RemoveAll

Remove all countries that match a specified condition.

```csharp
countries.RemoveAll(c => c.Name.StartsWith("G"));
PrintCountries(countries);
```

## PrintCountries Method

Utility method to print the list of countries.

```csharp
public static void PrintCountries(List<Country> countries)
{
    Console.WriteLine("\nCountries:");
    Console.WriteLine($"Count: {countries.Count}");
    Console.WriteLine($"Capacity: {countries.Capacity}");
    Console.WriteLine("----------");
    foreach (var country in countries)
    {
        Console.WriteLine(country);
    }
}
```

This method prints the number of countries and the capacity of the list, followed by each country's name and ISO code.
