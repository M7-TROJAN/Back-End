using System;
using System.Collections.Generic;
using System.Linq;

namespace Stack
{
    public class Program
    {
        public static void Main()
        {
            var Redo = new Stack<Command>();
            var Undo = new Stack<Command>();

            string line;

            while (true)
            {
                Console.Write("URL ('exit' to quit): ");
                line = Console.ReadLine().ToLower();
                if (line == "exit")
                {
                    break;
                }
                else if(line == "back")
                {
                    if(Undo.Count > 0)
                    {
                        var command = Undo.Pop();
                        Redo.Push(command);
                    }
                    else
                    {
                        continue;
                    }

                }
                else if(line == "forward")
                {
                    if(Redo.Count > 0)
                    {
                        var command = Redo.Pop();
                        Undo.Push(command);
                    }
                    else
                    {
                        continue;
                    }

                }
                else
                {
                    // add url to undo stack
                    Undo.Push(new Command(line));

                }
                Console.Clear();
                PrintStack("back", Undo);
                PrintStack("forward", Redo);

            } // end while

            Console.ReadKey();

        } // end Main

        public static void PrintStack(string name, Stack<Command> stack)
        {
            Console.WriteLine($"{name} History");
            Console.BackgroundColor = name.ToLower() == "back"? ConsoleColor.DarkGreen : ConsoleColor.DarkBlue;
            foreach (var item in stack)
            {
                Console.WriteLine($"\t{item}");
            }
            Console.ResetColor();
        }
    }

    public class Command     
    {
        private readonly DateTime _createdAt;
        private readonly string _url;

        public Command(string url)
        {
            _createdAt = DateTime.Now;
            _url = url;
        }

        override public string ToString()
        {
            return $"[{this._createdAt.ToString("dddd yyyy-MM-dd hh:mm")}] {this._url}";
        }
    }
}
