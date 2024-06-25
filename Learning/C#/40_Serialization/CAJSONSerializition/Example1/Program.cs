using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

public class Employee
{
    public int Id { get; set; }
    public required string Fname { get; set; }
    public required string Lname { get; set; }
    public List<string> Benefits { get; set; } = new List<string>();

    public override string ToString()
    {
        return $"Id: {Id}, Fname: {Fname}, Lname: {Lname}, Benefits: {string.Join(", ", Benefits)}";
    }
}

class Program
{
    static void Main()
    {
        Employee emp = new Employee
        {
            Id = 1,
            Fname = "Mahmoud",
            Lname = "Mattar",
            Benefits = new List<string> { "Pension", "Health Insurance", "Vision" }
        };
        SerializeToJson(emp);
        DeserializeFromJson();
    }

    private static void SerializeToJson(Employee emp)
    {
        try
        {
            // Set the JsonSerializerOptions to format the JSON output
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            // Serialize the employee object to JSON
            string jsonString = JsonSerializer.Serialize(emp, options);
            File.WriteAllText("employee.json", jsonString);
            Console.WriteLine("Serialization succeeded.");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine("Serialization failed due to unauthorized access: " + ex.Message);
        }
        catch (IOException ex)
        {
            Console.WriteLine("Serialization failed due to an I/O error: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Serialization failed due to an unexpected error: " + ex.Message);
        }
    }

    private static void DeserializeFromJson()
    {
        Employee? emp2 = null;
        try
        {
            // Deserialize the JSON back to an Employee object
            string jsonString = File.ReadAllText("employee.json");
            emp2 = JsonSerializer.Deserialize<Employee>(jsonString);
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine("Deserialization failed due to unauthorized access: " + ex.Message);
        }
        catch (IOException ex)
        {
            Console.WriteLine("Deserialization failed due to an I/O error: " + ex.Message);
        }
        catch (JsonException ex)
        {
            Console.WriteLine("Deserialization failed due to a JSON error: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Deserialization failed due to an unexpected error: " + ex.Message);
        }

        if (emp2 is not null)
        {
            Console.WriteLine("Employee object deserialized from JSON");
            Console.WriteLine(emp2);
        }
        else
        {
            throw new InvalidOperationException("Deserialization resulted in a null object.");
        }
    }
}
