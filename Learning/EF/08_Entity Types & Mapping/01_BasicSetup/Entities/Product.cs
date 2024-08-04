
namespace _01_BasicSetup.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public decimal UnitPrice { get; set; }

        public string Description { get; set; }

        // Navigation property
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
