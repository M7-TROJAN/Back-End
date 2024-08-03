namespace _01_ReverseEngineering
{
    internal class Program
    {
        // Step #1: Package Manager Console (PMC)
        //    Tools -> Nuget Package Manager -> Package Manager Console

        // Step #2: Package Manager Console (PMC) Tool 
        //    Install-Package Microsoft.EntityFrameworkCore.Tools

        // Step #3: Install Nuget Page on Project Microsoft.EntityFrameworkCore.Design
        // Microsoft.EntityFrameworkCore.SqlServer

        // Step #4: Install Provider in the project Microsoft.EntityFrameworkCore.SqlServer

        // Step #5: Run Command (Full)
        //    Scaffold-DbContext '[Connection String]' [Provider] -context [Context Name] -context-dir [Context Directory] -output-dir [Output Directory] -OutputDir [Output Directory] -UseDatabaseNames
        //   Scaffold-DbContext "Data Source=M7_TROJAN;Initial Catalog=TechTalk;Integrated Security=SSPI;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -context TechTalkContext -context-dir Data -output-dir Models -UseDatabaseNames


        static void Main(string[] args)
        {
            using (var context = new TechTalkContext())
            {
                var events = context.Events.ToList();
                var speakers = context.Speakers.ToList();

                foreach (var speaker in speakers)
                {
                    Console.WriteLine($"Speaker: {speaker.FirstName} {speaker.LastName}");
                    foreach (var talk in speaker.Events)
                    {
                        Console.WriteLine($"\t{talk.Title}");
                    }
                }

                Console.WriteLine("\n----------------------\n");

                foreach (var talk in events)
                {
                    Console.WriteLine($"Event: {talk.Title}");
                    Console.WriteLine($"\tSpeaker: {talk.Speaker.FirstName} {talk.Speaker.LastName}");
                }
            }
        }
    }
}
