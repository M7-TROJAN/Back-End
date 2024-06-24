namespace CA12ConcurrencyAndParallelism
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Concurrency and Parallelism
            // Concurrency is the ability to run multiple tasks at the same time using a single processor
            // Parallelism is the ability to run multiple tasks simultaneously using multiple processors


            // Concurrency
            // Concurrency is achieved by using async and await keywords
            // async and await keywords are used to run multiple tasks at the same time using a single processor

            // Parallelism
            // Parallelism is achieved by using Parallel class
            // Parallel class is used to run multiple tasks simultaneously using multiple processors


            var dailyDuties = new List<DailyDuty>
            {
                new DailyDuty("Wake up"),
                new DailyDuty("Brush teeth"),
                new DailyDuty("Take a shower"),
                new DailyDuty("Eat breakfast"),
                new DailyDuty("Go to work"),
                new DailyDuty("Work"),
                new DailyDuty("Eat lunch"),
                new DailyDuty("Work"),
                new DailyDuty("Go home"),
                new DailyDuty("Eat dinner"),
                new DailyDuty("Watch TV"),
                new DailyDuty("Go to bed")
            };

            Console.WriteLine("Processing things in parallel");
            Console.WriteLine("-------------------------------");
            await ProcessThingsInParaller(dailyDuties);

            Console.WriteLine("\nProcessing things in concurrent");
            Console.WriteLine("-------------------------------");
            await ProcessThingsInConCurrent(dailyDuties);


            Console.WriteLine("\nAll tasks completed");
            Console.ReadKey();
        }

        static Task ProcessThingsInParaller(IEnumerable<DailyDuty> dailyDuties)
        {
                Parallel.ForEach(dailyDuties, dailyDuty =>
                {
                    dailyDuty.Process();
                });

                return Task.CompletedTask;
        }

        static Task ProcessThingsInConCurrent(IEnumerable<DailyDuty> dailyDuties)
        {
                foreach (var dailyDuty in dailyDuties)
                {
                     dailyDuty.Process();
                }

                return Task.CompletedTask;
        }
    }

    class DailyDuty
    {
        
        public string Title { get; private set; }
        public bool Processed { get; private set; }

        public DailyDuty(string title)
        {
            this.Title = title;
        }

        public void Process()
        {
            Console.WriteLine($"TID: {Thread.CurrentThread.ManagedThreadId}, " +
                $"ProcessorID: {Thread.GetCurrentProcessorId()} ");

            Task.Delay(200).Wait();
            this.Processed = true;
        }
        
    }
}
