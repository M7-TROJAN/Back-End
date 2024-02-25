namespace Exceptions.DeliveryExample
{
    public class Delivery
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = "";
        public string Address { get; set; } = "";
        public string DeliveredBy { get; set; } = "";
        public DeliveryStatus Status { get; set; }

        public override string ToString()
        {
            return $"{{\n" +
                   $"   ID: {Id}\n" +
                   $"   Customer: {CustomerName}\n" + 
                   $"   Address: {Address}\n" +
                   $"   Status: {Status}\n" +
                   $"   Delivered By: {DeliveredBy}\n" + 
                   $"}}\n";
        }
    }
}
