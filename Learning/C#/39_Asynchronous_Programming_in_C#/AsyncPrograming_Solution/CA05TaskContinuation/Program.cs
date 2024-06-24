
namespace CA05TaskContinuation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task<int> task = Task.Run(() => CountPrimeNumbersInRange(2, 3000000));
           
            // -- 1 --
           // Console.WriteLine(task.Result); 
           // Bad practice,Because it blocks the main thread, the main thread will be blocked until the task is completed


           // -- 2 --
           // Use ContinueWith method to avoid blocking the main thread
           task.ContinueWith(res =>
            {
                Console.WriteLine(res.Result);
            });

            // by using the ContinueWith method, the main thread will not be blocked, and the result will be printed when the task is completed.
            // The main thread will continue to execute the next line of code even if the task is not completed.

            // -- 3 --
            // Use GetAwaiter method to avoid blocking the main thread
            var awaiter = task.GetAwaiter();

            awaiter.OnCompleted(() =>
            {
                Console.WriteLine(awaiter.GetResult()); // GetResult method block the thread, but task is arlready compleated 
            });

            // by using the GetAwaiter method, the main thread will not be blocked, and the result will be printed when the task is completed.
            // The main thread will continue to execute the next line of code even if the task is not completed.

            Console.WriteLine("Press any key to exit...");

            Console.ReadKey();
        }

        static int CountPrimeNumbersInRange(int lowerBound, int upperBound)
        {
            int count = 0;
            for (int i = lowerBound; i <= upperBound; i++)
            {
                if (IsPrime(i))
                {
                    count++;
                }
            }
            return count;

            bool IsPrime(int num)
            {
                if (num < 2)
                {
                    return false;
                }
                for (int i = 2; i <= Math.Sqrt(num); i++)
                {
                    if (num % i == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        
    }
}
