namespace CA04ExceptionPropagation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // -- 1 --
            //try
            //{
            //    var th = new Thread(ThrowException);
            //    th.Start();
            //    th.Join();
            //}
            //catch
            //{
            //    Console.WriteLine($"Exception is Thrown !!");
            //}

            // -- 2 --
            //var th = new Thread(ThrowExceptionWithTryCatch);
            //th.Start();
            //th.Join();

            // -- 3 --

            try
            {
                Task.Run(() => ThrowException()).Wait();
            }
            catch
            {
                Console.WriteLine($"Exception is Thrown !!");
            }
        }
        static void ThrowException()
        {
            throw new NotImplementedException();
        }
        
        static void ThrowExceptionWithTryCatch()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch
            {
                Console.WriteLine($"Exception is Thrown !!");
            }
        }
    }
}

// the main difference between the three methods is that the first method will not catch the exception
// the second method will catch the exception and print the message
// the third method will catch the exception and print the message as well
// the third method is the best method to use because it is the most efficient and the most readable
// the first method is the worst method to use because it is the least efficient and the least readable
// the second method is the second best method to use because it is more efficient than the first method but less efficient than the third method

