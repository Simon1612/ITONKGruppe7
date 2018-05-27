using System;
using System.Collections.Generic;

namespace TradeBrokerAPI
{
    public class TradeDataModel
    {
        public TradeDataModel()
        {
        }

        public int Id { get; set; }
        public string StockId { get; set; }
        public int SharesAmount { get; set; }
        public List<Guid> StockProviders { get; set; }
        public Guid StockRequester { get; set; }
    }
}
