using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Queue
{
    public class Program
    {
        public static void Main()
        {
            Queue<PrintingJop> printQueue = new Queue<PrintingJop>();

            printQueue.Enqueue(new PrintingJop("Documentation.docx", 3));
            printQueue.Enqueue(new PrintingJop("Presentation.pptx", 2));
            printQueue.Enqueue(new PrintingJop("Photo.jpg", 1));
            printQueue.Enqueue(new PrintingJop("user-stories.pdf", 4));
            printQueue.Enqueue(new PrintingJop("report.xlsx", 4));
            printQueue.Enqueue(new PrintingJop("payrool.xlsx", 1));
            printQueue.Enqueue(new PrintingJop("algorithm.ppt", 5));
            printQueue.Enqueue(new PrintingJop("logo.png", 2));
            printQueue.Enqueue(new PrintingJop("letter.docx", 2));

            Random rnd = new Random();

            while (printQueue.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                PrintingJop jop = printQueue.Dequeue();
                Console.WriteLine($"Printing... [{jop}]");
                System.Threading.Thread.Sleep(rnd.Next(1000, 5000));
            }
            Console.ResetColor();




            Console.ReadKey();
        }

    }
    
    public class PrintingJop
    {
        private readonly string _file;
        private readonly int _copies;

        public PrintingJop(string file, int copies)
        {
            _file = file;
            _copies = copies;
        }

        public override string ToString()
        {
            return $"{_file} x #{_copies} copies";
        }
    }
}
