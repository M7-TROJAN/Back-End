namespace _03_ReverseEngineeringNETCLI
{
    internal class Program
    {
        // Step #1: Windows Terminal (Command Prompt) 

        // Step #2: Install Ef-Core tool globally
        //    dotnet tool install --global dotnet-ef    (new)
        //    dotnet tool update --global dotnet-ef     (to upgrade)

        // Step #3: Install Provider in the project Microsoft.EntityFrameworkCore.SqlServer

        // Step #4: Run Command (Full)
        //    dotnet ef dbcontext scaffold '[Connection String]' [Provider]
        //   dotnet ef dbcontext scaffold "Data Source=M7_TROJAN;Initial Catalog=TechTalk;Integrated Security=SSPI;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer
        static void Main(string[] args)
        {
            //using (var context = new TechTalkContext())
            //{
            //    var events = context.Events.ToList();
            //    var speakers = context.Speakers.ToList();

            //    foreach (var speaker in speakers)
            //    {
            //        Console.WriteLine($"Speaker: {speaker.FirstName} {speaker.LastName}");
            //        foreach (var talk in speaker.Events)
            //        {
            //            Console.WriteLine($"\t{talk.Title}");
            //        }
            //    }

            //    Console.WriteLine("\n----------------------\n");

            //    foreach (var talk in events)
            //    {
            //        Console.WriteLine($"Event: {talk.Title}");
            //        Console.WriteLine($"\tSpeaker: {talk.Speaker.FirstName} {talk.Speaker.LastName}");
            //    }
            //}

        }
    }
}
