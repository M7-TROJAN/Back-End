
namespace CA01ThreadVsTasks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main Thread";

            var th = new Thread(() => Display("M7Trojan using Thread !!!"));
            th.Start();

            var task = new Task(() => Display("M7Trojan using Task !!!"));
            task.Start();

            // or
            //Task.Run(() => Display("M7Trojan using Task !!!")).Wait();

            th.Join();
            task.Wait();


            Console.ReadKey();
        }

        private static void Display(string text)
        {
            ShowThreadInfo(Thread.CurrentThread);
            Console.WriteLine(text);
        }

        private static void ShowThreadInfo(Thread th)
        {
            Console.WriteLine($"\nThID: {th.ManagedThreadId}, ThName: {th.Name}, " +
                $"Pooled: {th.IsThreadPoolThread}, BackGround: {th.IsBackground}");
            
        }
    }
}
