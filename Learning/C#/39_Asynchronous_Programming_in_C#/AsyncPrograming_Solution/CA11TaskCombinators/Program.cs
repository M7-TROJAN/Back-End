namespace CA11TaskCombinators
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // TaskCombinators means combining multiple tasks together
            // Task.WhenAll() and Task.WhenAny() are the two main task combinators
            // Task.WhenAll() waits for all tasks to complete
            // Task.WhenAny() waits for any task to complete


            var has1000SubcribersTask = Task.Run(() => Has1000Subcribers());
            var has4000ViewHoursTask = Task.Run(() => Has4000ViewHours());

            // using Task.WhenAny()
            Console.WriteLine("Using Task.WhenAny()");
            Console.WriteLine("---------------------");
            var any = await Task.WhenAny(has1000SubcribersTask, has4000ViewHoursTask);
            Console.WriteLine(any.Result);

            // using Task.WhenAll()
            Console.WriteLine("\nUsing Task.WhenAll()");
            Console.WriteLine("---------------------");
            var all = await Task.WhenAll(has1000SubcribersTask, has4000ViewHoursTask);
            foreach (var result in all)
            {
                Console.WriteLine(result);
            }

             


            Console.ReadKey();
        }

        // moduling Youtube requirements to monetize the channel

        static Task<string> Has1000Subcribers()
        {
            return Task.Run(() =>
            {
                Task.Delay(4000).Wait();
                return Task.FromResult("Congratulation !! you have 1000 subscribers");
            });
        }

        static Task<string> Has4000ViewHours()
        {
            return Task.Run(() =>
            {
                Task.Delay(5000).Wait();
                return Task.FromResult("Congratulation !! you have 4000 view hours");
            });
        }
    }
}
