namespace CA06TaskDelay
{
    internal class Program
    {
        static void Main(string[] args)
        {
           DelayUsingTask(5000);

            Console.ReadKey();
        }

        static void DelayUsingTask(int ms)
        {
            Task.Delay(ms).ContinueWith( x => 
                Console.WriteLine($"Task.Delay({ms}) completed")
            );
        }

        static void DelayUsingThreadSleep(int ms)
        {
            Thread.Sleep(ms);
            Console.WriteLine($"Thread.Sleep({ms}) completed");
        }
    }

}


