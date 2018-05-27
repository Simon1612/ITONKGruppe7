using ServiceStack.DataAnnotations;

namespace ShareOwnerControlAPI.Models
{
    public class ShareOwnerDataModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public StockDataModel Stock { get; set; }
        public int SharesAmount { get; set; }
        public OwnerDataModel ShareOwner { get; set; }
    }
}
