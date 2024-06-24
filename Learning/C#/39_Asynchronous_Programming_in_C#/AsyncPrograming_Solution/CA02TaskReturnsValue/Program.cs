namespace CA02TaskReturnsValue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // How can a Task return a value?
            // A Task can return a value by using the Task<T> class.
            // The Task<T> class is a generic class that represents a task that returns a value.
            // The value returned by the task is of type T.
            // The Task<T> class is derived from the Task class.
            // The Task<T> class has a property called Result that returns the value returned by the task.
            // The Result property is of type T.
            // The Result property blocks the calling thread until the task completes and returns the value.
            // The Result property throws an exception if the task is canceled or throws an exception.

            Task<int> task = Task.Run(() =>
            {
                Thread.Sleep(2000);
                return 42;
            });

            Task<string> task2 = Task.Run(() =>
            {
                Thread.Sleep(2000);

                return "Hello";
            });

            Task<DateTime> task3 = Task.Run(() => GetDateTime(new DateTime(2010,2,2)));


            Console.WriteLine(task.Result); // blocks the calling thread until the task completes and returns the value
            Console.WriteLine(task2.Result); // blocks the calling thread until the task completes and returns the value
            Console.WriteLine(task3.Result); // blocks the calling thread until the task completes and returns the value

            Console.ReadKey();
        }

        private static DateTime GetDateTime(DateTime date = default)
        {
            Thread.Sleep(2000);
            if (date != default)
            {
                return date;
            }

            return DateTime.Now;
        }
    }
}
