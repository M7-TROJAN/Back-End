namespace CA09CancellationToken
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // CancellationToken is the ability to cancel a task or thread.
            // It is a struct that is passed to the task or thread that needs to be cancelled.
            // The task or thread can check the token to see if it has been cancelled and then stop its work.


            CancellationTokenSource cts = new CancellationTokenSource();

            // -- 1 --  
            // await DoCheck01(cts);

            // -- 2 --
            // await DoCheck02(cts);

            // -- 3 --
            await DoCheck03(cts);

            Console.ReadKey();
        }

        static async Task DoCheck01(CancellationTokenSource cts)
        {
            CancellationToken token = cts.Token;
            Task.Run( () => 
            { 
                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.Q)
                {
                    cts.Cancel();
                    Console.WriteLine("\nTask has been Cancelled !!");
                }
            });

            while (!token.IsCancellationRequested)
            {
                Console.Write("Checking ...");
                await Task.Delay(4000);
                Console.Write($"Compleated on {DateTime.Now}");
                Console.WriteLine();

            }

            Console.WriteLine("Check Was Terminated");
            cts.Dispose();
        }
        static async Task DoCheck02(CancellationTokenSource cts)
        {
            CancellationToken token = cts.Token;
            Task.Run( () => 
            { 
                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.Q)
                {
                    cts.Cancel();
                    Console.WriteLine("\nTask has been Cancelled !!");
                }
            });

            while (true)
            {
                Console.Write("Checking ...");
                await Task.Delay(4000, token);
                Console.Write($"Compleated on {DateTime.Now}");
                Console.WriteLine();

            }

            Console.WriteLine("Check Was Terminated");
            cts.Dispose();
        }


        static async Task DoCheck03(CancellationTokenSource cts)
        {
            CancellationToken token = cts.Token;
            Task.Run(() =>
            {
                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.Q)
                {
                    cts.Cancel();
                }
            });

            try
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    Console.Write("Checking ...");
                    await Task.Delay(4000);
                    Console.Write($"Compleated on {DateTime.Now}");
                    Console.WriteLine();

                }
            }
            catch (OperationCanceledException ex)
            {
               Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Check Was Terminated");
            cts.Dispose();
        }
    }
}
