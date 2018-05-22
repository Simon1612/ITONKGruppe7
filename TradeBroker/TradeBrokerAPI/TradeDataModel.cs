using System;

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
        public Guid StockProvider { get; set; }
        public Guid StockRequester { get; set; }
    }
}
