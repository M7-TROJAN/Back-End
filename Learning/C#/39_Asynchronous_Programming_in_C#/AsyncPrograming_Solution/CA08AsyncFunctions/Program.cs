namespace CA08AsyncFunctions
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // 1- the old way
            //var url = "https://www.microsoft.com";
            //var task = ReadContent(url);
            //var awaiter = task.GetAwaiter();
            //awaiter.OnCompleted( () =>
            //{
            //    Console.WriteLine(awaiter.GetResult());
            //});

            // 2- the new way
            var url = "https://www.microsoft.com";
            
            // the await keyword is used to pause the execution of the method until the awaited task completes
            var content = await ReadContentAsync(url);

            Console.WriteLine(content);





            Console.ReadKey();
        }

        // the old way
        static Task<string> ReadContent(string url)
        {
            return Task.Run(() =>
            {
                using (var client = new HttpClient())
                {
                    return client.GetStringAsync(url).Result;
                }
            });
        }
        
        // the new way
        // by ading the async keyword to the method signature, the compiler will generate a state machine
        // the satate machine is responsible for managing the state of the method and the state of the task
        static async Task<string> ReadContentAsync(string url)
        {
            // each none blocking operation is replaced with await

            var client = new HttpClient();
            var task = client.GetStringAsync(url);
            var content =  await task; 


            DoSomeThing(); // this line will be executed even before the content is fetched

            return content;


        }

        static void DoSomeThing()
        {
            Console.WriteLine("DoSomeThing...");
        }
    }
}

// note: 
// 1- the async keyword is used to define an asynchronous method
// 2- the await keyword is used to pause the execution of the method until the awaited task completes
// 3- the async and await keywords are used together to create an asynchronous method
// 4- you cannot use the await keyword in a method that is not marked with the async keyword
