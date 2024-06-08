using System;


namespace Learning
{
    class Program
    {
        static void Main()
        {
            var type = typeof(Program);
            var assembly = type.Assembly;

            // List all embedded resources
            foreach (var resource_Name in assembly.GetManifestResourceNames())
            {
                Console.WriteLine(resource_Name);
            }

            // Use the correct resource name
            var resourceName = "ConsoleApp1.data.countries.json";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    Console.WriteLine($"Resource '{resourceName}' not found.");
                    return;
                }

                var data = new BinaryReader(stream).ReadBytes((int)stream.Length);
                for (int i = 0; i < data.Length; i++)
                {
                    Console.Write((char)data[i]);
                    System.Threading.Thread.Sleep(100);
                }
            }
        }
    }
}








/*

### Steps to Embed a Resource

1. * *Add the File to Your Project**:
   -Right - click on your project in the Solution Explorer.
   - Choose "Add" -> "Existing Item..."
   - Select the file you want to add (e.g., `countries.json`).

2. **Set the Build Action**:
   -Right - click on the file in the Solution Explorer.
   - Choose "Properties".
   - Set the "Build Action" to "Embedded Resource".

### Retrieve the Embedded Resource

To retrieve the embedded resource, you need to use the correct name. 
The name typically follows the format: `Namespace.Folder.Filename`. 

*/
