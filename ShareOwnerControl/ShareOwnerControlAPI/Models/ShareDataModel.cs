using System.ComponentModel.DataAnnotations;

namespace ShareOwnerControlAPI.Models
{
    public class ShareDataModel
    {
        public ShareDataModel()
        {
        }

        [Key]
        public int Id { get; set; }
        public string StockId { get; set; }
        public int AllSharesAmount { get; set; }
    }
}
