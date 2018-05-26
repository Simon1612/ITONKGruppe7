using System.ComponentModel.DataAnnotations;

namespace ShareOwnerControlAPI.Models
{
    public class ShareOwnerDataModel
    {
        public ShareOwnerDataModel()
        {
        }

        [Key]
        public int Id { get; set; }
        public ShareDataModel Stock { get; set; }
        public int SharesAmount { get; set; }
        public OwnerDataModel ShareOwner { get; set; }
    }
}
