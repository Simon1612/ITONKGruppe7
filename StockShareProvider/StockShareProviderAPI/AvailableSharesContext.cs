using Microsoft.EntityFrameworkCore;

namespace StockShareProviderAPI
{
    public class AvailableSharesContext : DbContext
    {
        public AvailableSharesContext(DbContextOptions<AvailableSharesContext> options) : base(options) { }
        public DbSet<AvailableSharesDataModel> AvailableSharesData { get; set; }
    }
}
