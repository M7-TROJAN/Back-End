namespace _03_RetrieveSingleItem
{
    public class Wallet
    {
        public int Id { get; set; }
        public string? Holder { get; set; }
        public decimal Balance { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {Holder} ({Balance:C})";
        }
    }
}
