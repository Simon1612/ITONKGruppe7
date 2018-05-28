using System;
using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace TradeBrokerAPI
{
    public class TradeDataModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string StockId { get; set; }
        public int SharesAmount { get; set; }
        public List<Guid> StockProviders { get; set; }
        public Guid StockRequester { get; set; }
    }
}
