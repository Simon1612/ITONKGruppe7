using System;
using Microsoft.EntityFrameworkCore;

namespace TradeBrokerAPI
{
    public class TradeLogContext : DbContext
    {
        public TradeLogContext(DbContextOptions<TradeLogContext> options) : base(options) { }
        public DbSet<TradeDataModel> TradeData { get; set; }
    }
}
