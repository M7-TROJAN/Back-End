using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Queue
{
    public class Program
    {
        public static void Main()
        {
            var lesson1 = new YTVideo { Id = "YTV1", Title = "C# Variables", Duration = new TimeSpan(00, 20, 00) };
            var lesson2 = new YTVideo { Id = "YTV2", Title = "Class VS Structure", Duration = new TimeSpan(00, 57, 10) };
            var lesson3 = new YTVideo { Id = "YTV3", Title = "Expressions", Duration = new TimeSpan(00, 47, 10) };
            var lesson4 = new YTVideo { Id = "YTV4", Title = "Iterations", Duration = new TimeSpan(01, 45, 00) };
            var lesson5 = new YTVideo { Id = "YTV5", Title = "Generics", Duration = new TimeSpan(00, 35, 33) };

            //var list = new LinkedList<YTVideo>(new YTVideo[]
            //{
            //    lesson1,
            //    lesson2,
            //    lesson3,
            //    lesson4,
            //    lesson5
            //});

            var linkedList = new LinkedList<YTVideo>();

            linkedList.AddFirst(lesson1);

            linkedList.AddAfter(linkedList.First, lesson2);

            var node3 = new LinkedListNode<YTVideo>(lesson3);
            linkedList.AddAfter(linkedList.First.Next, node3);

            linkedList.AddBefore(linkedList.Last, lesson4);

            linkedList.AddLast(lesson5);

            Print("Dot Net from Zoro to Hero", linkedList);
            Console.ReadKey();
        }

        static void Print(string title, LinkedList<YTVideo> playList)
        {
            if (playList == null)
            {
                throw new ArgumentNullException(nameof(playList), "Playlist cannot be null");
            }

            Console.WriteLine($"┌{title}");
            foreach (var item in playList)
                Console.WriteLine(item);
            Console.WriteLine("└");
            Console.WriteLine($"Total: {playList.Count} item(s)");
     
            // └ => alt + 192
            // ┌ => alt + 218
        }

    }

    public class YTVideo
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public TimeSpan Duration { get; set; } // HH:MM:SS

        public override string ToString()
        {
            // C# Variables (00:24:00)
            //https://www.youtube.com/watch?v=

            return $"├── {Title} ({Duration})\n│\thttps://www.youtube.com/watch?v={Id}";

            // ├ => alt + 195
            // ─ => alt + 196
            // │ => alt + 179
        }

    }

}
