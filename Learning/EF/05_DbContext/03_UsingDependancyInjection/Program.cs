﻿using _03_UsingDependancyInjection.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _03_UsingDependancyInjection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Build the configuration to read from appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Add the appsettings.json file to the configuration builder
                .Build(); // Build the configuration

            // Get the connection string from the configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Create a service collection
            var services = new ServiceCollection();

            // Register the services
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            // Create the service provider
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Get the AppDbContext service
            using (var context = serviceProvider.GetService<AppDbContext>())
            {
                try
                {
                    var wallets = context.Wallets;

                    foreach (var wallet in wallets)
                    {
                        Console.WriteLine(wallet);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
    }
}
