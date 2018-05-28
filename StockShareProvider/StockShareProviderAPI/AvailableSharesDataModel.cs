using System;
using ServiceStack.DataAnnotations;

namespace StockShareProviderAPI
{
    public class AvailableSharesDataModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string StockId { get; set; }
        public int SharesAmount { get; set; }
        public Guid StockOwner { get; set; }
    }
}
