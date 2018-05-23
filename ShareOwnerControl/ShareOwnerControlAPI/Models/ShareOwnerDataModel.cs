using System;
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
        public string StockId { get; set; }
        public int SharesAmount { get; set; }
        public Guid ShareOwner { get; set; }
    }
}
