using Microsoft.Extensions.Configuration;

namespace _01_ConnectionString
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Console.WriteLine(configuration.GetSection("constr").Value);
        }
    }
}
