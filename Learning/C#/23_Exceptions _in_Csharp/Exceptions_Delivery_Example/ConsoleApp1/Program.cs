using System;

namespace Exceptions.DeliveryExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var delivery = new Delivery 
            { 
                Id = 1,
                CustomerName = "Mahmoud Mohamed",
                Address = "Cairo - Egypt",
            };

            var service = new DeliveryService();

            try 
            { 
                service.Start(delivery);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            
            Console.WriteLine(delivery);
        }
    }
}
