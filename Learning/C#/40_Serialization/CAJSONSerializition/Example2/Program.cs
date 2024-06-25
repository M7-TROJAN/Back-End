using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace CAJSONSerialization
{
    internal class Program
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

            var jsonContent = SerializeToJSONString(emp);

            Console.WriteLine(jsonContent);

            try
            {
                var deserializedEmp = DeserializeFromJSONString(jsonContent);
                Console.WriteLine(deserializedEmp);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }

        private static string SerializeToJSONString(Employee emp)
        {
            // Create an instance of JsonSerializerOptions specifying indented formatting.
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            return JsonSerializer.Serialize(emp, options);
        }

        private static Employee? DeserializeFromJSONString(string jsonContent)
        {
            try
            {
                return JsonSerializer.Deserialize<Employee>(jsonContent);
            }
            catch (JsonException ex)
            {
                throw new Exception("Error in Deserialization", ex);
            }
        }
    }

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
}
