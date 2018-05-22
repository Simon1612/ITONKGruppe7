using System;
using System.ComponentModel.DataAnnotations;

namespace StockShareProviderAPI
{
    public class AvailableSharesDataModel
    {
        [Key]
        public int Id { get; set; }
        public string StockId { get; set; }
        public int SharesAmount { get; set; }
        public Guid StockOwner { get; set; }
    }
}
