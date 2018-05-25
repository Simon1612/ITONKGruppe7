using System;

namespace TradeClient.Models
{
    public class Share
    {
        public string StockId { get; set; }

        public int Amount { get; set; }

        public Decimal Price { get; set; }
    }
}