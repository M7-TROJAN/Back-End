
namespace CA07SyncVsAsync
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ShowThreadInfo(Thread.CurrentThread, 8);
            CallSyncMethod();

            ShowThreadInfo(Thread.CurrentThread, 11);
            CallAsyncMethod();

            ShowThreadInfo(Thread.CurrentThread, 14);

            Console.ReadKey();
        }

        static void CallSyncMethod()
        {
            Thread.Sleep(4000);
            ShowThreadInfo(Thread.CurrentThread, 20);
            Task.Run(() => { Console.WriteLine("++++++++++ Synchronous ++++++++++"); }).Wait();
        }

        static void CallAsyncMethod()
        {
            ShowThreadInfo(Thread.CurrentThread, 28);
            Task.Delay(4000).GetAwaiter().OnCompleted(() => { 
                ShowThreadInfo(Thread.CurrentThread, 30);
                Console.WriteLine("++++++++++ Asynchronous ++++++++++"); 
            });

        }

        static void ShowThreadInfo(Thread th, int lineNumber)
        {
            Console.WriteLine($"\nLine#: {lineNumber}, ThID: {th.ManagedThreadId}, ThName: {th.Name}, " +
                $"Pooled: {th.IsThreadPoolThread}, BackGround: {th.IsBackground}");   
        }
    }
}
