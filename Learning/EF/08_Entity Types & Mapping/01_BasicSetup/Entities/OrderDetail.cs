
namespace _01_BasicSetup.Entities
{

    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        // Navigation properties 
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
