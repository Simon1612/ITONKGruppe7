using ServiceStack.DataAnnotations;

namespace ShareOwnerControlAPI.Models
{
    public class StockDataModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string StockId { get; set; }
        public int SharePrice { get; set; }
    }
}
