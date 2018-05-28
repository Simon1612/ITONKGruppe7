namespace StockShareRequesterAPI.Models
{
    public class ShareDataModel
    {
        public ShareDataModel()
        {
        }

        public int Id { get; set; }
        public string StockId { get; set; }
        public int SharePrice { get; set; }
    }
}