using System;
using System.Collections.Generic;
using Humanizer;

namespace HumanizerExample
{
    public class Program
    {
        static void Main(string[] args)
        {
            // List of Facebook-like comments
            var comments = new List<FBComment>
            {
                new FBComment
                {
                    User = "Mahmoud Mattar",
                    Comment = "This is a great post",
                    CreatedAt = new DateTime(2024, 5, 1, 12, 0, 0)
                },
                new FBComment
                {
                    User = "Ahmed Ali",
                    Comment = "I think ASP .Net Core is the most powerful web framework",
                    CreatedAt = new DateTime(2024, 4, 14, 3, 24, 11)
                },
                new FBComment
                {
                    User = "Mohamed Samir",
                    Comment = "Personally I prefer using C# with it",
                    CreatedAt = new DateTime(2024, 6, 14, 2, 12, 10)
                },
                new FBComment
                {
                    User = "Ali Ahmed",
                    Comment = "Have you tried using Blazor?",
                    CreatedAt = new DateTime(2024, 6, 14, 3, 00, 00)
                },
                new FBComment
                {
                    User = "Rahma Yasser",
                    Comment = "I don't like it",
                    CreatedAt = new DateTime(2024, 6, 14, 3, 24, 17)
                },
                new FBComment
                {
                    User = "Yasser Mohamed",
                    Comment = "I prefer VB over C#",
                    CreatedAt = new DateTime(2024, 6, 14, 3, 24, 20)
                },
                new FBComment
                {
                    User = "Abeer Essam",
                    Comment = "VB is not from the C family languages, it's hard for me",
                    CreatedAt = DateTime.Now.AddMinutes(-5)
                }
            };

            // Print each comment
            foreach (var comment in comments)
            {
                Console.WriteLine(comment);
            }

            Console.ReadKey();
        }
    }

    // Class representing a Facebook-like comment
    public class FBComment
    {
        public string User { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        // Override ToString to provide a formatted string output
        public override string ToString()
        {
            return $"{User} says:\n" +
                   $"\"{Comment}\"\n" +
                   $"\t\t\t\t {CreatedAt.Humanize()}";
        }
    }
}
