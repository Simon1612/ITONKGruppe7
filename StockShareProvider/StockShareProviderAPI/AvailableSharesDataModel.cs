
namespace StockShareProviderAPI
{
    public class AvailableSharesDataModel
    {
        public int Id { get; set; }
        public string StockId { get; set; }
        public int SharesAmount { get; set; }
        public bool Available { get; set; }
    }
}
