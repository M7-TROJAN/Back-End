using System;
using System.Reflection;

namespace AttributesDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize a list of updates
            Update[] updates = new Update[]
            {
                new Update("Update 1", "1.0.0", "This is the first update"),
                new Update("Update 2", "1.1.0", "This is the second update"),
                new Update("Update 3", "1.2.0", "This is the third update"),
                new Update("Update 4", "1.3.0", "This is the fourth update"),
                new Update("Update 5", "1.4.0", "This is the fifth update"),
                new Update("Security Update", "1.4.1", "This is a security update"),
                new Update("UI Enhancements", "1.4.2", "This update contains UI enhancements"),
                new Update("Bug Fixes", "1.4.3", "This update contains bug fixes")
            };

            // Process the updates
            UpdateProcessor.DownloadAndInstall(updates);

            Console.ReadKey();
        }
    }

    class UpdateProcessor
    {
        // Obsolete attribute: Marks the method as obsolete
        // The message parameter provides a message to the user when the method is used
        // The error parameter specifies whether the method should throw a compilation error or warning
        [Obsolete("This method is obsolete. Use DownloadAndInstall instead.", true)]
        public static void DownloadUpdates(Update[] updates)
        {
            Console.WriteLine("\nDownloading updates...");
            foreach (var update in updates)
            {
                Console.WriteLine($"Downloading {update}");
                System.Threading.Thread.Sleep(750); // Simulate download delay
            }
            Console.WriteLine("\nUpdates downloaded successfully!");
        }

        [Obsolete("This method is obsolete. Use DownloadAndInstall instead.", false)]
        public static void InstallUpdates(Update[] updates)
        {
            Console.WriteLine("\nInstalling updates...");
            foreach (var update in updates)
            {
                Console.WriteLine($"Installing {update}");
                System.Threading.Thread.Sleep(750); // Simulate installation delay
            }
            Console.WriteLine("\nUpdates installed successfully!");
        }

        public static void DownloadAndInstall(Update[] updates)
        {
            Console.WriteLine("\nDownloading and installing updates...");
            foreach (var update in updates)
            {
                Console.WriteLine($"Downloading {update}");
                System.Threading.Thread.Sleep(750); // Simulate download delay
                Console.WriteLine($"Installing {update}\n");
                System.Threading.Thread.Sleep(750); // Simulate installation delay
            }
            Console.WriteLine("\nUpdates downloaded and installed successfully!");
        }
    }

    class Update
    {
        private string _name;
        private string _version;
        private string _description;

        public Update(string name, string version, string description)
        {
            _name = name;
            _version = version;
            _description = description;
        }

        public override string ToString()
        {
            return $"Name: {_name}, Version: {_version}, Description: {_description}";
        }
    }
}
