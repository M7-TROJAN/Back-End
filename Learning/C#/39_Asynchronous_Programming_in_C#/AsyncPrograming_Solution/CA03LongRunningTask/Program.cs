namespace CA03LongRunningTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var task = Task.Factory.StartNew( ()=> RunLongTask(), TaskCreationOptions.LongRunning);
            



            Console.ReadKey();
        }

        static void RunLongTask()
        {
            Thread.Sleep(3000);
            ShowThreadInfo(Thread.CurrentThread);
            Console.WriteLine("Long task completed");
        }

        static void ShowThreadInfo(Thread th)
        {
            Console.WriteLine($"\nThID: {th.ManagedThreadId}, ThName: {th.Name}, " +
                $"Pooled: {th.IsThreadPoolThread}, BackGround: {th.IsBackground}");

        }
    }
}
