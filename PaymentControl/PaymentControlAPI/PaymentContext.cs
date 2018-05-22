using Microsoft.EntityFrameworkCore;

namespace PaymentControlAPI
{
    public class PaymentContext : DbContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options) { }
        public DbSet<PaymentDataModel> TradeData { get; set; }
    }
}
