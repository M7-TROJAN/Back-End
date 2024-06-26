using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CAHttpClientJSON
{
    // the JSONPlaceholder API: https://jsonplaceholder.typicode.com/
    internal class Program
    {
        // Define a static HttpClient instance to be reused throughout the application
        private static readonly HttpClient _client = new HttpClient();

        static async Task Main(string[] args)
        {
            // URL to fetch data from
            string url = "https://jsonplaceholder.typicode.com/todos";

            try
            {
                // Fetch JSON data from the URL asynchronously
                var response = await _client.GetStringAsync(url);

                // Write the JSON response to a file
                File.WriteAllText("todos.json", response);
                Console.WriteLine("JSON data written to todos.json");

                // Deserialize the JSON data into a list of Todo objects
                var todos = JsonSerializer.Deserialize<List<Todo>>(response,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Check if deserialization was successful
                if (todos != null)
                {
                    foreach (var todo in todos)
                        Console.WriteLine(todo);
                }
                else
                {
                    Console.WriteLine("Failed to deserialize JSON response.");
                }

                // Deserialize the JSON data from the file
                var fileContent = File.ReadAllText("todos.json");
                var todosFromFile = JsonSerializer.Deserialize<List<Todo>>(fileContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Check if deserialization from file was successful
                if (todosFromFile != null)
                {
                    Console.WriteLine("\nDeserialized from file:");
                    foreach (var todo in todosFromFile)
                        Console.WriteLine(todo);
                }
                else
                {
                    Console.WriteLine("Failed to deserialize JSON from file.");
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP request error: {httpEx.Message}");
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"File I/O error: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            // Wait for user input before closing the console
            Console.ReadKey();
        }
    }

    public class Todo
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }

        public override string ToString()
        {
            return $"\n[Id: {Id}, UserId: {UserId}] - Title: {Title} - Status: {(Completed ? "Completed" : "Not Completed")}";
        }
    }
}
