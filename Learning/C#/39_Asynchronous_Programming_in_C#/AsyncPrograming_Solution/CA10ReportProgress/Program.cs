namespace CA10ReportProgress
{
    internal class Program
    {
        static async Task  Main(string[] args)
        {
            // -- 1 --
            Action<int> progress1 = (p) => {Console.Clear(); Console.WriteLine($"{p}%"); };

            await Download(progress1);

            // -- 2 --
            Progress<int> progress2 = new Progress<int>(p => { Console.Clear(); Console.WriteLine($"{p}%"); });

            await Download(progress2);

            Console.ReadKey();


        }

        static Task Download(Action<int> onProgressPercentChanged)
        {
            return Task.Run(() =>
            {
                for (int i = 0; i <= 100; i++)
                {
                    Task.Delay(100).Wait();
                    if (i % 10 == 0)
                    {
                        onProgressPercentChanged(i);
                    }
                }
            });

        }

        static async Task Download(IProgress<int> progress)
        {
            await Task.Run(() =>
            {
                for (int i = 0; i <= 100; i++)
                {
                    Task.Delay(100).Wait();
                    if (i % 10 == 0)
                    {
                        progress.Report(i);
                    }
                }
            });

        }
    }

    
}
