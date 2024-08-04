using _01_BasicSetup.Data;
using Microsoft.EntityFrameworkCore;

namespace _01_BasicSetup
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                var orderDetailsWithProducts = context.OrderDetails
                                          .Include(od => od.Product).Where(od => od.OrderId == 1)
                                          .ToList();

                foreach (var od in orderDetailsWithProducts)
                {
                    Console.WriteLine(od.Product.Description);
                }

                var orderWithDetails = context.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .FirstOrDefault(o => o.Id == 1);

                foreach (var od in orderWithDetails.OrderDetails)
                {
                    Console.WriteLine(od.Product.Description);
                }

                var productWithOrders = context.Products
                    .Include(p => p.OrderDetails)
                    .ThenInclude(od => od.Order)
                    .FirstOrDefault(p => p.Id == 1);

                foreach (var od in productWithOrders.OrderDetails)
                {
                    Console.WriteLine(od.Order.CustomerEmail);
                }

            }
        }
    }
}
